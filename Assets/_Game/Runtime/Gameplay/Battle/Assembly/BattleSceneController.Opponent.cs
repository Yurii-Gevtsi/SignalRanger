using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Core.Modes;
using Game.Core.Battle;
using Game.Data.Progression;
using Game.Data.Save;
using Game.Core.Localization;
using Game.Gameplay.Battle.Arenas;
using Game.Gameplay.Battle.AI;
using Game.Gameplay.Battle.AI.Ballistics;
using Game.Gameplay.Battle.AI.Movement;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Feedback;
using Game.Gameplay.Battle.Motion;
using Game.Gameplay.Battle.Net;
using Game.Gameplay.Battle.Projectiles;
using Game.Gameplay.Battle.Results;
using Game.Gameplay.Battle.Runtime;
using Game.Gameplay.Battle.Simulation;
using Game.Gameplay.Battle.Terrain;
using Game.Gameplay.Battle.Units;
using Game.Gameplay.Battle.Visuals;
using Game.Gameplay.Battle.Weapons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.Gameplay.Battle.Assembly
{
    public sealed partial class BattleSceneController
    {
        private static AiDifficulty MapDifficulty(int tier)
        {
            return tier switch
            {
                >= 2 => AiDifficulty.Hard,
                1 => AiDifficulty.Normal,
                _ => AiDifficulty.Easy
            };
        }

        private static CharacterArchetype ResolveOpponentArchetype(string archetypeId, AiDifficulty fallbackDifficulty)
        {
            return PrototypeArchetypeLibrary.TryGetById(archetypeId, out var archetype)
                ? archetype
                : PrototypeArchetypeLibrary.ForDifficulty(fallbackDifficulty);
        }

        /// <summary>
        /// Connects the duel transport and states who owns which unit. Both sides
        /// build their own side as <c>player.*</c>, so the authority map is what lets
        /// a command name a commander rather than a local id.
        /// </summary>
        /// <summary>
        /// Mirrors a local action onto the wire. No-op outside a networked duel, and
        /// suppressed while a remote command is being replayed so an incoming action
        /// is never echoed back at its sender.
        /// </summary>
        private void EmitDuelCommand(
            BattleCommandKind kind,
            int value = 0,
            Vector2 origin = default,
            Vector2 direction = default,
            float power = 0f)
        {
            if (_duel == null || _duel.IsApplyingRemote)
            {
                return;
            }

            var localUnit = GetPresenter(BattleTeamId.Player)?.State;
            if (localUnit != null)
            {
                _duel.SendLocalCommand(kind, localUnit.UnitId, value, origin, direction, power);
            }
        }

        private void SetUpDuelNetworking()
        {
            _duel = null;
            if (GameSessionContext.SelectedBattleMode != BattleMode.Pvp)
            {
                return;
            }

            var session = PvpSessionBridge.Resolve();
            var opponent = GameSessionContext.SelectedPvpOpponent;
            // No transport, or an opponent that never came from a live match: the
            // duel simply plays out locally against the profile it was handed.
            if (session == null || opponent == null || opponent.IsBotBackfill)
            {
                return;
            }

            var localPlayerId = GameSessionContext.LocalPlayerId;
            var authority = new BattleUnitAuthority();
            authority.SetLocalPlayer(localPlayerId);
            foreach (var unit in _battleState.Teams.SelectMany(team => team.Units))
            {
                authority.Assign(
                    unit.TeamId == BattleTeamId.Player ? localPlayerId : opponent.PlayerId,
                    unit.UnitId);
            }

            var coordinator = new BattleDuelCoordinator(
                session,
                this,
                authority,
                CaptureTurnSnapshot,
                ApplyAuthoritativeTurn);
            coordinator.OpponentLeft += OnDuelOpponentLeft;
            _duel = coordinator;

            session.Connect(
                GameSessionContext.SelectedMatchId,
                localPlayerId,
                PvpSessionRole.Guest,
                connected =>
                {
                    if (!connected)
                    {
                        Debug.LogWarning("Duel transport did not connect; the battle continues locally.");
                    }
                });
        }

        private void ApplyAuthoritativeTurn(BattleTurnSnapshot snapshot)
        {
            var result = BattleTurnReconciler.Apply(_battleState, _battleTerrain, snapshot);
            _duel?.ReportReconcileResult(result);
            if (result.CorrectedUnits > 0)
            {
                RefreshUnitViews();
            }
        }

        private void OnDuelOpponentLeft(string playerId)
        {
            // A commander who leaves mid-battle concedes it; the player is not left
            // staring at a turn that will never come.
            _status = GameText.OpponentLeftDuel();
        }

        /// <summary>
        /// The battle as the authoritative side sees it right now, for publishing at a
        /// turn boundary or for comparing against what another client believes.
        /// </summary>
        public BattleTurnSnapshot CaptureTurnSnapshot()
        {
            return BattleTurnSnapshot.Capture(_battleState, _turnCraters.ToArray());
        }

        private void UpdateOpponentBot()
        {
            // The bot plans and paces its own turn; ticking the simulation lets any
            // shot it fires (Stage 3: also movement) integrate this frame.
            _botTurnController?.Tick(Time.deltaTime);
            _simulation.Tick(Time.deltaTime);
            _status = _botTurnController?.Activity switch
            {
                BotActivity.Repositioning => GameText.Moved(DescribeTeam(BattleTeamId.Opponent)),
                BotActivity.Firing => GameText.OpponentFiring(),
                _ => GameText.OpponentAiming()
            };
        }

        private BotTurnController CreateBotTurnController()
        {
            var probe = new TrajectoryProbe(
                _battleTerrain.Grid,
                Physics2D.gravity,
                SimulationTickSeconds,
                BattleSimulation.ProjectileSubStepLength,
                GetFallDeathY());
            var profile = BotProfileProvider.Resolve(_aiDifficulty);
            var random = new SystemBotRandom(System.Environment.TickCount);
            IBotShotPlanner planner = _aiDifficulty == AiDifficulty.Easy
                ? new HeuristicShotPlanner(
                    _aiDifficulty,
                    new BallisticShotPlanner(probe, profile, random, Physics2D.gravity))
                : new BallisticShotPlanner(probe, profile, random, Physics2D.gravity);

            var opponentMotionProfile = CreateMotionProfile(_opponentArchetype);
            var scanner = new WalkReachabilityScanner(_battleTerrain.Grid, opponentMotionProfile, GetFallDeathY());
            var jumpGenerator = new JumpCandidateGenerator(_battleTerrain.Grid, opponentMotionProfile, _opponentArchetype, GetFallDeathY());
            var positionPlanner = new BotPositionPlanner(scanner, planner, profile, opponentMotionProfile.WalkSpeed, jumpGenerator);

            return new BotTurnController(
                _simulation,
                planner,
                positionPlanner,
                profile,
                random,
                UnitCollisionHalfWidth,
                _playerArchetype,
                _opponentArchetype,
                _baseAimSettings);
        }

        /// <summary>
        /// Who the HUD names as the opponent. Story and training already say it
        /// through the mission briefing, so only the repeatable modes fill this in:
        /// the antagonist for a raid, the matched commander's call sign for a duel.
        /// </summary>
        private static string ResolveOpponentName()
        {
            return GameSessionContext.SelectedBattleMode switch
            {
                BattleMode.Raid => GameText.SigmaName(),
                BattleMode.Pvp => GameSessionContext.SelectedPvpOpponent?.DisplayName ?? string.Empty,
                _ => string.Empty
            };
        }
    }
}

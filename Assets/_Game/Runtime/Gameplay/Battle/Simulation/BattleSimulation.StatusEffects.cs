using System;
using System.Collections.Generic;
using Game.Data.Progression;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Motion;
using Game.Gameplay.Battle.Projectiles;
using Game.Gameplay.Battle.Terrain;
using Game.Gameplay.Battle.Units;
using Game.Gameplay.Battle.Weapons;
using UnityEngine;

namespace Game.Gameplay.Battle.Simulation
{
    public sealed partial class BattleSimulation
    {
        private void ApplyPoison(
            BattleUnitState unit,
            BattleTeamId sourceTeam,
            int resolvedDirectDamage,
            WeaponEvolutionProfile evolution,
            int durationTurns = ToxicBaseDurationTurns,
            float movementDamagePerSecond = 0f)
        {
            if (unit == null || unit.IsDefeated || resolvedDirectDamage <= 0)
            {
                return;
            }

            var duration = Mathf.Max(1, durationTurns);
            var damagePerTurn = Mathf.Max(1, Mathf.RoundToInt(resolvedDirectDamage * ElementalStatusDamageRatio));
            if (_poisonEffects.TryGetValue(unit.UnitId, out var existing))
            {
                existing.SourceTeam = sourceTeam;
                existing.DamagePerTurn = damagePerTurn;
                existing.TurnsRemaining = duration;
                existing.ArmorWeakenPercent = Mathf.Max(existing.ArmorWeakenPercent, evolution.ToxicArmorWeakenPercent);
                existing.ArmorWeakenTurnsRemaining = Mathf.Max(existing.ArmorWeakenTurnsRemaining, evolution.ToxicArmorWeakenTurns);
                // A new hit replaces the movement poison of the old one instead of stacking it.
                existing.MovementDamagePerSecond = movementDamagePerSecond;
                return;
            }

            _poisonEffects[unit.UnitId] = new ActivePoisonEffect(
                unit.UnitId,
                sourceTeam,
                damagePerTurn,
                duration,
                evolution.ToxicArmorWeakenPercent,
                evolution.ToxicArmorWeakenTurns,
                movementDamagePerSecond);
            _poisonLastPositions[unit.UnitId] = unit.Position;
        }

        /// <summary>The same shared +25% for poison: nanotoxic charge, toxic sticky release and the poison spider.</summary>
        private int ResolvePoisonRepeatBonus(bool applies, BattleUnitState unit, int amount, float bonusPercent)
        {
            if (!applies || unit == null || amount <= 0
                || !_poisonEffects.TryGetValue(unit.UnitId, out var poison) || poison.TurnsRemaining <= 0)
            {
                return amount;
            }

            return Mathf.CeilToInt(amount * (1f + bonusPercent / 100f));
        }

        private int ResolveElementalRepeatBonus(
            Dictionary<string, ElementalStatus> effects, bool applies, BattleUnitState unit, int amount, float bonusPercent)
        {
            if (!applies || unit == null || amount <= 0 || !effects.ContainsKey(unit.UnitId))
            {
                return amount;
            }

            // A second hit of the same element boosts only its own direct damage.
            return Mathf.CeilToInt(amount * (1f + bonusPercent / 100f));
        }

        /// <summary>
        /// The repeat bonus of an element for the attacking side: the shared +25%, raised to 33⅓ / 41⅔ / 50% by the
        /// bonus scale of that side's cannon of the element. It boosts every weapon of that element the side fires.
        /// </summary>
        private float ResolveRepeatBonusPercent(BattleTeamId sourceTeam, string elementCannonId)
        {
            var steps = ProgressionFor(sourceTeam).ResolveEvolution(elementCannonId).ElementSecondFeatureSteps;
            return ResolveRepeatBonusPercentForSteps(steps);
        }

        public static float ResolveRepeatBonusPercentForSteps(int bonusSteps)
        {
            return ElementalRepeatBonusPercent + Mathf.Clamp(bonusSteps, 0, 3) * (ElementalRepeatBonusPercent / 3f);
        }

        /// <summary>Status turns an attack applies: 2, or 2 + the Radius purchases (up to 2) of an element cannon.</summary>
        private int ResolveElementStatusTurns(BattleTeamId sourceTeam, string weaponId)
        {
            return WeaponUpgradeChoiceResolver.IsElementCannon(weaponId)
                ? WeaponUpgradeChoiceResolver.ResolveElementCannonStatusTurns(
                    ProgressionFor(sourceTeam).ResolveUpgradeCount(weaponId, WeaponUpgradePath.Radius))
                : ElementalBaseDurationTurns;
        }

        /// <summary>
        /// The nanotoxic poison for movement: 0.5% of the unlock-level HP cap per second of any change of position.
        /// Until the HP cap curve exists it is derived from the charge's base damage (16% of the same cap), and it
        /// follows the Core purchases like the direct damage. The second step doubles it.
        /// </summary>
        private float ResolveMovementPoisonPerSecond(BattleTeamId sourceTeam, WeaponDefinition weapon,
            WeaponEvolutionProfile evolution)
        {
            if (weapon == null || evolution.ElementControlSteps <= 0)
            {
                return 0f;
            }

            var perSecond = weapon.Damage * (MovementPoisonCapPercent / NanotoxicBaseDamageCapPercent)
                * ResolveModifiersFor(sourceTeam, weapon).DamageMultiplier;
            return evolution.ElementControlSteps >= 2 ? perSecond * 2f : perSecond;
        }

        private void ApplyElementalStatus(
            Dictionary<string, ElementalStatus> effects, BattleUnitState unit, BattleTeamId sourceTeam, int directDamage,
            int attackId = 0, int turns = ElementalBaseDurationTurns)
        {
            if (unit == null || unit.IsDefeated || directDamage <= 0)
            {
                return;
            }

            // The tick is 20% of the direct hit this unit took; a repeat hit replaces it and the
            // remaining time with the full base duration instead of stacking.
            var tick = Mathf.Max(1, Mathf.RoundToInt(directDamage * ElementalStatusDamageRatio));
            effects[unit.UnitId] = new ElementalStatus(unit.UnitId, sourceTeam, tick, Mathf.Max(1, turns))
            {
                AttackId = attackId
            };
        }

        private void ProcessElementalForEndingActiveTurn(Dictionary<string, ElementalStatus> effects)
        {
            if (effects.Count == 0)
            {
                return;
            }

            var activeTeam = _state.Turn.ActiveTeam;
            var finished = new List<string>();
            foreach (var pair in effects)
            {
                var effect = pair.Value;
                var unit = FindUnit(effect.UnitId);
                if (unit == null || unit.IsDefeated)
                {
                    finished.Add(pair.Key);
                    continue;
                }

                // A status counts the turns of the droid that carries it, not of any droid of its team.
                if (unit.TeamId != activeTeam || unit.UnitId != _state.Turn.ActiveUnitId)
                {
                    continue;
                }

                ApplyDamage(unit, effect.DamagePerTurn, ElementalDamageSourceId, effect.SourceTeam);
                effect.TurnsRemaining--;
                if (effect.TurnsRemaining <= 0)
                {
                    finished.Add(pair.Key);
                }
            }

            foreach (var unitId in finished)
            {
                effects.Remove(unitId);
            }
        }

        public bool IsUnitChilled(string unitId)
        {
            return !string.IsNullOrWhiteSpace(unitId) && _chillEffects.ContainsKey(unitId);
        }

        private sealed class ElementalStatus
        {
            public ElementalStatus(string unitId, BattleTeamId sourceTeam, int damagePerTurn, int turnsRemaining)
            {
                UnitId = unitId;
                SourceTeam = sourceTeam;
                DamagePerTurn = damagePerTurn;
                TurnsRemaining = turnsRemaining;
            }

            public string UnitId { get; }
            public BattleTeamId SourceTeam { get; }
            public int DamagePerTurn { get; }
            public int TurnsRemaining { get; set; }
            /// <summary>The stream that applied it, so that its own later drops do not count as a repeat hit.</summary>
            public int AttackId { get; set; }
        }

        private void ProcessPoisonForEndingActiveTurn()
        {
            if (_poisonEffects.Count == 0)
            {
                return;
            }

            var activeTeam = _state.Turn.ActiveTeam;
            var completed = new List<string>();
            foreach (var pair in _poisonEffects)
            {
                var effect = pair.Value;
                var unit = FindUnit(effect.UnitId);
                if (unit == null || unit.IsDefeated)
                {
                    completed.Add(pair.Key);
                    continue;
                }

                if (unit.TeamId != activeTeam || unit.UnitId != _state.Turn.ActiveUnitId)
                {
                    continue;
                }

                if (effect.TurnsRemaining > 0)
                {
                    if (effect.DamagePerTurn > 0)
                    {
                        ApplyDamage(unit, effect.DamagePerTurn, PoisonDamageSourceId, effect.SourceTeam);
                    }

                    effect.TurnsRemaining--;
                    if (effect.TurnsRemaining <= 0)
                    {
                        effect.MovementDamagePerSecond = 0f;
                    }
                }

                if (effect.ArmorWeakenTurnsRemaining > 0)
                {
                    effect.ArmorWeakenTurnsRemaining--;
                }

                if (effect.TurnsRemaining <= 0 && effect.ArmorWeakenTurnsRemaining <= 0)
                {
                    completed.Add(pair.Key);
                }
            }

            foreach (var unitId in completed)
            {
                _poisonEffects.Remove(unitId);
            }
        }

        private void StepPoisonMovementDamage(float deltaTime)
        {
            if (_poisonEffects.Count == 0)
            {
                return;
            }

            foreach (var pair in _poisonEffects)
            {
                var effect = pair.Value;
                if (effect.MovementDamagePerSecond <= 0f)
                {
                    continue;
                }

                var unit = FindUnit(effect.UnitId);
                if (unit == null || unit.IsDefeated)
                {
                    continue;
                }

                // Any change of position counts: walking, jumping, knockback, a pull or a fall. A jump of more
                // than a unit within one tick is a teleport and counts as one second.
                var last = _poisonLastPositions.TryGetValue(effect.UnitId, out var stored) ? stored : unit.Position;
                _poisonLastPositions[effect.UnitId] = unit.Position;
                var moved = Vector2.Distance(last, unit.Position);
                var movedSeconds = moved > TeleportPositionJump ? 1f : moved > 0.0005f ? deltaTime : 0f;
                if (movedSeconds <= 0f)
                {
                    continue;
                }

                effect.MovementSecondsAccumulator += movedSeconds * effect.MovementDamagePerSecond;
                var damage = Mathf.FloorToInt(effect.MovementSecondsAccumulator);
                if (damage > 0)
                {
                    effect.MovementSecondsAccumulator -= damage;
                    ApplyDamage(unit, damage, PoisonDamageSourceId, effect.SourceTeam);
                }
            }
        }

        private void ProcessRepairEffectsForActiveTurn()
        {
            if (_repairEffects.Count == 0)
            {
                return;
            }

            var activeTeam = _state.Turn.ActiveTeam;
            var completed = new List<string>();
            foreach (var pair in _repairEffects)
            {
                var effect = pair.Value;
                if (effect.TeamId != activeTeam || effect.UnitId != _state.Turn.ActiveUnitId)
                {
                    continue;
                }

                var unit = FindUnit(effect.UnitId);
                if (unit == null || unit.IsDefeated)
                {
                    completed.Add(pair.Key);
                    continue;
                }

                if (effect.RegenerationChecksRemaining > 0)
                {
                    if (!effect.DamageTakenSinceLastCheck && !IsUnitRepairJammed(unit.UnitId))
                    {
                        var restored = unit.RestoreHealth(effect.HealAmount);
                        if (restored > 0)
                        {
                            HealthRestored?.Invoke(new HealthRestoredEvent(unit.Position, restored, effect.TeamId));
                        }
                    }

                    effect.RegenerationChecksRemaining--;
                    effect.DamageTakenSinceLastCheck = false;
                }

                if (effect.ProtectionTurnsRemaining > 0)
                {
                    effect.ProtectionTurnsRemaining--;
                }

                if (effect.ProtectionTurnsRemaining <= 0 && effect.RegenerationChecksRemaining <= 0)
                {
                    completed.Add(pair.Key);
                }
            }

            foreach (var unitId in completed)
            {
                _repairEffects.Remove(unitId);
            }
        }

        private bool IsActiveUnitDisoriented()
        {
            var activeUnitId = _state.Turn.ActiveUnitId;
            return !string.IsNullOrWhiteSpace(activeUnitId)
                && _disorientations.TryGetValue(activeUnitId, out var effect)
                && effect.FirstAffectedTurnSequence <= _turnSequence
                && effect.TurnsRemaining > 0;
        }

        private void ApplyDisorientation(string unitId, int turns)
        {
            if (string.IsNullOrWhiteSpace(unitId) || turns <= 0)
            {
                return;
            }

            var firstAffectedTurn = _turnSequence + 1;
            if (_disorientations.TryGetValue(unitId, out var existing))
            {
                existing.TurnsRemaining = Mathf.Max(existing.TurnsRemaining, turns);
                existing.FirstAffectedTurnSequence = Mathf.Min(existing.FirstAffectedTurnSequence, firstAffectedTurn);
                return;
            }

            _disorientations[unitId] = new ActiveDisorientation(firstAffectedTurn, turns);
        }

        private void ConsumeDisorientationForActiveTurn()
        {
            var activeUnitId = _state.Turn.ActiveUnitId;
            if (string.IsNullOrWhiteSpace(activeUnitId)
                || !_disorientations.TryGetValue(activeUnitId, out var effect)
                || effect.FirstAffectedTurnSequence > _turnSequence)
            {
                return;
            }

            effect.TurnsRemaining--;
            if (effect.TurnsRemaining <= 0)
            {
                _disorientations.Remove(activeUnitId);
            }
        }

        private bool IsActiveUnitFrozen()
        {
            var activeUnitId = _state.Turn.ActiveUnitId;
            return !string.IsNullOrWhiteSpace(activeUnitId)
                && _freezes.TryGetValue(activeUnitId, out var effect)
                && effect.FirstAffectedTurnSequence <= _turnSequence
                && effect.TurnsRemaining > 0;
        }

        /// <summary>
        /// True while the unit may not move by itself: the full freeze, the burning or electric movement lock (and
        /// the old disorientation) for the unit's own turns they cover.
        /// </summary>
        public bool IsUnitMovementLocked(string unitId)
        {
            return IsLockActiveFor(_freezes, unitId) || IsLockActiveFor(_disorientations, unitId);
        }

        /// <summary>True while the EMP aim jam forbids choosing a weapon that needs aiming.</summary>
        public bool IsUnitAimJammed(string unitId)
        {
            return IsLockActiveFor(_aimJams, unitId);
        }

        /// <summary>Whether the active unit may use this weapon now: an aim jam leaves only weapons without aiming.</summary>
        public bool CanActiveUnitUseWeapon(WeaponDefinition weapon)
        {
            return weapon == null || !RequiresAim(weapon) || !IsUnitAimJammed(_state.Turn.ActiveUnitId);
        }

        private bool IsLockActiveFor(Dictionary<string, ActiveDisorientation> locks, string unitId)
        {
            return !string.IsNullOrWhiteSpace(unitId)
                && locks.TryGetValue(unitId, out var effect)
                && effect.TurnsRemaining > 0
                && (unitId != _state.Turn.ActiveUnitId || effect.FirstAffectedTurnSequence <= _turnSequence);
        }

        private void ApplyAimJam(string unitId, int turns)
        {
            if (string.IsNullOrWhiteSpace(unitId) || turns <= 0)
            {
                return;
            }

            var firstAffectedTurn = _turnSequence + 1;
            if (_aimJams.TryGetValue(unitId, out var existing))
            {
                existing.TurnsRemaining = Mathf.Max(existing.TurnsRemaining, turns);
                existing.FirstAffectedTurnSequence = Mathf.Min(existing.FirstAffectedTurnSequence, firstAffectedTurn);
                return;
            }

            _aimJams[unitId] = new ActiveDisorientation(firstAffectedTurn, turns);
        }

        private void ConsumeAimJamForActiveTurn()
        {
            var activeUnitId = _state.Turn.ActiveUnitId;
            if (string.IsNullOrWhiteSpace(activeUnitId)
                || !_aimJams.TryGetValue(activeUnitId, out var effect)
                || effect.FirstAffectedTurnSequence > _turnSequence)
            {
                return;
            }

            effect.TurnsRemaining--;
            if (effect.TurnsRemaining <= 0)
            {
                _aimJams.Remove(activeUnitId);
            }
        }

        /// <summary>Own walking is halved by the shared cold and fire statuses.</summary>
        private float ResolveManualMovementFactor(string unitId)
        {
            return !string.IsNullOrWhiteSpace(unitId)
                && (_chillEffects.ContainsKey(unitId) || _fireEffects.ContainsKey(unitId))
                ? ElementalSlowMovementFactor
                : 1f;
        }

        public bool IsUnitFrozen(string unitId)
        {
            return !string.IsNullOrWhiteSpace(unitId)
                && _freezes.TryGetValue(unitId, out var effect)
                && effect.TurnsRemaining > 0;
        }

        public bool IsUnitPoisoned(string unitId)
        {
            return !string.IsNullOrWhiteSpace(unitId)
                && _poisonEffects.TryGetValue(unitId, out var effect)
                && (effect.TurnsRemaining > 0 || effect.ArmorWeakenTurnsRemaining > 0);
        }

        public bool IsUnitLavaBurning(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return (_lavaOverlayRemainingSeconds.TryGetValue(unitId, out var secondsRemaining)
                    && secondsRemaining > 0f)
                || _fireEffects.ContainsKey(unitId)
                || (_burnEffects.TryGetValue(unitId, out var burn)
                    && burn.TurnsRemaining > 0);
        }

        public bool IsUnitElectrified(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return (_electrifiedOverlayRemainingSeconds.TryGetValue(unitId, out var secondsRemaining)
                    && secondsRemaining > 0f)
                || _electricEffects.ContainsKey(unitId)
                || (_empStuns.TryGetValue(unitId, out var stun)
                    && stun.TurnsRemaining > 0)
                || (_empWeakens.TryGetValue(unitId, out var weaken)
                    && weaken.TurnsRemaining > 0);
        }

        private void ApplyElectrifiedOverlay(string unitId)
        {
            if (!string.IsNullOrWhiteSpace(unitId))
            {
                _electrifiedOverlayRemainingSeconds[unitId] = EmpOverlayDurationSeconds;
            }
        }

        private void StepElectrifiedOverlayStates(float deltaTime)
        {
            if (_electrifiedOverlayRemainingSeconds.Count == 0)
            {
                return;
            }

            var expiredUnitIds = new List<string>();
            var unitIds = new List<string>(_electrifiedOverlayRemainingSeconds.Keys);
            foreach (var unitId in unitIds)
            {
                var secondsRemaining = _electrifiedOverlayRemainingSeconds[unitId]
                    - Mathf.Max(0f, deltaTime);
                if (secondsRemaining <= 0f)
                {
                    expiredUnitIds.Add(unitId);
                }
                else
                {
                    _electrifiedOverlayRemainingSeconds[unitId] = secondsRemaining;
                }
            }

            foreach (var unitId in expiredUnitIds)
            {
                _electrifiedOverlayRemainingSeconds.Remove(unitId);
            }
        }

        private bool ApplyBurn(
            BattleUnitState unit,
            BattleTeamId sourceTeam,
            WeaponEvolutionProfile evolution,
            out bool wasAlreadyActive)
        {
            wasAlreadyActive = false;
            if (unit == null
                || unit.IsDefeated
                || evolution.FireGunIgniteTurns <= 0
                || evolution.FireGunIgniteDamagePerTurn <= 0)
            {
                return false;
            }

            if (_burnEffects.TryGetValue(unit.UnitId, out var existing))
            {
                wasAlreadyActive = existing.TurnsRemaining > 0;
                if (existing.LastApplicationTurnSequence == _turnSequence)
                {
                    return false;
                }

                existing.SourceTeam = sourceTeam;
                existing.DamagePerTurn = Mathf.Max(existing.DamagePerTurn, evolution.FireGunIgniteDamagePerTurn);
                existing.TurnsRemaining = Mathf.Max(existing.TurnsRemaining, evolution.FireGunIgniteTurns);
                existing.LastApplicationTurnSequence = _turnSequence;
                return true;
            }

            _burnEffects[unit.UnitId] = new ActiveBurnEffect(
                unit.UnitId,
                sourceTeam,
                evolution.FireGunIgniteDamagePerTurn,
                evolution.FireGunIgniteTurns,
                _turnSequence);
            return true;
        }

        private void ProcessBurnForActiveTurn()
        {
            if (_burnEffects.Count == 0)
            {
                return;
            }

            var activeTeam = _state.Turn.ActiveTeam;
            var expiredUnitIds = new List<string>();
            foreach (var pair in _burnEffects)
            {
                var burn = pair.Value;
                var unit = FindUnit(burn.UnitId);
                if (unit == null || unit.IsDefeated)
                {
                    expiredUnitIds.Add(pair.Key);
                    continue;
                }

                if (unit.TeamId != activeTeam)
                {
                    continue;
                }

                ApplyDamage(unit, burn.DamagePerTurn, BurnDamageSourceId, burn.SourceTeam);
                burn.TurnsRemaining--;
                if (burn.TurnsRemaining <= 0)
                {
                    expiredUnitIds.Add(pair.Key);
                }
            }

            foreach (var unitId in expiredUnitIds)
            {
                _burnEffects.Remove(unitId);
            }
        }

        private void StepLavaOverlayStates(float deltaTime)
        {
            if (_lavaOverlayRemainingSeconds.Count == 0)
            {
                return;
            }

            var expiredUnitIds = new List<string>();
            var unitIds = new List<string>(_lavaOverlayRemainingSeconds.Keys);
            foreach (var unitId in unitIds)
            {
                var secondsRemaining = _lavaOverlayRemainingSeconds[unitId]
                    - Mathf.Max(0f, deltaTime);
                if (secondsRemaining <= 0f)
                {
                    expiredUnitIds.Add(unitId);
                }
                else
                {
                    _lavaOverlayRemainingSeconds[unitId] = secondsRemaining;
                }
            }

            foreach (var unitId in expiredUnitIds)
            {
                _lavaOverlayRemainingSeconds.Remove(unitId);
            }
        }

        private void ApplyFreeze(string unitId, int turns)
        {
            if (string.IsNullOrWhiteSpace(unitId) || turns <= 0)
            {
                return;
            }

            var firstAffectedTurn = _turnSequence + 1;
            if (_freezes.TryGetValue(unitId, out var existing))
            {
                existing.TurnsRemaining = Mathf.Max(existing.TurnsRemaining, turns);
                existing.FirstAffectedTurnSequence = Mathf.Min(existing.FirstAffectedTurnSequence, firstAffectedTurn);
                return;
            }

            _freezes[unitId] = new ActiveDisorientation(firstAffectedTurn, turns);
        }

        private void ConsumeFreezeForActiveTurn()
        {
            var activeUnitId = _state.Turn.ActiveUnitId;
            if (string.IsNullOrWhiteSpace(activeUnitId)
                || !_freezes.TryGetValue(activeUnitId, out var effect)
                || effect.FirstAffectedTurnSequence > _turnSequence)
            {
                return;
            }

            effect.TurnsRemaining--;
            if (effect.TurnsRemaining <= 0)
            {
                _freezes.Remove(activeUnitId);
            }
        }

        private bool IsActiveUnitEmpStunned()
        {
            var activeUnitId = _state.Turn.ActiveUnitId;
            return !string.IsNullOrWhiteSpace(activeUnitId)
                && _empStuns.TryGetValue(activeUnitId, out var effect)
                && effect.FirstAffectedTurnSequence <= _turnSequence
                && effect.TurnsRemaining > 0;
        }

        private void ApplyEmpStun(string unitId, int turns)
        {
            if (string.IsNullOrWhiteSpace(unitId) || turns <= 0)
            {
                return;
            }

            var firstAffectedTurn = _turnSequence + 1;
            if (_empStuns.TryGetValue(unitId, out var existing))
            {
                existing.TurnsRemaining = Mathf.Max(existing.TurnsRemaining, turns);
                existing.FirstAffectedTurnSequence = Mathf.Min(existing.FirstAffectedTurnSequence, firstAffectedTurn);
                return;
            }

            _empStuns[unitId] = new ActiveDisorientation(firstAffectedTurn, turns);
        }

        private void ConsumeEmpStunForActiveTurn()
        {
            var activeUnitId = _state.Turn.ActiveUnitId;
            if (string.IsNullOrWhiteSpace(activeUnitId)
                || !_empStuns.TryGetValue(activeUnitId, out var effect)
                || effect.FirstAffectedTurnSequence > _turnSequence)
            {
                return;
            }

            effect.TurnsRemaining--;
            if (effect.TurnsRemaining <= 0)
            {
                _empStuns.Remove(activeUnitId);
            }
        }

        private void ApplyEmpWeaken(string unitId, int damagePercent, int turns)
        {
            if (string.IsNullOrWhiteSpace(unitId) || damagePercent <= 0 || turns <= 0)
            {
                return;
            }

            if (_empWeakens.TryGetValue(unitId, out var existing))
            {
                existing.DamagePercent = Mathf.Max(existing.DamagePercent, damagePercent);
                existing.TurnsRemaining = Mathf.Max(existing.TurnsRemaining, turns);
                return;
            }

            _empWeakens[unitId] = new ActiveEmpWeaken(damagePercent, turns);
        }

        private void ProcessEmpWeakenForActiveTurn()
        {
            if (_empWeakens.Count == 0)
            {
                return;
            }

            var activeTeam = _state.Turn.ActiveTeam;
            var completed = new List<string>();
            foreach (var pair in _empWeakens)
            {
                var unit = FindUnit(pair.Key);
                if (unit == null || unit.IsDefeated)
                {
                    completed.Add(pair.Key);
                    continue;
                }

                if (unit.TeamId != activeTeam)
                {
                    continue;
                }

                var effect = pair.Value;
                effect.TurnsRemaining--;
                if (effect.TurnsRemaining <= 0)
                {
                    completed.Add(pair.Key);
                }
            }

            foreach (var unitId in completed)
            {
                _empWeakens.Remove(unitId);
            }
        }

        public bool IsUnitSelfDestructing(string unitId)
        {
            return !string.IsNullOrWhiteSpace(unitId) && _selfDestructMarks.ContainsKey(unitId);
        }

        /// <summary>
        /// Marks the droid. Marking spends the charge but neither the action nor the turn: the droid may still
        /// move and use another weapon until its turn ends.
        /// </summary>
        private BattleFireResult ArmSelfDestruct(BattleUnitState unit, WeaponInventoryRuntime inventory, WeaponDefinition weapon)
        {
            if (_selfDestructMarks.ContainsKey(unit.UnitId))
            {
                return new BattleFireResult(BattleFireOutcome.Blocked, unit.TeamId, weapon);
            }

            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            _selfDestructMarks[unit.UnitId] = new SelfDestructMark(weapon);
            ShotFired?.Invoke(unit.TeamId, weapon, false);
            UnitsMoved?.Invoke();
            return new BattleFireResult(BattleFireOutcome.UtilityUsed, unit.TeamId, weapon, unit.Position);
        }

        /// <summary>
        /// When the marked droid's turn ends, its death and the blast are resolved together and only then is the
        /// winner decided (so the last two droids dying at once is a draw).
        /// </summary>
        private void ProcessSelfDestructForEndingActiveTurn()
        {
            if (_selfDestructMarks.Count == 0)
            {
                return;
            }

            var unit = FindUnit(_state.Turn.ActiveUnitId);
            if (unit == null || !_selfDestructMarks.TryGetValue(unit.UnitId, out var mark))
            {
                return;
            }

            _selfDestructMarks.Remove(unit.UnitId);
            if (unit.IsDefeated)
            {
                return;
            }

            var team = unit.TeamId;
            var modifiers = ResolveModifiersFor(team, mark.Weapon);
            var impact = new ProjectileImpact(
                $"projectile.{mark.Weapon.WeaponId}",
                unit.UnitId,
                unit.Position,
                Vector2.up,
                Mathf.RoundToInt(mark.Weapon.Damage * modifiers.DamageMultiplier),
                mark.Weapon.ImpactRadius * modifiers.ImpactRadiusMultiplier);
            ProjectileExploded?.Invoke(++_nextProjectileId, impact);
            var terrainChanged = ResolveDamageImpact(impact, mark.Weapon);
            ImpactResolved?.Invoke(new ImpactResolvedEvent(impact.Point, mark.Weapon, terrainChanged));
            // The protocol sacrifices its owner even if the blast and any protection left them standing.
            if (!unit.IsDefeated)
            {
                ApplyDamage(unit, unit.Health, SelfDestructWeaponId, team, canBeProtected: false);
            }

            UnitsMoved?.Invoke();
            ResolveBattleAfterMotion();
        }

        public bool IsUnitInfected(string unitId)
        {
            return !string.IsNullOrWhiteSpace(unitId) && _virusInfections.ContainsKey(unitId);
        }

        /// <summary>
        /// The virus appears 1.5 s after the device attaches. It remembers how much health the carrier was
        /// missing at that moment: if the carrier dies before the status ends, the owner gets exactly that back.
        /// </summary>
        private void InfectWithVirus(string carrierUnitId, string ownerUnitId, WeaponDefinition weapon)
        {
            var carrier = FindUnit(carrierUnitId);
            if (carrier == null || carrier.IsDefeated || _virusInfections.ContainsKey(carrierUnitId))
            {
                return;
            }

            var ownerTeam = ResolveTeamForOwner(ownerUnitId);
            var finalDamage = Mathf.RoundToInt(weapon.Damage * ResolveModifiersFor(ownerTeam, weapon).DamageMultiplier);
            _virusInfections[carrierUnitId] = new VirusInfection(
                ownerUnitId, ownerTeam, VirusDurationTurns,
                Mathf.Max(0, carrier.MaxHealth - carrier.Health), finalDamage);
        }

        /// <summary>
        /// Counts the carrier's own turn ends. The first two add nothing; at the third the carrier takes the
        /// final damage if it is alive, and the status ends.
        /// </summary>
        private void ProcessVirusForEndingActiveTurn()
        {
            if (_virusInfections.Count == 0)
            {
                return;
            }

            var activeTeam = _state.Turn.ActiveTeam;
            List<string> finished = null;
            foreach (var pair in _virusInfections)
            {
                var carrier = FindUnit(pair.Key);
                if (carrier == null || carrier.IsDefeated || carrier.TeamId != activeTeam
                    || carrier.UnitId != _state.Turn.ActiveUnitId)
                {
                    continue;
                }

                var infection = pair.Value;
                infection.TurnsRemaining--;
                if (infection.TurnsRemaining > 0)
                {
                    continue;
                }

                ApplyDamage(carrier, ResolveOutgoingDamage(infection.OwnerUnitId, infection.FinalDamage),
                    VirusWeaponId, infection.OwnerTeam);
                (finished ??= new List<string>()).Add(pair.Key);
            }

            if (finished == null)
            {
                return;
            }

            foreach (var carrierId in finished)
            {
                var infection = _virusInfections[carrierId];
                _virusInfections.Remove(carrierId);
                RestoreVirusOwner(infection, FindUnit(carrierId));
            }
        }

        /// <summary>A carrier that died at any moment of the status (by any damage) pays the saved deficit to the owner.</summary>
        private void StepVirusCarriers()
        {
            if (_virusInfections.Count == 0)
            {
                return;
            }

            List<string> dead = null;
            foreach (var pair in _virusInfections)
            {
                var carrier = FindUnit(pair.Key);
                if (carrier == null || carrier.IsDefeated)
                {
                    (dead ??= new List<string>()).Add(pair.Key);
                }
            }

            if (dead == null)
            {
                return;
            }

            foreach (var carrierId in dead)
            {
                var infection = _virusInfections[carrierId];
                _virusInfections.Remove(carrierId);
                RestoreVirusOwner(infection, FindUnit(carrierId));
            }
        }

        private void RestoreVirusOwner(VirusInfection infection, BattleUnitState carrier)
        {
            if (carrier == null || !carrier.IsDefeated || infection.HealReserve <= 0)
            {
                return;
            }

            // A dead owner is not revived and the health is not given to someone else; a repair jammer on the
            // owner blocks this source of healing too.
            var owner = FindUnit(infection.OwnerUnitId);
            if (owner == null || owner.IsDefeated || IsUnitRepairJammed(owner.UnitId))
            {
                return;
            }

            var restored = owner.RestoreHealth(infection.HealReserve);
            if (restored > 0)
            {
                HealthRestored?.Invoke(new HealthRestoredEvent(owner.Position, restored, owner.TeamId));
            }
        }

        private sealed class SelfDestructMark
        {
            public SelfDestructMark(WeaponDefinition weapon)
            {
                Weapon = weapon;
            }

            public WeaponDefinition Weapon { get; }
        }

        private sealed class VirusInfection
        {
            public VirusInfection(string ownerUnitId, BattleTeamId ownerTeam, int turns, int healReserve, int finalDamage)
            {
                OwnerUnitId = ownerUnitId;
                OwnerTeam = ownerTeam;
                TurnsRemaining = turns;
                HealReserve = healReserve;
                FinalDamage = finalDamage;
            }

            public string OwnerUnitId { get; }
            public BattleTeamId OwnerTeam { get; }
            public int TurnsRemaining { get; set; }
            public int HealReserve { get; }
            public int FinalDamage { get; }
        }

        public bool IsUnitRepairJammed(string unitId)
        {
            return !string.IsNullOrWhiteSpace(unitId)
                && _repairJammerTurns.TryGetValue(unitId, out var turns)
                && turns > 0;
        }

        private void ProcessRepairJammerForEndingActiveTurn()
        {
            var unitId = _state.Turn.ActiveUnitId;
            if (!IsUnitRepairJammed(unitId))
            {
                return;
            }

            var turns = _repairJammerTurns[unitId] - 1;
            if (turns <= 0)
            {
                _repairJammerTurns.Remove(unitId);
                return;
            }

            _repairJammerTurns[unitId] = turns;
        }

        private sealed class ActiveRepairEffect
        {
            public ActiveRepairEffect(
                string unitId,
                BattleTeamId teamId,
                int healAmount,
                int protectionTurns,
                int regenerationChecks)
            {
                UnitId = unitId;
                TeamId = teamId;
                HealAmount = Mathf.Max(0, healAmount);
                ProtectionTurnsRemaining = Mathf.Max(0, protectionTurns);
                RegenerationChecksRemaining = Mathf.Max(0, regenerationChecks);
            }

            public string UnitId { get; }
            public BattleTeamId TeamId { get; }
            public int HealAmount { get; }
            public int ProtectionTurnsRemaining { get; set; }
            public int RegenerationChecksRemaining { get; set; }
            public bool DamageTakenSinceLastCheck { get; set; }
        }

        private sealed class ActivePoisonEffect
        {
            public ActivePoisonEffect(
                string unitId,
                BattleTeamId sourceTeam,
                int damagePerTurn,
                int turnsRemaining,
                int armorWeakenPercent,
                int armorWeakenTurns,
                float movementDamagePerSecond)
            {
                UnitId = unitId;
                SourceTeam = sourceTeam;
                DamagePerTurn = Mathf.Max(0, damagePerTurn);
                TurnsRemaining = Mathf.Max(0, turnsRemaining);
                ArmorWeakenPercent = Mathf.Max(0, armorWeakenPercent);
                ArmorWeakenTurnsRemaining = Mathf.Max(0, armorWeakenTurns);
                MovementDamagePerSecond = Mathf.Max(0f, movementDamagePerSecond);
            }

            public string UnitId { get; }
            public BattleTeamId SourceTeam { get; set; }
            public int DamagePerTurn { get; set; }
            public int TurnsRemaining { get; set; }
            public int ArmorWeakenPercent { get; set; }
            public int ArmorWeakenTurnsRemaining { get; set; }
            public float MovementDamagePerSecond { get; set; }
            public float MovementSecondsAccumulator { get; set; }
        }

        private sealed class ActiveBurnEffect
        {
            public ActiveBurnEffect(
                string unitId,
                BattleTeamId sourceTeam,
                int damagePerTurn,
                int turnsRemaining,
                int lastApplicationTurnSequence)
            {
                UnitId = unitId;
                SourceTeam = sourceTeam;
                DamagePerTurn = Mathf.Max(0, damagePerTurn);
                TurnsRemaining = Mathf.Max(0, turnsRemaining);
                LastApplicationTurnSequence = Mathf.Max(0, lastApplicationTurnSequence);
            }

            public string UnitId { get; }
            public BattleTeamId SourceTeam { get; set; }
            public int DamagePerTurn { get; set; }
            public int TurnsRemaining { get; set; }
            public int LastApplicationTurnSequence { get; set; }
        }

        private sealed class ActiveEmpWeaken
        {
            public ActiveEmpWeaken(int damagePercent, int turnsRemaining)
            {
                DamagePercent = Mathf.Clamp(damagePercent, 0, 100);
                TurnsRemaining = Mathf.Max(0, turnsRemaining);
            }

            public int DamagePercent { get; set; }
            public int TurnsRemaining { get; set; }
        }

        private sealed class ActiveDisorientation
        {
            public ActiveDisorientation(int firstAffectedTurnSequence, int turnsRemaining)
            {
                FirstAffectedTurnSequence = Mathf.Max(0, firstAffectedTurnSequence);
                TurnsRemaining = Mathf.Max(0, turnsRemaining);
            }

            public int FirstAffectedTurnSequence { get; set; }
            public int TurnsRemaining { get; set; }
        }
    }
}

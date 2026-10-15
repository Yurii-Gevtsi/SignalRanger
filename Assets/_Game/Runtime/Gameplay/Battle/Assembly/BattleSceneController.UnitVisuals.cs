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
        private BattleUnitView CreateUnitView(BattleUnitState state)
        {
            var bodyHeight = ArchetypeFor(state.TeamId).BodyHeight;
            var unitObject = new GameObject(state.UnitId);
            unitObject.transform.position = state.Position;

            var rigPrefab = GetUnitRigPrefab(state.TeamId);
            var skin = rigPrefab != null ? null : GetUnitSkin(state.TeamId);
            GameObject rigInstance = null;
            if (rigPrefab != null)
            {
                rigInstance = Object.Instantiate(rigPrefab, unitObject.transform);
                rigInstance.name = "Rig Visual";
                var spriteOffset = _visualTheme != null ? _visualTheme.UnitSpriteOffset : Vector2.zero;
                var spriteScale = _visualTheme != null ? _visualTheme.UnitSpriteScale : Vector2.one;
                rigInstance.transform.localPosition = new Vector3(spriteOffset.x, spriteOffset.y, 0f);
                rigInstance.transform.localScale = new Vector3(spriteScale.x, spriteScale.y, 1f);
                ApplyKnownRigLayoutOverrides(rigInstance.transform, state.TeamId);
            }
            else if (skin == null)
            {
                var bodyObject = new GameObject("Body Visual");
                bodyObject.transform.SetParent(unitObject.transform);
                bodyObject.transform.localScale = new Vector3(0.72f, bodyHeight, 1f);

                var renderer = bodyObject.AddComponent<SpriteRenderer>();
                renderer.sprite = _circleSprite;
                renderer.sortingOrder = 20;
            }
            else
            {
                var bodyObject = new GameObject("Body Visual");
                bodyObject.transform.SetParent(unitObject.transform);
                var spriteOffset = _visualTheme != null ? _visualTheme.UnitSpriteOffset : Vector2.zero;
                var spriteScale = _visualTheme != null ? _visualTheme.UnitSpriteScale : Vector2.one;
                bodyObject.transform.localPosition = new Vector3(spriteOffset.x, spriteOffset.y, 0f);
                bodyObject.transform.localScale = new Vector3(spriteScale.x, spriteScale.y, 1f);

                var renderer = bodyObject.AddComponent<SpriteRenderer>();
                renderer.sprite = skin;
                renderer.color = Color.white;
                renderer.sortingOrder = 20;
            }

            var view = unitObject.AddComponent<BattleUnitView>();
            view.SetPreserveSpriteColor(skin != null || rigInstance != null);
            if ((skin != null || rigInstance != null) && _visualTheme != null)
            {
                view.SetArtFacing(state.TeamId == BattleTeamId.Player
                    ? _visualTheme.PlayerSkinFacingDirectionX
                    : _visualTheme.OpponentSkinFacingDirectionX);
            }

            if (rigInstance != null)
            {
                view.SetVisualRoot(rigInstance.transform);
            }

            if (state.TeamId == BattleTeamId.Player)
            {
                view.SetHealthLabelPositionY(1.3f);
                view.SetHealthLabelScale(0.15f);
            }

            return view;
        }

        private void ApplyKnownRigLayoutOverrides(Transform rigRoot, BattleTeamId teamId)
        {
            if (rigRoot == null || _visualTheme == null)
            {
                return;
            }

            var resourcePath = teamId == BattleTeamId.Player
                ? _visualTheme.PlayerRigResourcePath
                : _visualTheme.OpponentRigResourcePath;
            if (resourcePath != DroidRigResourcePath)
            {
                return;
            }

            var body = rigRoot.Find("Body");
            if (body == null)
            {
                return;
            }

            var position = body.localPosition;
            body.localPosition = new Vector3(position.x, DroidBodyLocalY, position.z);
        }

        private void UpdateRopeVisual()
        {
            if (_ropeLine == null || _simulation == null || !_simulation.HasPlayerRopeTraversal)
            {
                if (_ropeLine != null) _ropeLine.enabled = false;
                if (_ropeHookVisual != null) _ropeHookVisual.SetActive(false);
                return;
            }

            var presenter = GetPresenter(BattleTeamId.Player);
            if (presenter == null)
            {
                return;
            }

            var hookPosition = _simulation.RopeHookPosition;
            _ropeLine.enabled = true;
            _ropeLine.SetPosition(0, presenter.State.Position);
            _ropeLine.SetPosition(1, hookPosition);
            if (_ropeHookVisual == null)
            {
                _ropeHookVisual = new GameObject("Rope Hook");
                _arenaObjects.Add(_ropeHookVisual);
                _ropeHookVisual.AddComponent<SpriteRenderer>();
                var renderer = _ropeHookVisual.GetComponent<SpriteRenderer>();
                renderer.sortingOrder = 32;
                _ropeHookVisual.transform.localScale = Vector3.one * 0.045f;
            }

            var hookRenderer = _ropeHookVisual.GetComponent<SpriteRenderer>();
            var hookSprite = LoadWeaponIconSprite(RopeHookSpriteId);
            if (hookRenderer != null && hookSprite != null && hookRenderer.sprite != hookSprite)
            {
                hookRenderer.sprite = hookSprite;
            }

            _ropeHookVisual.SetActive(true);
            _ropeHookVisual.transform.position = hookPosition;
            var hookDirection = _simulation.RopeHookDirection;
            if (hookDirection.sqrMagnitude > 0.0001f)
            {
                _ropeHookVisual.transform.rotation = Quaternion.Euler(0f, 0f,
                    Mathf.Atan2(hookDirection.y, hookDirection.x) * Mathf.Rad2Deg);
            }
        }

        private void UpdateElectricHarpoonVisual()
        {
            if (_electricHarpoonLine == null || _simulation == null || !_simulation.HasPlayerElectricHarpoon)
            {
                if (_electricHarpoonLine != null) _electricHarpoonLine.enabled = false;
                if (_electricHarpoonExtraLine != null) _electricHarpoonExtraLine.enabled = false;
                if (_electricHarpoonHookVisual != null) _electricHarpoonHookVisual.SetActive(false);
                return;
            }

            var presenter = GetPresenter(BattleTeamId.Player);
            if (presenter == null)
            {
                return;
            }

            var hookPosition = _simulation.ElectricHarpoonHookPosition;
            _electricHarpoonLine.enabled = true;
            _electricHarpoonLine.SetPosition(0, presenter.State.Position);
            _electricHarpoonLine.SetPosition(1, hookPosition);
            var extraTargets = _simulation.ElectricHarpoonExtraTargets;
            if (_electricHarpoonExtraLine != null)
            {
                // The double grab holds a second droid on its own taut rope.
                _electricHarpoonExtraLine.enabled = extraTargets.Count > 0;
                if (extraTargets.Count > 0)
                {
                    _electricHarpoonExtraLine.SetPosition(0, presenter.State.Position);
                    _electricHarpoonExtraLine.SetPosition(1, extraTargets[0]);
                }
            }
            if (_electricHarpoonHookVisual == null)
            {
                _electricHarpoonHookVisual = new GameObject("Electric Harpoon Hook");
                _arenaObjects.Add(_electricHarpoonHookVisual);
                _electricHarpoonHookVisual.AddComponent<SpriteRenderer>();
                var renderer = _electricHarpoonHookVisual.GetComponent<SpriteRenderer>();
                renderer.sortingOrder = 33;
                _electricHarpoonHookVisual.transform.localScale = Vector3.one * 0.045f;
            }

            var hookRenderer = _electricHarpoonHookVisual.GetComponent<SpriteRenderer>();
            var evolvedHarpoonId = _weaponsById.TryGetValue("weapon.electric-harpoon", out var harpoon)
                ? ResolveEvolvedIconId(harpoon.IconId, harpoon, _playerProgression)
                : "electric_harpoon";
            var hookSprite = LoadWeaponIconSprite(
                evolvedHarpoonId == "electric_harpoon_5A"
                    ? "electric_harpoon_5A_shot"
                    : "electric_harpoon_shot");
            if (hookRenderer != null && hookSprite != null && hookRenderer.sprite != hookSprite)
            {
                hookRenderer.sprite = hookSprite;
            }

            _electricHarpoonHookVisual.SetActive(true);
            _electricHarpoonHookVisual.transform.position = hookPosition;
            var hookDirection = _simulation.ElectricHarpoonDirection;
            if (hookDirection.sqrMagnitude > 0.0001f)
            {
                _electricHarpoonHookVisual.transform.rotation = Quaternion.Euler(0f, 0f,
                    Mathf.Atan2(hookDirection.y, hookDirection.x) * Mathf.Rad2Deg);
            }
        }

        private void RefreshUnitViews()
        {
            var freezeOverlay = LoadCenteredTextureSprite(FreezeOverlayResourcePath, "Freeze Overlay Ice 01");
            var poisonOverlay = LoadCenteredTextureSprite(PoisonOverlayResourcePath, "Poison Overlay Toxic 01");
            var lavaOverlay = LoadCenteredTextureSprite(LavaOverlayResourcePath, "Lava Overlay 01");
            var electroOverlayFrames = LoadElectroOverlayFrames();
            foreach (var presenter in _unitPresenters)
            {
                var isJumping = _unitMotors.TryGetValue(presenter.State.UnitId, out var motor)
                    && (motor.IsJumping || motor.IsHovering);
                var jumpTrailScale = isJumping ? motor.JumpTrailScale : 0f;
                presenter.Refresh(presenter.State.UnitId == _battleState.Turn.ActiveUnitId, isJumping, jumpTrailScale);
                presenter.View.SetFreezeOverlay(freezeOverlay,
                    _simulation.IsUnitFrozen(presenter.State.UnitId) || _simulation.IsUnitChilled(presenter.State.UnitId));
                presenter.View.SetPoisonOverlay(poisonOverlay, _simulation.IsUnitPoisoned(presenter.State.UnitId));
                presenter.View.SetLavaOverlay(lavaOverlay, _simulation.IsUnitLavaBurning(presenter.State.UnitId));
                presenter.View.SetElectroOverlay(electroOverlayFrames, _simulation.IsUnitElectrified(presenter.State.UnitId));
                presenter.SetRepairJammerActive(_simulation.IsUnitRepairJammed(presenter.State.UnitId));
                presenter.SetVirusActive(_simulation.IsUnitInfected(presenter.State.UnitId));
                presenter.SetSelfDestructActive(_simulation.IsUnitSelfDestructing(presenter.State.UnitId));
            }
        }

        private Sprite[] LoadElectroOverlayFrames()
        {
            if (_electroOverlayFrames != null)
            {
                return _electroOverlayFrames;
            }

            _electroOverlayFrames = new Sprite[ElectroOverlayResourcePaths.Length];
            for (var index = 0; index < ElectroOverlayResourcePaths.Length; index++)
            {
                _electroOverlayFrames[index] = LoadCenteredTextureSprite(
                    ElectroOverlayResourcePaths[index],
                    $"Electro Overlay {index + 1}");
            }

            return _electroOverlayFrames;
        }

        private BattleUnitPresenter GetActivePresenter()
        {
            return _unitPresenters.FirstOrDefault(presenter => presenter.State.UnitId == _battleState.Turn.ActiveUnitId);
        }

        private BattleUnitPresenter GetPresenter(BattleTeamId teamId)
        {
            return _unitPresenters.FirstOrDefault(presenter => presenter.State.TeamId == teamId && !presenter.State.IsDefeated);
        }

        private Sprite GetUnitSkin(BattleTeamId teamId)
        {
            if (_visualTheme == null)
            {
                return null;
            }

            var resourcePath = teamId == BattleTeamId.Player
                ? _visualTheme.PlayerSkinResourcePath
                : _visualTheme.OpponentSkinResourcePath;
            return LoadSpriteResource(resourcePath);
        }

        private GameObject GetUnitRigPrefab(BattleTeamId teamId)
        {
            if (_visualTheme == null)
            {
                return null;
            }

            var resourcePath = teamId == BattleTeamId.Player
                ? _visualTheme.PlayerRigResourcePath
                : _visualTheme.OpponentRigResourcePath;
            if (string.IsNullOrWhiteSpace(resourcePath))
            {
                return null;
            }

            return Resources.Load<GameObject>(resourcePath);
        }

        private Sprite LoadSpriteResource(string resourcePath)
        {
            if (string.IsNullOrWhiteSpace(resourcePath))
            {
                return null;
            }

            if (_spriteResourceCache.TryGetValue(resourcePath, out var cached))
            {
                return cached;
            }

            var sprite = Resources.Load<Sprite>(resourcePath);
            if (sprite == null)
            {
                var texture = Resources.Load<Texture2D>(resourcePath);
                if (texture != null)
                {
                    sprite = Sprite.Create(
                        texture,
                        new Rect(0f, 0f, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f),
                        100f);
                }
            }

            if (sprite != null)
            {
                _spriteResourceCache.Add(resourcePath, sprite);
            }

            return sprite;
        }

        /// <summary>The turn transfer art above the ally chosen to receive the turn, while the choice is open.</summary>
        private void UpdateTurnTransferMarker()
        {
            var targetId = _simulation != null ? _simulation.TurnTransferTargetUnitId : string.Empty;
            var target = string.IsNullOrEmpty(targetId)
                ? null
                : _unitPresenters.FirstOrDefault(presenter => presenter.State.UnitId == targetId);
            if (target == null)
            {
                if (_turnTransferMarker != null)
                {
                    _turnTransferMarker.SetActive(false);
                }

                return;
            }

            if (_turnTransferMarker == null)
            {
                var sprite = LoadWeaponIconSprite("turn_transfer");
                if (sprite == null)
                {
                    return;
                }

                _turnTransferMarker = new GameObject("Turn Transfer Marker");
                var renderer = _turnTransferMarker.AddComponent<SpriteRenderer>();
                renderer.sprite = sprite;
                renderer.sortingOrder = 35;
                _turnTransferMarker.transform.localScale = Vector3.one * (TurnTransferMarkerWorldWidth / Mathf.Max(0.001f, sprite.bounds.size.x));
                _arenaObjects.Add(_turnTransferMarker);
            }

            _turnTransferMarker.SetActive(true);
            _turnTransferMarker.transform.position = new Vector3(target.State.Position.x, target.State.Position.y + 1.05f, 0f);
        }

        private Sprite LoadWeaponIconSprite(string iconId)
        {
            if (string.IsNullOrWhiteSpace(iconId))
            {
                return null;
            }

            var resourcePath = $"UI/Weapons/{iconId}";
            if (_spriteResourceCache.TryGetValue(resourcePath, out var cached))
            {
                return cached;
            }

            var texture = Resources.Load<Texture2D>(resourcePath);
            if (texture == null)
            {
                return null;
            }

            var sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                100f,
                0,
                SpriteMeshType.FullRect);
            sprite.name = $"{iconId} Battle Mount";
            _spriteResourceCache.Add(resourcePath, sprite);
            return sprite;
        }

        private Sprite LoadCenteredTextureSprite(string resourcePath, string spriteName)
        {
            if (string.IsNullOrWhiteSpace(resourcePath))
            {
                return null;
            }

            var cacheKey = resourcePath + "#center-pivot";
            if (_spriteResourceCache.TryGetValue(cacheKey, out var cached))
            {
                return cached;
            }

            var texture = Resources.Load<Texture2D>(resourcePath);
            if (texture == null)
            {
                return null;
            }

            var sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                100f,
                0,
                SpriteMeshType.FullRect);
            sprite.name = spriteName;
            _spriteResourceCache.Add(cacheKey, sprite);
            return sprite;
        }

        private Sprite LoadTopPivotTextureSprite(string resourcePath, string spriteName)
        {
            if (string.IsNullOrWhiteSpace(resourcePath))
            {
                return null;
            }

            var cacheKey = resourcePath + "#top-pivot";
            if (_spriteResourceCache.TryGetValue(cacheKey, out var cached))
            {
                return cached;
            }

            var texture = Resources.Load<Texture2D>(resourcePath);
            if (texture == null)
            {
                return null;
            }

            var sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 1f),
                100f,
                0,
                // The imported texture already has alpha. A tight mesh prevents
                // transparent canvas pixels from being submitted by the renderer,
                // which was the source of the black rectangular beam in-game.
                SpriteMeshType.Tight);
            sprite.name = spriteName;
            _spriteResourceCache.Add(cacheKey, sprite);
            return sprite;
        }

        private Sprite GetTopPivotSolidSprite()
        {
            if (_topPivotSolidSprite != null)
            {
                return _topPivotSolidSprite;
            }

            _topPivotSolidSprite = Sprite.Create(
                Texture2D.whiteTexture,
                new Rect(0f, 0f, 1f, 1f),
                new Vector2(0.5f, 1f),
                1f,
                0,
                SpriteMeshType.FullRect);
            _topPivotSolidSprite.name = "Support Drop Beam Core";
            return _topPivotSolidSprite;
        }

        private void BuildSprites()
        {
            _boxSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            _circleSprite = CreateCircleSprite(64);
        }

        private static Sprite CreateCircleSprite(int size)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            var pixels = new Color[size * size];
            var center = (size - 1) * 0.5f;
            var radius = center - 1f;

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var dx = x - center;
                    var dy = y - center;
                    var distance = Mathf.Sqrt(dx * dx + dy * dy);
                    var alpha = Mathf.Clamp01(radius + 0.5f - distance);
                    pixels[y * size + x] = new Color(1f, 1f, 1f, alpha);
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();
            return Sprite.Create(texture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f), size);
        }
    }
}

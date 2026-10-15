using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.Prototype
{
    public static class FairShotPrototypeBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreatePrototypeOnSceneStart()
        {
            if (SceneManager.GetActiveScene().name != "SampleScene")
            {
                return;
            }

            if (Object.FindAnyObjectByType<FairShotBattleController>() != null)
            {
                return;
            }

            var root = new GameObject("Fair Shot Prototype");
            root.AddComponent<FairShotBattleController>();
        }
    }

    public sealed class FairShotBattleController : MonoBehaviour
    {
        private const int MaxHealth = 100;
        private const float MinShotPower = 8f;
        private const float MaxShotPower = 20f;

        private readonly Color _blue = new(0.18f, 0.56f, 0.96f);
        private readonly Color _red = new(0.95f, 0.28f, 0.22f);

        private Sprite _boxSprite;
        private Sprite _circleSprite;
        private PrototypeUnit _blueUnit;
        private PrototypeUnit _redUnit;
        private PrototypeUnit _activeUnit;
        private Camera _camera;
        private LineRenderer _aimLine;
        private LineRenderer _trajectoryLine;
        private Vector2 _aimDirection = new(1f, 0.45f);
        private float _shotPower = 14f;
        private bool _projectileInFlight;
        private bool _battleEnded;
        private string _status = "Aim, set power, fire.";

        private void Awake()
        {
            Physics2D.gravity = new Vector2(0f, -9.8f);
            BuildPrototypeSprites();
            BuildScene();
            StartBattle();
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            {
                RestartBattle();
                return;
            }

            if (!_battleEnded && !_projectileInFlight && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                FireActiveWeapon();
                return;
            }

            if (_battleEnded || _projectileInFlight || _activeUnit == null)
            {
                return;
            }

            UpdateAimFromPointer();
            DrawAim();
            DrawTrajectoryPreview();
        }

        public void ResolveProjectileHit(Vector2 impactPoint, PrototypeUnit owner)
        {
            StartCoroutine(ResolveImpactRoutine(impactPoint, owner));
        }

        private IEnumerator ResolveImpactRoutine(Vector2 impactPoint, PrototypeUnit owner)
        {
            SpawnImpactFlash(impactPoint);
            ApplyExplosionDamage(impactPoint, owner);

            yield return new WaitForSeconds(0.45f);

            _projectileInFlight = false;

            if (TryFinishBattle())
            {
                yield break;
            }

            SwitchTurn();
        }

        private void BuildScene()
        {
            BuildCamera();
            BuildWorld();
            BuildUnits();
            BuildAimRenderers();
        }

        private void BuildCamera()
        {
            _camera = Camera.main;
            if (_camera == null)
            {
                var cameraObject = new GameObject("Main Camera");
                _camera = cameraObject.AddComponent<Camera>();
                cameraObject.tag = "MainCamera";
                cameraObject.AddComponent<AudioListener>();
            }

            _camera.orthographic = true;
            _camera.orthographicSize = 5.6f;
            _camera.transform.position = new Vector3(0f, 1.2f, -10f);
            _camera.backgroundColor = new Color(0.54f, 0.78f, 0.95f);
        }

        private void BuildWorld()
        {
            CreateBox("Sky Accent", new Vector2(0f, 1.4f), new Vector2(18f, 9f), new Color(0.64f, 0.83f, 0.96f), false, -20);
            CreateBox("Ground", new Vector2(0f, -2.45f), new Vector2(15f, 1.1f), new Color(0.31f, 0.62f, 0.34f), true);
            CreateBox("Left Hill", new Vector2(-4.4f, -1.85f), new Vector2(3.2f, 1.2f), new Color(0.37f, 0.67f, 0.37f), true);
            CreateBox("Right Hill", new Vector2(4.2f, -1.95f), new Vector2(3.4f, 1f), new Color(0.37f, 0.67f, 0.37f), true);
            CreateBox("Center Cover", new Vector2(0f, -1.55f), new Vector2(1.2f, 1.25f), new Color(0.45f, 0.36f, 0.26f), true);
        }

        private void BuildUnits()
        {
            _blueUnit = CreateUnit("Blue Striker", new Vector2(-4.4f, -0.85f), 0, _blue);
            _redUnit = CreateUnit("Red Striker", new Vector2(4.2f, -0.95f), 1, _red);
        }

        private void BuildAimRenderers()
        {
            _aimLine = CreateLine("Aim Line", 0.08f, Color.white);
            _trajectoryLine = CreateLine("Trajectory Preview", 0.035f, new Color(1f, 1f, 1f, 0.55f));
        }

        private void StartBattle()
        {
            _blueUnit.ResetHealth(MaxHealth);
            _redUnit.ResetHealth(MaxHealth);
            _activeUnit = _blueUnit;
            _shotPower = 14f;
            _aimDirection = Vector2.right;
            _projectileInFlight = false;
            _battleEnded = false;
            _status = "Blue turn. Aim with mouse/touch, then press Fire.";
        }

        private void UpdateAimFromPointer()
        {
            if (!TryGetPointerWorldPosition(out var pointerWorld))
            {
                return;
            }

            var origin = (Vector2)_activeUnit.FirePoint.position;
            var toPointer = pointerWorld - origin;
            if (toPointer.sqrMagnitude < 0.04f)
            {
                return;
            }

            _aimDirection = toPointer.normalized;
            _shotPower = Mathf.Clamp(toPointer.magnitude * 2.15f, MinShotPower, MaxShotPower);
        }

        private bool TryGetPointerWorldPosition(out Vector2 pointerWorld)
        {
            pointerWorld = default;

            if (_camera == null)
            {
                return false;
            }

            var hasPointer = false;
            var screenPosition = Vector2.zero;

            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                screenPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                hasPointer = true;
            }
            else if (Mouse.current != null)
            {
                screenPosition = Mouse.current.position.ReadValue();
                hasPointer = true;
            }

            if (!hasPointer)
            {
                return false;
            }

            pointerWorld = _camera.ScreenToWorldPoint(screenPosition);
            return true;
        }

        private void FireActiveWeapon()
        {
            if (_battleEnded || _projectileInFlight || _activeUnit == null)
            {
                return;
            }

            _projectileInFlight = true;
            _status = $"{_activeUnit.DisplayName} fired.";
            _aimLine.enabled = false;
            _trajectoryLine.enabled = false;

            var projectileObject = new GameObject("Prototype Shell");
            projectileObject.name = "Prototype Shell";
            projectileObject.transform.position = _activeUnit.FirePoint.position + (Vector3)(_aimDirection * 0.45f) + new Vector3(0f, 0f, -0.2f);
            projectileObject.transform.localScale = Vector3.one * 0.26f;

            var renderer = projectileObject.AddComponent<SpriteRenderer>();
            renderer.sprite = _circleSprite;
            renderer.color = new Color(0.08f, 0.08f, 0.08f);
            renderer.sortingOrder = 30;

            var collider = projectileObject.AddComponent<CircleCollider2D>();
            collider.radius = 0.5f;

            var body = projectileObject.AddComponent<Rigidbody2D>();
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            body.linearVelocity = _aimDirection * _shotPower;

            var projectile = projectileObject.AddComponent<PrototypeProjectile>();
            projectile.Initialize(this, _activeUnit, collider);
        }

        private void ApplyExplosionDamage(Vector2 impactPoint, PrototypeUnit owner)
        {
            const float radius = 1.65f;
            const int maxDamage = 42;

            ApplyDamageToUnit(_blueUnit, owner, impactPoint, radius, maxDamage);
            ApplyDamageToUnit(_redUnit, owner, impactPoint, radius, maxDamage);
        }

        private void ApplyDamageToUnit(PrototypeUnit unit, PrototypeUnit owner, Vector2 impactPoint, float radius, int maxDamage)
        {
            if (unit == null || unit.IsDefeated)
            {
                return;
            }

            var distance = Vector2.Distance(impactPoint, unit.transform.position);
            if (distance > radius)
            {
                return;
            }

            var damageScale = Mathf.InverseLerp(radius, 0f, distance);
            var damage = Mathf.CeilToInt(maxDamage * damageScale);
            if (unit == owner)
            {
                damage = Mathf.CeilToInt(damage * 0.65f);
            }

            unit.TakeDamage(damage);
        }

        private bool TryFinishBattle()
        {
            if (_blueUnit.IsDefeated && _redUnit.IsDefeated)
            {
                EndBattle("Draw. Both units are down.");
                return true;
            }

            if (_blueUnit.IsDefeated)
            {
                EndBattle("Red wins.");
                return true;
            }

            if (_redUnit.IsDefeated)
            {
                EndBattle("Blue wins.");
                return true;
            }

            return false;
        }

        private void EndBattle(string result)
        {
            _battleEnded = true;
            _projectileInFlight = false;
            _status = $"{result} Press Restart to play again.";
            _aimLine.enabled = false;
            _trajectoryLine.enabled = false;
        }

        private void SwitchTurn()
        {
            _activeUnit = _activeUnit == _blueUnit ? _redUnit : _blueUnit;
            var defaultDirection = _activeUnit == _blueUnit ? Vector2.right : Vector2.left;
            _aimDirection = new Vector2(defaultDirection.x, 0.45f).normalized;
            _shotPower = 14f;
            _status = $"{_activeUnit.DisplayName} turn.";
        }

        private void RestartBattle()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void DrawAim()
        {
            if (_aimLine == null || _activeUnit == null)
            {
                return;
            }

            var origin = _activeUnit.FirePoint.position;
            var end = origin + (Vector3)(_aimDirection * Mathf.Lerp(1.2f, 2.5f, Mathf.InverseLerp(MinShotPower, MaxShotPower, _shotPower)));

            _aimLine.enabled = true;
            _aimLine.SetPosition(0, origin);
            _aimLine.SetPosition(1, end);
        }

        private void DrawTrajectoryPreview()
        {
            if (_trajectoryLine == null || _activeUnit == null)
            {
                return;
            }

            const int steps = 24;
            const float stepTime = 0.085f;

            _trajectoryLine.enabled = true;
            _trajectoryLine.positionCount = steps;

            var start = (Vector2)_activeUnit.FirePoint.position + (_aimDirection * 0.45f);
            var velocity = _aimDirection * _shotPower;

            for (var i = 0; i < steps; i++)
            {
                var t = i * stepTime;
                var position = start + velocity * t + 0.5f * Physics2D.gravity * (t * t);
                _trajectoryLine.SetPosition(i, position);
            }
        }

        private void SpawnImpactFlash(Vector2 position)
        {
            var flash = new GameObject("Impact Flash");
            flash.name = "Impact Flash";
            flash.transform.position = new Vector3(position.x, position.y, -0.2f);
            flash.transform.localScale = Vector3.one * 1.2f;

            var renderer = flash.AddComponent<SpriteRenderer>();
            renderer.sprite = _circleSprite;
            renderer.color = new Color(1f, 0.74f, 0.18f, 0.85f);
            renderer.sortingOrder = 40;

            Destroy(flash, 0.25f);
        }

        private PrototypeUnit CreateUnit(string displayName, Vector2 position, int teamIndex, Color color)
        {
            var unitObject = new GameObject(displayName);
            unitObject.name = displayName;
            unitObject.transform.position = new Vector3(position.x, position.y, -0.1f);
            unitObject.transform.localScale = new Vector3(0.7f, 0.95f, 0.7f);

            var renderer = unitObject.AddComponent<SpriteRenderer>();
            renderer.sprite = _circleSprite;
            renderer.color = color;
            renderer.sortingOrder = 20;

            var collider = unitObject.AddComponent<CapsuleCollider2D>();
            collider.size = new Vector2(0.7f, 1f);

            var body = unitObject.AddComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Kinematic;

            var unit = unitObject.AddComponent<PrototypeUnit>();
            unit.Initialize(displayName, teamIndex, color);
            return unit;
        }

        private void CreateBox(string objectName, Vector2 position, Vector2 size, Color color, bool solid, int sortingOrder = 0)
        {
            var box = new GameObject(objectName);
            box.name = objectName;
            box.transform.position = new Vector3(position.x, position.y, solid ? 0f : 1f);
            box.transform.localScale = new Vector3(size.x, size.y, 1f);

            var renderer = box.AddComponent<SpriteRenderer>();
            renderer.sprite = _boxSprite;
            renderer.color = color;
            renderer.sortingOrder = sortingOrder;

            if (solid)
            {
                var collider = box.AddComponent<BoxCollider2D>();
                collider.size = Vector2.one;
            }
        }

        private LineRenderer CreateLine(string objectName, float width, Color color)
        {
            var lineObject = new GameObject(objectName);
            var line = lineObject.AddComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.startColor = color;
            line.endColor = color;
            line.startWidth = width;
            line.endWidth = width;
            line.positionCount = 2;
            line.useWorldSpace = true;
            line.sortingOrder = 50;
            line.enabled = false;
            return line;
        }

        private void BuildPrototypeSprites()
        {
            _boxSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            _circleSprite = CreateCircleSprite(64);
        }

        private static Sprite CreateCircleSprite(int size)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;

            var center = (size - 1) * 0.5f;
            var radius = center - 1f;
            var pixels = new Color[size * size];

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

        private void OnGUI()
        {
            const int padding = 18;
            const int panelWidth = 330;
            const int buttonHeight = 42;

            GUI.Box(new Rect(padding, padding, panelWidth, 178), "Fair Shot Prototype");
            GUI.Label(new Rect(padding + 14, padding + 28, panelWidth - 28, 24), _status);
            GUI.Label(new Rect(padding + 14, padding + 54, panelWidth - 28, 24), $"Blue HP: {_blueUnit.Health}   Red HP: {_redUnit.Health}");
            GUI.Label(new Rect(padding + 14, padding + 80, panelWidth - 28, 24), $"Power: {_shotPower:0.0}");
            GUI.Label(new Rect(padding + 14, padding + 106, panelWidth - 28, 22), "Aim with mouse or touch.");
            GUI.Label(new Rect(padding + 14, padding + 128, panelWidth - 28, 22), "Fire button or Space. Restart or R.");

            GUI.enabled = !_battleEnded && !_projectileInFlight;
            if (GUI.Button(new Rect(padding + 14, padding + 156, 146, buttonHeight), "Fire"))
            {
                FireActiveWeapon();
            }

            GUI.enabled = true;
            if (GUI.Button(new Rect(padding + 170, padding + 156, 146, buttonHeight), "Restart"))
            {
                RestartBattle();
            }
        }
    }

    public sealed class PrototypeUnit : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private Color _baseColor;

        public string DisplayName { get; private set; }
        public int TeamIndex { get; private set; }
        public int Health { get; private set; }
        public bool IsDefeated => Health <= 0;
        public Transform FirePoint { get; private set; }

        public void Initialize(string displayName, int teamIndex, Color color)
        {
            DisplayName = displayName;
            TeamIndex = teamIndex;
            _baseColor = color;
            _renderer = GetComponent<SpriteRenderer>();

            FirePoint = new GameObject("Fire Point").transform;
            FirePoint.SetParent(transform);
            FirePoint.localPosition = new Vector3(teamIndex == 0 ? 0.45f : -0.45f, 0.22f, 0f);
        }

        public void ResetHealth(int health)
        {
            Health = health;
            SetBodyColor(_baseColor);
            transform.localScale = new Vector3(0.7f, 0.95f, 0.7f);
        }

        public void TakeDamage(int amount)
        {
            Health = Mathf.Max(0, Health - Mathf.Max(0, amount));

            if (IsDefeated)
            {
                SetBodyColor(Color.gray);
                transform.localScale = new Vector3(0.85f, 0.35f, 0.7f);
            }
            else
            {
                StartCoroutine(DamageFlash());
            }
        }

        private IEnumerator DamageFlash()
        {
            SetBodyColor(Color.white);
            yield return new WaitForSeconds(0.12f);
            SetBodyColor(_baseColor);
        }

        private void SetBodyColor(Color color)
        {
            if (_renderer != null)
            {
                _renderer.color = color;
            }
        }
    }

    public sealed class PrototypeProjectile : MonoBehaviour
    {
        private FairShotBattleController _battle;
        private PrototypeUnit _owner;
        private bool _resolved;

        public void Initialize(FairShotBattleController battle, PrototypeUnit owner, Collider2D projectileCollider)
        {
            _battle = battle;
            _owner = owner;

            var ownerCollider = owner.GetComponent<Collider2D>();
            if (ownerCollider != null && projectileCollider != null)
            {
                Physics2D.IgnoreCollision(projectileCollider, ownerCollider, true);
            }

            Destroy(gameObject, 8f);
        }

        private void Update()
        {
            if (transform.position.y < -8f && !_resolved)
            {
                Resolve(transform.position);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_resolved)
            {
                return;
            }

            var contactPoint = collision.contactCount > 0 ? collision.GetContact(0).point : (Vector2)transform.position;
            Resolve(contactPoint);
        }

        private void Resolve(Vector2 impactPoint)
        {
            _resolved = true;
            _battle.ResolveProjectileHit(impactPoint, _owner);
            Destroy(gameObject);
        }
    }
}

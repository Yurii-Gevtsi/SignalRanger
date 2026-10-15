using UnityEngine;

namespace Game.Gameplay.Battle.AI
{
    /// <summary>
    /// All the knobs that turn one bot brain into an easy, medium or hard
    /// opponent. Kept data-driven so difficulty is content, not branches in the
    /// planning code. Movement fields are consumed from Stage 3 onward.
    /// </summary>
    [CreateAssetMenu(menuName = "Game/Battle/Bot Profile", fileName = "BotProfile")]
    public sealed class BotProfile : ScriptableObject
    {
        [Header("Aiming")]
        [SerializeField] private int _powerSteps = 8;
        [SerializeField] private float _angleStepDegrees = 2f;
        [SerializeField] private float _scatterBase = 0.45f;
        [SerializeField] private float _scatterPerDistance = 0.05f;
        [SerializeField, Range(0f, 1f)] private float _perfectShotChance = 0.05f;
        [SerializeField] private float _scoreJitter = 0.5f;

        [Header("Weapon choice")]
        [SerializeField] private bool _evaluateAllWeapons;

        [Header("Movement (Stage 3+)")]
        [SerializeField] private bool _movementEnabled;
        [SerializeField] private bool _jumpEnabled;
        [SerializeField] private float _minEnemySeparation = 2f;
        [SerializeField] private float _maxWalkBudgetSeconds = 1.5f;
        [SerializeField] private int _maxPositionCandidates = 8;

        [Header("Tempo (readability)")]
        [SerializeField] private float _thinkPauseSeconds = 0.6f;
        [SerializeField] private float _aimPauseSeconds = 0.35f;

        public int PowerSteps => Mathf.Max(1, _powerSteps);
        public float AngleStepDegrees => Mathf.Max(0.1f, _angleStepDegrees);
        public float ScatterBase => Mathf.Max(0f, _scatterBase);
        public float ScatterPerDistance => Mathf.Max(0f, _scatterPerDistance);
        public float PerfectShotChance => Mathf.Clamp01(_perfectShotChance);
        public float ScoreJitter => Mathf.Max(0f, _scoreJitter);

        public bool EvaluateAllWeapons => _evaluateAllWeapons;

        public bool MovementEnabled => _movementEnabled;
        public bool JumpEnabled => _jumpEnabled;
        public float MinEnemySeparation => Mathf.Max(0f, _minEnemySeparation);
        public float MaxWalkBudgetSeconds => Mathf.Max(0f, _maxWalkBudgetSeconds);
        public int MaxPositionCandidates => Mathf.Max(1, _maxPositionCandidates);

        public float ThinkPauseSeconds => Mathf.Max(0f, _thinkPauseSeconds);
        public float AimPauseSeconds => Mathf.Max(0f, _aimPauseSeconds);

        public static BotProfile CreateRuntime(
            int powerSteps,
            float angleStepDegrees,
            float scatterBase,
            float scatterPerDistance,
            float perfectShotChance,
            float scoreJitter,
            bool evaluateAllWeapons,
            bool movementEnabled,
            bool jumpEnabled,
            float minEnemySeparation,
            float maxWalkBudgetSeconds,
            int maxPositionCandidates,
            float thinkPauseSeconds,
            float aimPauseSeconds)
        {
            var profile = CreateInstance<BotProfile>();
            profile._powerSteps = powerSteps;
            profile._angleStepDegrees = angleStepDegrees;
            profile._scatterBase = scatterBase;
            profile._scatterPerDistance = scatterPerDistance;
            profile._perfectShotChance = perfectShotChance;
            profile._scoreJitter = scoreJitter;
            profile._evaluateAllWeapons = evaluateAllWeapons;
            profile._movementEnabled = movementEnabled;
            profile._jumpEnabled = jumpEnabled;
            profile._minEnemySeparation = minEnemySeparation;
            profile._maxWalkBudgetSeconds = maxWalkBudgetSeconds;
            profile._maxPositionCandidates = maxPositionCandidates;
            profile._thinkPauseSeconds = thinkPauseSeconds;
            profile._aimPauseSeconds = aimPauseSeconds;
            return profile;
        }
    }
}

#nullable enable

namespace VampireSurvival.Core.Stats
{
    using UnityEngine;
    using VampireSurvival.Core.Entities;

    [CreateAssetMenu(menuName = "VampireSurvival/Stats/Enemy Stats")]

    public sealed class EnemyStatsConfig : ScriptableObject
    {
        public Enemy Prefab        = null!;
        public float SpawnInterval = 1f;
        public float SpawnRadius   = 0.2f;
    }
}
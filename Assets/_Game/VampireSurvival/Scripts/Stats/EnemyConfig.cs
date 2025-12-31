#nullable enable

namespace VampireSurvival.Core.Stats
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "VampireSurvival/Enemy Config")]
    public sealed class EnemyConfig : ScriptableObject
    {
        public float spawnInterval = 1f;
        public float spawnRadius   = 10f;
        public int   xpReward      = 10;
    }
}
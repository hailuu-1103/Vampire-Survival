#nullable enable
using UnityEngine;

namespace VampireSurvival.Core.Stats
{
    [CreateAssetMenu(menuName = "VampireSurvival/Stats/Character Stats Config")]
    public sealed class CharacterStatsConfig : ScriptableObject
    {
        public float maxHealth = 100f;

        public float attack         = 10f;
        public float attackRange    = 0.2f;
        public float attackCooldown = 1f;

        public float moveSpeed = 5f;
    }
}
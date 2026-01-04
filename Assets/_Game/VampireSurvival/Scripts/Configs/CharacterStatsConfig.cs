#nullable enable

namespace VampireSurvival.Configs
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "VampireSurvival/Stats/Character Stats Config")]
    public sealed class CharacterStatsConfig : ScriptableObject
    {
        public float maxHealth = 100f;
        public float attack    = 10f;
        public float moveSpeed = 5f;
    }
}
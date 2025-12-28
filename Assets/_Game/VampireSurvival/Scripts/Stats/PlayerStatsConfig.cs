#nullable enable
using UnityEngine;

namespace VampireSurvival.Core
{
    [CreateAssetMenu(menuName = "VampireSurvival/Player/Stats Config")]
    public sealed class PlayerStatsConfig : ScriptableObject
    {
        public float MoveSpeed = 5f;

        public float Damage = 10f;

        public float MaxHealth = 100f;
    }
}
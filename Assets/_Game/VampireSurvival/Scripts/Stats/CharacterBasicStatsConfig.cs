#nullable enable
using UnityEngine;

namespace VampireSurvival.Core.Stats
{
    [CreateAssetMenu(menuName = "VampireSurvival/Stats/Character Basic Stats")]
    public sealed class CharacterBasicStatsConfig : ScriptableObject
    {
        public float MoveSpeed = 3f;
        public float Damage    = 5f;
        public float MaxHealth = 10f;
    }
}
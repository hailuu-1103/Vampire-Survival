#nullable enable

namespace VampireSurvival.Configs
{
    using UnityEngine;
    using VampireSurvival.Components;

    [CreateAssetMenu(menuName = "VampireSurvival/Enemy Config")]
    public sealed class EnemyConfig : ScriptableObject
    {
        [SerializeField] private Weapon[] initialWeapons = null!;
        public                   Weapon[] InitialWeapons => this.initialWeapons;
        public                   float    spawnInterval = 1f;
        public                   float    spawnRadius   = 10f;
        public                   int      xpReward      = 10;
    }
}
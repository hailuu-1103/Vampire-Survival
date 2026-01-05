#nullable enable

namespace VampireSurvival.Configs
{
    using UnityEngine;
    using VampireSurvival.Entities;

    public enum AttackStrategy
    {
        Homing,
        Directional,
    }

    [CreateAssetMenu(menuName = "VampireSurvival/Weapons/Ranged Weapon Config")]
    public sealed class RangedWeaponConfig : WeaponConfig
    {
        public Projectile     projectilePrefab = null!;
        public AttackStrategy attackStrategy;
        public float          speed = 8f;
    }
}
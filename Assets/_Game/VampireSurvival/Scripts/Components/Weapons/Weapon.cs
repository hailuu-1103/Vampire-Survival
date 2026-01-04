#nullable enable

namespace VampireSurvival.Components
{
    using Component = global::Core.Entities.Component;
    using UnityEngine;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Configs;
    using VampireSurvival.Models;

    public sealed class Weapon : Component, IWeapon
    {
        [SerializeField] private WeaponConfig config = null!;

        private IAttacker    owner = null!;
        private IStatsHolder? stats;
        private int          level = 1;

        protected override void OnInstantiate()
        {
            this.owner = this.GetComponentInParent<IAttacker>();
            this.stats = this.GetComponent<IStatsHolder>();
        }

        IAttacker IWeapon.     Owner    => this.owner;
        WeaponConfig IWeapon.  Config   => this.config;
        IStatsHolder? IWeapon. Stats    => this.stats;
        int IWeapon.           Level    => this.level;
        int IWeapon.           MaxLevel => this.config.maxLevel;

        void IWeapon.Upgrade()
        {
            if (this.level >= this.config.maxLevel) return;
            if (this.stats == null) return;

            this.level++;
            this.stats.Add(WeaponStatNames.DAMAGE, this.config.damagePerLevel);
            this.stats.Add(WeaponStatNames.COOLDOWN, -this.config.cooldownPerLevel);
        }
    }
}

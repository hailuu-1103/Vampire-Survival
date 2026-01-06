#nullable enable

namespace Game.Services
{
    using System;
    using System.Linq;
    using Core.Entities;
    using Core.Lifecycle;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Progression.Abstractions;

    public sealed class LevelUpService : ILateLoadable, IDisposable
    {
        private readonly IPlayerProgressionService progressionService;
        private readonly IEntityManager            entityManager;
        private readonly Random                    random = new();

        public LevelUpService(
            IPlayerProgressionService progressionService,
            IEntityManager            entityManager
        )
        {
            this.progressionService = progressionService;
            this.entityManager      = entityManager;
        }

        void ILateLoadable.OnLateLoad()
        {
            this.progressionService.LeveledUp -= this.OnLeveledUp;
            this.progressionService.LeveledUp += this.OnLeveledUp;
        }

        private void OnLeveledUp(int level)
        {
            this.UpgradeRandomWeapon();
        }

        private void UpgradeRandomWeapon()
        {
            var player = this.entityManager.Query<IPlayer>().SingleOrDefault();
            if (player is null) return;

            var upgradableWeapons = player.Weapons.Where(w => !w.IsMaxLevel).ToArray();
            if (upgradableWeapons.Length == 0) return;

            var index  = this.random.Next(upgradableWeapons.Length);
            var weapon = upgradableWeapons[index];
            weapon.Upgrade();
        }

        void IDisposable.Dispose()
        {
            this.progressionService.LeveledUp -= this.OnLeveledUp;
        }
    }
}
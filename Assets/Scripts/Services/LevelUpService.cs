#nullable enable

namespace Game.Services
{
    using System;
    using System.Linq;
    using Core.Lifecycle;
    using UnityEngine;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Services;
    using Random = System.Random;

    public sealed class LevelUpService : ILateLoadable, IDisposable
    {
        private readonly IPlayerProgressionService progressionService;
        private readonly IPlayerInventory          playerInventory;
        private readonly Random                    random = new();

        public LevelUpService(
            IPlayerProgressionService progressionService,
            IPlayerInventory          playerInventory
        )
        {
            this.progressionService = progressionService;
            this.playerInventory    = playerInventory;
        }

        void ILateLoadable.OnLateLoad()
        {
            this.progressionService.LeveledUp += this.OnLeveledUp;
        }

        private void OnLeveledUp(int level)
        {
            var weapon = this.UpgradeRandomWeapon();
            Debug.Log($"Player leveled up {level}, weapon: {weapon.Config.name}");
        }

        private IWeapon? UpgradeRandomWeapon()
        {
            var upgradableWeapons = this.playerInventory.Weapons
                .Where(w => !w.IsMaxLevel)
                .ToArray();

            if (upgradableWeapons.Length == 0) return null;

            var index  = this.random.Next(upgradableWeapons.Length);
            var weapon = upgradableWeapons[index];
            if (!this.playerInventory.UpgradeWeapon(weapon)) return null;
            return weapon;
        }

        void IDisposable.Dispose()
        {
            this.progressionService.LeveledUp -= this.OnLeveledUp;
        }
    }
}
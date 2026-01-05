#nullable enable

namespace Game.Services
{
    using System;
    using System.Linq;
    using Core.Lifecycle;
    using UnityEngine;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Progression.Abstractions;

    public sealed class LevelUpService : ILateLoadable, IDisposable
    {
        private readonly IPlayerProgressionService progressionService;

        public LevelUpService(
            IPlayerProgressionService progressionService
        )
        {
            this.progressionService = progressionService;
        }

        void ILateLoadable.OnLateLoad()
        {
            this.progressionService.LeveledUp += this.OnLeveledUp;
        }

        private void OnLeveledUp(int level)
        {
            this.UpgradeRandomWeapon();
            Debug.Log($"Player leveled up {level}, weapon:");
        }

        private void UpgradeRandomWeapon()
        {
            // var upgradableWeapons = this.playerInventory.Weapons
            //     .Where(w => !w.IsMaxLevel)
            //     .ToArray();
            //
            // if (upgradableWeapons.Length == 0) return null;
            //
            // var index  = this.random.Next(upgradableWeapons.Length);
            // var weapon = upgradableWeapons[index];
            // if (!this.playerInventory.UpgradeWeapon(weapon)) return null;
            // return weapon;
        }

        void IDisposable.Dispose()
        {
            this.progressionService.LeveledUp -= this.OnLeveledUp;
        }
    }
}
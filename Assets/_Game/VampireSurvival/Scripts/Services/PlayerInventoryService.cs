#nullable enable

using Core.Entities;
using Core.Lifecycle;

namespace VampireSurvival.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Configs;
    using Object = UnityEngine.Object;

    public interface IPlayerInventory
    {
        public IReadOnlyList<IWeapon> Weapons { get; }
        public void                   AddStartingWeapons();

        public IWeapon? AddWeapon(GameObject  prefab);
        public bool     UpgradeWeapon(IWeapon weapon);
    }

    public sealed class PlayerInventoryService : IPlayerInventory, IDisposable
    {
        private readonly IEntityManager manager;
        private readonly PlayerConfig   config;
        private readonly List<IWeapon>  weapons = new();

        private IPlayer? player;

        public PlayerInventoryService(IEntityManager manager, PlayerConfig config)
        {
            this.manager = manager;
            this.config  = config;
        }

        void IDisposable.Dispose()
        {
            this.weapons.Clear();
            this.player = null;
        }

        void IPlayerInventory.AddStartingWeapons()
        {
            foreach (var weaponPrefab in this.config.StartingWeapons)
            {
                ((IPlayerInventory)this).AddWeapon(weaponPrefab);
            }
        }

        IReadOnlyList<IWeapon> IPlayerInventory.Weapons => this.weapons;

        IWeapon? IPlayerInventory.AddWeapon(GameObject prefab)
        {
            this.player ??= this.manager.Query<IPlayer>().Single();
            var instance   = Object.Instantiate(prefab, this.player.transform);
            var weapon     = instance.GetComponent<IWeapon>();
            var components = instance.GetComponentsInChildren<IComponent>();

            foreach (var component in components)
            {
                this.manager.RegisterComponent(this.player, component);
            }

            this.weapons.Add(weapon);
            return weapon;
        }

        bool IPlayerInventory.UpgradeWeapon(IWeapon weapon)
        {
            if (weapon.IsMaxLevel) return false;

            weapon.Upgrade();
            return true;
        }
    }
}
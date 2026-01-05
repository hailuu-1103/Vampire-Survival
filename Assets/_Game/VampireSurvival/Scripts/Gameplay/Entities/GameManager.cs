#nullable enable

namespace VampireSurvival.Entities
{
    using System;
    using global::Core.Utils;
    using Entity = global::Core.Entities.Entity;
    using IUpdateable = global::Core.Entities.IUpdateable;
    using IPauseable = global::Core.Entities.IPauseable;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Components;

    public sealed class GameManager : Entity
    {
        [SerializeField] private Player playerPrefab = null!;

        public  Action?                    onLoaded;
        public  Action?                    onUnloaded;
        public  Action?                    onLost;
        private IPlayer?                   player;
        private IReadOnlyList<IUpdateable> updateables = null!;

        protected override void OnInstantiate()
        {
            this.updateables = this.GetComponentsInChildren<IUpdateable>().ToArray();
            this.Manager.Load(this.playerPrefab);
        }

        private void Update()
        {
            var dt = Time.deltaTime;
            this.updateables.ForEach(system => system.Tick(dt));
        }

        public void Load()
        {
            this.player = this.Manager.Spawn(this.playerPrefab);
            this.onLoaded?.Invoke();
        }

        public void Unload()
        {
            this.ForceClearAllUnits();
            this.onUnloaded?.Invoke();
        }

        public void Pause()
        {
            foreach (var pauseable in this.Manager.Query<IPauseable>()) pauseable.Pause();
        }

        public void Resume()
        {
            foreach (var pauseable in this.Manager.Query<IPauseable>()) pauseable.Resume();
        }

        public void PauseEnemySpawner()
        {
            var enemySpawner = this.Manager.Query<IPauseable>().Single(pause => pause is EnemyManager);
            enemySpawner.Pause();
        }

        public void ResumeEnemySpawner()
        {
            var enemySpawner = this.Manager.Query<IPauseable>().Single(pause => pause is EnemyManager);
            enemySpawner.Resume();
        }

        public void SetPlayerImmortal(bool immortal)
        {
            if (this.player is null) return;
            this.player.SetImmortal(immortal);
        }

        public void ForceClearAllUnits()
        {
            if (this.player != null)
            {
                this.Manager.Recycle(this.player);
                this.player = null;
            }

            foreach (var enemy in this.Manager.Query<IEnemy>().ToArray()) this.Manager.Recycle(enemy);
            foreach (var collectable in this.Manager.Query<ICollectable>().ToArray()) this.Manager.Recycle(collectable);
        }
    }
}
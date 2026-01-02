#nullable enable

namespace VampireSurvival.Core.Entities
{
    using global::Core.Utils;
    using Entity = global::Core.Entities.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Components;

    public sealed class GameManager : Entity
    {
        [SerializeField] private Player playerPrefab = null!;

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
        }

        public void Unload()
        {
            this.ForceClearAllUnits();
        }

        public void Pause()
        {
            foreach (var pauseable in this.Manager.Query<IPauseable>())
                pauseable.Pause();
        }

        public void Resume()
        {
            foreach (var pauseable in this.Manager.Query<IPauseable>())
                pauseable.Resume();
        }

        public void PauseEnemySpawner()
        {
            var enemySpawner = this.Manager.Query<IPauseable>().Single(pause => pause is EnemySpawner);
            enemySpawner.Pause();
        }

        public void ResumeEnemySpawner()
        {
            var enemySpawner = this.Manager.Query<IPauseable>().Single(pause => pause is EnemySpawner);
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

            foreach (var enemy in this.Manager.Query<IEnemy>().ToArray())
                this.Manager.Recycle(enemy);
            foreach (var collectable in this.Manager.Query<ICollectable>().ToArray())
                this.Manager.Recycle(collectable);
        }
    }
}
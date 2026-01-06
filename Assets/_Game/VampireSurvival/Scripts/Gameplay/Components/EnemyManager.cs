#nullable enable

namespace VampireSurvival.Components
{
    using Component   = global::Core.Entities.Component;
    using IEventBus   = global::Core.Observer.IEventBus;
    using IUpdateable = global::Core.Entities.IUpdateable;
    using IPauseable  = global::Core.Entities.IPauseable;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Entities;
    using VampireSurvival.Events;
    using VampireSurvival.Configs;

    public sealed class EnemyManager : Component, IUpdateable
    {
        [SerializeField] private Enemy enemyPrefab = null!;

        private IEventBus   eventBus = null!;
        private EnemyConfig config   = null!;

        protected override void OnInstantiate()
        {
            this.eventBus = this.Container.Resolve<IEventBus>();
            this.config   = this.Container.Resolve<EnemyConfig>();
            this.Manager.Load(this.enemyPrefab, 100);
        }

        protected override void OnCleanup()
        {
            this.Manager.Cleanup(this.enemyPrefab);
        }

        private readonly HashSet<IEnemy> deadEnemies = new();

        private float timer;
        private bool  isPaused;

        protected override void OnSpawn()
        {
            this.eventBus.Subscribe<EnemyDiedEvent>(this.OnEnemyDied);
        }

        private void OnEnemyDied(EnemyDiedEvent obj)
        {
            this.deadEnemies.Add(obj.Enemy);
        }

        public bool IsDead(IEnemy enemy) => this.deadEnemies.Contains(enemy);

        void IPauseable.Pause()  => this.isPaused = true;
        void IPauseable.Resume() => this.isPaused = false;

        void IUpdateable.Tick(float deltaTime)
        {
            if (this.isPaused) return;

            this.timer -= deltaTime;
            if (this.timer > 0f) return;
            this.timer = this.config.spawnInterval;

            var    player = this.Manager.Query<IPlayer>().Single();
            var    center = (Vector2)player.transform.position;
            var    dir    = Random.insideUnitCircle.normalized;
            var    pos    = center + dir * this.config.spawnRadius;
            IEnemy enemy  = this.Manager.Spawn(this.enemyPrefab, pos);
            this.deadEnemies.Remove(enemy);
        }

        protected override void OnRecycle()
        {
            this.eventBus.Unsubscribe<EnemyDiedEvent>(this.OnEnemyDied);
            this.deadEnemies.Clear();
        }
    }
}
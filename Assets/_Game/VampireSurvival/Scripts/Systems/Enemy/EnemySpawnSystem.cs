#nullable enable
using System.Linq;
using Core.Entities;
using UnityEngine;

namespace VampireSurvival.Core.Systems
{
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Stats;

    public sealed class EnemySpawnSystem : IUpdateable
    {
        private readonly IEntityManager entityManager;
        private readonly EnemyConfig    config;

        private float timer;
        private bool  isPaused;

        public EnemySpawnSystem(IEntityManager entityManager, EnemyConfig config)
        {
            this.entityManager = entityManager;
            this.config        = config;
        }

        public void Pause()  => this.isPaused = true;
        public void Resume() => this.isPaused = false;

        public void Tick(float dt)
        {
            if (this.isPaused) return;

            this.timer -= dt;
            if (this.timer > 0f) return;
            this.timer = this.config.SpawnInterval;

            var player = this.entityManager.Query<IPlayer>().FirstOrDefault();
            if (player == null) return;

            var center = (Vector2)player.transform.position;
            var dir    = Random.insideUnitCircle.normalized;
            var pos    = center + dir * this.config.SpawnRadius;
            this.entityManager.Spawn(this.config.Prefab, pos);
        }
    }
}
#nullable enable
using Core.Entities;
using UnityEngine;

namespace VampireSurvival.Core.Systems
{
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Stats;

    public sealed class EnemySpawnSystem : ISystem
    {
        private readonly IEntityManager entityManager;
        private readonly EnemyConfig    config;

        private float        timer;

        public EnemySpawnSystem(IEntityManager entityManager, EnemyConfig config)
        {
            this.entityManager = entityManager;
            this.config   = config;
        }

        public void Tick(float dt)
        {
            this.timer -= dt;
            if (this.timer > 0f) return;
            this.timer = this.config.SpawnInterval;

            IPlayer? player = null;
            foreach (var p in this.entityManager.Query<IPlayer>())
            {
                player = p;
                break;
            }
            if (player == null) return;

            var center = (Vector2)player.transform.position;
            var dir    = Random.insideUnitCircle.normalized;
            var pos    = center + dir * this.config.SpawnRadius;

            this.entityManager.Spawn(this.config.Prefab, pos);
        }
    }
}
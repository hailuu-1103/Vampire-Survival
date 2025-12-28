#nullable enable
using Core.Entities;
using UnityEngine;

namespace VampireSurvival.Core.Systems
{
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Stats;

    public sealed class EnemySpawnSystem
    {
        private readonly IEntityManager entityManager;

        private EnemyConfig? config;
        private float        timer;

        public EnemySpawnSystem(IEntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        public void Bind(EnemyConfig config)
        {
            this.config = config;
            this.timer  = this.config!.SpawnInterval;
        }

        public void Tick(float dt)
        {
            this.timer -= dt;
            if (this.timer > 0f) return;
            this.timer = this.config!.SpawnInterval;

            IPlayer? player = null;
            foreach (var p in this.entityManager.Query<IPlayer>())
            {
                player = p;
                break;
            }
            if (player == null) return;

            var center = (Vector2)player.transform.position;
            var dir    = Random.insideUnitCircle.normalized;
            var pos    = center + dir * this.config.SpawnRadious;

            this.entityManager.Spawn(this.config.Prefab, pos);
        }
    }
}
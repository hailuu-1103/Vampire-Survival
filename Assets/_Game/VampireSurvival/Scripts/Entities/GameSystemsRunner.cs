#nullable enable
using Core.Entities;

namespace VampireSurvival.Core.Entities
{
    using UnityEngine;
    using VampireSurvival.Core.Stats;
    using VampireSurvival.Core.Systems;

    public sealed class GameSystemsRunner : Entity
    {
        [SerializeField] private EnemyStatsConfig enemyStatsConfig = null!;

        private PlayerMovementSystem playerMovementSystem = null!;
        private EnemySpawnSystem     enemySpawnSystem     = null!;
        private EnemyChaseSystem     enemyChaseSystem     = null!;

        protected override void OnInstantiate()
        {
            var container = this.Container;
            this.playerMovementSystem = container.Resolve<PlayerMovementSystem>();
            this.enemySpawnSystem     = container.Resolve<EnemySpawnSystem>();
            this.enemyChaseSystem     = container.Resolve<EnemyChaseSystem>();
        }

        protected override void OnSpawn()
        {
            this.enemySpawnSystem.Bind(this.enemyStatsConfig);
        }

        private void Update()
        {
            this.playerMovementSystem.Tick();
            this.enemySpawnSystem.Tick(Time.deltaTime);
            this.enemyChaseSystem.Tick();
        }
    }
}
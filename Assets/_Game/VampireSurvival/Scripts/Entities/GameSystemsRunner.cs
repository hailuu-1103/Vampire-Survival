#nullable enable
using Core.Entities;

namespace VampireSurvival.Core.Entities
{
    using UnityEngine;
    using VampireSurvival.Core.Components;
    using VampireSurvival.Core.Stats;
    using VampireSurvival.Core.Systems;

    public sealed class GameSystemsRunner : Entity
    {
        [SerializeField] private Player      player      = null!;
        [SerializeField] private EnemyConfig enemyConfig = null!;

        private PlayerMovementSystem playerMovementSystem = null!;
        private EnemySpawnSystem     enemySpawnSystem     = null!;
        private EnemyChaseSystem     enemyChaseSystem     = null!;
        private EnemyAttackSystem    enemyAttackSystem    = null!;

        protected override void OnInstantiate()
        {
            var container = this.Container;
            this.playerMovementSystem = container.Resolve<PlayerMovementSystem>();
            this.enemySpawnSystem     = container.Resolve<EnemySpawnSystem>();
            this.enemyChaseSystem     = container.Resolve<EnemyChaseSystem>();
            this.enemyAttackSystem    = container.Resolve<EnemyAttackSystem>();
        }

        protected override void OnSpawn()
        {
            this.Manager.Spawn(this.player);
            this.enemySpawnSystem.Bind(this.enemyConfig);
        }

        private void Update()
        {
            this.playerMovementSystem.Tick();
            this.enemySpawnSystem.Tick(Time.deltaTime);
            this.enemyChaseSystem.Tick();
            this.enemyAttackSystem.Tick(Time.deltaTime);
        }
    }
}
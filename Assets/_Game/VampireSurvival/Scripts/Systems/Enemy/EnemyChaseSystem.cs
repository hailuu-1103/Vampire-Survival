#nullable enable
using System.Linq;
using Core.Entities;
using UnityEngine;

namespace VampireSurvival.Core.Systems
{
    using VampireSurvival.Core.Abstractions;

    public sealed class EnemyChaseSystem : IUpdateable
    {
        private readonly IEntityManager entityManager;

        private bool isPaused;

        public EnemyChaseSystem(IEntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        public void Pause()  => this.isPaused = true;
        public void Resume() => this.isPaused = false;

        public void Tick(float deltaTime)
        {
            if (this.isPaused) return;

            var player = this.entityManager.Query<IPlayer>().Single();
            var playerPos = (Vector2)player.transform.position;

            foreach (var enemy in this.entityManager.Query<IEnemy>().ToList())
            {
                if (!enemy.Animation.CanMove)
                {
                    enemy.Movement.Move(Vector2.zero);
                    continue;
                }

                var enemyPos = (Vector2)enemy.transform.position;
                var dir = playerPos - enemyPos;
                var moveDir = dir.sqrMagnitude < 0.0001f ? Vector2.zero : dir.normalized;

                enemy.Animation.SetFacing(dir.x);
                enemy.Animation.PlayRunAnimation();
                enemy.Movement.Move(moveDir);
            }
        }
    }
}
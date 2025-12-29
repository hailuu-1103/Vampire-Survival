#nullable enable
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using UnityEngine;

namespace VampireSurvival.Core.Systems
{
    using VampireSurvival.Core.Abstractions;

    public sealed class EnemyChaseSystem : IUpdateable
    {
        private readonly IEntityManager entityManager;
        private readonly HashSet<IEnemy> animatedEnemies = new();

        private bool isPaused;

        public EnemyChaseSystem(IEntityManager entityManager)
        {
            this.entityManager = entityManager;
            this.entityManager.Recycled += this.OnRecycled;
        }



        private void OnRecycled(IEntity entity, IReadOnlyList<IComponent> _)
        {
            if (entity is IEnemy enemy)
                this.animatedEnemies.Remove(enemy);
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
                var enemyPos = (Vector2)enemy.transform.position;
                var dir = playerPos - enemyPos;
                var moveDir = dir.sqrMagnitude < 0.0001f ? Vector2.zero : dir.normalized;

                if (this.animatedEnemies.Add(enemy)) enemy.Animation.PlayRunAnimation();

                enemy.Animation.SetFacing(dir.x);
                enemy.Movement.Move(moveDir);
            }
        }
    }
}
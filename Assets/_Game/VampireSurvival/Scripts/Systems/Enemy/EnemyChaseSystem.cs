#nullable enable
using System.Linq;
using Core.Entities;
using UnityEngine;

namespace VampireSurvival.Core.Systems
{
    using VampireSurvival.Core.Abstractions;

    public sealed class EnemyChaseSystem : ISystem
    {
        private readonly IEntityManager entityManager;

        public EnemyChaseSystem(IEntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        public void Tick(float deltaTime)
        {
            var player = this.entityManager.Query<IPlayer>().FirstOrDefault();
            if (player == null) return;

            var playerPos = (Vector2)player.gameObject.transform.position;

            foreach (var enemy in this.entityManager.Query<IEnemyMoveable>())
            {
                var dir = playerPos - enemy.Position;

                enemy.Move(dir.sqrMagnitude < 0.0001f ? Vector2.zero : dir.normalized);
            }
        }
    }
}
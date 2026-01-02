#nullable enable
using System.Linq;
using UnityEngine;

namespace VampireSurvival.Core.Systems
{
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Components;
    using VampireSurvival.Core.Models;

    public sealed class EnemyChaseSystem : System<IEnemy>
    {
        private EnemySpawner enemySpawner = null!;

        protected override void OnSystemSpawn()
        {
            this.enemySpawner = this.Manager.Query<EnemySpawner>().Single();
        }

        protected override bool Filter(IEnemy enemy)
        {
            var player = this.Manager.Query<IPlayer>().Single();
            return player.StatsHolder.Stats[StatNames.HEALTH] > 0 && !this.enemySpawner.IsDead(enemy);
        }

        protected override void Apply(IEnemy enemy)
        {
            var player = this.Manager.Query<IPlayer>().Single();

            if (!enemy.Animation.CanMove)
            {
                enemy.Rigidbody.linearVelocity = Vector2.zero;
                return;
            }

            var playerPos = (Vector2)player.transform.position;
            var enemyPos  = (Vector2)enemy.transform.position;
            var dir       = playerPos - enemyPos;
            var moveDir   = dir.sqrMagnitude < 0.0001f ? Vector2.zero : dir.normalized;

            var speed = player.StatsHolder.Stats[StatNames.MOVE_SPEED].Value;
            enemy.Rigidbody.linearVelocity = moveDir * speed;
            enemy.Animation.SetFacing(dir.x);
            enemy.Animation.PlayRunAnimation();
        }
    }
}
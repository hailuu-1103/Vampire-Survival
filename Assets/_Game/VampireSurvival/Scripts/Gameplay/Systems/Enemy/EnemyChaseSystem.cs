#nullable enable

using System.Collections.Generic;
using UnityEngine;
using Core.Entities;

namespace VampireSurvival.Systems
{
    using System.Linq;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Components;
    using VampireSurvival.Models;

    public sealed class EnemyChaseSystem : System<IEnemy>
    {
        private EnemyManager enemyManager = null!;

        protected override void OnSystemSpawn()
        {
            this.enemyManager = this.Manager.Query<EnemyManager>().Single();
        }

        protected override bool Filter(IEnemy enemy)
        {
            var player = this.Manager.Query<IPlayer>().Single();
            return player.IsAlive && !this.enemyManager.IsDead(enemy);
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

            var speedStat = enemy.StatsHolder.Stats.GetValueOrDefault(CharacterStatNames.MOVE_SPEED);
            var speed     = speedStat?.Value ?? 0f;
            enemy.Rigidbody.linearVelocity = moveDir * speed;
            enemy.Animation.SetFacing(dir.x);
            enemy.Animation.SetAnimation(AnimationType.Run);
        }
    }
}
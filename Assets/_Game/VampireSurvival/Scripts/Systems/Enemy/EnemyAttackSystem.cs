#nullable enable

namespace VampireSurvival.Core.Systems
{
    using IComponent     = global::Core.Entities.IComponent;
    using IEntity        = global::Core.Entities.IEntity;
    using IEventBus      = global::Core.Observer.IEventBus;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Components;
    using VampireSurvival.Core.Events;
    using VampireSurvival.Core.Models;

    public sealed class EnemyAttackSystem : System<IEnemy>
    {
        private IEventBus    eventBus     = null!;
        private EnemySpawner enemySpawner = null!;

        protected override void OnInstantiate()
        {
            this.eventBus = this.Container.Resolve<IEventBus>();
        }

        protected override void OnSystemSpawn()
        {
            this.enemySpawner = this.Manager.Query<EnemySpawner>().Single();
        }

        private readonly Dictionary<IEnemy, float> cooldowns = new();

        protected override void OnEntitySpawned(IEntity entity, IReadOnlyList<IComponent> components)
        {
            base.OnEntitySpawned(entity, components);
            if (entity is not IEnemy enemy) return;
            this.cooldowns.TryAdd(enemy, 0);
        }

        protected override void OnEntityRecycled(IEntity entity, IReadOnlyList<IComponent> components)
        {
            base.OnEntityRecycled(entity, components);
            if (entity is not IEnemy enemy) return;
            this.cooldowns.Remove(enemy);
        }

        protected override bool Filter(IEnemy enemy)
        {
            var player = this.Manager.Query<IPlayer>().Single();
            return player.StatsHolder.Stats[StatNames.HEALTH] > 0 && !this.enemySpawner.IsDead(enemy) && this.cooldowns.ContainsKey(enemy);
        }

        protected override void Apply(IEnemy enemy)
        {
            var player   = this.Manager.Query<IPlayer>().Single();
            var cooldown = this.cooldowns[enemy];
            cooldown              -= Time.deltaTime;
            this.cooldowns[enemy] =  Mathf.Max(0, cooldown);
            if (cooldown > 0f) return;

            var attackRange = enemy.StatsHolder.Stats[StatNames.ATTACK_RANGE].Value;

            var distance = enemy.Collider.Distance(player.Collider);
            if (!distance.isValid || distance.distance > attackRange) return;

            var dir            = player.transform.position.x - enemy.transform.position.x;
            var isFacingPlayer = (enemy.Animation.FacingDirection > 0 && dir > 0) || (enemy.Animation.FacingDirection < 0 && dir < 0);
            if (!isFacingPlayer) return;

            var damage = player.IsImmortal ? 0 : enemy.StatsHolder.Stats[StatNames.ATTACK].Value;
            player.StatsHolder.Add(StatNames.HEALTH, -damage);

            enemy.Animation.PlayAttackAnimation();
            player.Animation.PlayHitAnimation();

            if (player.StatsHolder.Stats[StatNames.HEALTH].Value <= 0)
            {
                this.eventBus.Publish(new PlayerDiedEvent());
            }

            this.cooldowns[enemy] = enemy.StatsHolder.Stats[StatNames.ATTACK_COOLDOWN];
        }
    }
}
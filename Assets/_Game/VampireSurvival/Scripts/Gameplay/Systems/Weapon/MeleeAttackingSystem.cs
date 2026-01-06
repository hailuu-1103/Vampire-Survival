#nullable enable

using Core.Entities;
using Core.Observer;

namespace VampireSurvival.Systems
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Configs;
    using VampireSurvival.Events;
    using VampireSurvival.Models;

    public sealed class MeleeAttackingSystem : System<IWeapon>, IAttackingSystem
    {
        private readonly Dictionary<IWeapon, float> cooldowns = new();

        private IEventBus eventBus = null!;

        protected override void OnInstantiate()
        {
            this.eventBus = this.Container.Resolve<IEventBus>();
        }

        protected override void OnEntitySpawned(IEntity entity, IReadOnlyList<IComponent> components)
        {
            base.OnEntitySpawned(entity, components);

            if (entity is IWeapon entityWeapon)
                this.ResetCooldown(entityWeapon);

            foreach (var component in components)
            {
                if (component is IWeapon weapon && !ReferenceEquals(component, entity))
                    this.ResetCooldown(weapon);
            }
        }

        protected override void OnEntityRecycled(IEntity entity, IReadOnlyList<IComponent> components)
        {
            base.OnEntityRecycled(entity, components);
            foreach (var component in components)
            {
                if (component is IWeapon weapon) this.cooldowns.Remove(weapon);
            }
        }

        protected override bool Filter(IWeapon weapon)
        {
            if (weapon is not { Config: MeleeWeaponConfig }) return false;
            if (weapon.Owner.StatsHolder.Stats[CharacterStatNames.HEALTH].Value <= 0) return false;

            var target = this.FindTarget(weapon);
            if (target is null) return false;

            var ownerPosition  = (Vector2)weapon.Owner.transform.position;
            var targetPosition = (Vector2)target.transform.position;
            var distance       = Vector2.Distance(ownerPosition, targetPosition);

            return distance <= weapon.Config.range;
        }

        protected override void Apply(IWeapon weapon)
        {
            this.TickCooldown(weapon);
            if (this.GetCooldown(weapon) > 0f) return;

            var target = this.FindTarget(weapon);
            if (target is null) return;

            this.ApplyDamage(weapon, target);
            this.ResetCooldown(weapon);
        }

        private ITarget? FindTarget(IWeapon weapon)
        {
            return weapon.Owner.OwnerType switch
            {
                OwnerType.Player => this.Manager.Query<IEnemy>().FirstOrDefault(e => e.IsAlive),
                OwnerType.Enemy  => this.Manager.Query<IPlayer>().SingleOrDefault(p => p.IsAlive),
                _                => null,
            };
        }

        private float GetCooldown(IWeapon weapon)
        {
            if (!this.cooldowns.ContainsKey(weapon))
                this.ResetCooldown(weapon);

            return this.cooldowns[weapon];
        }

        private void TickCooldown(IWeapon weapon)
        {
            var current                              = this.GetCooldown(weapon);
            if (current > 0f) this.cooldowns[weapon] = Mathf.Max(0f, current - Time.deltaTime);
        }

        private void ResetCooldown(IWeapon weapon)
        {
            var baseCooldown   = weapon.Config.cooldown;
            var weaponCooldown = weapon.Stats?.Stats.GetValueOrDefault(WeaponStatNames.COOLDOWN)?.Value ?? 1f;

            this.cooldowns[weapon] = baseCooldown * weaponCooldown;
        }

        private void ApplyDamage(IWeapon weapon, ITarget target)
        {
            var damage = this.GetDamage(weapon);

            if (target is IImmortalable { IsImmortal: true }) damage = 0f;

            target.StatsHolder.Add(CharacterStatNames.HEALTH, -damage);
            target.PlayHitAnim();

            if (target.StatsHolder.Stats[CharacterStatNames.HEALTH].Value > 0) return;

            switch (target)
            {
                case IPlayer:
                    this.eventBus.Publish(new PlayerDiedEvent());
                    break;
                case IEnemy enemy:
                    this.eventBus.Publish(new EnemyDiedEvent(enemy));
                    break;
            }
        }

        private float GetDamage(IWeapon weapon)
        {
            var baseDamage  = weapon.Config.damage;
            var weaponBonus = weapon.Stats?.Stats.GetValueOrDefault(WeaponStatNames.DAMAGE)?.Value ?? 0f;
            var ownerAttack = weapon.Owner.StatsHolder.Stats.GetValueOrDefault(CharacterStatNames.ATTACK)?.Value ?? 1f;

            return baseDamage + weaponBonus + ownerAttack;
        }
    }
}
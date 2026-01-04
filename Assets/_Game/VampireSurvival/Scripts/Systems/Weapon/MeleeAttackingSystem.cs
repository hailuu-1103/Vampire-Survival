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

    // Temporary used for Enemy only
    public sealed class MeleeAttackingSystem : System<IWeapon>
    {
        private readonly Dictionary<IWeapon, float> cooldowns = new();

        private IEventBus eventBus = null!;

        protected override void OnInstantiate()
        {
            this.eventBus = this.Container.Resolve<IEventBus>();
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
            var player = this.Manager.Query<IPlayer>().SingleOrDefault();
            if (player == null) return false;
            if (!player.IsAlive) return false;

            var ownerPosition  = (Vector2)weapon.Owner.transform.position;
            var playerPosition = (Vector2)player.transform.position;
            var direction      = playerPosition - ownerPosition;
            var distance       = direction.magnitude;
            var range          = weapon.Config.range;

            if (distance > range) return false;
            return weapon is { Config: MeleeWeaponConfig, Owner: { OwnerType: OwnerType.Enemy } }
                && weapon.Owner.StatsHolder.Stats[CharacterStatNames.HEALTH].Value > 0;
        }

        protected override void Apply(IWeapon weapon)
        {
            this.TickCooldown(weapon);
            if (this.GetCooldown(weapon) > 0f) return;

            var target = this.Manager.Query<IPlayer>().Single();
            this.ApplyDamage(weapon, target);
            this.ResetCooldown(weapon);
        }

        private float GetCooldown(IWeapon weapon) =>
            this.cooldowns.GetValueOrDefault(weapon, 0f);

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

            if (target.StatsHolder.Stats[CharacterStatNames.HEALTH].Value <= 0)
            {
                this.eventBus.Publish(new PlayerDiedEvent());
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
#nullable enable

using Core.Entities;

namespace VampireSurvival.Systems
{
    using System.Collections.Generic;
    using UnityEngine;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Configs;
    using VampireSurvival.Entities;
    using VampireSurvival.Models;

    public sealed class RangedAttackingSystem : System<IWeapon>
    {
        private const float SPAWN_OFFSET = 0.5f;

        private readonly Dictionary<IWeapon, float> cooldowns = new();

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
            if (weapon is not { }) return false;
            if (weapon.Owner.OwnerType == OwnerType.Enemy) return false;

            return weapon.Config is RangedWeaponConfig
                && weapon.Owner.StatsHolder.Stats[CharacterStatNames.HEALTH].Value > 0;
        }

        protected override void Apply(IWeapon weapon)
        {
            this.TickCooldown(weapon);
            if (this.GetCooldown(weapon) > 0f) return;

            var config = (RangedWeaponConfig)weapon.Config;
            //TODO
            var target    = config.attackStrategy == AttackStrategy.Homing ? this.FindTarget(weapon) : null;
            var direction = this.GetDirection(weapon, target);

            this.SpawnProjectile(weapon, config, direction, target);
            this.ResetCooldown(weapon);
        }

        private ITarget? FindTarget(IWeapon weapon)
        {
            var owner         = weapon.Owner;
            var config        = weapon.Config;
            var ownerPosition = (Vector2)owner.transform.position;
            var range         = config.range;

            IEnemy? closestEnemy    = null;
            var     closestDistance = float.MaxValue;

            foreach (var enemy in this.Manager.Query<IEnemy>())
            {
                if (!enemy.IsAlive) continue;

                var enemyPosition = (Vector2)enemy.transform.position;
                var distance      = Vector2.Distance(ownerPosition, enemyPosition);

                if (distance > range) continue;
                if (distance >= closestDistance) continue;

                closestEnemy    = enemy;
                closestDistance = distance;
            }

            return closestEnemy;
        }

        private void SpawnProjectile(IWeapon weapon, RangedWeaponConfig config, Vector2 direction, ITarget? target)
        {
            var owner = weapon.Owner;
            var spawnPosition = owner.transform.position
                + new Vector3(direction.x * SPAWN_OFFSET, direction.y * SPAWN_OFFSET, 0f);

            var damage = this.GetDamage(weapon);
            var speed  = config.speed;

            this.Manager.Spawn(
                config.projectilePrefab,
                new ProjectileParams(damage, speed, direction, owner, target),
                spawnPosition,
                Quaternion.identity
            );
        }

        private float GetCooldown(IWeapon weapon) => this.cooldowns.GetValueOrDefault(weapon, 0f);

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

        private Vector2 GetDirection(IWeapon weapon, ITarget? target)
        {
            var owner = weapon.Owner;

            if (target == null) return new Vector2(owner.FacingDirection, 0f).normalized;
            var ownerPosition  = (Vector2)owner.transform.position;
            var targetPosition = (Vector2)target.transform.position;
            return (targetPosition - ownerPosition).normalized;
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
#nullable enable

using IEntityManager = Core.Entities.IEntityManager;
using IEventBus = Core.Observer.IEventBus;

namespace VampireSurvival.Core.Systems
{
    using System;
    using VampireSurvival.Core.Events;

    public sealed class DamageSystem
    {
        private readonly IEntityManager entityManager;

        public DamageSystem(IEntityManager entityManager, IEventBus eventBus)
        {
            this.entityManager = entityManager;
            eventBus.Subscribe<DamageRequestEvent>(this.OnDamageRequested);
        }

        private void OnDamageRequested(DamageRequestEvent e)
        {
            if (!e.Target.TryGet<HealthComponent>(out var health) || health is null) return;

            health.Current = Math.Max(0f, health.Current - e.Amount);
        }
    }
}
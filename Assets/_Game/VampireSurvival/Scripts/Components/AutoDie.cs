#nullable enable

using Entities_Component = Core.Entities.Component;
using IEventBus = Core.Observer.IEventBus;

namespace VampireSurvival.Core.Components
{
    using UnityEngine;
    using VampireSurvival.Core.Events;

    public class AutoDie : Entities_Component
    {
        [SerializeField] private int experience;

        private IEventBus eventBus = null!;

        private const float LIFETIME_SECONDS = 10f;
        private       float timer;

        protected override void OnInstantiate()
        {
            base.OnInstantiate();
            this.eventBus = this.Container.Resolve<IEventBus>();
        }

        protected override void OnSpawn()
        {
            this.timer = LIFETIME_SECONDS;
        }

        private void Update()
        {
            this.timer -= Time.deltaTime;
            if (this.timer > 0f) return;

            this.eventBus.Publish(new EnemyDiedEvent(this.experience));
            this.Manager.Recycle(this.Entity);
        }
    }
}
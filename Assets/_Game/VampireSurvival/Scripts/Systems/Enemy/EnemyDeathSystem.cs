#nullable enable

namespace VampireSurvival.Core.Systems
{
    using IEntityManager = global::Core.Entities.IEntityManager;
    using IEventBus      = global::Core.Observer.IEventBus;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Events;

    public sealed class EnemyDeathSystem : ReactiveSystem<EnemyDiedEvent>
    {
        private readonly IEntityManager entityManager;
        public EnemyDeathSystem(IEventBus eventBus, IEntityManager entityManager) : base(eventBus)
        {
            this.entityManager = entityManager;
        }

        protected override void OnEvent(EnemyDiedEvent e)
        {
            this.HandleDeathAsync(e.Enemy).Forget();
        }

        private async UniTaskVoid HandleDeathAsync(IEnemy enemy)
        {
            enemy.Rigidbody.linearVelocity = Vector2.zero;
            await enemy.Animation.PlayDeathAnimationAsync();
            this.entityManager.Recycle(enemy);
        }
    }
}
#nullable enable

using Core.Entities;
namespace VampireSurvival.Systems
{
    using IEntityManager   = global::Core.Entities.IEntityManager;
    using IEventBus        = global::Core.Observer.IEventBus;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Events;
    using VampireSurvival.Models;

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
            enemy.Collider.isTrigger       = true;
            enemy.Rigidbody.linearVelocity = Vector2.zero;
            await enemy.Animation.PlayAnimationAsync(AnimationType.Death);
            this.entityManager.Recycle(enemy);
        }
    }
}
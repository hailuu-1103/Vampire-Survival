#nullable enable

using Core.Entities;

namespace VampireSurvival.Systems
{
    using IEntityManager = global::Core.Entities.IEntityManager;
    using IEventBus = global::Core.Observer.IEventBus;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Events;
    using VampireSurvival.Models;
    using VampireSurvival.Services;

    public sealed class PlayerDeathSystem : ReactiveSystem<PlayerDiedEvent>
    {
        private readonly IEntityManager           entityManager;
        private readonly PlayerProgressionService progressionService;

        public PlayerDeathSystem(
            IEventBus                eventBus,
            IEntityManager           entityManager,
            PlayerProgressionService progressionService
        ) : base(eventBus)
        {
            this.entityManager      = entityManager;
            this.progressionService = progressionService;
        }

        protected override void OnEvent(PlayerDiedEvent e)
        {
            this.progressionService.Reset();
            this.HandleDeathAsync().Forget();
        }

        private async UniTaskVoid HandleDeathAsync()
        {
            var player = this.entityManager.Query<IPlayer>().Single();
            await player.Animation.PlayAnimationAsync(AnimationType.Death);
            this.eventBus.Publish(new LostEvent());
        }
    }
}
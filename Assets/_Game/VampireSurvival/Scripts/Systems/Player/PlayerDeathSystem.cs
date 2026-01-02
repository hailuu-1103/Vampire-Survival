#nullable enable

namespace VampireSurvival.Core.Systems
{
    using IEntityManager = global::Core.Entities.IEntityManager;
    using IEventBus      = global::Core.Observer.IEventBus;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Events;
    using VampireSurvival.Core.Services;

    public sealed class PlayerDeathSystem : ReactiveSystem<PlayerDiedEvent>
    {
        private readonly IEntityManager           entityManager;
        private readonly PlayerProgressionService progressionService;

        public PlayerDeathSystem(
            IEventBus                 eventBus,
            IEntityManager            entityManager,
            PlayerProgressionService  progressionService) : base(eventBus)
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
            var player = this.entityManager.Query<IPlayer>().SingleOrDefault();
            if (player == null)
            {
                this.eventBus.Publish(new LostEvent());
                return;
            }

            await player.Animation.PlayDeathAnimationAsync();
            this.eventBus.Publish(new LostEvent());
        }
    }
}
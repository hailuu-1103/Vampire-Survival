#nullable enable
using System.Linq;
using IEventBus = Core.Observer.IEventBus;
using IEntityManager = Core.Entities.IEntityManager;

namespace VampireSurvival.Core.Systems
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;
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

        protected override void OnLoad()
        {
            Debug.Log("PlayerDeathSystem subscribed to PlayerDiedEvent");
        }

        protected override void OnEvent(PlayerDiedEvent e)
        {
            Debug.Log("PlayerDeathSystem received PlayerDiedEvent");
            this.progressionService.Reset();
            this.HandleDeathAsync().Forget();
        }

        private async UniTaskVoid HandleDeathAsync()
        {
            var player = this.entityManager.Query<IPlayer>().SingleOrDefault();
            if (player == null)
            {
                Debug.LogError("PlayerDeathSystem: Player not found!");
                this.eventBus.Publish(new LostEvent());
                return;
            }

            Debug.Log("PlayerDeathSystem: Death animation start");
            await player.Animation.PlayDeathAnimationAsync();
            Debug.Log("PlayerDeathSystem: Death animation completed, publishing LostEvent");
            this.eventBus.Publish(new LostEvent());
        }
    }
}
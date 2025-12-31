#nullable enable

namespace Game.Services
{
    using System;
    using Core.GameFlow;
    using Core.Observer;
    using Cysharp.Threading.Tasks;
    using VampireSurvival.Core.Events;
    using VampireSurvival.Core.Services;
    using VContainer.Unity;

    public sealed class GameplayService :
        IGameplayService,
        IInitializable,
        IDisposable
    {
        private readonly IEventBus              eventBus;
        private readonly VampireSurvivalService vampireSurvivalService;

        public GameplayService(
            VampireSurvivalService vampireSurvivalService,
            IEventBus              eventBus
        )
        {
            this.vampireSurvivalService = vampireSurvivalService;
            this.eventBus               = eventBus;
        }

        event Action          IGameplayService.OnStarted { add => this.onStarted += value; remove => this.onStarted -= value; }
        event Action          IGameplayService.OnWon     { add => this.onWon += value;     remove => this.onWon -= value; }
        event Action          IGameplayService.OnLost    { add => this.onLost += value;    remove => this.onLost -= value; }
        bool IGameplayService.IsLoaded => this.vampireSurvivalService.IsLoaded;

        private Action? onStarted;
        private Action? onWon;
        private Action? onLost;

        void IGameplayService.Load()
        {
            this.vampireSurvivalService.LoadAsync().Forget();
            this.onStarted?.Invoke();
        }

        void IGameplayService.Play()
        {
            this.vampireSurvivalService.Play();
        }

        void IGameplayService.Pause()  => this.vampireSurvivalService.Pause();
        void IGameplayService.Resume() => this.vampireSurvivalService.Resume();

        void IGameplayService.Unload() => this.vampireSurvivalService.Unload();

        void IInitializable.Initialize()
        {
            this.eventBus.Subscribe<WonEvent>(this.OnWonHandler);
            this.eventBus.Subscribe<LostEvent>(this.OnLostHandler);
        }

        private void OnWonHandler(WonEvent obj)
        {
            if (!this.vampireSurvivalService.IsLoaded) return;

            this.vampireSurvivalService.Pause();
            this.vampireSurvivalService.Unload();
            this.onWon?.Invoke();
        }

        private void OnLostHandler(LostEvent obj)
        {
            if (!this.vampireSurvivalService.IsLoaded) return;

            this.vampireSurvivalService.Pause();
            this.vampireSurvivalService.Unload();
            this.onLost?.Invoke();
        }

        void IDisposable.Dispose()
        {
            this.eventBus.Unsubscribe<WonEvent>(this.OnWonHandler);
            this.eventBus.Unsubscribe<LostEvent>(this.OnLostHandler);
        }
    }
}
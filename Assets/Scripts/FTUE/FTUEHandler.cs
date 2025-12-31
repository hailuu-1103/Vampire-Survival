#nullable enable
using IEntityManager = Core.Entities.IEntityManager;
using IFTUEService = Core.FTUE.IFTUEService;

namespace Game.FTUE
{
    using System;
    using Core.GameFlow;
    using Cysharp.Threading.Tasks;
    using VampireSurvival.Core.Services;
    using VContainer.Unity;

    public sealed class FTUEHandler : IInitializable, IDisposable
    {
        private readonly IGameplayService       gameplayService;
        private readonly VampireSurvivalService vampireSurvivalService;
        private readonly IEntityManager         entityManager;
        private readonly IFTUEService           ftueService;
        private readonly FTUEConfig             ftueConfigPrefab;

        private FTUEConfig? spawnedConfig;

        public FTUEHandler(
            IGameplayService       gameplayService,
            VampireSurvivalService vampireSurvivalService,
            IEntityManager         entityManager,
            IFTUEService           ftueService,
            FTUEConfig             ftueConfigPrefab
        )
        {
            this.gameplayService        = gameplayService;
            this.vampireSurvivalService = vampireSurvivalService;
            this.entityManager          = entityManager;
            this.ftueService            = ftueService;
            this.ftueConfigPrefab       = ftueConfigPrefab;
        }

        public event Action? OnFTUECompleted;
        public bool          IsCompleted => this.ftueService.IsCompleted("Main");

        void IInitializable.Initialize()
        {
            UniTask.WaitUntil(() => this.gameplayService.IsLoaded).ContinueWith(this.OnGameStarted).Forget();
        }

        private void OnGameStarted()
        {
            this.RunAsync().Forget();
        }

        private async UniTaskVoid RunAsync()
        {
            if (this.IsCompleted)
            {
                this.OnFTUECompleted?.Invoke();
                return;
            }

            this.vampireSurvivalService.ForceClearAllUnits();
            this.entityManager.Load(this.ftueConfigPrefab);
            this.spawnedConfig = this.entityManager.Spawn(this.ftueConfigPrefab);

            this.gameplayService.Resume();
            this.vampireSurvivalService.PauseEnemySpawner();

            await this.spawnedConfig.RunAsync();

            this.entityManager.Recycle(this.spawnedConfig);
            this.spawnedConfig = null;

            this.gameplayService.Pause();
            this.OnFTUECompleted?.Invoke();
        }

        public void ForceStop()
        {
            if (this.spawnedConfig is null) return;
            this.entityManager.Recycle(this.spawnedConfig);
            this.spawnedConfig = null;
        }

        void IDisposable.Dispose()
        {
            this.ForceStop();
            this.gameplayService.OnStarted -= this.OnGameStarted;
        }
    }
}
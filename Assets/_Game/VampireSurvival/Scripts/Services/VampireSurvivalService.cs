#nullable enable

namespace VampireSurvival.Services
{
    using IEntityManager    = global::Core.Entities.IEntityManager;
    using ILifecycleManager = global::Core.Lifecycle.ILifecycleManager;
    using IReactiveSystem   = global::Core.Entities.IReactiveSystem;
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using VampireSurvival.Entities;

    public sealed class VampireSurvivalService
    {
        private readonly IEntityManager              entityManager;
        private readonly ILifecycleManager           lifecycleManager;
        private readonly IEnumerable<IReactiveSystem> reactiveSystems;

        private GameManager? loaded;

        public VampireSurvivalService(
            IEntityManager               entityManager,
            ILifecycleManager            lifecycleManager,
            IEnumerable<IReactiveSystem> reactiveSystems)
        {
            this.entityManager    = entityManager;
            this.lifecycleManager = lifecycleManager;
            this.reactiveSystems  = reactiveSystems;
        }

        public bool IsLoaded => this.loaded is { };

        public async UniTask LoadAsync()
        {
            this.Unload();
            await this.lifecycleManager.LoadAsync();

            this.loaded = this.entityManager.Spawn<GameManager>();
            this.loaded.Load();
            this.Pause();
        }

        public void Load()
        {
            this.LoadAsync().Forget();
        }

        public void Unload()
        {
            if (this.loaded is null) return;

            this.loaded.Unload();
            this.entityManager.Recycle(this.loaded);
            this.loaded = null;

            foreach (var system in this.reactiveSystems)
            {
                if (system is IDisposable disposable)
                    disposable.Dispose();
            }
        }

        public void Pause()
        {
            if (this.loaded is null) return;
            this.loaded.Pause();
        }

        public void Resume()
        {
            if (this.loaded is null) return;
            this.loaded.Resume();
        }

        public void Play()
        {
            if (this.loaded is null) throw new NullReferenceException("GameManager is null!");
            this.loaded.Resume();
        }

        public void ForceClearAllUnits()
        {
            if (this.loaded is null) throw new NullReferenceException("GameManager is null!");
            this.loaded.ForceClearAllUnits();
        }

        public void PauseEnemySpawner()
        {
            if (this.loaded is null) return;
            this.loaded.PauseEnemySpawner();
        }

        public void ResumeEnemySpawner()
        {
            if (this.loaded is null) return;
            this.loaded.ResumeEnemySpawner();
        }

        public void SetPlayerImmortal(bool immortal)
        {
            if (this.loaded is null) return;
            this.loaded.SetPlayerImmortal(immortal);
        }
    }
}

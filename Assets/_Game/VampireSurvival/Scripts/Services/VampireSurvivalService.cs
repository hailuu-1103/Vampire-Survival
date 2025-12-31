#nullable enable

using IEntityManager = Core.Entities.IEntityManager;
using ILifecycleManager = Core.Lifecycle.ILifecycleManager;

namespace VampireSurvival.Core.Services
{
    using System;
    using Cysharp.Threading.Tasks;
    using VampireSurvival.Core.Entities;

    public sealed class VampireSurvivalService
    {
        private readonly IEntityManager entityManager;
        private readonly ILifecycleManager lifecycleManager;

        private GameManager? loaded;

        public VampireSurvivalService(IEntityManager entityManager, ILifecycleManager lifecycleManager)
        {
            this.entityManager = entityManager;
            this.lifecycleManager = lifecycleManager;
        }

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
    }
}
#nullable enable

using IEntityManager = Core.Entities.IEntityManager;

namespace VampireSurvival.Core.Services
{
    using System;
    using VampireSurvival.Core.Entities;

    public sealed class VampireSurvivalService
    {
        private readonly IEntityManager entityManager;

        private GameManager? loaded;

        public VampireSurvivalService(IEntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        public void Load()
        {
            this.Unload();
            this.loaded = this.entityManager.Spawn<GameManager>();
            this.loaded.Load();
            this.Pause();
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
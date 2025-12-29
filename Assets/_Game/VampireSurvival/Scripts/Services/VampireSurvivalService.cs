#nullable enable

using IEntityManager = Core.Entities.IEntityManager;

namespace VampireSurvival.Core.Services
{
    using System;
    using VampireSurvival.Core.Entities;

    public class VampireSurvivalService
    {
        private readonly IEntityManager       entityManager;

        public VampireSurvivalService(IEntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        private GameManager? loaded;
        private Tutorial?    tutorial;

        public void Load()
        {
            this.Unload();
            this.loaded = this.entityManager.Spawn<GameManager>();
            this.loaded.Load();
            this.Pause();
        }

        public void Unload()
        {
            if (this.loaded is { })
            {
                this.loaded.Unload();
                this.entityManager.Recycle(this.loaded);
                this.loaded = null;
            }
        }

        public void Pause()
        {
            if(this.loaded is null) throw new NullReferenceException("GameManager is not loaded");
            this.loaded.Pause();
        }

        public void Resume()
        {
            if(this.loaded is null) throw new NullReferenceException("GameManager is not loaded");
            this.loaded.Resume();
        }
    }
}
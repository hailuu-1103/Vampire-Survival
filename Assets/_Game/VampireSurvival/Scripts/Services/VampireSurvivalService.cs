#nullable enable

using IEntityManager = Core.Entities.IEntityManager;
using IDependencyContainer = Core.DI.IDependencyContainer;

namespace VampireSurvival.Core.Services
{
    using System.Linq;
    using VampireSurvival.Core.Entities;

    public class VampireSurvivalService
    {
        private readonly IEntityManager       entityManager;
        private readonly IDependencyContainer container;

        public VampireSurvivalService(IEntityManager entityManager, IDependencyContainer container)
        {
            this.entityManager = entityManager;
            this.container     = container;
        }

        private GameManager? gameManager;
        private Tutorial?    tutorial;

        private void Init()
        {
            this.gameManager = this.entityManager.Query<GameManager>().Single();
        }

        public void Load(bool hasTutorial = false)
        {
            this.Init();
            if (hasTutorial && this.tutorial is { })
            {
                this.tutorial = this.entityManager.Spawn(this.tutorial, parent: this.gameManager.transform);
            }
            this.Pause();
        }

        public void Unload()
        {
            if (this.gameManager is null) return;
            this.entityManager.Unload(this.gameManager);
            this.gameManager.Recycle();
        }

        public void Play()
        {
            this.Resume();
        }

        public void Pause()
        {
            this.gameManager.Pause();
        }

        public void Resume()
        {
            this.gameManager.Resume();
        }

        public void GiveUp()
        {
            this.Pause();
        }
    }
}
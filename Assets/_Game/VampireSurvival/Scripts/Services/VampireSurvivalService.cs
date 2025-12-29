#nullable enable

using IEntityManager = Core.Entities.IEntityManager;
using Core.Utils;

namespace VampireSurvival.Core.Services
{
    using System.Linq;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Components;
    using VampireSurvival.Core.Entities;
    using VampireSurvival.Core.UI;

    public class VampireSurvivalService
    {
        private readonly IEntityManager entityManager;

        public VampireSurvivalService(IEntityManager entityManager)
        {
            this.entityManager = entityManager;
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
            this.entityManager.Query<IPauseable>().ForEach(component => component.Pause());
        }

        public void Resume()
        {
            this.entityManager.Query<IPauseable>().ForEach(component => component.Resume());
        }

        public void GiveUp()
        {
            this.Pause();
        }
    }
}
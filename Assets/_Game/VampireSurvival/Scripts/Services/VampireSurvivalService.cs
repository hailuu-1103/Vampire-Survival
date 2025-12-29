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

        private Tutorial?          tutorial;
        private GameCanvas?        gameCanvas;
        private GameSystemsRunner? gameSystemsRunner;

        private void Init()
        {
            var gameManager = this.entityManager.Query<GameManager>().Single();
            this.tutorial          ??= gameManager.Tutorial;
            this.gameCanvas        ??= gameManager.GameCanvas;
            this.gameSystemsRunner ??= gameManager.GameSystemsRunner;
        }

        public void Load(bool hasTutorial = false)
        {
            this.Init();
            this.Unload();
            if (this.gameCanvas is { }) this.entityManager.Spawn(this.gameCanvas);
            if (this.gameSystemsRunner is { }) this.entityManager.Spawn(this.gameSystemsRunner);
            if (hasTutorial && this.tutorial is { })
            {
                this.tutorial = this.entityManager.Spawn(this.tutorial);
            }
            this.Pause();
        }

        public void Unload()
        {
            if (this.tutorial is { })
            {
                this.entityManager.Unload(this.tutorial);
                this.tutorial.Recycle();
                this.tutorial = null;
            }

            if (this.gameCanvas is { })
            {
                this.entityManager.Unload(this.gameCanvas);
                this.gameCanvas.Recycle();
                this.gameCanvas = null;
            }
            if (this.gameSystemsRunner is { })
            {
                this.entityManager.Unload(this.gameSystemsRunner);
                this.gameSystemsRunner.Recycle();
                this.gameSystemsRunner = null;
            }
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
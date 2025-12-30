#nullable enable

namespace Game.UI
{
    using Core.GameFlow;
    using Core.ScreenFlow;
    using UnityEngine;
    using UnityEngine.UI;
    using VContainer;

    public sealed class LoseScreenView : BaseScreenView
    {
        [SerializeField] private Button btnReplay = null!;

        private IGameplayService gameplayService = null!;
        private IScreenManager   screenManager   = null!;

        [Inject]
        private void Construct(IGameplayService gameplayService, IScreenManager screenManager)
        {
            this.gameplayService = gameplayService;
            this.screenManager   = screenManager;
        }

        private void OnEnable()
        {
            this.btnReplay.onClick.AddListener(this.OnReplay);
        }

        private void OnReplay()
        {
            this.gameplayService.Load();
            this.gameplayService.Play();
            this.screenManager.CloseAll();
        }
    }
}
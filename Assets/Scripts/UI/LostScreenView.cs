#nullable enable

namespace Game.UI
{
    using System;
    using Core.GameFlow;
    using Core.ScreenFlow;
    using UnityEngine;
    using UnityEngine.UI;
    using VContainer;

    public sealed class LostScreenView : BaseScreenView
    {
        [SerializeField] private Button  btnReplay = null!;

        private                  IGameplayService gameplayService = null!;
        private                  IScreenManager   screenManager   = null!;

        [Inject]
        private void Construct(IGameplayService gameplayService, IScreenManager screenManager)
        {
            this.gameplayService = gameplayService;
            this.screenManager   = screenManager;
        }

        public override void Open()
        {
            base.Open();
            this.btnReplay.onClick.AddListener(this.OnReplay);
        }

        public override void Close()
        {
            base.Close();
            this.btnReplay.onClick.RemoveAllListeners();
        }

        private void OnReplay()
        {
            this.gameplayService.Load();
        }
    }
}
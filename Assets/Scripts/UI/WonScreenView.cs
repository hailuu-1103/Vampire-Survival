#nullable enable

namespace Game.UI
{
    using Core.GameFlow;
    using Core.ScreenFlow;
    using UnityEngine;
    using UnityEngine.UI;
    using VContainer;

    public class WonScreenView : BaseScreenView
    {
        [SerializeField] private Button btnReplay = null!;

        private IGameplayService gameplayService = null!;

        [Inject]
        private void Construct(IGameplayService gameplayService)
        {
            this.gameplayService = gameplayService;
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
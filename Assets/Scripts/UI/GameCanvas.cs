#nullable enable

namespace Game.UI
{
    using Core.GameFlow;
    using Core.ScreenFlow;
    using Game.FTUE;
    using Game.UI.Player;
    using UnityEngine;
    using UnityEngine.UI;
    using VContainer;

    public sealed class GameCanvas : MonoBehaviour
    {
        [SerializeField] private Button                btnPlayerStats        = null!;
        [SerializeField] private RectTransform         screenHolder          = null!;
        [SerializeField] private GameplayScreenView    gameplayScreenView    = null!;
        [SerializeField] private PlayerStatsScreenView playerStatsScreenView = null!;
        [SerializeField] private LostScreenView        lostScreenView        = null!;
        [SerializeField] private WonScreenView         wonScreenView         = null!;

        public  RectTransform    ScreenHolder => this.screenHolder;
        private IGameplayService gameplayService = null!;
        private IScreenManager   screenManager   = null!;
        private FTUEHandler      ftueHandler     = null!;

        [Inject]
        private void Construct(IGameplayService gameplayService, IScreenManager screenManager, FTUEHandler ftueHandler)
        {
            this.gameplayService = gameplayService;
            this.screenManager   = screenManager;
            this.ftueHandler     = ftueHandler;
        }

        private void Start()
        {
            this.screenManager.Load(this.gameplayScreenView);
            this.screenManager.Load(this.lostScreenView);
            this.screenManager.Load(this.wonScreenView);
            this.screenManager.Load(this.playerStatsScreenView);
        }

        public void OnEnable()
        {
            this.btnPlayerStats.onClick.AddListener(this.OnClickPlayerStats);
            this.gameplayService.OnStarted   += this.OnGameStarted;
            this.gameplayService.OnWon       += this.OnWon;
            this.gameplayService.OnLost      += this.OnLost;
            this.ftueHandler.OnFTUECompleted += this.OpenGameplayScreenView;
        }

        private void OnGameStarted()
        {
            if (!this.ftueHandler.IsCompleted) return;
            this.OpenGameplayScreenView();
        }

        private void OnClickPlayerStats()
        {
            this.screenManager.Open(this.playerStatsScreenView, this.screenHolder);
        }

        private void OpenGameplayScreenView()
        {
            this.screenManager.CloseAll();
            this.screenManager.Open(this.gameplayScreenView, this.screenHolder);
        }

        private void OnWon()
        {
            this.screenManager.CloseAll();
            this.screenManager.Open(this.wonScreenView, this.screenHolder);
        }

        private void OnLost()
        {
            this.screenManager.CloseAll();
            this.screenManager.Open(this.lostScreenView, this.screenHolder);
        }

        public void OnDisable()
        {
            this.btnPlayerStats.onClick.RemoveAllListeners();
            this.gameplayService.OnStarted   -= this.OnGameStarted;
            this.gameplayService.OnLost      -= this.OnLost;
            this.ftueHandler.OnFTUECompleted -= this.OpenGameplayScreenView;
        }
    }
}
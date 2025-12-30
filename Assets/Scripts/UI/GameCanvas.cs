#nullable enable

namespace Game.UI
{
    using Core.GameFlow;
    using Core.ScreenFlow;
    using UnityEngine;
    using VContainer;

    public sealed class GameCanvas : MonoBehaviour
    {
        [SerializeField] private RectTransform  screenHolder   = null!;
        [SerializeField] private LoseScreenView loseScreenView = null!;

        private IGameplayService gameplayService = null!;
        private IScreenManager   screenManager   = null!;

        [Inject]
        private void Construct(IGameplayService gameplayService, IScreenManager screenManager)
        {
            this.gameplayService = gameplayService;
            this.screenManager   = screenManager;
        }

        private void Start()
        {
            this.screenManager.Load(this.loseScreenView);
        }

        public void OnEnable()
        {
            this.gameplayService.OnLost += this.OnLost;
        }

        public void OnDisable()
        {
            this.gameplayService.OnLost -= this.OnLost;
        }

        private void OnLost()
        {
            this.screenManager.Open(this.loseScreenView, this.screenHolder);
        }
    }
}
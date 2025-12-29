#nullable enable

namespace Game.UI
{
    using Core.GameFlow;
    using UnityEngine;
    using VContainer;

    public sealed class GameCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanel = null!;

        private IGameplayService gameplayService = null!;
        [Inject]
        private void Construct(IGameplayService gameplayService)
        {
            this.gameplayService = gameplayService;
        }
        private void OnEnable()
        {
            this.gameOverPanel.SetActive(false);
            this.gameplayService.OnLost += this.OnLost;
        }

        private void OnDisable()
        {
            this.gameplayService.OnLost -= this.OnLost;
        }

        private void OnLost()
        {
            this.gameOverPanel.SetActive(true);
        }
    }
}
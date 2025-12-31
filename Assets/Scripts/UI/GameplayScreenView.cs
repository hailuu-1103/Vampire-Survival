#nullable enable

namespace Game.UI
{
    using Core.GameFlow;
    using Core.ScreenFlow;
    using Core.Utils;
    using Game.Services;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using VampireSurvival.Core.Services;
    using VContainer;

    public class GameplayScreenView : BaseScreenView
    {
        [SerializeField] private GameObject[] waitingObjs  = null!;
        [SerializeField] private GameObject[] startObjs    = null!;
        [SerializeField] private Button       btnPlay      = null!;
        [SerializeField] private TMP_Text     killCountTxt = null!;
        [SerializeField] private TMP_Text     levelText    = null!;
        [SerializeField] private TMP_Text     expText      = null!;
        [SerializeField] private Slider       xpSlider     = null!;

        private KillCountTracker          killCountTracker         = null!;
        private IGameplayService          gameplayService          = null!;
        private IPlayerProgressionService playerProgressionService = null!;

        [Inject]
        private void Construct(
            KillCountTracker          killCountTracker,
            IGameplayService          gameplayService,
            IPlayerProgressionService playerProgressionService
        )
        {
            this.gameplayService          = gameplayService;
            this.playerProgressionService = playerProgressionService;
            this.killCountTracker         = killCountTracker;
        }

        public override void Open()
        {
            base.Open();
            this.SetWaitingObj(false);
            this.UpdateLevelText(this.playerProgressionService.Level);
            this.xpSlider.value = 0f;
            this.expText.text   = $"{this.playerProgressionService.CurrentXp}";

            this.OnKillChanged();

            this.killCountTracker.onKillChanged           += this.OnKillChanged;
            this.playerProgressionService.PlayerXPChanged += this.OnXPChanged;
            this.playerProgressionService.LeveledUp       += this.OnLeveledUp;
            this.btnPlay.onClick.AddListener(this.OnClickPlay);
        }

        private void OnKillChanged()
        {
            this.killCountTxt.text = this.killCountTracker.KillCounts.ToString();
        }

        private void SetWaitingObj(bool isActive)
        {
            this.waitingObjs.ForEach(obj => obj.SetActive(!isActive));
            this.startObjs.ForEach(obj => obj.SetActive(isActive));
        }

        private void OnXPChanged(PlayerXPChanged data)
        {
            this.expText.text   = $"{data.CurrentXp}/{data.XpToNextLevel}";
            this.xpSlider.value = data.Progress;
        }

        private void OnLeveledUp(int level)
        {
            this.UpdateLevelText(level);
        }

        private void UpdateLevelText(int level)
        {
            this.levelText.text = $"Lv. {level}";
        }

        private void OnClickPlay()
        {
            this.gameplayService.Play();
            this.SetWaitingObj(true);
        }

        public override void Close()
        {
            base.Close();
            this.btnPlay.onClick.RemoveAllListeners();
            this.killCountTracker.onKillChanged           -= this.OnKillChanged;
            this.playerProgressionService.PlayerXPChanged -= this.OnXPChanged;
            this.playerProgressionService.LeveledUp       -= this.OnLeveledUp;
        }
    }
}
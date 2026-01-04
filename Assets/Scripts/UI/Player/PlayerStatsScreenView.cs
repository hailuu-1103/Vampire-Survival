#nullable enable

namespace Game.UI.Player
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.ScreenFlow;
    using UnityEngine;
    using UnityEngine.UI;
    using VampireSurvival.Abstractions;
    using IEntityManager = Core.Entities.IEntityManager;
    using VContainer;

    public class PlayerStatsScreenView : BaseScreenView
    {
        [SerializeField] private Button       btnClose       = null!;
        [SerializeField] private StatItemView statItemPrefab = null!;
        [SerializeField] private Transform    statsContainer = null!;

        private IEntityManager entityManager = null!;
        private IScreenManager screenManager = null!;

        [Inject]
        private void Construct(IEntityManager entityManager, IScreenManager screenManager)
        {
            this.entityManager = entityManager;
            this.screenManager = screenManager;
        }

        private readonly List<StatItemView> spawnedItems = new();

        public override void Open()
        {
            base.Open();
            this.btnClose.onClick.AddListener(this.OnClickClose);
            this.RefreshStats();
        }

        private void OnClickClose()
        {
            this.screenManager.Close<PlayerStatsScreenView>();
        }

        private void RefreshStats()
        {
            this.ClearStats();

            var player = this.entityManager.Query<IPlayer>().FirstOrDefault();
            if (player == null) return;

            foreach (var (statName, statValue) in player.StatsHolder.Stats)
            {
                var item = Instantiate(this.statItemPrefab, this.statsContainer);
                item.Bind(statName, statValue);
                this.spawnedItems.Add(item);
            }
        }

        private void ClearStats()
        {
            foreach (var item in this.spawnedItems)
            {
                item.Unbind();
                Destroy(item.gameObject);
            }
            this.spawnedItems.Clear();
        }

        public override void Close()
        {
            base.Close();
            this.btnClose.onClick.RemoveAllListeners();
            this.ClearStats();
        }
    }
}
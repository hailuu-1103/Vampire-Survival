#nullable enable

using IEventBus = Core.Observer.IEventBus;
using ILateLoadable = Core.Lifecycle.ILateLoadable;

namespace VampireSurvival.Core.Services
{
    using System;
    using VampireSurvival.Core.Stats;

    public sealed record PlayerXPChanged(int CurrentXp, int XpToNextLevel, float Progress);

    public interface IPlayerProgressionService
    {
        public Action<PlayerXPChanged>? PlayerXPChanged { get; set; }
        public Action<int>?             LeveledUp       { get; set; }
        public int                      Level           { get; }
        public int                      CurrentXp       { get; }
    }

    public sealed class PlayerProgressionService : ILateLoadable, IPlayerProgressionService
    {
        private readonly PlayerConfig config;
        private readonly IEventBus    eventBus;

        public PlayerProgressionService(PlayerConfig config, IEventBus eventBus)
        {
            this.config   = config;
            this.eventBus = eventBus;
        }

        Action<PlayerXPChanged>? IPlayerProgressionService.PlayerXPChanged { get => this.playerXPChanged; set => this.playerXPChanged = value; }
        Action<int>? IPlayerProgressionService.            LeveledUp       { get => this.leveledUp;       set => this.leveledUp = value; }
        int IPlayerProgressionService.                     CurrentXp       => this.currentXp;

        int IPlayerProgressionService.Level => this.level;

        private int currentXp;
        private int level;

        private Action<PlayerXPChanged>? playerXPChanged;
        private Action<int>?             leveledUp;

        public int XpToNextLevel => this.config.GetXpToNextLevel(this.level);

        public float Progress
        {
            get
            {
                var xpForCurrentLevel = this.config.GetXpForLevel(this.level);
                var xpToNext          = this.XpToNextLevel;
                return xpToNext > 0
                    ? (float)(this.currentXp - xpForCurrentLevel) / xpToNext
                    : 1f;
            }
        }

        void ILateLoadable.OnLateLoad()
        {
            this.Reset();
        }

        public void AddXp(int amount)
        {
            if (amount <= 0) return;

            this.currentXp += amount;
            var newLevel = this.config.GetLevelForXp(this.currentXp);

            while (this.level < newLevel)
            {
                this.level++;
                this.leveledUp?.Invoke(this.level);
            }

            this.NotifyChanged();
        }

        public void Reset()
        {
            this.currentXp = 0;
            this.level     = 1;
            this.NotifyChanged();
        }

        private void NotifyChanged()
        {
            this.eventBus.Publish(new PlayerXPChanged(
                this.currentXp,
                this.XpToNextLevel,
                this.Progress
            ));
        }
    }
}
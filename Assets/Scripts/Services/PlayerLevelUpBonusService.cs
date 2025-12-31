#nullable enable

namespace Game.Services
{
    using System;
    using System.Linq;
    using Core.Lifecycle;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Models;
    using VampireSurvival.Core.Services;
    using IEntityManager = Core.Entities.IEntityManager;

    public sealed class PlayerLevelUpBonusService : ILateLoadable, IDisposable
    {
        private readonly IPlayerProgressionService progressionService;
        private readonly IEntityManager            entityManager;

        private const float HP_BONUS_PER_LEVEL     = 5f;
        private const float ATTACK_BONUS_PER_LEVEL = 2f;

        public PlayerLevelUpBonusService(
            IPlayerProgressionService progressionService,
            IEntityManager            entityManager
        )
        {
            this.progressionService = progressionService;
            this.entityManager      = entityManager;
        }

        void ILateLoadable.OnLateLoad()
        {
            this.progressionService.LeveledUp += this.OnLeveledUp;
        }

        private void OnLeveledUp(int level)
        {
            var player = this.entityManager.Query<IPlayer>().FirstOrDefault();
            if (player == null) return;

            player.StatsHolder.Add(StatNames.MAX_HEALTH, HP_BONUS_PER_LEVEL);
            player.StatsHolder.Add(StatNames.HEALTH, HP_BONUS_PER_LEVEL);
            player.StatsHolder.Add(StatNames.ATTACK, ATTACK_BONUS_PER_LEVEL);
        }

        void IDisposable.Dispose()
        {
            this.progressionService.LeveledUp -= this.OnLeveledUp;
        }
    }
}

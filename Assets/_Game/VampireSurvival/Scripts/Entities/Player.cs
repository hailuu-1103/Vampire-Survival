#nullable enable

using Entity = Core.Entities.Entity;

namespace VampireSurvival.Core.Entities
{
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Stats;

    public sealed class Player : Entity, IPlayer
    {
        public IStats      Stats      => this.GetComponent<IStats>();
        public IHealthStat HealthStat => this.GetComponent<IHealthStat>();
    }
}
#nullable enable
using IEntity = Core.Entities.IEntity;

namespace VampireSurvival.Core.Abstractions
{
    using System.Collections.Generic;
    using VampireSurvival.Core.Stats;

    public interface IPlayer : IEntity
    {
        public IStats Stats { get; }
        public IHealthStat HealthStat { get; }
    }
}
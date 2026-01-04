#nullable enable

namespace VampireSurvival.Abstractions
{
    using IEntity = global::Core.Entities.IEntity;

    public interface ITarget : IEntity, IHasCollider
    {
        public IStatsHolder StatsHolder { get; }
        public void         PlayHitAnim();
    }
}
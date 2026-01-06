#nullable enable

using IEntity = Core.Entities.IEntity;

namespace VampireSurvival.Abstractions
{
    using VampireSurvival.Configs;

    public interface IWeapon : IEntity
    {
        public IAttacker     Owner      { get; }
        public WeaponConfig  Config     { get; }
        public IStatsHolder? Stats      { get; }
        public int           Level      { get; }
        public int           MaxLevel   { get; }
        public bool          IsMaxLevel => this.Level >= this.MaxLevel;

        public void Upgrade();
    }
}
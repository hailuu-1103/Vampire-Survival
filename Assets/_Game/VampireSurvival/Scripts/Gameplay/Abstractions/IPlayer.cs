#nullable enable

namespace VampireSurvival.Abstractions
{
    using System.Collections.Generic;

    public interface IPlayer : IAttacker, IHasRigidbody, IImmortalable
    {
        public bool                         IsAlive { get; }
        public IAnimation                   Animation { get; }
        public IReadOnlyCollection<IWeapon> Weapons { get; }
    }
}
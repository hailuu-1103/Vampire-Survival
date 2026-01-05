#nullable enable

namespace VampireSurvival.Abstractions
{
    public interface IPlayer : IAttacker, IHasRigidbody, IImmortalable
    {
        public bool       IsAlive   { get; }
        public IAnimation Animation { get; }
    }
}
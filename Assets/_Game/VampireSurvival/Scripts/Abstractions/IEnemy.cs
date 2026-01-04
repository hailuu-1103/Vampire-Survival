#nullable enable

namespace VampireSurvival.Abstractions
{
    public interface IEnemy : IAttacker, IHasRigidbody
    {
        public bool       IsAlive   { get; }
        public IAnimation Animation { get; }
    }
}
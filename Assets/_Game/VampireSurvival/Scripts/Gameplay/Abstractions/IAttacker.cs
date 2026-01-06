#nullable enable

namespace VampireSurvival.Abstractions
{
    using VampireSurvival.Models;

    public interface IAttacker : ITarget
    {
        public OwnerType OwnerType       { get; }
        public float     FacingDirection { get; }
    }
}
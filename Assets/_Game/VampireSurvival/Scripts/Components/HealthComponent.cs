#nullable enable
using IComponent = Core.Entities.IComponent;

namespace VampireSurvival.Core
{
    public sealed class HealthComponent : IComponent
    {
        public float Max;
        public float Current;
    }
}
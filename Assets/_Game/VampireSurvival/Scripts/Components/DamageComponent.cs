#nullable enable

using IComponent = Core.Entities.IComponent;

namespace VampireSurvival.Core
{
    public sealed class DamageComponent : IComponent
    {
        public float Value;
    }
}
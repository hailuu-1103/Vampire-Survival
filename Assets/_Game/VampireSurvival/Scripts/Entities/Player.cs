#nullable enable

using Entity = Core.Entities.Entity;

namespace VampireSurvival.Core
{
    public sealed record PlayerParam(float Speed);
    public sealed class Player : Entity
    {
    }
}
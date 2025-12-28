#nullable enable
using IEntity = Core.Entities.IEntity;

namespace VampireSurvival.Core
{
    public record DamageRequestEvent(IEntity Target, float Amount);
}
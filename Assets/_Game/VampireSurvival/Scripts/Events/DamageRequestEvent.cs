#nullable enable
using IEntity = Core.Entities.IEntity;

namespace VampireSurvival.Core.Events
{
    public record DamageRequestEvent(IEntity Target, float Amount);
}
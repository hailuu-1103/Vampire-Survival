#nullable enable
using IEntity = Core.Entities.IEntity;

namespace VampireSurvival.Core.Abstractions
{

    public interface ICollectable : IEntity, IHasCollider
    {
        void OnCollected(IPlayer player);
    }
}
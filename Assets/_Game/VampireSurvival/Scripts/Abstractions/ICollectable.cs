#nullable enable

namespace VampireSurvival.Core.Abstractions
{
    using IEntity = global::Core.Entities.IEntity;

    public interface ICollectable : IEntity, IHasCollider
    {
        void OnCollected(IPlayer player);
    }
}
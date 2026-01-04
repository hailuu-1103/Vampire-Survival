#nullable enable

namespace VampireSurvival.Abstractions
{
    using IEntity = global::Core.Entities.IEntity;

    public interface ICollectable : IEntity, IHasCollider
    {
        public void OnCollected(IPlayer player);
    }
}

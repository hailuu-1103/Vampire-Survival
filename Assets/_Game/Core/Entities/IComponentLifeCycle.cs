#nullable enable
namespace Core.Entities
{
    public interface IComponentLifecycle
    {
        public void OnInstantiate();

        public void OnSpawn();

        public void OnRecycle();

        public void OnCleanup();
    }
}
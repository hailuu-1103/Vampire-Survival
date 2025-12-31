#nullable enable
namespace Core.Lifecycle
{
    using Cysharp.Threading.Tasks;

    public interface ILifecycleManager
    {
        public UniTask LoadAsync();
    }
}
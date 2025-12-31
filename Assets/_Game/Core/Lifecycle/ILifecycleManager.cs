#nullable enable
namespace Core.Lifecycle
{
    using Cysharp.Threading.Tasks;

    public interface ILifecycleManager
    {
        /// <summary>
        /// Executes all load phases in order:
        /// 1. IEarlyLoadable.OnEarlyLoad() / IEarlyLoadableAsync.OnEarlyLoadAsync()
        /// 2. ILoadable.OnLoad() / ILoadableAsync.OnLoadAsync()
        /// 3. ILateLoadable.OnLateLoad() / ILateLoadableAsync.OnLateLoadAsync()
        /// </summary>
        UniTask LoadAsync();
    }
}

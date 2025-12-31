#nullable enable
namespace Core.Lifecycle
{
    using Cysharp.Threading.Tasks;

    /// <summary>
    /// Called first during the load phase. Use for initializing core systems.
    /// </summary>
    public interface IEarlyLoadable
    {
        void OnEarlyLoad();
    }

    /// <summary>
    /// Async variant of IEarlyLoadable.
    /// </summary>
    public interface IEarlyLoadableAsync
    {
        UniTask OnEarlyLoadAsync();
    }

    /// <summary>
    /// Called during the main load phase. Use for general initialization.
    /// </summary>
    public interface ILoadable
    {
        void OnLoad();
    }

    /// <summary>
    /// Async variant of ILoadable.
    /// </summary>
    public interface ILoadableAsync
    {
        UniTask OnLoadAsync();
    }

    /// <summary>
    /// Called last during the load phase. Use for initialization that depends on other systems.
    /// </summary>
    public interface ILateLoadable
    {
        void OnLateLoad();
    }

    /// <summary>
    /// Async variant of ILateLoadable.
    /// </summary>
    public interface ILateLoadableAsync
    {
        UniTask OnLateLoadAsync();
    }
}

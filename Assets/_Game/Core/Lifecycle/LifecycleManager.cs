#nullable enable
namespace Core.Lifecycle
{
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;

    public sealed class LifecycleManager : ILifecycleManager
    {
        private readonly IReadOnlyList<IEarlyLoadable>      earlyLoadables;
        private readonly IReadOnlyList<IEarlyLoadableAsync> earlyLoadableAsyncs;
        private readonly IReadOnlyList<ILoadable>           loadables;
        private readonly IReadOnlyList<ILoadableAsync>      loadableAsyncs;
        private readonly IReadOnlyList<ILateLoadable>       lateLoadables;
        private readonly IReadOnlyList<ILateLoadableAsync>  lateLoadableAsyncs;

        public LifecycleManager(
            IEnumerable<IEarlyLoadable>      earlyLoadables,
            IEnumerable<IEarlyLoadableAsync> earlyLoadableAsyncs,
            IEnumerable<ILoadable>           loadables,
            IEnumerable<ILoadableAsync>      loadableAsyncs,
            IEnumerable<ILateLoadable>       lateLoadables,
            IEnumerable<ILateLoadableAsync>  lateLoadableAsyncs)
        {
            this.earlyLoadables      = earlyLoadables.ToList();
            this.earlyLoadableAsyncs = earlyLoadableAsyncs.ToList();
            this.loadables           = loadables.ToList();
            this.loadableAsyncs      = loadableAsyncs.ToList();
            this.lateLoadables       = lateLoadables.ToList();
            this.lateLoadableAsyncs  = lateLoadableAsyncs.ToList();
        }

        public async UniTask LoadAsync()
        {
            foreach (var loadable in this.earlyLoadables)
                loadable.OnEarlyLoad();

            await UniTask.WhenAll(this.earlyLoadableAsyncs.Select(l => l.OnEarlyLoadAsync()));

            foreach (var loadable in this.loadables)
                loadable.OnLoad();

            await UniTask.WhenAll(this.loadableAsyncs.Select(l => l.OnLoadAsync()));

            foreach (var loadable in this.lateLoadables)
                loadable.OnLateLoad();

            await UniTask.WhenAll(this.lateLoadableAsyncs.Select(l => l.OnLateLoadAsync()));
        }
    }
}
#nullable enable
namespace Core.FTUE.Conditions
{
    using System.Threading;
    using Core.Observer;
    using Cysharp.Threading.Tasks;

    public sealed class WaitForEventCondition<TEvent> : IFTUECondition
    {
        private readonly IEventBus eventBus;

        public WaitForEventCondition(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        public async UniTask WaitAsync(CancellationToken ct = default)
        {
            var tcs = new UniTaskCompletionSource();

            void Handler(TEvent _) => tcs.TrySetResult();

            this.eventBus.Subscribe<TEvent>(Handler);
            try
            {
                await tcs.Task.AttachExternalCancellation(ct);
            }
            finally
            {
                this.eventBus.Unsubscribe<TEvent>(Handler);
            }
        }
    }
}

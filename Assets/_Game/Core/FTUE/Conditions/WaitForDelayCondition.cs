#nullable enable
namespace Core.FTUE.Conditions
{
    using System.Threading;
    using Cysharp.Threading.Tasks;

    public sealed class WaitForDelayCondition : IFTUECondition
    {
        private readonly float seconds;

        public WaitForDelayCondition(float seconds)
        {
            this.seconds = seconds;
        }

        public async UniTask WaitAsync(CancellationToken ct = default)
        {
            await UniTask.WaitForSeconds(this.seconds, cancellationToken: ct);
        }
    }
}

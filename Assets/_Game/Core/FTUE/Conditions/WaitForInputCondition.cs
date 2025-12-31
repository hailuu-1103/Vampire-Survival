#nullable enable
namespace Core.FTUE.Conditions
{
    using System.Linq;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public sealed class WaitForInputCondition : IFTUECondition
    {
        private readonly string[] inputAxes;

        public WaitForInputCondition(params string[] axes)
        {
            this.inputAxes = axes;
        }

        public async UniTask WaitAsync(CancellationToken ct = default)
        {
            await UniTask.WaitUntil(
                () => this.inputAxes.Any(axis => Mathf.Abs(Input.GetAxisRaw(axis)) > 0.1f),
                cancellationToken: ct
            );
        }
    }
}

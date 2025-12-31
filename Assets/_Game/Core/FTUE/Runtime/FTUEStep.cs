#nullable enable
namespace Core.FTUE
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;

    public sealed class FTUEStep : IFTUEStep
    {
        private readonly Action? onStart;
        private readonly IFTUEPresenter? presenter;
        private readonly IFTUECondition condition;
        private readonly Action? onFinish;

        public FTUEStep(
            IFTUECondition condition,
            Action? onStart = null,
            IFTUEPresenter? presenter = null,
            Action? onFinish = null)
        {
            this.condition = condition;
            this.onStart = onStart;
            this.presenter = presenter;
            this.onFinish = onFinish;
        }

        public async UniTask ExecuteAsync(CancellationToken ct = default)
        {
            this.onStart?.Invoke();
            this.presenter?.Show();

            await this.condition.WaitAsync(ct);

            this.presenter?.Hide();
            this.onFinish?.Invoke();
        }
    }
}

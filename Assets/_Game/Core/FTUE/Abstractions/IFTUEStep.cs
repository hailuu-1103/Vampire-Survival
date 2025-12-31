#nullable enable
namespace Core.FTUE
{
    using System.Threading;
    using Cysharp.Threading.Tasks;

    public interface IFTUEStep
    {
        public UniTask ExecuteAsync(CancellationToken ct = default);
    }
}

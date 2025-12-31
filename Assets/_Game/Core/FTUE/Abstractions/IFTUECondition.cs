#nullable enable
namespace Core.FTUE
{
    using System.Threading;
    using Cysharp.Threading.Tasks;

    public interface IFTUECondition
    {
        public UniTask WaitAsync(CancellationToken ct = default);
    }
}

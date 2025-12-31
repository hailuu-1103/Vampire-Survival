#nullable enable
namespace Core.FTUE
{
    using System.Collections.Generic;
    using System.Threading;
    using Cysharp.Threading.Tasks;

    public interface IFTUEService
    {
        public bool IsCompleted(string sequenceId);
        public void MarkCompleted(string sequenceId);
        public void Reset(string sequenceId);
        public void ResetAll();
        public UniTask RunSequenceAsync(string sequenceId, IEnumerable<IFTUEStep> steps, CancellationToken ct = default);
    }
}

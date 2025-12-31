#nullable enable
namespace Core.FTUE
{
    using System.Collections.Generic;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public sealed class FTUEService : IFTUEService
    {
        private const string PREFS_PREFIX = "FTUE_";

        public bool IsCompleted(string sequenceId)
            => PlayerPrefs.GetInt(PREFS_PREFIX + sequenceId, 0) == 1;

        public void MarkCompleted(string sequenceId)
        {
            PlayerPrefs.SetInt(PREFS_PREFIX + sequenceId, 1);
            PlayerPrefs.Save();
        }

        public void Reset(string sequenceId)
        {
            PlayerPrefs.DeleteKey(PREFS_PREFIX + sequenceId);
            PlayerPrefs.Save();
        }

        public void ResetAll()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        public async UniTask RunSequenceAsync(
            string sequenceId,
            IEnumerable<IFTUEStep> steps,
            CancellationToken ct = default)
        {
            if (this.IsCompleted(sequenceId)) return;

            foreach (var step in steps)
            {
                await step.ExecuteAsync(ct);
            }

            this.MarkCompleted(sequenceId);
        }
    }
}

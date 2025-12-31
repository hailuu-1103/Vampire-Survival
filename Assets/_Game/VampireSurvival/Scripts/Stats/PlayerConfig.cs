#nullable enable

namespace VampireSurvival.Core.Stats
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "VampireSurvival/Player Config")]
    public sealed class PlayerConfig : ScriptableObject
    {
        [SerializeField] private int[] xpThresholds = null!;

        public int MaxLevel => this.xpThresholds.Length;

        public int GetLevelForXp(int totalXp)
        {
            for (var i = this.xpThresholds.Length - 1; i >= 0; i--)
            {
                if (totalXp >= this.xpThresholds[i]) return i + 1;
            }
            return 1;
        }

        public int GetXpForLevel(int level)
            => this.xpThresholds[Mathf.Clamp(level - 1, 0, this.xpThresholds.Length - 1)];

        public int GetXpToNextLevel(int level)
            => level >= this.MaxLevel ? 0 : this.xpThresholds[level] - this.xpThresholds[level - 1];
    }
}
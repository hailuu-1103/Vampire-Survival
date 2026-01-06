#nullable enable

namespace VampireSurvival.Configs
{
    using UnityEngine;
    using VampireSurvival.Components;

    [CreateAssetMenu(menuName = "VampireSurvival/Player Config")]
    public sealed class PlayerConfig : ScriptableObject
    {
        [SerializeField] private int[]    xpThresholds   = null!;
        [SerializeField] private Weapon[] initialWeapons = null!;
        public                   Weapon[] InitialWeapons => this.initialWeapons;

        public int MaxLevel => this.xpThresholds.Length + 1;

        public int GetLevelForXp(int totalXp)
        {
            for (var i = this.xpThresholds.Length - 1; i >= 0; i--)
            {
                if (totalXp >= this.xpThresholds[i]) return i + 2;
            }
            return 1;
        }

        public int GetXpForLevel(int level)
            => level <= 1 ? 0 : this.xpThresholds[Mathf.Clamp(level - 2, 0, this.xpThresholds.Length - 1)];

        public int GetXpToNextLevel(int level)
            => level >= this.MaxLevel ? 0 : this.GetXpForLevel(level + 1) - this.GetXpForLevel(level);
    }
}
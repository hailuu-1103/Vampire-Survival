#nullable enable

namespace VampireSurvival.Configs
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "VampireSurvival/Player Config")]
    public sealed class PlayerConfig : ScriptableObject
    {
        [Header("Progression")]
        [SerializeField] private int[] xpThresholds = null!;

        [Header("Starting Weapons")]
        [SerializeField] private GameObject[] startingWeapons = null!;

        public GameObject[] StartingWeapons => this.startingWeapons;

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

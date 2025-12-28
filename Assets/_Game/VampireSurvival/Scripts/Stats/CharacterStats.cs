#nullable enable
using Entities_Component = Core.Entities.Component;

namespace VampireSurvival.Core.Stats
{
    using UnityEngine;

    public interface ICharacterStats<out T>
    {
        T Config { get; }
    }

    public sealed class CharacterStats : Entities_Component, ICharacterStats<CharacterBasicStatsConfig>
    {
        [SerializeField] private CharacterBasicStatsConfig config = null!;
        public                   CharacterBasicStatsConfig Config => this.config;
    }
}
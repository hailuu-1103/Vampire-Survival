#nullable enable

using Entities_Component = Core.Entities.Component;

namespace VampireSurvival.Core
{
    using UnityEngine;

    public interface IPlayerStatsConfigProvider
    {
        public PlayerStatsConfig Config { get; }
    }
    public sealed class PlayerStats : Entities_Component, IPlayerStatsConfigProvider
    {
        [SerializeField] private PlayerStatsConfig config = null!;
        public                   PlayerStatsConfig Config => this.config;
    }
}
#nullable enable

namespace VampireSurvival.Progression.Abstractions
{
    using System;
    using VampireSurvival.Progression.Models;

    public interface IPlayerProgressionService
    {
        public Action<PlayerXPChanged>? PlayerXPChanged { get; set; }
        public Action<int>?             LeveledUp       { get; set; }
        public int                      Level           { get; }
        public int                      CurrentXp       { get; }
    }
}
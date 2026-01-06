#nullable enable

namespace VampireSurvival.Progression.Models
{
    public sealed record PlayerXPChanged(int CurrentXp, int XpToNextLevel, float Progress);
}

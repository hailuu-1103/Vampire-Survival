#nullable enable
namespace VampireSurvival.Core.Abstractions
{
    public interface IPauseable
    {
        public void Pause();
        public void Resume();
    }
}
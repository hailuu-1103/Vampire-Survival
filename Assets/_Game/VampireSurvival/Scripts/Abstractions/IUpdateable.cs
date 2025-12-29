#nullable enable
namespace VampireSurvival.Core.Abstractions
{
    public interface IUpdateable : IPauseable
    {
        public void Tick(float deltaTime);
    }
}
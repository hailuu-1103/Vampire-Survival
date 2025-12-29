#nullable enable
namespace VampireSurvival.Core.Abstractions
{
    public interface ISystem
    {
        public void Tick(float deltaTime);
    }
}
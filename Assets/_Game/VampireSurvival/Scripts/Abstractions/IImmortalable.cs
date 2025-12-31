#nullable enable
namespace VampireSurvival.Core.Abstractions
{
    public interface IImmortalable
    {
        public void SetImmortal(bool immortal);
        public bool IsImmortal { get; }
    }
}
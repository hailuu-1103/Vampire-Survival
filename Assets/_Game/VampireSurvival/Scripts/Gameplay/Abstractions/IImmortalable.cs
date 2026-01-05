#nullable enable

namespace VampireSurvival.Abstractions
{
    public interface IImmortalable
    {
        public void SetImmortal(bool immortal);
        public bool IsImmortal { get; }
    }
}

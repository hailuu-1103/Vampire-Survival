#nullable enable
namespace VampireSurvival.Core.Abstractions
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public interface ICharacterAnimation
    {
        public void    Play(string      animationName, bool   loop = false, float? mix = null);
        public UniTask PlayAsync(string animationName, float? mix  = null);
        public void    Stop(bool        fadeOut = true);
        public void    SetColor(Color   color);
        public void    SetFacing(float  direction);
        public bool    IsPlaying(string name);
    }
}
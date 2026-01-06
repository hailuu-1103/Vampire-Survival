#nullable enable

namespace VampireSurvival.Abstractions
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using VampireSurvival.Models;

    public interface IAnimation
    {
        public float   FacingDirection { get; }
        public bool    CanMove         { get; }
        public void    SetAnimation(AnimationType animationType);
        public void    SetFacing(float            direction);
        public void    SetColor(Color             color);
        public void    Stop();
        public UniTask PlayAnimationAsync(AnimationType animationType);
    }
}
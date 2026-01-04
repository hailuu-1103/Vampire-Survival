#nullable enable

namespace VampireSurvival.Core.Components
{
    using Component = global::Core.Entities.Component;
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using Spine.Unity;
    using UnityEngine;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Models;

    public sealed class CharacterAnimationController : Component, IAnimation
    {
        [Serializable]
        private struct AnimationConfig
        {
            public AnimationType type;
            [SpineAnimation] public string name;
        }

        [SerializeField] private AnimationConfig[] animations = null!;

        private ICharacterAnimation characterAnimation = null!;
        private AnimationType?      currentAnimation;

        private readonly Dictionary<AnimationType, string> animationMap = new();

        protected override void OnInstantiate()
        {
            this.characterAnimation = this.GetComponent<ICharacterAnimation>();

            foreach (var config in this.animations)
                this.animationMap[config.type] = config.name;
        }

        float IAnimation.FacingDirection => this.characterAnimation.FacingDirection;

        bool IAnimation.CanMove =>
            this.currentAnimation is not (AnimationType.Hit or AnimationType.Attack or AnimationType.Death)
            || !this.IsCurrentAnimationPlaying();

        void IAnimation.SetAnimation(AnimationType animationType)
        {
            if (!this.CanPlayAnimation(animationType)) return;
            if (!this.animationMap.TryGetValue(animationType, out var animationName)) return;

            var isLooping = animationType is AnimationType.Idle or AnimationType.Run;
            this.characterAnimation.Play(animationName, isLooping);
            this.currentAnimation = animationType;
        }

        void IAnimation.SetFacing(float direction) => this.characterAnimation.SetFacing(direction);

        void IAnimation.SetColor(Color color) => this.characterAnimation.SetColor(color);

        void IAnimation.Stop()
        {
            this.characterAnimation.Stop();
            this.currentAnimation = null;
        }

        async UniTask IAnimation.PlayAnimationAsync(AnimationType animationType)
        {
            if (!this.animationMap.TryGetValue(animationType, out var animationName)) return;

            ((IAnimation)this).Stop();
            this.currentAnimation = animationType;
            await this.characterAnimation.PlayAsync(animationName);
            this.currentAnimation = null;
        }

        private bool CanPlayAnimation(AnimationType requested)
        {
            if (this.currentAnimation == null) return true;
            if (this.currentAnimation == requested) return false;

            var current = this.currentAnimation.Value;

            if (IsMovementAnimation(current) && IsMovementAnimation(requested)) return true;

            var currentPriority   = GetPriority(current);
            var requestedPriority = GetPriority(requested);

            if (requestedPriority > currentPriority) return true;

            var currentName = this.animationMap.GetValueOrDefault(current);
            if (currentName != null && this.characterAnimation.IsPlaying(currentName)) return false;

            return true;
        }

        private bool IsCurrentAnimationPlaying()
        {
            if (this.currentAnimation is not { } current) return false;
            if (!this.animationMap.TryGetValue(current, out var name)) return false;
            return this.characterAnimation.IsPlaying(name);
        }

        private static bool IsMovementAnimation(AnimationType type) =>
            type is AnimationType.Idle or AnimationType.Run;

        private static int GetPriority(AnimationType type) => type switch
        {
            AnimationType.Idle   => 1,
            AnimationType.Run    => 1,
            AnimationType.Won    => 1,
            AnimationType.Hit    => 2,
            AnimationType.Attack => 3,
            AnimationType.Death  => 4,
            _                    => 0,
        };
    }
}
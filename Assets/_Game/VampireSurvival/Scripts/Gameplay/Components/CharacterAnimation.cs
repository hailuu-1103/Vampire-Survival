#nullable enable

namespace VampireSurvival.Core.Components
{
    using Component = global::Core.Entities.Component;
    using System;
    using Cysharp.Threading.Tasks;
    using Spine;
    using Spine.Unity;
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;
    using AnimationState = Spine.AnimationState;

    public sealed class CharacterAnimation : Component, ICharacterAnimation
    {
        [SerializeField] private new SkeletonAnimation animation = null!;

        private const int   TRACK_INDEX = 0;
        private const float DEFAULT_MIX = 0.15f;

        private Vector3        baseScale;
        private AnimationState State => this.animation.AnimationState;

        float ICharacterAnimation.FacingDirection => this.animation.skeleton.ScaleX;

        private void Awake()
        {
            if (this.State is { Data: { } }) this.State.Data.DefaultMix = DEFAULT_MIX;
        }

        void ICharacterAnimation.Play(string animationName, bool loop, float? mix)
        {
            if (!this.HasAnimation(animationName)) throw new InvalidOperationException($"Animation {animationName} not found");

            this.SetMix(mix);
            this.State.SetAnimation(TRACK_INDEX, animationName, loop);
        }

        async UniTask ICharacterAnimation.PlayAsync(string animationName, float? mix)
        {
            if (!this.HasAnimation(animationName)) throw new InvalidOperationException($"Animation {animationName} not found");

            this.SetMix(mix);
            var track = this.State.SetAnimation(TRACK_INDEX, animationName, false);

            var tcs = new UniTaskCompletionSource();
            track.Complete += OnComplete;
            await tcs.Task;

            void OnComplete(TrackEntry entry)
            {
                entry.Complete -= OnComplete;
                tcs.TrySetResult();
            }
        }

        void ICharacterAnimation.Stop(bool fadeOut)
        {
            if (fadeOut)
                this.State.SetEmptyAnimation(TRACK_INDEX, this.State.Data.DefaultMix);
            else
                this.State.ClearTrack(TRACK_INDEX);
        }

        bool ICharacterAnimation.IsPlaying(string name)
        {
            var current = this.State.GetCurrent(TRACK_INDEX);
            if (current == null || current.Animation?.Name != name) return false;
            if (!current.Loop && current.TrackTime >= current.AnimationEnd) return false;
            return true;
        }

        void ICharacterAnimation.SetColor(Color color)
        {
            this.animation.skeleton.SetColor(color);
        }

        void ICharacterAnimation.SetFacing(float direction)
        {
            if (Mathf.Abs(direction) < 0.01f) return;
            this.animation.skeleton.ScaleX = direction > 0f ? 1f : -1f;
        }

        private bool HasAnimation(string name) => this.animation != null && this.animation.Skeleton?.Data?.FindAnimation(name) != null;

        private void SetMix(float? mix)
        {
            if (mix.HasValue && this.State.Data != null) this.State.Data.DefaultMix = Mathf.Max(0f, mix.Value);
        }
    }
}
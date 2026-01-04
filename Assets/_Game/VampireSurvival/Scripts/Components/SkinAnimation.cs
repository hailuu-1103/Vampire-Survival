#nullable enable

namespace VampireSurvival.Components
{
    using System;
    using Spine;
    using Spine.Unity;
    using UnityEngine;
    using Animation = Spine.Animation;

    [Serializable]
    internal sealed class SkinAnimation
    {
        [SerializeField] [SpineSkin]      private string _skin      = null!;
        [SerializeField] [SpineAnimation] private string _animation = null!;

        private SkeletonAnimation skeleton  = null!;
        private Skin              skin      = null!;
        private Animation         animation = null!;

        public void Init(SkeletonAnimation skeleton)
        {
            this.skeleton  = skeleton;
            this.skin      = skeleton.Skeleton.Data.FindSkin(this._skin);
            this.animation = skeleton.Skeleton.Data.FindAnimation(this._animation);
        }

        public void Play(bool loop)
        {
            this.skeleton.Skeleton.SetSkin(this.skin);
            this.skeleton.Skeleton.SetSlotsToSetupPose();
            this.skeleton.AnimationState.SetAnimation(0, this.animation, loop);
        }
    }
}
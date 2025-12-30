#nullable enable

using Component = Core.Entities.Component;

namespace VampireSurvival.Core.Components
{
    using Cysharp.Threading.Tasks;
    using VampireSurvival.Core.Abstractions;

    public sealed class PlayerAnimation : Component, IPlayerAnimation
    {
        private const string IDLE_ANIM = "idle_1";
        private const string RUN_ANIM  = "run";
        private const string HIT_ANIM  = "hit";

        private ICharacterAnimation characterAnimation = null!;

        protected override void OnInstantiate()
        {
            this.characterAnimation = this.GetComponent<ICharacterAnimation>();
        }

        void IPlayerAnimation.PlayIdleAnimation()
        {
            if (this.characterAnimation.IsPlaying(IDLE_ANIM)) return;
            if (this.characterAnimation.IsPlaying(HIT_ANIM)) return;
            this.characterAnimation.Play(IDLE_ANIM, true);
        }

        void IPlayerAnimation.PlayRunAnimation()
        {
            if (this.characterAnimation.IsPlaying(RUN_ANIM)) return;
            if (this.characterAnimation.IsPlaying(HIT_ANIM)) return;
            this.characterAnimation.Play(RUN_ANIM, true);
        }

        void IPlayerAnimation.PlayHitAnimation()
        {
            this.characterAnimation.Play(HIT_ANIM);
        }

        void IPlayerAnimation.SetFacing(float direction)
        {
            this.characterAnimation.SetFacing(direction);
        }

        UniTask IPlayerAnimation.PlayDeathAnimationAsync()
        {
            return this.characterAnimation.PlayAsync("dead");
        }
    }
}
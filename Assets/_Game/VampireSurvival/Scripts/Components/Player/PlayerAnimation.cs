#nullable enable

using Component = Core.Entities.Component;

namespace VampireSurvival.Core.Components
{
    using Cysharp.Threading.Tasks;
    using VampireSurvival.Core.Abstractions;

    public sealed class PlayerAnimation : Component, IPlayerAnimation
    {
        private const string IDLE_ANIM   = "idle_1";
        private const string RUN_ANIM    = "run";
        private const string HIT_ANIM    = "hit";
        private const string ATTACK_ANIM = "sword_attack";
        private const string DEAD_ANIM   = "dead";

        private ICharacterAnimation characterAnimation = null!;

        protected override void OnInstantiate()
        {
            this.characterAnimation = this.GetComponent<ICharacterAnimation>();
        }

        bool IPlayerAnimation.CanMove =>
            !this.characterAnimation.IsPlaying(HIT_ANIM) &&
            !this.characterAnimation.IsPlaying(ATTACK_ANIM) &&
            !this.characterAnimation.IsPlaying(DEAD_ANIM);

        float IPlayerAnimation.FacingDirection => this.characterAnimation.FacingDirection;

        void IPlayerAnimation.PlayIdleAnimation()
        {
            if (this.characterAnimation.IsPlaying(IDLE_ANIM)) return;
            if (this.characterAnimation.IsPlaying(HIT_ANIM)) return;
            if (this.characterAnimation.IsPlaying(ATTACK_ANIM)) return;
            if (this.characterAnimation.IsPlaying(DEAD_ANIM)) return;
            this.characterAnimation.Play(IDLE_ANIM, true);
        }

        void IPlayerAnimation.PlayRunAnimation()
        {
            if (this.characterAnimation.IsPlaying(RUN_ANIM)) return;
            if (this.characterAnimation.IsPlaying(HIT_ANIM)) return;
            if (this.characterAnimation.IsPlaying(ATTACK_ANIM)) return;
            if (this.characterAnimation.IsPlaying(DEAD_ANIM)) return;
            this.characterAnimation.Play(RUN_ANIM, true);
        }

        void IPlayerAnimation.PlayHitAnimation()
        {
            if (this.characterAnimation.IsPlaying(DEAD_ANIM)) return;
            if (this.characterAnimation.IsPlaying(ATTACK_ANIM)) return;
            this.characterAnimation.Play(HIT_ANIM);
        }

        void IPlayerAnimation.PlayAttackAnimation()
        {
            if (this.characterAnimation.IsPlaying(DEAD_ANIM)) return;
            this.characterAnimation.Play(ATTACK_ANIM);
        }

        void IPlayerAnimation.SetFacing(float direction)
        {
            this.characterAnimation.SetFacing(direction);
        }

        UniTask IPlayerAnimation.PlayDeathAnimationAsync()
        {
            return this.characterAnimation.PlayAsync(DEAD_ANIM);
        }
    }
}
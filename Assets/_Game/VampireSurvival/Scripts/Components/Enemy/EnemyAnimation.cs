#nullable enable

namespace VampireSurvival.Core.Components
{
    using Component = global::Core.Entities.Component;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;

    public sealed class EnemyAnimation : Component, IEnemyAnimation
    {
        private const string RUN_ANIM    = "run";
        private const string ATTACK_ANIM = "sword_attack";
        private const string HIT_ANIM    = "hit";
        private const string DEAD_ANIM   = "dead";
        private const string WON_ANIM    = "jump";

        private ICharacterAnimation characterAnimation = null!;

        protected override void OnInstantiate()
        {
            this.characterAnimation = this.GetComponent<ICharacterAnimation>();
        }

        bool IEnemyAnimation.CanMove =>
            !this.characterAnimation.IsPlaying(HIT_ANIM) &&
            !this.characterAnimation.IsPlaying(ATTACK_ANIM) &&
            !this.characterAnimation.IsPlaying(DEAD_ANIM);

        float IEnemyAnimation.FacingDirection => this.characterAnimation.FacingDirection;

        void IEnemyAnimation.SetColor(Color color)
        {
            this.characterAnimation.SetColor(color);
        }

        void IEnemyAnimation.PlayRunAnimation()
        {
            if (this.characterAnimation.IsPlaying(RUN_ANIM)) return;
            if (this.characterAnimation.IsPlaying(ATTACK_ANIM)) return;
            if (this.characterAnimation.IsPlaying(HIT_ANIM)) return;
            if (this.characterAnimation.IsPlaying(DEAD_ANIM)) return;
            this.characterAnimation.Play(RUN_ANIM, loop: true);
        }

        void IEnemyAnimation.SetFacing(float direction)
        {
            this.characterAnimation.SetFacing(direction);
        }

        void IEnemyAnimation.PlayAttackAnimation()
        {
            if (this.characterAnimation.IsPlaying(DEAD_ANIM)) return;
            this.characterAnimation.Play(ATTACK_ANIM);
        }

        void IEnemyAnimation.PlayHitAnimation()
        {
            if (this.characterAnimation.IsPlaying(DEAD_ANIM)) return;
            this.characterAnimation.Play(HIT_ANIM);
        }

        void IEnemyAnimation.Stop()
        {
            this.characterAnimation.Stop();
        }

        UniTask IEnemyAnimation.PlayDeathAnimationAsync()
        {
            ((IEnemyAnimation)this).Stop();
            return this.characterAnimation.PlayAsync(DEAD_ANIM);
        }

        UniTask IEnemyAnimation.PlayWonAnimationAsync()
        {
            ((IEnemyAnimation)this).Stop();
            return this.characterAnimation.PlayAsync(WON_ANIM);
        }
    }
}
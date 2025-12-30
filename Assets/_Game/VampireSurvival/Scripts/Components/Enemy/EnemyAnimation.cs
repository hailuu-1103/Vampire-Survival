#nullable enable

using Component = Core.Entities.Component;
using Entities_Component = Core.Entities.Component;

namespace VampireSurvival.Core.Enemies
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;

    public sealed class EnemyAnimation : Entities_Component, IEnemyAnimation
    {
        private const string RUN_ANIM    = "run";
        private const string ATTACK_ANIM = "sword_attack";

        private ICharacterAnimation characterAnimation = null!;

        protected override void OnInstantiate()
        {
            this.characterAnimation = this.GetComponent<ICharacterAnimation>();
        }

        void IEnemyAnimation.SetColor(Color color)
        {
            this.characterAnimation.SetColor(color);
        }

        void IEnemyAnimation.PlayRunAnimation()
        {
            if (this.characterAnimation.IsPlaying(RUN_ANIM)) return;
            if (this.characterAnimation.IsPlaying(ATTACK_ANIM)) return;
            this.characterAnimation.Play(RUN_ANIM, loop: true);
        }

        void IEnemyAnimation.SetFacing(float direction)
        {
            this.characterAnimation.SetFacing(direction);
        }

        void IEnemyAnimation.PlayAttackAnimation()
        {
            this.characterAnimation.Play(ATTACK_ANIM, mix: 0.5f);
        }

        UniTask IEnemyAnimation.PlayDeathAnimationAsync()
        {
            return this.characterAnimation.PlayAsync("dead");
        }

        UniTask IEnemyAnimation.PlayWonAnimationAsync()
        {
            return this.characterAnimation.PlayAsync("jump");
        }
    }
}
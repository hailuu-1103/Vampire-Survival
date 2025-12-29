#nullable enable

using Entities_Component = Core.Entities.Component;

namespace VampireSurvival.Core.Enemies
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;

    public sealed class EnemyAnimation : Entities_Component, IEnemyAnimation
    {
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
            this.characterAnimation.Play("run", loop: true);
        }

        void IEnemyAnimation.SetFacing(float direction)
        {
            this.characterAnimation.SetFacing(direction);
        }

        void IEnemyAnimation.PlayAttackAnimation()
        {
            this.characterAnimation.Play("sword_attack", mix:0.5f);
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
#nullable enable

using Entities_Component = Core.Entities.Component;

namespace VampireSurvival.Core.Components
{
    using Cysharp.Threading.Tasks;
    using VampireSurvival.Core.Abstractions;

    public sealed class PlayerAnimation : Entities_Component, IPlayerAnimation
    {
        private ICharacterAnimation characterAnimation = null!;

        protected override void OnInstantiate()
        {
            this.characterAnimation = this.GetComponent<ICharacterAnimation>();
        }

        void IPlayerAnimation.PlayIdleAnimation()
        {
            this.characterAnimation.Play("idle_1", true);
        }

        void IPlayerAnimation.PlayRunAnimation()
        {
            this.characterAnimation.Play("run", true);
        }

        void IPlayerAnimation.PlayHitAnimation()
        {
            this.characterAnimation.Play("hit");
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
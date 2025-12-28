#nullable enable
using Core.Entities;

namespace VampireSurvival.Core.Systems
{
    public sealed class GameSystemsRunner : Component
    {
        private MovementSystem movementSystem = null!;

        protected override void OnInstantiate()
        {
            this.movementSystem = this.Container.Resolve<MovementSystem>();
        }

        private void Update()
        {
            this.movementSystem.Tick();
        }
    }
}
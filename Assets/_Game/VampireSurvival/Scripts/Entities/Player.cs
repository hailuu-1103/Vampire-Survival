#nullable enable

using EntityBase = Core.Entities.EntityBase;

namespace VampireSurvival.Core
{
    public sealed class Player : EntityBase
    {
        protected override void BuildComponents()
        {
            this.AddComponent(new HealthComponent
            {
                Max     = 100f,
                Current = 100f,
            });

            this.AddComponent(new DamageComponent
            {
                Value = 10f,
            });
        }
    }
}
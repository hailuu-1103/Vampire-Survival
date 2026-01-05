#nullable enable

using Core.Entities;
namespace VampireSurvival.Entities
{
    using UnityEngine;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Progression.Services;

    public sealed record XPCollectableParams(int XpValue);

    [RequireComponent(typeof(Collider2D))]
    public sealed class XPCollectible : Entity<XPCollectableParams>, ICollectable
    {
        private PlayerProgressionService progressionService = null!;
        private Collider2D               col                = null!;
        private int                      xpValue;

        protected override void OnInstantiate()
        {
            this.progressionService = this.Container.Resolve<PlayerProgressionService>();
            this.col                = this.GetComponent<Collider2D>();
        }

        public Collider2D Collider => this.col;

        protected override void OnSpawn()
        {
            this.xpValue = this.Params.XpValue;
        }

        public void OnCollected(IPlayer player)
        {
            this.progressionService.AddXp(this.xpValue);
            this.Recycle();
        }
    }
}
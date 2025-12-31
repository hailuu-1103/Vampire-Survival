#nullable enable
using Core.Entities;

namespace VampireSurvival.Core.Entities
{
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Services;

    public sealed record XPCollectableParams(int XpValue);

    [RequireComponent(typeof(Collider2D))]
    public sealed class XPCollectible : Entity<XPCollectableParams>, ICollectable
    {
        private PlayerProgressionService progressionService = null!;

        protected override void OnInstantiate()
        {
            this.progressionService = this.Container.Resolve<PlayerProgressionService>();
        }

        public  Collider2D Collider => this.GetComponent<Collider2D>();
        private int        xpValue;

        protected override void OnSpawn()
        {
            Debug.Log("OnSpawn XPCollectible");
            this.xpValue = this.Params.XpValue;
        }

        public void OnCollected(IPlayer player)
        {
            this.progressionService.AddXp(this.xpValue);
            this.Recycle();
        }
    }
}
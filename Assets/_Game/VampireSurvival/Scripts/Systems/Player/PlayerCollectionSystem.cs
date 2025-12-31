#nullable enable
using System.Linq;

namespace VampireSurvival.Core.Systems
{
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Models;

    public sealed class PlayerCollectionSystem : System<IPlayer>
    {
        //TODO
        private const float COLLECTION_RANGE = 0.5f;

        protected override bool Filter(IPlayer player)
        {
            return player.StatsHolder.Stats[StatNames.HEALTH].Value > 0;
        }

        protected override void Apply(IPlayer player)
        {
            foreach (var collectable in this.Manager.Query<ICollectable>().ToList())
            {
                var distance = player.Collider.Distance(collectable.Collider);
                if (!distance.isValid || distance.distance > COLLECTION_RANGE) continue;

                collectable.OnCollected(player);
            }
        }
    }
}
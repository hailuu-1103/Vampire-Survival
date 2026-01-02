#nullable enable

namespace VampireSurvival.Core.Systems
{
    using System.Linq;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Models;

    public sealed class PlayerCollectionSystem : System<IPlayer>
    {
        private const float COLLECTION_RANGE = 0.5f;

        protected override bool Filter(IPlayer player)
        {
            return player.StatsHolder.Stats[StatNames.HEALTH].Value > 0;
        }

        protected override void Apply(IPlayer player)
        {
            foreach (var collectable in this.Manager.Query<ICollectable>().ToArray())
            {
                var distance = player.Collider.Distance(collectable.Collider);
                if (!distance.isValid || distance.distance > COLLECTION_RANGE) continue;

                collectable.OnCollected(player);
            }
        }
    }
}
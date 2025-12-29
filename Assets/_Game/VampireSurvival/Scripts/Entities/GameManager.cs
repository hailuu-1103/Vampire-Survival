#nullable enable

using Core.Utils;
using Entity = Core.Entities.Entity;
using IEventBus = Core.Observer.IEventBus;

namespace VampireSurvival.Core.Entities
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Events;

    public sealed class GameManager : Entity
    {
        [SerializeField] private Player playerPrefab = null!;

        private IEnumerable<ISystem> systems  = null!;
        private IPlayer              player   = null!;
        private IEventBus            eventBus = null!;

        protected override void OnInstantiate()
        {
            this.eventBus = this.Container.Resolve<IEventBus>();
            this.systems  = this.Container.ResolveAll<ISystem>();
            this.Manager.Load(this.playerPrefab);
        }

        private void Update()
        {
            var dt = Time.deltaTime;
            this.systems.ForEach(system => system.Tick(dt));
        }

        protected override void OnSpawn()
        {
            this.Manager.Spawn(this.playerPrefab);
            this.player                 =  this.Manager.Query<IPlayer>().Single();
            this.player.HealthStat.Died += this.OnPlayerDied;
        }

        private void OnPlayerDied()
        {
            this.eventBus.Publish<LostEvent>(new());
        }

        protected override void OnRecycle()
        {
            this.Manager.Recycle(this.playerPrefab);
            // this.Manager.RecycleAll();
            this.player.HealthStat.Died -= this.OnPlayerDied;
        }
    }
}
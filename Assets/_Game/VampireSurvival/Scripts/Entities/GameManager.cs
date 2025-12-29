#nullable enable

using Core.Utils;
using Entity = Core.Entities.Entity;
using IEventBus = Core.Observer.IEventBus;

namespace VampireSurvival.Core.Entities
{
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Events;

    public sealed class GameManager : Entity
    {
        [SerializeField] private Player    playerPrefab = null!;
        [SerializeField] private Entity    enemyPrefab  = null!;
        [SerializeField] private Tutorial? tutorialPrefab;

        public Tutorial? TutorialPrefab => this.tutorialPrefab;

        private IPlayer player = null!;

        private IEnumerable<IUpdateable> systems  = null!;
        private IEventBus                eventBus = null!;

        protected override void OnInstantiate()
        {
            this.eventBus = this.Container.Resolve<IEventBus>();
            this.systems  = this.Container.ResolveAll<IUpdateable>();
            this.Manager.Load(this.playerPrefab);
            this.Manager.Load(this.enemyPrefab, 20);
        }

        private void Update()
        {
            var dt = Time.deltaTime;
            this.systems.ForEach(system => system.Tick(dt));
        }

        protected override void OnSpawn()
        {
            this.player = this.Manager.Spawn(this.playerPrefab);
            this.eventBus.Subscribe<PlayerDiedEvent>(this.OnPlayerDied);
        }

        private void OnPlayerDied(PlayerDiedEvent obj)
        {
            this.Pause();
            this.HandlePlayerDeathAsync().Forget();
        }

        private async UniTaskVoid HandlePlayerDeathAsync()
        {
            await this.player.Animation.PlayDeathAnimationAsync();
            var enemies = this.Manager.Query<IEnemy>().ToList();
            await UniTask.WhenAll(enemies.Select(e => e.Animation.PlayWonAnimationAsync()));
            this.ClearAllUnits();
            this.eventBus.Publish(new LostEvent());
        }

        private void ClearAllUnits()
        {
            var player = this.Manager.Query<IPlayer>().SingleOrDefault();
            if (player is null) return;
            this.Manager.Recycle(player);
            this.Manager.Query<IEnemy>().ToList().ForEach(this.Manager.Recycle);
            this.player = null!;
        }

        protected override void OnRecycle()
        {
            this.ClearAllUnits();
            this.eventBus.Unsubscribe<PlayerDiedEvent>(this.OnPlayerDied);
        }

        public void Pause()
        {
            this.Container.ResolveAll<IPauseable>().ForEach(pause => pause.Pause());
            this.Manager.Query<IPauseable>().ForEach(pause => pause.Pause());
        }

        public void Resume()
        {
            this.Container.ResolveAll<IPauseable>().ForEach(pause => pause.Resume());
            this.Manager.Query<IPauseable>().ForEach(pause => pause.Resume());
        }
    }
}
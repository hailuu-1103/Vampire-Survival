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

        private IPlayer?                   player;
        private IReadOnlyList<IUpdateable> updateables = null!;
        private IReadOnlyList<IPauseable>  pauseables  = null!;
        private IEventBus                  eventBus    = null!;

        protected override void OnInstantiate()
        {
            this.eventBus    = this.Container.Resolve<IEventBus>();
            this.updateables = this.Container.ResolveAll<IUpdateable>().ToList();
            this.pauseables  = this.Container.ResolveAll<IPauseable>().ToList();
            this.Manager.Load(this.playerPrefab);
            this.Manager.Load(this.enemyPrefab, 20);
        }

        private void Update()
        {
            var dt = Time.deltaTime;
            this.updateables.ForEach(system => system.Tick(dt));
        }

        protected override void OnSpawn()
        {
            this.eventBus.Subscribe<PlayerDiedEvent>(this.OnPlayerDied);
        }

        protected override void OnRecycle()
        {
            this.ClearAllUnits();
            this.eventBus.Unsubscribe<PlayerDiedEvent>(this.OnPlayerDied);
        }

        public void Load()
        {
            this.player = this.Manager.Spawn(this.playerPrefab);
        }

        public void Unload()
        {
            this.ClearAllUnits();
        }

        public void Pause()
        {
            this.pauseables.ForEach(p => p.Pause());
            this.Manager.Query<IPauseable>().ForEach(p => p.Pause());
        }

        public void Resume()
        {
            this.pauseables.ForEach(p => p.Resume());
            this.Manager.Query<IPauseable>().ForEach(p => p.Resume());
        }

        private void OnPlayerDied(PlayerDiedEvent _)
        {
            this.Pause();
            this.HandlePlayerDeathAsync().Forget();
        }

        private async UniTaskVoid HandlePlayerDeathAsync()
        {
            if (this.player == null) return;
            await this.player.Animation.PlayDeathAnimationAsync();
            var enemies = this.Manager.Query<IEnemy>().ToList();
            await UniTask.WhenAll(enemies.Select(e => e.Animation.PlayWonAnimationAsync()));
            this.ClearAllUnits();
            this.eventBus.Publish(new LostEvent());
        }

        private void ClearAllUnits()
        {
            if (this.player == null) return;
            this.Manager.Recycle(this.player);
            this.Manager.Query<IEnemy>().ToList().ForEach(this.Manager.Recycle);
            this.player = null;
        }
    }
}
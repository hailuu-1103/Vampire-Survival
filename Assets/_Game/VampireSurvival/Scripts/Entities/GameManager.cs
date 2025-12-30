#nullable enable

using Core.Utils;
using Entity = Core.Entities.Entity;
using IEventBus = Core.Observer.IEventBus;

namespace VampireSurvival.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Events;

    public sealed class GameManager : Entity
    {
        [SerializeField] private Player playerPrefab = null!;
        [SerializeField] private Entity enemyPrefab  = null!;

        private IPlayer?                   player;
        private IReadOnlyList<IUpdateable> updateables = null!;
        private IEventBus                  eventBus    = null!;

        private CancellationTokenSource?    enemyDeathCts;

        protected override void OnInstantiate()
        {
            this.eventBus    = this.Container.Resolve<IEventBus>();
            this.updateables = this.Container.ResolveAll<IUpdateable>().ToList();
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
            this.enemyDeathCts = new();
            this.eventBus.Subscribe<PlayerDiedEvent>(this.OnPlayerDied);
            this.eventBus.Subscribe<EnemyDiedEvent>(this.OnEnemyDied);
        }

        protected override void OnRecycle()
        {
            this.CancelEnemyDeathHandlers();
            this.ForceClearAllUnits();
            this.eventBus.Unsubscribe<PlayerDiedEvent>(this.OnPlayerDied);
            this.eventBus.Unsubscribe<EnemyDiedEvent>(this.OnEnemyDied);
        }

        public void Load()
        {
            this.player = this.Manager.Spawn(this.playerPrefab);
        }

        public void Unload()
        {
            this.CancelEnemyDeathHandlers();
            this.ForceClearAllUnits();
        }

        public void Pause()
        {
            this.updateables.ForEach(s => s.Pause());
            this.Manager.Query<IMoveable>().ForEach(p => p.Stop());
            this.Manager.Query<IPauseable>().ForEach(p => p.Pause());
        }

        public void Resume()
        {
            this.updateables.ForEach(s => s.Resume());
            this.Manager.Query<IMoveable>().ForEach(p => p.Stop());
            this.Manager.Query<IPauseable>().ForEach(p => p.Resume());
        }

        private void OnEnemyDied(EnemyDiedEvent obj)
        {
            this.HandleEnemyDeathAsync(obj.Enemy).Forget();
        }

        private void OnPlayerDied(PlayerDiedEvent _)
        {
            this.CancelEnemyDeathHandlers();
            this.Pause();
            this.HandlePlayerDeathAsync().Forget();
        }

        private void CancelEnemyDeathHandlers()
        {
            this.enemyDeathCts?.Cancel();
            this.enemyDeathCts?.Dispose();
            this.enemyDeathCts = new();
        }

        private async UniTaskVoid HandleEnemyDeathAsync(IEnemy enemy)
        {
            if (this.enemyDeathCts == null) return;

            var token = this.enemyDeathCts.Token;

            try
            {
                await enemy.Animation.PlayDeathAnimationAsync().AttachExternalCancellation(token);
                if (token.IsCancellationRequested) return;
                this.Manager.Recycle(enemy);
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async UniTaskVoid HandlePlayerDeathAsync()
        {
            if (this.player == null) throw new NullReferenceException("Player is null!");

            var deadEnemies = this.Manager.Query<IEnemy>()
                .Where(e => e.HealthStat.Current <= 0)
                .ToList();
            deadEnemies.ForEach(this.Manager.Recycle);

            var aliveEnemies = this.Manager.Query<IEnemy>().ToList();
            aliveEnemies.ForEach(e => e.Animation.Stop());

            await this.player.Animation.PlayDeathAnimationAsync();
            this.Manager.Recycle(this.player);
            this.player = null;

            await aliveEnemies.ForEachAsync(enemy =>
                enemy.Animation.PlayWonAnimationAsync().ContinueWith(() => this.Manager.Recycle(enemy)));

            this.eventBus.Publish(new LostEvent());
        }

        private void ForceClearAllUnits()
        {
            if (this.player == null) return;
            this.Manager.Recycle(this.player);
            this.Manager.Query<IEnemy>().ToList().ForEach(this.Manager.Recycle);
            this.player = null;
        }
    }
}
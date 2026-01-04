#nullable enable
namespace Game.FTUE
{
    using System.Collections.Generic;
    using Core.Entities;
    using Core.FTUE;
    using Core.FTUE.Conditions;
    using Core.FTUE.Presenters;
    using Core.Observer;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using VampireSurvival.Abstractions;
    using VampireSurvival.Entities;
    using VampireSurvival.Events;

    public sealed class FTUEConfig : Entity
    {
        [SerializeField] private FTUETextView textView     = null!;
        [SerializeField] private Player       playerPrefab = null!;
        [SerializeField] private Enemy        enemyPrefab  = null!;

        private IFTUEService ftueService = null!;
        private IEventBus    eventBus    = null!;
        private Player?      spawnedPlayer;

        protected override void OnInstantiate()
        {
            this.ftueService = this.Container.Resolve<IFTUEService>();
            this.eventBus    = this.Container.Resolve<IEventBus>();
        }

        public bool IsCompleted => this.ftueService.IsCompleted("Main");

        public async UniTask RunAsync()
        {
            if (this.IsCompleted) return;

            var steps = this.BuildMainSequence();
            await this.ftueService.RunSequenceAsync("Main", steps);

            if (this.spawnedPlayer is IImmortalable immortalable)
            {
                immortalable.SetImmortal(false);
            }
        }

        private IEnumerable<IFTUEStep> BuildMainSequence()
        {
            yield return new FTUEStep(
                condition: new WaitForInputCondition("Horizontal", "Vertical"),
                onStart: () =>
                {
                    this.Manager.Load(this.playerPrefab);
                    this.spawnedPlayer = this.Manager.Spawn(this.playerPrefab);
                    ((IImmortalable)this.spawnedPlayer).SetImmortal(true);
                },
                presenter: new TextPresenter(this.textView, "Use WASD to move your character")
            );

            yield return new FTUEStep(
                condition: new WaitForEventCondition<EnemyDiedEvent>(this.eventBus),
                onStart: () =>
                {
                    this.Manager.Load(this.enemyPrefab);
                    this.Manager.Spawn(this.enemyPrefab, new(5, 0, 0), Quaternion.identity);
                },
                presenter: new TextPresenter(this.textView, "Move towards enemy to attack")
            );

            yield return new FTUEStep(
                condition: new WaitForDelayCondition(3f),
                presenter: new TextPresenter(this.textView, "Clear 100 enemies to win!")
            );
        }
    }
}
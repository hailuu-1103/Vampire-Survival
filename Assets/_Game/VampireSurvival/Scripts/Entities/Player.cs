#nullable enable

using Entity = Core.Entities.Entity;

namespace VampireSurvival.Core.Entities
{
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;
    using VampireSurvival.Core.Stats;

    public sealed class Player : Entity, IPlayer
    {
        public IStats      Stats      => this.GetComponent<IStats>();
        public IHealthStat HealthStat => this.GetComponent<IHealthStat>();

        protected override void OnSpawn()
        {
            Time.timeScale       =  1;
            this.HealthStat.Died += this.OnDied;
        }

        private void OnDied()
        {
            Time.timeScale = 0;
        }
    }
}
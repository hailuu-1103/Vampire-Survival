#nullable enable

namespace VampireSurvival.Core.Abstractions
{
    using System;
    using VampireSurvival.Core.Stats;

    public interface IStats
    {
        public event Action<StatId, float, float> Changed;
        public float                              Get(StatId      id);
        public void                               Add(StatId      id, float delta);
        public void                               Multiply(StatId id, float delta);
        public void                               ResetModifiers();
    }
}
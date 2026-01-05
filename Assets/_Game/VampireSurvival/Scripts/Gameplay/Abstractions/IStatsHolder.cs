#nullable enable

namespace VampireSurvival.Abstractions
{
    using System.Collections.Generic;
    using VampireSurvival.Models;

    public interface IStatsHolder
    {
        public IReadOnlyDictionary<string, ObservableValue> Stats { get; }

        public void Add(string name, float value);

        public void Remove(string name);
    }
}

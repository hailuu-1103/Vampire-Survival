#nullable enable
namespace VampireSurvival.Core.Abstractions
{
    using System.Collections.Generic;
    using VampireSurvival.Core.Models;

    public interface IStatsHolder
    {
        public IReadOnlyDictionary<string, ObservableValue> Stats { get; }

        public void Add(string name, float value);

        public void Remove(string name);
    }
}
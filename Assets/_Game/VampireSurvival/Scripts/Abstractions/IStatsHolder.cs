#nullable enable
namespace AbilitySystem.Components
{
    using System.Collections.Generic;
    using Core.Entities;
    using Core.Utils.TheOne.Extensions;
    using VampireSurvival.Core.Models;

    public interface IStatsHolder
    {
        public IReadOnlyDictionary<string, ObservableValue> Stats { get; }

        public void Add(string name, float value);

        public void Remove(string name);
    }

    public static class StatsHolderExtensions
    {
        public static bool HasStat(this IEntity entity, string name)
        {
            return entity.gameObject.TryGetComponentInChildren<IStatsHolder>(out var statsHolder) && statsHolder.Stats.ContainsKey(name);
        }

        public static IReadOnlyDictionary<string, ObservableValue> GetStats(this IEntity entity)
        {
            return entity.gameObject.GetComponentInChildren<IStatsHolder>().Stats;
        }

        public static ObservableValue GetStat(this IEntity entity, string name)
        {
            return entity.gameObject.GetComponentInChildren<IStatsHolder>().Stats[name];
        }

        public static ObservableValue? GetStatOrDefault(this IEntity entity, string name)
        {
            return entity.gameObject.TryGetComponentInChildren<IStatsHolder>(out var statsHolder) ? statsHolder.Stats.GetValueOrDefault(name) : null;
        }

        public static void AddStat(this IEntity entity, string name, float value)
        {
            entity.gameObject.GetComponentInChildren<IStatsHolder>().Add(name, value);
        }

        public static void RemoveStat(this IEntity entity, string name)
        {
            entity.gameObject.GetComponentInChildren<IStatsHolder>().Remove(name);
        }
    }
}
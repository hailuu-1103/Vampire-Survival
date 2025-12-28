#nullable enable
using System;
using UnityEngine;

namespace VampireSurvival.Core.Stats
{
    public enum StatId
    {
        MoveSpeed,
        Damage,
        Health,
    }

    [CreateAssetMenu(menuName = "VampireSurvival/Stats/Character Stats Config")]
    public sealed class CharacterStatsConfig : ScriptableObject
    {
        [Serializable]
        public struct Entry
        {
            public StatId Id;
            public float  Value;
        }

        [SerializeField] private Entry[] entries = Array.Empty<Entry>();

        public bool TryGet(StatId id, out float value)
        {
            for (var i = 0; i < this.entries.Length; i++)
            {
                if (this.entries[i].Id != id) continue;
                value = this.entries[i].Value;
                return true;
            }

            value = 0f;
            return false;
        }
    }
}
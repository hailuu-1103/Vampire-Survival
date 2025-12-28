#nullable enable

namespace VampireSurvival.Core.Stats
{
    using UnityEngine;
    using VampireSurvival.Core.Entities;

    [CreateAssetMenu(menuName = "VampireSurvival/Enemy Config")]
    public sealed class EnemyConfig : ScriptableObject
    {
        public Enemy Prefab        = null!;
        public float SpawnInterval = 1f;
        public float SpawnRadious  = 10f;
    }
}
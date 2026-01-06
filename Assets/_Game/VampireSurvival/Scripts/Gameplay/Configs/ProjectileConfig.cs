#nullable enable

namespace VampireSurvival.Configs
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "VampireSurvival/Projectile Config")]
    public sealed class ProjectileConfig : ScriptableObject
    {
        public float hitRadius    = 0.1f;
        public float defaultSpeed = 8f;
        public float lifetime     = 3f;
    }
}

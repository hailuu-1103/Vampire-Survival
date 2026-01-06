#nullable enable

namespace VampireSurvival.Configs
{
    using UnityEngine;

    public abstract class WeaponConfig : ScriptableObject
    {
        [Header("Base Stats")]
        public float damage   = 10f;
        public float range    = 1f;
        public float cooldown = 1f;

        [Header("Upgrade")]
        public int   maxLevel            = 3;
        public float damagePerLevel      = 5f;
        public float cooldownPerLevel    = 0.1f;
    }
}
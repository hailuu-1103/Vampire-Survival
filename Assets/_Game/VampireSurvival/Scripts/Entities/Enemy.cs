#nullable enable

using Entity = Core.Entities.Entity;

namespace VampireSurvival.Core.Entities
{
    using Spine.Unity;
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;

    public sealed class Enemy : Entity, IEnemy
    {
        [SerializeField] private SkeletonAnimation skeletonAnimation = null!;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            this.skeletonAnimation.skeleton.SetColor(Color.red);
        }
    }
}
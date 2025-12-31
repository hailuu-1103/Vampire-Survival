#nullable enable
using UnityEngine;
using IEntityManager = Core.Entities.IEntityManager;

namespace VampireSurvival.Core
{
    using System.Linq;
    using VampireSurvival.Core.Abstractions;
    using VContainer;

    [RequireComponent(typeof(Camera))]
    public sealed class GameCamera : MonoBehaviour
    {
        [SerializeField] private Vector3 offset     = new(0f, 0f, -10f);
        [SerializeField] private float   smoothTime = 0.12f;

        public  Camera         Camera => this.GetComponent<Camera>();
        private IEntityManager entityManager = null!;
        private IPlayer?       player;

        [Inject]
        private void Construct(IEntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        private Vector3    velocity;

        private void Start()
        {
            this.player = this.entityManager.Query<IPlayer>().Single();
        }

        private void LateUpdate()
        {
            if (this.player == null) return;

            var desired = this.player.transform.position + this.offset;
            desired.z = this.offset.z;

            this.transform.position = Vector3.SmoothDamp(
                this.transform.position,
                desired,
                ref this.velocity,
                this.smoothTime
            );
        }
    }
}
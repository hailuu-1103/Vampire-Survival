#nullable enable

namespace VampireSurvival.Core
{
    using IEntityManager = global::Core.Entities.IEntityManager;
    using System.Linq;
    using UnityEngine;
    using VampireSurvival.Core.Abstractions;
    using VContainer;

    [RequireComponent(typeof(Camera))]
    public sealed class GameCamera : MonoBehaviour
    {
        [SerializeField] private Vector3 offset     = new(0f, 0f, -10f);
        [SerializeField] private float   smoothTime = 0.12f;

        private IEntityManager entityManager = null!;
        private IPlayer?       player;
        private Vector3        velocity;

        public Camera Camera { get; private set; } = null!;

        [Inject]
        private void Construct(IEntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        private void Awake()
        {
            this.Camera = this.GetComponent<Camera>();
        }

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
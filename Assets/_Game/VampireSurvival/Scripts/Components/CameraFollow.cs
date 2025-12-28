#nullable enable
using UnityEngine;

namespace VampireSurvival.Core
{
    using VampireSurvival.Core.Entities;

    public sealed class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Vector3 offset     = new(0f, 0f, -10f);
        [SerializeField] private float   smoothTime = 0.12f;

        private Transform? target;
        private Vector3    velocity;

        private void Start()
        {
            var player = FindFirstObjectByType<Player>();
            this.target = player != null ? player.transform : null;
        }

        private void LateUpdate()
        {
            if (this.target == null) return;

            var desired = this.target.position + this.offset;
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
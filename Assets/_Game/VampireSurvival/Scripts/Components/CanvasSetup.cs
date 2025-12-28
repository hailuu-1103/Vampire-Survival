#nullable enable

using Entities_Component = Core.Entities.Component;

namespace VampireSurvival.Core
{
    using UnityEngine;

    [RequireComponent(typeof(Canvas))]
    public class CanvasSetup : Entities_Component
    {
        private Canvas canvas = null!;

        protected override void OnInstantiate()
        {
            this.canvas = this.GetComponent<Canvas>();
        }

        protected override void OnSpawn()
        {
            this.canvas.worldCamera = Camera.main;
        }
    }
}
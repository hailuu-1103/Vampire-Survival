#nullable enable

namespace Core.ScreenFlow
{
    using UnityEngine;

    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseScreenView : MonoBehaviour, IScreenView
    {
        private CanvasGroup?   canvasGroup;
        private RectTransform? rectTransform;

        public CanvasGroup    CanvasGroup   => this.canvasGroup ??= this.GetComponent<CanvasGroup>();
        public RectTransform  RectTransform => this.rectTransform ??= this.GetComponent<RectTransform>();
        public virtual GameObject GameObject => this.gameObject;

        public virtual void Open()
        {
            this.RectTransform.anchoredPosition = Vector2.zero;
            this.SetVisible(true);
        }

        public virtual void Close()
        {
            this.SetVisible(false);
        }

        private void SetVisible(bool visible)
        {
            this.CanvasGroup.alpha          = visible ? 1f : 0f;
            this.CanvasGroup.blocksRaycasts = visible;
            this.CanvasGroup.interactable   = visible;
        }
    }
}
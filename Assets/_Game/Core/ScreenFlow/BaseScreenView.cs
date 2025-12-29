#nullable enable

namespace Core.ScreenFlow
{
    using UnityEngine;

    public abstract class BaseScreenView : MonoBehaviour, IScreenView
    {
        public virtual GameObject GameObject => this.gameObject;

        public virtual void Open()
        {
        }

        public virtual void Close()
        {
        }
    }
}
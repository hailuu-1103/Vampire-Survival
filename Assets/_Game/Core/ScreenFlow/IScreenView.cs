#nullable enable
namespace Core.ScreenFlow
{
    using UnityEngine;

    public interface IScreenView
    {
        public GameObject GameObject { get; }
        public void       Open();
        public void       Close();
    }
}
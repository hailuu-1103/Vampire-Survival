#nullable enable
namespace Core.GameFlow
{
    using System;

    public interface IGameplayService
    {
        public event Action OnStarted;
        public event Action OnWon;
        public event Action OnLost;

        public void Load();
        public void Play();
        public void Pause();
        public void Resume();
        public void Unload();
        public bool IsLoaded { get; }
    }
}
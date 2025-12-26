#nullable enable
namespace Core.DI
{
    public interface ILateTickable
    {
        public void LateTick();
    }
}
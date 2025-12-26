#nullable enable
namespace Core.DI
{
    public interface ILateDisposable
    {
        public void LateDispose();
    }
}
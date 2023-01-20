using OpenTK.Mathematics;

namespace MyGame
{
    public interface IEngine : IDisposable
    {
        public void RenderFrame();
        public void UpdateFrame();
        public new void Dispose();
    }
}
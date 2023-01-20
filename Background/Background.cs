using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MyGame
{
    public class Background : IEngine
    {
        private Shader shader = new Shader("Background/shader.vert", "Background/shader.frag");
        private static Random rand = new Random();
        private List<Texture> Textures = new List<Texture>();
        private static int index = 0, texCount = 0;
        public Background()
        {
            for( int i = 1; i < 9; i++)
                Textures.Add( new Texture($"Resources/Background/space{i}.jpg"));

            
            texCount = Textures.Count;
            index = rand.Next(0, texCount);

        }
        public void RenderFrame()
        {
            shader.Use();
            shader.SetUniform("view", Camera.ViewMatrix);
            shader.SetUniform("projection", Camera.ProjectionMatrix);

            shader.SetUniform("inputTexture", Textures[index].Use);

            shader.SetUniform("gamma", 1.55f);
            shader.SetUniform("interpolation", Values.interpolatedBack);

            var model = Matrix4.Identity;
            model = model * Matrix4.CreateScale(500f, 500f, 500f);
            model = model * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(TimerGL.Time));
            model = model * Matrix4.CreateTranslation(0.0f, 0.0f, -400f);

            shader.SetUniform("model", model);
            Sphere.RenderSphere();

        }
        public void UpdateFrame()
        {

        }
        public static void SetIndex()
        {
            index = rand.Next(0, texCount);
        }
        public void Dispose()
        {
            shader.Dispose();
            Textures.ForEach(i => i.Dispose());
        }
    }
}
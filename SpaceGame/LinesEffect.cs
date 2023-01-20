using OpenTK.Mathematics;


namespace MyGame
{
    public class LinesEffect : IEngine
    {
        private Shader shader { get => Uses.shaderLines; }
        private List<Vector2> positions = new List<Vector2>();
        private Random rand = new Random();
        public static float vel = 1500f;
        public static float alpha = 0.30f;
        private int Width  { get => (int)Uses.Width; }
        private int Height { get => (int)Uses.Height; }
        public LinesEffect()
        {
            for( int i = 0; i < 400f; i++)
            {
                var posx = rand.Next(Width + 50, Width * 3);
                var posy = rand.Next(0, Height);

                positions.Add(new Vector2(posx, posy));
            }
        }
        public void RenderFrame()
        {
            shader.Use();
            shader.SetUniform("projection", Uses.Projection2D);
            shader.SetUniform("color", Color4.White);
            shader.SetUniform("alpha", alpha);


            Matrix4 model;

            // var time = TimerGL.ElapsedTime * Values.posX;
            var time = TimerGL.ElapsedTime * vel;

            for(int i = 0; i < positions.Count; i++)
            {
                positions[i] -= new Vector2(time, 0.0f);

                if(positions[i].X < -10)
                {
                    positions[i] = new Vector2( Width + 50f, rand.Next(0, Height));
                }

                model = Matrix4.Identity;
                model = model * Matrix4.CreateScale(0.0163767f * 100f, 0.0049929f * 100f, 1.0f);
                model = model * Matrix4.CreateTranslation(positions[i].X, positions[i].Y, 2.0f);
                shader.SetUniform("model", model);
                Quad.RenderQuad();
            }
        }
        public void UpdateFrame()
        {

        }
        public void Dispose()
        {

        }
    }
}
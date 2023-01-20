using OpenTK.Mathematics;


namespace MyGame
{
    public class Spark
    {
        private Shader shader { get => Uses.shaderSpark; }
        private List<Texture> TexturesSpark { get => Uses.texturesSpark; }
        public Vector2 position = Vector2.Zero; 
        public Vector2 scale = Vector2.One;
        public Color4 color = Color4.White;
        public Vector2 dir = Vector2.Zero;
        public Spark()
        {

        }
        public Spark(Vector2 position, Vector2 scale, Color4 color, Vector2 dir = new Vector2())
        {
            this.position = position; 
            this.scale = scale;
            this.color = color;
            this.dir = dir;
        }
        private float contSpark = 0f;
        public bool RenderSpark(float speed = 0.05f)
        {
            if(contSpark < TexturesSpark.Count)
            {
            
                shader.Use();
                shader.SetUniform("projection", Uses.Projection2D);
                shader.SetUniform("inputTexture", TexturesSpark[(int)contSpark].Use);
                shader.SetUniform("LightForce", Values.ForceLightScene);
                shader.SetUniform("color", color);
                shader.SetUniform("Timer", TimerGL.Time);
                shader.SetUniform("alpha", 1.0f);

                var model = Matrix4.Identity;
                model = model * Matrix4.CreateScale(20f + contSpark * 2, 20f + contSpark * 2, 1.0f);
                position -= dir;
                model = model * Matrix4.CreateTranslation(position.X, position.Y, 0.5f);
                shader.SetUniform("model", model);

                Quad.RenderQuad();

                if(contSpark > 6)
                {
                    contSpark += speed * speed;
                }
                else
                {
                    contSpark += speed;
                }

                return false;
            }
            else
            {
                contSpark = 0;
                return true;
            }
        }
    }
}
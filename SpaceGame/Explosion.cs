using OpenTK.Mathematics;

namespace MyGame
{
    public class Explosion
    {
        private Shader shader { get => Uses.shaderExplosions; }
        private List<Texture> texturesExplosion { get => Uses.texturesExplosions; }
        private List<Texture> texturesBoom { get => Uses.texturesBoom; }
        private float contExplosion = 0f;
        public Vector2 position = Vector2.Zero; 
        public Vector2 scale = Vector2.One;
        public Color4 color = Color4.White;
        public Vector2 dir = Vector2.Zero;
        public bool RenderFrame(float speed = 35.0f)
        {
            var cont = TimerGL.ElapsedTime * speed;

            if(contExplosion < texturesExplosion.Count)
            {
            
                shader.Use();
                shader.SetUniform("projection", Uses.Projection2D);
                shader.SetUniform("inputTexture", texturesExplosion[(int)contExplosion].Use);
                shader.SetUniform("LightForce", Values.ForceLightScene);
                shader.SetUniform("color", color);
                shader.SetUniform("Timer", TimerGL.Time);
                shader.SetUniform("alpha", 1.0f);
                shader.SetUniform("enableAlpha", true);

                var model = Matrix4.Identity;
                model = model * Matrix4.CreateScale(scale.X * 150f + contExplosion * 2, scale.Y * 150f + contExplosion * 2, 1.0f);
                position -= dir;
                model = model * Matrix4.CreateTranslation(position.X, position.Y, 0.5f);
                shader.SetUniform("model", model);

                Quad.RenderQuad();

                if(contExplosion > (float)texturesExplosion.Count - 0.2f)
                {
                    contExplosion += cont * cont;
                }
                else
                {
                    contExplosion += cont;
                }

                return false;
            }
            else
            {
                contExplosion = 0;
                return true;
            }
        }
        public bool RenderExplosionBoom(float speed = 185f)
        {
            var cont = TimerGL.ElapsedTime * speed;

            if(contExplosion < texturesBoom.Count)
            {
            
                shader.Use();
                shader.SetUniform("projection", Uses.Projection2D);
                shader.SetUniform("inputTexture", texturesBoom[(int)contExplosion].Use);
                shader.SetUniform("LightForce", Values.ForceLightScene);
                shader.SetUniform("color", color);
                shader.SetUniform("Timer", TimerGL.Time);
                shader.SetUniform("alpha", 1.0f);
                shader.SetUniform("enableAlpha", false);


                var model = Matrix4.Identity;
                model = model * Matrix4.CreateScale(scale.X * 50f + contExplosion * 1, scale.Y * 50f + contExplosion * 1, 1.0f);
                position -= dir;
                model = model * Matrix4.CreateTranslation(position.X, position.Y, 0.5f);
                shader.SetUniform("model", model);

                Quad.RenderQuad();

                
                if(contExplosion > texturesExplosion.Count - 1)
                {
                    contExplosion += cont * cont;
                }
                else
                {
                    contExplosion += cont;
                }

                return false;
            }
            else
            {
                contExplosion = 0;
                return true;
            }
        }
    }
}
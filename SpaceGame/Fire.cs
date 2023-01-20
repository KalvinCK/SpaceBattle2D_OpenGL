using OpenTK.Mathematics;

namespace MyGame
{
    public class Fire
    {
        private Shader shader { get => Uses.shader; }
        private Texture firePlayer { get => Uses.textureFirePlayer; }
        private Texture fireEnemie { get => Uses.textureFireEnemies; }
        public Vector3 position;

        public bool RenderFirePlayer(float speed = 375f)
        {
            return RenderFrame(speed, firePlayer.Use, Player.players[Player.indexPlayer].color);
        }
        public bool RenderFireEnemies(float speed = 375f)
        {
            return RenderFrame(speed, fireEnemie.Use, Color4.White);
        }
        private float contTime = 0f;
        private bool RenderFrame(float speed, int indexTex , Color4 color)
        {
            var cont = TimerGL.ElapsedTime * speed;

            if(contTime < 10f)
            {
                shader.Use();
                shader.SetUniform("projection", Uses.Projection2D);
                shader.SetUniform("color", color);
                shader.SetUniform("LightForce", Values.ForceLightScene);
                shader.SetUniform("alpha", 1.0f);
                shader.SetUniform("inputTexture", indexTex);
                shader.SetUniform("disableAlpha", true);


                var model = Matrix4.Identity;
                model = model * Matrix4.CreateScale(0.50f * 100f, 0.16f * 100f, 1.0f);
                model = model * Matrix4.CreateTranslation(position.X, position.Y, position.Z + 0.2f);
                shader.SetUniform("model", model);
                Quad.RenderQuad();


                contTime += cont;

                return false;
            }
            else
            {
                contTime = 0f;
                return true;
            }


        }
    }
}
using OpenTK.Mathematics;


namespace MyGame
{
    public class EnemiessShot
    {
        private Shader shader { get => Uses.shader; }
        private Texture texture { get => Uses.textureBulletsEnemies; }
        public Vector4 BoxCollide { get; private set; }
        public Vector2 position;
        public Vector2 scale;
        public float Damage;
        public float vel;
        public void RenderFrame()
        {
            shader.Use();
            shader.SetUniform("projection", Uses.Projection2D);
            shader.SetUniform("inputTexture", texture.Use);
            shader.SetUniform("LightForce", Values.ForceLightScene);
            shader.SetUniform("color", Color4.White);
            shader.SetUniform("disableAlpha", true);


            position -= new Vector2(vel, 0.0f) * TimerGL.ElapsedTime * 500f;


            var model = Matrix4.Identity;
            model = model * Matrix4.CreateScale(65.0f * scale.X, 8.0f * scale.Y, 1.0f);
            model = model * Matrix4.CreateTranslation(position.X, position.Y, 0.0f);
            shader.SetUniform("model", model);

            BoxCollide = Collision.GetSizeBox(model, position, new Vector4(10f, 0f, 20f, 0f));
            Quad.RenderQuad();

            shader.SetUniform("disableAlpha", false);
        }
        public void Dispose()
        {
            shader.Dispose();
            texture.Dispose();
        }
    }
}
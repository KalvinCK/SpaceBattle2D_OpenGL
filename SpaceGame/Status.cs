using OpenTK.Mathematics;

namespace MyGame
{
    public class Status
    {
        private static Shader shader { get => Uses.shaderLife; }
        private static Texture texture { get => Uses.textureLife; }
        public static void RenderFrame(Vector2 position, Vector2 scale, float Life)
        {
            var model = Matrix4.Identity;
            model = model * Matrix4.CreateScale(scale.X * 100, scale.Y * 100, 1.0f);
            model = model * Matrix4.CreateTranslation(position.X, position.Y, 1.0f);

            shader.Use();
            shader.SetUniform("projection", Uses.Projection2D);
            shader.SetUniform("model", model);
            shader.SetUniform("alpha", 1.0f);
            shader.SetUniform("LifeLevel", Life);
            shader.SetUniform("inputTexture", texture!.Use);

            Quad.RenderQuad();
        }
    }
}
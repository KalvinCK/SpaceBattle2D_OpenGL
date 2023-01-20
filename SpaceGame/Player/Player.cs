using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MyGame
{
    class Player : IEngine
    {
        public struct Players
        {
            public Texture texture;
            public Vector2 scale;
            public Color4 color;
            public List<Vector3> firePos;
        }
        private List<Texture> texturesPlayer { get => Uses.PlayersSprites; }
        private Shader shader { get => Uses.ShaderPLayer; }
        public static int indexPlayer= 0;
        private Directions directs = new Directions(Values.playerVel.velMax, Values.playerVel.velMin, Values.playerVel.velUp, Values.playerVel.velDown);
        private static Vector2 posInit { get => new Vector2(150f, Program.Size.Y / 2f); }
        public static Vector2 position { get; private set; } = posInit;
        public static Vector4 BoxCollide;
        public static float lifeLevel = 100f;
        private static int Lifes = 3;
        public static int Score = 0;
        private Explosion explosion = new Explosion();
        private Sounds bigExplosion { get => Uses.bigExplosion; }
        public static List<Players> players = new List<Players>();
        public Player()
        {
            players.Add( new Players()
            {
                texture = texturesPlayer[0],
                color = Color4.SkyBlue,
                scale = new Vector2(0.7f, 0.42f),
                firePos = new List<Vector3>()
                {
                    new Vector3(81.17f, -20.40f, 0.0f),
                    new Vector3(94.15f, -19.12f, 0.0f),
                }
            });
            players.Add( new Players()
            {
                texture = texturesPlayer[1],
                color = Color4.SkyBlue,
                scale = new Vector2(0.7f, 0.33f),
                firePos = new List<Vector3>()
                {
                    new Vector3(92.87f,-24.39f, 0.0f),
                    new Vector3(99.15f, -21.68f, -1.0f),
                }
            });
            players.Add( new Players()
            {
                texture = texturesPlayer[2],
                color = Color4.OrangeRed,
                scale = new Vector2(0.7f, 0.33f),
                firePos = new List<Vector3>()
                {
                    new Vector3(64.19f, -8.27f, 0.0f),
                    new Vector3(77.17f, -0.29f, -1.0f)
                }
            });
            players.Add( new Players()
            {
                texture = texturesPlayer[3],
                color = Color4.OrangeRed,
                scale = new Vector2(0.7f, 0.26f),
                firePos = new List<Vector3>()
                {
                    new Vector3(51.60f, 5.43f, 0.0f),
                    new Vector3(37.95f, -10.69f, 0.0f),
                    new Vector3(72.60f, 5.43f, -1.0f),
                    new Vector3(56.35f, -2.70f, -1.0f),
                }
            });
        }
        private bool alphaBlink = true;
        public static float blink = 50f;
        public static bool isRenderExplosion = false;
        public void RenderFrame()
        {
            shader.Use();
            shader.SetUniform("projection", Uses.Projection2D);
            shader.SetUniform("inputTexture", texturesPlayer[indexPlayer].Use);
            shader.SetUniform("LightForce", 4.0f);

            if(blink > 0)
            {
                if(blink > 70f)
                {
                    shader.SetUniform("alpha", 0.0f);
                }
                else
                {
                    alphaBlink = !alphaBlink;
                    shader.SetUniform("alpha", alphaBlink ? 1.0f : 0.0f);
                }
                blink -= 0.05f;
            }
            else
            {
                shader.SetUniform("alpha", 1.0f);
            }

            var model = Matrix4.Identity;


            model = model * Matrix4.CreateScale(100f * players[indexPlayer].scale.X, 100f * players[indexPlayer].scale.Y, 1.0f);
            model = model * Matrix4.CreateTranslation(position.X, position.Y, 0.0f);
            shader.SetUniform("model", model);
            Quad.RenderQuad();

            BoxCollide = Collision.GetSizeBox(model, position);


            if(isRenderExplosion)
            {
                explosion.color = players[indexPlayer].color;
                explosion.scale = new Vector2(0.6f, 0.6f);
                isRenderExplosion = !explosion.RenderFrame(15.0f);
            }

            Status.RenderFrame(new Vector2(350f, 50f), new Vector2(2f, 0.174f), lifeLevel);

            if(lifeLevel >= 25 && lifeLevel <= 50)
            {
                Text.RenderText($"{(int)lifeLevel}%", new Vector2(550f, 40f), 1.2f, Color4.OrangeRed, 2.5f);
            }
            else if(lifeLevel < 25)
            {
                Text.RenderText($"{(int)lifeLevel}%", new Vector2(550f, 40f), 1.2f, Color4.Red, 2.5f);
            }
            else
            {
                Text.RenderText($"{(int)lifeLevel}%", new Vector2(550f, 40f), 1.2f, Color4.AliceBlue, 2.5f);
            }
            
            Text.RenderText($"LIFES : {Lifes}", new Vector2(20f, 40f), 1f, Color4.GreenYellow, 2.5f);

            Text.RenderText($"SCORE : {Score}", new Vector2(Uses.Width / 2 - 20f, Uses.Height - 100f), 1f, Color4.BlueViolet, 2.5f);


        }
        public void UpdateFrame()
        {
            directs.UpdateValues(Values.playerVel.velMax, Values.playerVel.velMin, Values.playerVel.velUp, Values.playerVel.velDown);

            var input = Program.window.IsKeyDown;

            var moveX = MathF.Sin(TimerGL.Time) * 0.032f;
            var moveY = MathF.Sin(TimerGL.Time) * 0.199f;

            position = new Vector2(position.X + moveX, position.Y + moveY);

            if(blink < 70f)
            {
                if(input(Keys.A) && BoxCollide.X > 30) position -= new Vector2(directs.X_negativeUp, 0.0f);
                else position -= new Vector2(directs.X_negativeDowm, 0.0f);

                if(input(Keys.D) && BoxCollide.Z < Uses.Width - 30) position += new Vector2(directs.X_positiveUp, 0.0f);
                else position += new Vector2(directs.X_positiveDowm, 0.0f);

                if(input(Keys.W) && BoxCollide.Y < Uses.Height - 30) position += new Vector2(0.0f, directs.Y_positiveUp);
                else position += new Vector2(0.0f, directs.Y_positiveDowm);

                if(input(Keys.S) && BoxCollide.W > 30) position -= new Vector2(0.0f, directs.Y_negativeUp);
                else position -= new Vector2(0.0f, directs.Y_negativeDowm);

            }


            if( lifeLevel < 1)
            {
                explosion.position = position;
                isRenderExplosion = true;
                bigExplosion.Play();
                blink = 100f;
                Lifes -= 1;
                lifeLevel = 100f;
                position = posInit;
            }
            if(Lifes < 1)
            {
                if(!isRenderExplosion)
                {
                    Clear();
                }
            }
        }
        public static void Clear()
        {
            lifeLevel = 100f;
            Lifes = 3;
            Score = 0;
            position = posInit;
            blink = 100f;
            
            Menu.renderMenu = true;
            Menu.End = true;

        }
        public void Dispose()
        {
            bigExplosion.Dispose();
        } 
    }
}
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MyGame
{

    class ShotsPlayer : IEngine
    {
        public struct Chars
        {
            public Texture ?texture;
            public Sounds ?sound;
            public Color4 color;
            public float timerShot;
            public float damage;
        }
        private static Random rand = new Random();
        private static Shader shader { get => Uses.shader; }
        public static List<Chars> chars = new List<Chars>();
        public static List<Vector3> positions = new List<Vector3>(); 
        public static List<Vector4> BoxCollide = new List<Vector4>();
        private static Vector2 scale = new Vector2(0.2f, 0.05f); 

        private static List<Spark> sparks = new List<Spark>();
        private static Sounds soundSpark { get => Uses.soundSpark; }
        
        private static List<Fire> fires = new List<Fire>();
        public static Texture TextureLaser = new Texture("Resources/Laser Sprites/laser1.png");
        public ShotsPlayer()
        {

            chars.Add(new Chars()
            {
                texture = TextureLaser,
                sound = new Sounds($"Resources/sounds/shoot2.wav"),
                color = Player.players[0].color,
                timerShot = 0.1f,
                damage = 13f,
            });

            chars.Add(new Chars()
            {
                texture = TextureLaser,
                sound = new Sounds($"Resources/sounds/shoot3.wav"),
                color = Player.players[1].color,
                timerShot = 0.1f,
                damage = 11f,
            });

            chars.Add(new Chars()
            {
                texture = TextureLaser,
                sound = new Sounds($"Resources/sounds/shoot4.wav"),
                color = Player.players[2].color,
                timerShot = 0.1f,
                damage = 13f,
            });

            chars.Add(new Chars()
            {
                texture = TextureLaser, 
                sound = new Sounds($"Resources/sounds/shoot5.wav"),
                color = Player.players[3].color,
                timerShot = 0.1f,
                damage = 12f,
            });

        }
        public void RenderFrame()
        {
            shader.Use();
            shader.SetUniform("projection", Uses.Projection2D);
            shader.SetUniform("inputTexture", chars[Player.indexPlayer].texture!.Use);
            shader.SetUniform("LightForce", Values.ForceLightScene);
            shader.SetUniform("color", chars[Player.indexPlayer].color);
            shader.SetUniform("alpha", 1.0f);
            shader.SetUniform("disableAlpha", true);

            for(int i = 0; i < positions.Count; i++)
            {
                var model = Matrix4.Identity;
                model = model * Matrix4.CreateScale(100f * scale.X, 100f * scale.Y, 1.0f);
                model = model * Matrix4.CreateTranslation(positions[i].X, positions[i].Y, positions[i].Z);
                shader.SetUniform("model", model);

                if(Bloom.EnableBloom)
                {
                    BoxCollide[i] = Collision.GetSizeBox(model, positions[i].Xy, new Vector4(20f, 0f, 20f, 0f));
                }
                else
                {
                    BoxCollide[i] = Collision.GetSizeBox(model, positions[i].Xy, new Vector4(20f, -10f, 20f, -10f));
                }

                Quad.RenderQuad();

            }

            for( int i = 0; i < sparks.Count; i++)
            {
                if(sparks[i].RenderSpark(0.08f))
                {
                    sparks.RemoveAt(i);
                }
            }

            for( int i = 0; i < fires.Count; i++)
            {
                if(fires[i].RenderFirePlayer())
                {
                    fires.RemoveAt(i);
                }
            }

        }
        private float elapsedTime = 0.0f;
        public void UpdateFrame()
        {
            var Time = TimerGL.Time;

            var input = Program.window.IsKeyDown;

            if(input(Keys.Space) && Time - elapsedTime > chars[Player.indexPlayer].timerShot && Player.blink < 70)
            {
                elapsedTime = Time;

                var firePos = Player.players[Player.indexPlayer].firePos;


                for( int i = 0; i < firePos.Count; i++)
                {
                    fires.Add(new Fire()
                    {
                        position = new Vector3( Player.position.X + firePos[i].X + 10.0f, Player.position.Y + firePos[i].Y, firePos[i].Z),
                    });

                    positions.Add( new Vector3( Player.position.X + firePos[i].X, Player.position.Y + firePos[i].Y, firePos[i].Z));
                    BoxCollide.Add(new Vector4());
                }

                chars[Player.indexPlayer].sound!.Play();
            }

            for( int i = 0; i < positions.Count; i++)
            {
                positions[i] += new Vector3(2.0f, 0.0f, 0.0f) * TimerGL.ElapsedTime * 600f;

                if(BoxCollide[i].X > Uses.Width)
                {
                    positions.RemoveAt(i);
                    BoxCollide.RemoveAt(i);
                }
            }

        }
        public static void Delete(int index)
        {
            soundSpark.Play();
            var pos = new Vector2(BoxCollide[index].Z, positions[index].Y);

            sparks.Add(new Spark(pos, scale, chars[Player.indexPlayer].color));

            positions.RemoveAt(index);
            BoxCollide.RemoveAt(index);
        }
        public static void Clear()
        {
            positions.Clear();
            BoxCollide.Clear();
        }
        public void Dispose()
        {
            foreach(var item in chars)
            {
                item.texture!.Dispose();
                item.sound!.Dispose();
            }
            soundSpark.Dispose();
        }
    }
}
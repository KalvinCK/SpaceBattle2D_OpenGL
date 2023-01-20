using OpenTK.Mathematics;

namespace MyGame
{
    public class Boss
    {
        private Shader shader { get => Uses.shader; }
        private Sounds bigExplosion { get => Uses.bigExplosion; }
        public static Random rand = new Random();
        private List<Texture> texturesBoss { get => Uses.enemiesBossSprites; }
        public static EnemiesDetails[] enemiesBossDetails = new EnemiesDetails[Uses.enemiesBossSprites.Count];
        private static Vector2 InitPos { get => new Vector2(Uses.Width + 1500, Uses.Height / 2); }
        private static Vector2 position = InitPos;
        public static Vector4 boxCollide;
        public static float Life = 1000f;
        public static int Max = 1500;
        public static int indexBoss = rand.Next(0, enemiesBossDetails.Length );
        public List<EnemiessShot> shots = new List<EnemiessShot>();
        public static bool bossDestruct = false;
        private static List<Fire> fires = new List<Fire>();

        public Boss()
        {
            Uses.musicGameplay.Stop();
            Uses.musicBoss.Play();
            SetDetails();
        }
        public void RenderFrame()
        {

            shader.Use();
            shader.SetUniform("projection", Uses.Projection2D);
            shader.SetUniform("LightForce", 2.0f);
            shader.SetUniform("color", Color4.White);
            shader.SetUniform("alpha", 1.0f);

            
            var details = enemiesBossDetails[indexBoss];
            shader.SetUniform("inputTexture", details.texture!.Use);

            var model = Matrix4.Identity;
            model = model * Matrix4.CreateScale(details.scale.X * 100f, details.scale.Y * 100f, 1.0f);
            model = model * Matrix4.CreateTranslation(position.X, position.Y, 0.0f);

            boxCollide = Collision.GetSizeBox(model, position, new Vector4(30f, 5f, 5f, 10f));
            shader.SetUniform("model", model);
            Quad.RenderQuad();


            for( int i = 0; i < shots.Count; i++)
            {
                shots[i].RenderFrame();
            }

            Status.RenderFrame(new Vector2(position.X, boxCollide.Y + 50f), new Vector2(1.86f, 0.09f), Life / 10f);


            for( int i = 0; i < fires.Count; i++)
            {
                if(fires[i].RenderFireEnemies(150f))
                {
                    fires.RemoveAt(i);
                }
            }

        }
        public void UpdateFrame()
        {
            
            if(position.X > Uses.Width - 300)
            {
                position -= new Vector2(0.5f, 0.0f);
            }
            else
            {
                position.X = Uses.Width - 300f;
                position.Y += MathF.Sin(TimerGL.Time) / 3.5f;

                LinesEffect.vel = 700f;
                LinesEffect.alpha = 0.20f;

                Fire();
            }

        }
        public float elapsedTime = 0f;
        public void Fire()
        {
            var Time = TimerGL.Time;
            
            if(Time - elapsedTime > 2.0)
            {
                elapsedTime = Time;
                
                var details = enemiesBossDetails[indexBoss];

                for(int i = 0; i < details.firePosition.Count; i++)
                {
                    fires.Add(new Fire()
                    {
                        position = new Vector3( position.X + details.firePosition[i].X - 10.0f, position.Y + details.firePosition[i].Y, 1.0f),
                    });

                    shots.Add( new EnemiessShot()
                    {
                        position = new Vector2(position.X + details.firePosition[i].X, position.Y + details.firePosition[i].Y),
                        scale = details.scale,
                        Damage = 40f,
                        vel = 2.5f,
                    });
                }
            }

            // detecd collisions 
            // player in boss
            if(Collision.Detect(boxCollide, Player.BoxCollide))
            {
                Player.lifeLevel -= Player.lifeLevel;
            }

            // enemys shots boss in player
            for(int i = 0; i < shots.Count; i++)
            {

                if(shots[i].BoxCollide.Z < 0)
                {
                    shots.RemoveAt(i);
                    break;
                }

                if(Collision.Detect(shots[i].BoxCollide, Player.BoxCollide) && Player.blink < 0)
                {
                    Player.lifeLevel -= 50f;
                }
            }
            // player shots in boss
            for( int i = 0; i < ShotsPlayer.BoxCollide.Count; i++)
            {

                if(Collision.Detect(ShotsPlayer.BoxCollide[i], boxCollide))
                {
                    var dano = ShotsPlayer.chars[Player.indexPlayer].damage / 3;

                    
                    if(rand.Next(0, 30) == 15)
                    {
                        dano = dano * 10;
                        Life -= dano;

                        Enemies.points.Add(new Points($"CRITICAL -{(int)dano}", new Vector2(ShotsPlayer.BoxCollide[i].Z - 50f, ShotsPlayer.positions[i].Y),
                            1.5f, Color4.SkyBlue, 110f, 
                            new Vector2(0.0f),
                            5.0f));


                    }
                    else
                    {
                        dano = dano * (rand.Next(10, 20) * 0.1f);
                        Life -= dano;

                        Enemies.points.Add(new Points($"-{(int)dano}", new Vector2(ShotsPlayer.BoxCollide[i].Z, ShotsPlayer.positions[i].Y),
                            1.0f, Uses.RandomColors[rand.Next(0, Uses.MaxColors)], 110f, 
                            new Vector2(0.0f),
                            5.0f));


                    }
  

                    ShotsPlayer.Delete(i);
                }
            }

            if(Life <= 50)
            {
                Enemies.explosions.Add(new Explosion()
                {
                    position = position,
                    scale =  new Vector2(enemiesBossDetails[indexBoss].scale.X),
                    color = Color4.White,
                    dir = Vector2.Zero

                });
                bigExplosion.Play();

                Delete();
            }

        }
        public void Delete()
        {
            var score = 300;

            Enemies.points.Add(new Points($"+{score}", new Vector2(position.X, position.Y + 300f),
                                            1.5f, Color4.BlueViolet,
                                            200f, new Vector2(0.0f, 0.1f),
                                            5.0f));


            position = InitPos;
            Max += Max;
            Enemies.EnemysDestruct += 1;
            Life = 500f;
            Player.Score += score;
            Boss.bossDestruct = true;
            indexBoss = rand.Next(0, enemiesBossDetails.Length );

            Uses.musicBoss.Stop();
            Uses.musicGameplay.Play();


            LinesEffect.vel = 1500f;
            LinesEffect.alpha = 0.30f;
            
        }
        public void SetDetails()
        {
            enemiesBossDetails[0] = new EnemiesDetails()
            {
                texture = texturesBoss[0],
                scale = new Vector2(2.43f, 1.47f),
                firePosition = new List<Vector2>()
                {
                    new Vector2(-191.07f, 88.80f),
                    new Vector2(-173.32f, -109.13f),
                    new Vector2(-173.32f, -74.54f),
                    new Vector2(-187.95f, -78.97f),
                }
            };
            enemiesBossDetails[1] = new EnemiesDetails()
            {
                texture = texturesBoss[1],
                scale = new Vector2(2.46f, 1.34f),
                firePosition = new List<Vector2>()
                {
                    new Vector2(-110.91f, 107.35f),
                    new Vector2(-182.24f, 111.27f),
                    new Vector2(-198.29f, 16.41f),
                    new Vector2(-67.40f, -1.42f),
                }
            };

            enemiesBossDetails[2] = new EnemiesDetails()
            {
                texture = texturesBoss[2],
                scale = new Vector2(2.61f, 1.39f),
                firePosition = new List<Vector2>()
                {
                    new Vector2(-80.60f, 8.92f),
                    new Vector2(-83.45f, -96.64f),
                    new Vector2(-72.39f, 105.57f),
                }
            };
            enemiesBossDetails[3] = new EnemiesDetails()
            {
                texture = texturesBoss[3],
                scale = new Vector2(2.61f, 1.39f),
                firePosition = new List<Vector2>()
                {
                    new Vector2(-45.29f, -28.53f),
                    new Vector2(-83.09f, -52.43f),
                    new Vector2(-14.91f, -77.43f),
                    new Vector2(-162.62f, -79.89f),
                }
            };
        }
    }
}
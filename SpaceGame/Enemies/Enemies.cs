using OpenTK.Mathematics;

namespace MyGame
{
    public class EnemiesDetails
    {
        public Texture ?texture;
        public List<Vector2> firePosition = new List<Vector2>();
        public Vector2 scale;
    }


    public class Enemies : IEngine
    {
        private Shader shader { get => Uses.shader; }
        private List<Texture> texturesSmall { get => Uses.enemiesSmallSprites; }
        private List<Texture> texturesMedium { get => Uses.enemiesMediumSprites; }

        private Random rand = new Random();
        public class Chars
        {
            public EnemiesDetails enemieDetails = new EnemiesDetails();
            public Vector2 positions;
            public float velX;
            public float Life = 200f;
            public float DamageShot;
            public float DamageLife;
            public float score;
            public Vector4 BoxCollide;
  
        } 
        public static int EnemysDestruct = 0;
        private static List<Chars> chars = new List<Chars>();
        public static List<Explosion> explosions = new List<Explosion>();
        private static List<Spark> sparks = new List<Spark>();
        private Sounds soundExplosion = new Sounds("Resources/sounds/explosion2.wav", 100);
        private Sounds soundSpark { get => Uses.soundSpark; }
        private Sounds EnemiesFire = new Sounds("Resources/sounds/enemyFire.wav");
        private static List<EnemiessShot> enemiesShots = new List<EnemiessShot>();
        public static List<Points> points = new List<Points>();

        public static EnemiesDetails[] enemiesSmallDetails = new EnemiesDetails[Uses.enemiesSmallSprites.Count];
        public static EnemiesDetails[] enemiesMediumDetails = new EnemiesDetails[Uses.enemiesMediumSprites.Count];
        private static List<Boss> boss = new List<Boss>();
        private static List<Fire> fires = new List<Fire>();


        public Enemies()
        {
            ConfigEnemies();

        }
        public void RenderFrame()
        {
            for( int i = 0; i < boss.Count; i++)
            {
                boss[i].RenderFrame();
            }
            for(int i = 0; i < chars.Count; i++)
            {
                shader.Use();
                shader.SetUniform("projection", Uses.Projection2D);
                shader.SetUniform("color", new Vector4(1.0f));
                shader.SetUniform("LightForce", 2.0f);
                shader.SetUniform("color", Color4.White);
                shader.SetUniform("alpha", 1.0f);

                var model = Matrix4.Identity;
                model = model * Matrix4.CreateScale(100f * chars[i].enemieDetails.scale.X, 100f * chars[i].enemieDetails.scale.Y, 1.0f);
                model = model * Matrix4.CreateTranslation(chars[i].positions.X, chars[i].positions.Y, 0.0f);
                chars[i].BoxCollide = Collision.GetSizeBox(model, chars[i].positions);

                shader.Use();
                shader.SetUniform("inputTexture", chars[i].enemieDetails.texture!.Use);
                shader.SetUniform("model", model);

                Quad.RenderQuad();
                Status.RenderFrame(new Vector2(chars[i].positions.X, chars[i].BoxCollide.Y + 10f), new Vector2(0.57f, 0.04f), chars[i].Life / 2f);

            }

            for( int i = 0; i < explosions.Count; i++)
            {
                var result = explosions[i].RenderExplosionBoom(200f);
                if(result)
                {
                    explosions.RemoveAt(i);
                }
            }
            for( int i = 0; i < enemiesShots.Count; i++)
            {
                enemiesShots[i].RenderFrame();
            }

            for( int i = 0; i < sparks.Count; i++)
            {
                if(sparks[i].RenderSpark(0.09f))
                {
                    sparks.RemoveAt(i);
                }
            }
            
            for( int i = 0; i < points.Count; i++)
            {
                if(points[i].RenderFrame())
                {
                    points.RemoveAt(i);
                }
            }
            for( int i = 0; i < fires.Count; i++)
            {
                if(fires[i].RenderFireEnemies(150f))
                {
                    fires.RemoveAt(i);
                }
            }

        }
        private float elapsedTime = 0.0f;
        private float elapsedTimeMeddums = 0.0f;
        public void UpdateFrame()
        {
            for( int i = 0; i < boss.Count; i++)
            {
                boss[i].UpdateFrame();
            }

            if(Player.Score > Boss.Max && boss.Count < 1)
            {
                Boss.bossDestruct = false;
                boss.Add( new Boss());
            }

            if( Boss.bossDestruct && boss.Count > 0)
            {
                boss.Clear();
                chars.Clear();
                EnemysDestruct += 1;
            }

            if(boss.Count == 0)
            {
                var Time = TimerGL.Time;
                
                if(Time - elapsedTime > 1.3f)
                {
                    elapsedTime = Time;

                    var scaleResult = (float)rand.Next(3, 5) * 0.1f;
                    var damages = CalcDamages(scaleResult);


                    var createPosition = new Vector2(Uses.Width + rand.Next(500, 1000), rand.Next(600, (int)Uses.Height) - 100 );
                    var detail = enemiesSmallDetails[rand.Next(0, enemiesSmallDetails.Length)];

                    chars.Add( new Chars()
                    {
                        enemieDetails = detail,
                        positions = createPosition,
                        velX = (float)rand.Next(0, 25) * 0.01f,
                        DamageShot = damages.TheShot,
                        DamageLife = damages.TheLife,
                        score = damages.score,
                    });

                }
                if(Time - elapsedTimeMeddums > 3.7f)
                {
                    elapsedTimeMeddums = Time;

                    var scaleResult = (float)rand.Next(5, 7) * 0.1f;
                    var damages = CalcDamages(scaleResult);

                    var detail = enemiesMediumDetails[rand.Next(0, enemiesMediumDetails.Length)];
                    var createPosition = new Vector2(Uses.Width + rand.Next(500, 1000), rand.Next(400, (int)Uses.Height) - 400 );

                    chars.Add( new Chars()
                    {
                        enemieDetails = detail,
                        positions = createPosition,
                        velX = (float)rand.Next(0, 25) * 0.01f,
                        DamageShot = damages.TheShot,
                        DamageLife = damages.TheLife,
                        score = damages.score,
                    });
                }
            }
        

            for( int i = 0; i < chars.Count; i++)
            {
                var dirX = TimerGL.ElapsedTime * 200f + chars[i].velX;
                var DirY = MathF.Sin(TimerGL.Time) / 3.5f;
                chars[i].positions -= new Vector2(dirX, DirY);

                var moveX = MathF.Sin(TimerGL.Time) * -0.116f;
                var moveY = MathF.Sin(TimerGL.Time) * 0.0827f;
                chars[i].positions = new Vector2(chars[i].positions.X + moveX, chars[i].positions.Y + moveY);         

                if(chars[i].BoxCollide.X < Uses.Width)
                {
                    if(chars[i].BoxCollide.Z < -10)
                    {
                        DeleteChars(i);
                    }
                    else
                    {
                        // colisao do inimigo com o player
                        if(Collision.Detect(chars[i].BoxCollide, Player.BoxCollide) && Player.blink < 0)
                        {
                            Player.lifeLevel -= Player.lifeLevel;

                            Player.Score += (int)chars[i].score;


                            explosions.Add(new Explosion()
                            {
                                position = chars[i].positions,
                                scale = new Vector2(chars[i].enemieDetails.scale.X),
                                color = new Color4(0.97f, 0.66f, 0.57f, 1f),
                                dir = new Vector2(dirX / 2, DirY / 2),

                            });

                            soundExplosion.Play();
                            DeleteChars(i);
                        }
                        else
                        {
                            
                            // colisao do tiro do player com o inimigo
                            for(int j = 0; j < ShotsPlayer.BoxCollide.Count; j++)
                            {
                                if(Collision.Detect(chars[i].BoxCollide, ShotsPlayer.BoxCollide[j]))
                                {
                                    var dano = ShotsPlayer.chars[Player.indexPlayer].damage + chars[i].DamageLife;
                                    chars[i].Life -= dano;

                                    points.Add(new Points($"-{(int)dano}", new Vector2(ShotsPlayer.BoxCollide[j].Z, chars[i].positions.Y),
                                        1.0f, Uses.RandomColors[rand.Next(0, Uses.MaxColors)], 110f, 
                                        new Vector2(0.0f),
                                        5.0f));

                                    // nave inimiga destruida
                                    if(chars[i].Life <= 0)
                                    {
                                        var score = (int)chars[i].score;
                                        Player.Score += score;

                                        points.Add(new Points($"+{score}", new Vector2(chars[i].positions.X, chars[i].positions.Y + 50f),
                                            1.5f, Color4.BlueViolet,
                                            170f, new Vector2(0.0f, 0.1f),
                                            5.0f));

                                        explosions.Add(new Explosion()
                                        {
                                            position = chars[i].positions,
                                            scale = new Vector2(chars[i].enemieDetails.scale.X),
                                            color = new Color4(0.97f, 0.66f, 0.57f, 1f),
                                            dir = new Vector2(dirX / 2, DirY / 2),

                                        });
                                        soundExplosion.Play();

                                        DeleteChars(i);
                                    }


                                    ShotsPlayer.Delete(j);
                                    break;
                                }
                            }

                        }
                    }
                    UpdateShots();
                }
            }
        }
        private struct Damage
        {
            public float TheShot, TheLife, score;
        }
        private Damage CalcDamages(float scale)
        {
            return new Damage()
            {
                TheShot = scale * 45f,
                TheLife = MathF.Pow(scale, -1) * 5,
                score = scale * 50,
            };
        }
        private float elapsedShoot = 0.0f;
        private void UpdateShots()
        {
            var Time = TimerGL.Time;

            if(Time - elapsedShoot > 1.0f)
            {
                elapsedShoot = Time;
                var rand_i = rand.Next(0, chars.Count);
                var randMax = rand.Next(1, chars.Count);

                for( int i = rand_i; i < randMax; i++)
                {
                    if(chars[i].BoxCollide.Z < Uses.Width)
                    {
                        var posFire = chars[i].enemieDetails.firePosition;
                        var vel = (float)rand.Next(5, 25) * 0.1f;

                        for( int j = 0; j < posFire.Count; j++)
                        {
                            fires.Add(new Fire()
                            {
                                position = new Vector3( chars[i].positions.X + posFire[j].X - 10.0f, chars[i].positions.Y + posFire[j].Y, 1.0f),
                            });
                            enemiesShots.Add( new EnemiessShot()
                            {
                                position = new Vector2(chars[i].positions.X + posFire[j].X, chars[i].positions.Y + posFire[j].Y),
                                scale = chars[i].enemieDetails.scale,
                                Damage = chars[i].DamageShot,
                                vel = vel,
                            });
                            EnemiesFire.Play();
                        }

                    }
                }
            }


            for( int i = 0; i < enemiesShots.Count; i++)
            {
                if(enemiesShots[i].BoxCollide.Z < 0)
                {
                    enemiesShots.RemoveAt(i);
                }
                else
                {
                    // colisao do disparos do inimigo com com o player
                    if(Collision.Detect(enemiesShots[i].BoxCollide, Player.BoxCollide) && Player.blink < 0)
                    {
                        Player.lifeLevel -= enemiesShots[i].Damage;


                        points.Add(new Points()
                        {
                            text = $"- {(int)enemiesShots[i].Damage}",
                            positionText = enemiesShots[i].position,
                            scale = 1.8f,
                            color = Color4.Red,
                            dir = new Vector2(0.0f, -0.1f),
                            speed = 180f,
                            Light = 5.0f,
                        });

                        soundSpark.Play();
                        sparks.Add(new Spark()
                        {
                            position = new Vector2(enemiesShots[i].BoxCollide.X, enemiesShots[i].position.Y),
                        });

                        enemiesShots.RemoveAt(i);
                    }

                }
            }
        }
        public static void DeleteAll()
        {
            chars.Clear();
            enemiesShots.Clear();
            sparks.Clear();
            explosions.Clear();
            EnemysDestruct = 0;
            boss.Clear();
        }
        private void DeleteChars(int index)
        {
            soundExplosion.Play();
            chars.RemoveAt(index);
            EnemysDestruct += 1;
        }

        public void Dispose()
        {
            soundExplosion.Dispose();
            soundSpark.Dispose();
            EnemiesFire.Dispose();
        }
        public void ConfigEnemies()
        {
            enemiesSmallDetails[0] = new EnemiesDetails()
            {
                texture = texturesSmall[0],
                scale = new Vector2(0.65f, 0.37f),
                firePosition = new List<Vector2>()
                {
                    new Vector2(-74.89f, -26.10f),
                    new Vector2(-96.89f, -24.10f),
                }
            };
            enemiesSmallDetails[1] = new EnemiesDetails()
            {
                texture = texturesSmall[1],
                scale = new Vector2(0.80f, 0.37f),
                firePosition = new List<Vector2>()
                {
                    new Vector2(-73.89f, -5.27f),
                    new Vector2(-105f, -2.27f),
                }
            };
            enemiesSmallDetails[2] = new EnemiesDetails()
            {
                texture = texturesSmall[2],
                scale = new Vector2(0.64f, 0.37f),
                firePosition = new List<Vector2>()
                {
                    new Vector2(-80.58f, -5.42f),
                    new Vector2(-105f, -6.56f),
                }
            };
            enemiesSmallDetails[3] = new EnemiesDetails()
            {
                texture = texturesSmall[3],
                scale = new Vector2(0.65f, 0.37f),
                firePosition = new List<Vector2>()
                {
                    new Vector2(-92.15f, -28.81f),
                    new Vector2(-76.89f, -31.09f),
                }
            };
            enemiesSmallDetails[4] = new EnemiesDetails()
            {
                texture = texturesSmall[4],
                scale = new Vector2(0.70f, 0.35f),
                firePosition = new List<Vector2>()
                {
                    new Vector2(-97.15f, -13.81f),
                    new Vector2(-72.46f, -14.97f),
                }
            };
            enemiesSmallDetails[5] = new EnemiesDetails()
            {
                texture = texturesSmall[5],
                scale = new Vector2(0.67f, 0.34f),
                firePosition = new List<Vector2>()
                {
                    new Vector2(-81.15f, -23.82f),
                }
            };

            enemiesSmallDetails[6] = new EnemiesDetails()
            {
                texture = texturesSmall[6],
                scale = new Vector2(0.60f, 0.37f),
                firePosition = new List<Vector2>()
                {
                    new Vector2(-0.85f, -2.71f),
                    new Vector2(-50.92f, -3.42f),
                }
            };
            enemiesSmallDetails[7] = new EnemiesDetails()
            {
                texture = texturesSmall[7],
                scale = new Vector2(0.60f, 0.37f),
                firePosition = new List<Vector2>()
                {
                    new Vector2(-80.88f, -17.54f),
                    new Vector2(-60.77f, -18.40f),
                }
            };


            // ----------------------------------------------
            enemiesMediumDetails[0] = new EnemiesDetails()
            {
                texture = texturesMedium[0],
                scale = new Vector2(1.25f, 0.51f),
                firePosition = new List<Vector2>()
                {
                    new Vector2(-143.58f, -5.20f),
                }
            };
            enemiesMediumDetails[1] = new EnemiesDetails()
            {
                texture = texturesMedium[1],
                scale = new Vector2(1.26f, 0.59f),
                firePosition = new List<Vector2>()
                {
                    new Vector2(-99.34f, -9.27f),
                }
            };
            enemiesMediumDetails[2] = new EnemiesDetails()
            {
                texture = texturesMedium[2],
                scale = new Vector2(1.26f, 0.59f),
                firePosition = new List<Vector2>()
                {
                    new Vector2(-57.98f, -12.34f),
                    new Vector2(-119.68f, 5.34f),
                }
            };
            enemiesMediumDetails[3] = new EnemiesDetails()
            {
                texture = texturesMedium[3],
                scale = new Vector2(1.22f, 0.60f),
                firePosition = new List<Vector2>()
                {
                    new Vector2(-136.37f, -52.49f),
                }
            };

        }
    }
}
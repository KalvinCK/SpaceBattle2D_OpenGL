using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MyGame
{
    public class Menu
    {
        public struct Bars
        {
            public string text;
            public Vector2 position;
            public Vector2 posText;
            public Color4 color;
            public bool tikSound;
        }
        public struct Arrow
        {
            public Vector2 position;
            public Vector2 scale;
            public bool tikSound;
        }
        private Texture BackMenu = new Texture("Resources/menu/back1.png");
        private Texture galaxy = new Texture("Resources/menu/galaxy.jpg");
        private Texture barTexture = new Texture("Resources/menu/barRed.png");
        private Texture texturesArrow = new Texture("Resources/menu/Arrow.png");
        private List<Texture> texturesPlayer { get => Uses.PlayersSprites; }
        private Shader shader = new Shader("SpaceGame/Menus/shaderMenu.vert", "SpaceGame/Menus/shaderMenu.frag");
        private Shader shaderBars = new Shader("SpaceGame/Menus/shaderMenu.vert", "SpaceGame/Menus/shaderBars.frag");
        private Sounds soundConfirm = new Sounds("Resources/menu/Menu-Confirm.wav");
        private Sounds soundError = new Sounds("Resources/menu/Menu-Error.wav");
        private Sounds soundMove = new Sounds("Resources/menu/Menu-Move.wav");
        private Sounds soundSelect = new Sounds("Resources/menu/Menu-Select.wav");

        public static GameMusic musicMenu = new GameMusic("Resources/menu/Sky-Game-Menu.ogg", 100);
        private float Width { get => Program.Size.X; }
        private float Height { get => Program.Size.Y; }
        
        private Bars []bars = new Bars[2];
        private Arrow []arrows = new Arrow[2];
        private Func<MouseButton, bool> input { get => Program.window.IsMouseButtonPressed; }
        public static bool renderMenu = true;
        public static bool End = false;

        public static EnemiesDetails[] enemiesDetails { get => Boss.enemiesBossDetails; }
        public Menu()
        {
            bars[0] = new Bars()
            {
                text = "Play",
                position = new Vector2(5.3f, 1.7f),
                posText = new Vector2(35f, 10f),
                color = Color4.AliceBlue,
                tikSound = true,
            };
            bars[1] = new Bars()
            {
                text = "Exit",
                position = new Vector2(5.3f, 2.1f),
                posText = new Vector2(35f, 10f),
                color = Color4.AliceBlue,
                tikSound = true,
            };

            
            
            musicMenu.Play();

        }
        public void RenderFrame()
        {
            if(End)
            {
                GameOver();
            }
            else
            {
                ImagenBack();
                RenderBars();
                Text.RenderText("Select Space ship", new Vector2(Width / 1.6f, Height / 1.4f), 2.0f, Color4.BlueViolet, 2.0f);
                Text.RenderText("Press F To FullScreen", new Vector2(Width / 2 - 150f, 50f), 1.0f, Color4.BlueViolet, 1.5f);

                RenderArrows();
                RenderPlayer();
            }

        }
        private Matrix4 projectionImage { get => Matrix4.CreateOrthographicOffCenter(-1f, 1, -1f, 1f, -10f, 10); }
        private void ImagenBack()
        {
            shader.Use();
            shader.SetUniform("projection", projectionImage);
            shader.SetUniform("Timer", TimerGL.Time * 0.05f);
            shader.SetUniform("moveTexture", true);
            var model = Matrix4.Identity;
            model = model * Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f);
            shader.SetUniform("model", model);
            shader.SetUniform("inputTexture", galaxy.Use);
            Quad.RenderQuad();

            model = Matrix4.Identity;
            model = model * Matrix4.CreateTranslation(0.0f, 0.0f, 0.1f);

            shader.SetUniform("moveTexture", false);
            shader.SetUniform("model", model);
            shader.SetUniform("inputTexture", BackMenu.Use);
            Quad.RenderQuad();
            
        }
        private Matrix4 projectionLayouts { get => Matrix4.CreateOrthographicOffCenter(0f, Program.Size.X, 0f, Program.Size.Y, -10f, 10f); }
        private void RenderBars()
        {

            for( int i = 0; i < bars.Length; i++)
            {
                shaderBars.Use();
                shaderBars.SetUniform("projection", projectionLayouts);
                shaderBars.SetUniform("color", Color4.White);
                shaderBars.SetUniform("inputTexture", barTexture.Use);
                var bar = bars[i];

                var position = new Vector2(Width / bar.position.X, Height / bar.position.Y);

                var model = Matrix4.Identity;
                model = model * Matrix4.CreateScale(135f, 27f, 1.0f);
                model = model * Matrix4.CreateTranslation(position.X, position.Y, 0.5f);
                shaderBars.SetUniform("model", model);

                var boxCollideBar = new Vector4(Collision.GetSizeBox(model, position, Vector4.Zero));

                var collide = Collision.DetectMouse(boxCollideBar);
                if(collide)
                {
                    model = Matrix4.Identity;
                    model = model * Matrix4.CreateScale(145f, 37f, 1.0f);
                    model = model * Matrix4.CreateTranslation(position.X, position.Y, 0.5f);
                    shaderBars.SetUniform("model", model);
                    shaderBars.SetUniform("Light", 2.0f);

                    if(bar.text == "Exit" && input(MouseButton.Button1))
                    {
                        soundSelect.Play();
                        Program.window.Close();

                    }
                    if(bar.text == "Play" && input(MouseButton.Button1))
                    {

                        soundSelect.Play();
                        musicMenu.Stop();
                        renderMenu = false;
                        Uses.musicGameplay.Play();
                    }

                    if(bars[i].tikSound)
                    {
                        soundMove.Play();
                        bars[i].tikSound = false;
                    }
                }
                else
                {
                    bars[i].tikSound = true;

                    shaderBars.SetUniform("Light", 1.0f);
                }
                Quad.RenderQuad();

                var postext = position - bar.posText;

                if(collide)
                {
                    Text.RenderText(bar.text, postext, 1.0f, bar.color, 10.0f);
                }
                else
                {
                    Text.RenderText(bar.text, postext, 1.0f, bar.color, 1.0f);
                }
            }


        }
        public void RenderArrows()
        {
            
            shaderBars.Use();
            shaderBars.SetUniform("projection", projectionLayouts);
            shaderBars.SetUniform("inputTexture", texturesArrow.Use);

            arrows[0] = new Arrow()
            {
                position = new Vector2(Width / 1.1f, Height / 2.2f),
                scale = new Vector2(35f, 35f),
            };
            arrows[1] = new Arrow()
            {
                position = new Vector2(Width / 1.7f, Height / 2.2f),
                scale = new Vector2(35f, 35f),
            };

            for( int i = 0; i < arrows.Length; i++)
            {
                var arrow = arrows[i];

                var model = Matrix4.Identity;
                if(i == 1)
                {
                    model = model * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(180f));
                }
                model = model * Matrix4.CreateScale(arrow.scale.X, arrow.scale.Y, 1.0f);
                model = model * Matrix4.CreateTranslation(arrow.position.X, arrow.position.Y, 0.5f);
                shaderBars.SetUniform("model", model);

                var boxCollide = Collision.GetSizeBox(model, arrow.position);


                if(Collision.DetectMouse(boxCollide))
                {
                    model = Matrix4.Identity;
                    if(i == 1)
                    {
                        model = model * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(180f));
                    }
                    model = model * Matrix4.CreateScale(45f, 45f, 1.0f);
                    model = model * Matrix4.CreateTranslation(arrow.position.X, arrow.position.Y, 0.5f);

                    shaderBars.SetUniform("model", model);
                    shaderBars.SetUniform("Light", 10.0f);
                    shaderBars.SetUniform("color", Color4.BlueViolet);

                    if(input(MouseButton.Button1))
                    {
                        if(i == 0)
                        {
                            Player.indexPlayer += 1;

                            if(Player.indexPlayer > texturesPlayer.Count - 1)
                            {
                                soundError.Play();
                                Player.indexPlayer = texturesPlayer.Count - 1;
                            }
                            else
                            {
                                soundConfirm.Play();
                            }
                        }
                        else
                        {
                            Player.indexPlayer -= 1;

                            if(Player.indexPlayer < 0)
                            {
                                soundError.Play();
                                Player.indexPlayer = 0;
                            }
                            else
                            {
                                soundConfirm.Play();
                            }
                        }
                    }
                    
                }
                else
                {
                    shaderBars.SetUniform("Light", 5.0f);
                    shaderBars.SetUniform("color", Color4.AliceBlue);
                }


                Quad.RenderQuad();
            }
        }
        private int indexPlayer { get => Player.indexPlayer; }
        private List<Vector2> positionsFire = new List<Vector2>();
        private List<Fire> fire = new List<Fire>();
        private float elapsedTime = 0f;
        public void RenderPlayer()
        {

            var plays = Player.players[indexPlayer];

            shaderBars.Use();
            shaderBars.SetUniform("projection", projectionLayouts);
            shaderBars.SetUniform("inputTexture", texturesArrow.Use);
            shaderBars.SetUniform("color", Color4.White);
            shaderBars.SetUniform("Light", 2.0f);
            shaderBars.SetUniform("inputTexture", Uses.PlayersSprites[indexPlayer].Use);

            var model = Matrix4.Identity;
            model = model * Matrix4.CreateScale(100f * plays.scale.X, 100f * plays.scale.Y, 1.0f);
            var positio = new Vector2(Width / 1.33f, Height / 2.14f);
            model = model * Matrix4.CreateTranslation(positio.X, positio.Y, 0.5f);

            shader.SetUniform("model", model);
            Quad.RenderQuad();

            var Time = TimerGL.Time;

            if(Time - elapsedTime > 0.1f)
            {
                elapsedTime = Time;
                for( int i = 0; i < plays.firePos.Count; i++)
                {
                    positionsFire.Add(new Vector2(positio.X + plays.firePos[i].X, positio.Y + plays.firePos[i].Y));
                    fire.Add( new Fire()
                    {
                        position = new Vector3( positio.X + plays.firePos[i].X + 10.0f, positio.Y + plays.firePos[i].Y, plays.firePos[i].Z),
                    });
                }
            }

            shader.SetUniform("inputTexture", ShotsPlayer.TextureLaser.Use);
            shaderBars.SetUniform("Light", 15.0f);
            shaderBars.SetUniform("color", plays.color);

            for( int i = 0; i < positionsFire.Count; i++)
            {
                positionsFire[i] += new Vector2(2.0f, 0.0f);

                if(positionsFire[i].X > Width + 200)
                {
                    positionsFire.RemoveAt(i);
                    break;
                }
                

                model = Matrix4.Identity;
                model = model * Matrix4.CreateScale(40f, 3f, 0.5f);
                model = model * Matrix4.CreateTranslation(positionsFire[i].X, positionsFire[i].Y, 0.6f);
                shader.SetUniform("model", model);

                Quad.RenderQuad();
            }

            for(int i = 0; i < fire.Count; i++)
            {
                if(fire[i].RenderFirePlayer())
                {
                    fire.RemoveAt(i);
                }
            }

        }
        public void UpdateFrame()
        {
            if(!End)
            {
                SelectPlayer();
            }
        }
        public void SelectPlayer()
        {
            var input = Program.window.IsKeyPressed;

            if(input(Keys.Right))
            {
                Player.indexPlayer += 1;

                if(Player.indexPlayer > texturesPlayer.Count - 1)
                {
                    soundError.Play();
                    Player.indexPlayer = texturesPlayer.Count - 1;
                }
                else
                {
                    soundConfirm.Play();
                }

            }
            if(input(Keys.Left))
            {
                Player.indexPlayer -= 1;

                if(Player.indexPlayer < 0)
                {
                    soundError.Play();
                    Player.indexPlayer = 0;
                }
                else
                {
                    soundConfirm.Play();
                }

            }

        }
        
        public void GameOver()
        {
            shader.Use();
            shader.SetUniform("projection", projectionImage);
            shader.SetUniform("Timer", TimerGL.Time * 0.05f);
            shader.SetUniform("moveTexture", true);

            var model = Matrix4.Identity;
            model = model * Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f);
            shader.SetUniform("model", model);
            shader.SetUniform("inputTexture", galaxy.Use);
            Quad.RenderQuad();

            Text.RenderText("Game Over", new Vector2(Width / 2 - 150f, Height / 2), 3.0f, Color4.BlueViolet, 2.0f);
            Text.RenderText("Press ENTER to continue or ESC to exit", new Vector2(Width / 2 - 150f, 50f), 1.0f, Color4.Red, 1.5f);

        }
        public void ReturnMenu()
        {
            var input = Program.window.IsKeyPressed;

            if( End && input(Keys.Escape))
            {
                Program.window.Close();
            }

            if( End && input(Keys.Enter))
            {

                Menu.renderMenu = true;
                Menu.musicMenu.Play();
                Uses.musicGameplay.Stop();
                Uses.musicBoss.Stop();
                Enemies.DeleteAll();
                Player.Clear();
                ShotsPlayer.Clear();
                Background.SetIndex();

                End = false;
            }
        }
        public void Dispose()
        {
            BackMenu.Dispose();
            shader.Dispose();
            shaderBars.Dispose();
            soundConfirm.Dispose();
            soundError.Dispose();
            soundMove.Dispose();
            soundSelect.Dispose();
            musicMenu.Dispose();
        } 
    }
}
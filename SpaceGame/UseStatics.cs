

using OpenTK.Mathematics;

namespace MyGame
{
    public class Uses
    {
        /// outers
        public static Matrix4 Projection2D { get => Matrix4.CreateOrthographicOffCenter(0f, Program.Size.X, 0f, Program.Size.Y, -500f, 500); }
        public static float Width { get => Program.Size.X; }
        public static float Height { get => Program.Size.Y; }

        // shaders
        public static Shader shader = new Shader("shaders/shader.vert", "shaders/shader.frag");
        public static Shader ShaderPLayer = new Shader("shaders/shader.vert", "shaders/player.frag");
        public static Shader shaderExplosions = new Shader("shaders/shader.vert", "shaders/shaderBum.frag");
        public static Shader shaderSpark = new Shader("shaders/shader.vert", "shaders/shaderSpark.frag");
        public static Shader shaderLife = new Shader("shaders/shader.vert", "shaders/shaderLife.frag");
        public static Shader shaderLines = new Shader("shaders/shader.vert", "shaders/shaderLines.frag");
        public static Texture textureLife = new Texture("Resources/outers/layoutLife.png");

        
        // textures
        public static List<Texture> PlayersSprites = new List<Texture>();
        public static List<Texture> enemiesSmallSprites = new List<Texture>();
        public static List<Texture> enemiesMediumSprites = new List<Texture>();
        public static List<Texture> enemiesBossSprites = new List<Texture>();
        public static Texture textureBulletsEnemies = new Texture("Resources/Laser Sprites/bulletsEnemys.png");

        public static Texture textureFireEnemies = new Texture("Resources/Laser Sprites/fireEnemie.png");
        public static Texture textureFirePlayer = new Texture("Resources/Laser Sprites/firePlayer.png");

        public static List<Texture> texturesExplosions = new List<Texture>();
        public static List<Texture> texturesBoom = new List<Texture>();
        public static List<Texture> texturesSpark = new List<Texture>();

        
        // sounds
        public static Sounds soundSpark = new Sounds("Resources/sounds/Distant Explosion.wav");
        public static GameMusic musicGameplay = new GameMusic("Resources/musics/music_theme.ogg", 60);
        public static GameMusic musicBoss = new GameMusic("Resources/musics/battle theme.ogg", 100);
        public static Sounds bigExplosion = new Sounds("Resources/sounds/bigExplosion.wav");


        //
        public static List<Color4> RandomColors = new List<Color4>();
        public static int MaxColors = 0;

        public void Init()
        {
            for(int i = 1; i < 5; i++)
            {
                PlayersSprites.Add(new Texture($"Resources/sprites player/{i}.png"));
            }
            for(int i = 1; i < 9; i++)
            {
                enemiesSmallSprites.Add(new Texture($"Resources/sprites enemys small/{i}.png"));
            }
            for( int i = 1; i < 5; i++)
            {
                enemiesMediumSprites.Add( new Texture($"Resources/sprites enemies medium/{i}.png"));
            }
            for( int i = 1; i < 5; i++)
            {
                enemiesBossSprites.Add( new Texture($"Resources/sprites enemies boss/{i}.png"));
            }

            for(int i = 1; i < 9; i++)
            {
                texturesExplosions.Add(new Texture($"Resources/magic explosion/{i}.png"));
            }

                

            for( int i = 5; i < 12; i++)
                texturesSpark.Add( new Texture($"Resources/spark/Dark-Bolt{i}.png"));


            for( int i = 60; i < 123; i++)
            {
                if(i < 100)
                {
                    texturesBoom.Add( new Texture($"Resources/Lave Blob/0000{i}.png"));
                }
                else
                {

                    texturesBoom.Add( new Texture($"Resources/Lave Blob/000{i}.png"));
                }

            }

            RandomColors.Add( Color4.AliceBlue);
            RandomColors.Add( Color4.White);
            RandomColors.Add( Color4.Red);
            RandomColors.Add( Color4.Green);
            RandomColors.Add( Color4.GreenYellow);
            RandomColors.Add( Color4.OrangeRed);
            RandomColors.Add( Color4.Blue);
            RandomColors.Add( Color4.BlueViolet);
            RandomColors.Add( Color4.Cyan);
            RandomColors.Add( Color4.Azure);
            RandomColors.Add( Color4.Gold);
            RandomColors.Add( Color4.YellowGreen);
            RandomColors.Add( Color4.Pink);
            RandomColors.Add( Color4.Tomato);
            RandomColors.Add( Color4.SkyBlue);

            MaxColors = RandomColors.Count;

        }
        public void Dispose()
        {
            shaderExplosions.Dispose();
            shaderSpark.Dispose();
            shader.Dispose();
            ShaderPLayer.Dispose();
            texturesExplosions.ForEach(i => i.Dispose());
            texturesSpark.ForEach(i => i.Dispose());

            musicGameplay.Dispose();
            musicBoss.Dispose();

            shaderLife.Dispose();
            textureLife.Dispose();
        }
    }
}
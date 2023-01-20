using OpenTK.Mathematics;


namespace MyGame
{
    public class Points
    {
        public Vector2 positionText = Vector2.Zero;
        public float scale = 0f;
        public Color4 color = Color4.White;
        public string text = string.Empty;
        public float speed = 0f;
        public Vector2 dir = Vector2.Zero;
        public float Light = 1.0f;
        public Points() {}
        public Points(string text, Vector2 position, float scale, Color4 color, float speed = 170f, Vector2 dir = new Vector2(), float Light = 1.0f)
        {
            this.text = text;
            this.positionText = position;
            this.scale = scale;
            this.color = color;
            this.speed = speed;
            this.dir = dir;
            this.Light = Light;
        }
        private float contTimer = 0f;
        public bool RenderFrame()
        {
            if(contTimer < 100f)
            {
                contTimer += TimerGL.ElapsedTime * speed;
                positionText += dir;
                Text.RenderText(text, positionText, scale, color, Light);

                return false;
            }
            else
            {
                return true;
            }
            
        }
    }
}
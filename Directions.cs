using OpenTK.Mathematics;

namespace MyGame
{
    public class Directions
    {
        private struct Movimento
        {
            private float vel;
            
            public float up;
            public float down;

            public float velMax;
            public float velMin;
            public Movimento(float max, float min, float up, float down)
            {
                vel = max;

                this.velMax = max;
                this.velMin = min;

                this.up = up;
                this.down = down;
            }
            public void UpdateValues(float max, float min, float up, float down)
            {
                this.velMax = max;
                this.velMin = min;

                this.up = up;
                this.down = down;
            }
            // Ganho de movimento
            public float upMove()
            {
                if(vel < velMax)
                {
                    vel += up;
                }
                return Exp(vel);
            }
            // perda de velocidade
            public float downMove()
            {
                if(vel > velMin)
                {
                    vel -= down;
                    return Exp(vel);
                }
                return 0;
            }
            // ganho de velocidade
            private float Exp(float x) => (float)Math.Pow(2, x);
        }
        private Movimento[] movimento = new Movimento[6];
        public Directions(float max, float min, float up, float down)
        {
            for(int i = 0; i < movimento.Length; i++)
            {
                movimento[i] = new Movimento(max, min, up, down);
            }
        }
        public void UpdateValues(float max, float min, float up, float down)
        {
            for(int i = 0; i < movimento.Length; i++)
            {
                movimento[i].UpdateValues(max, min, up, down);
            }
        }
        // Moving X
        public float X_positiveUp { get => (TimerGL.ElapsedTime * 500) * movimento[0].upMove(); } 
        public float X_positiveDowm { get => (TimerGL.ElapsedTime * 500) * movimento[0].downMove(); } 
        public float X_negativeUp { get => (TimerGL.ElapsedTime * 500) * movimento[1].upMove(); } 
        public float X_negativeDowm { get => (TimerGL.ElapsedTime * 500) * movimento[1].downMove(); } 
        // Moving X
        public float Y_positiveUp { get => (TimerGL.ElapsedTime * 500) * movimento[2].upMove(); } 
        public float Y_positiveDowm { get => (TimerGL.ElapsedTime * 500) * movimento[2].downMove(); } 
        public float Y_negativeUp { get => (TimerGL.ElapsedTime * 500) * movimento[3].upMove(); } 
        public float Y_negativeDowm { get => (TimerGL.ElapsedTime * 500) * movimento[3].downMove(); } 
        // Moving X
        public float Z_positiveUp { get => (TimerGL.ElapsedTime * 500) * movimento[4].upMove(); } 
        public float Z_positiveDowm { get => (TimerGL.ElapsedTime * 500) * movimento[4].downMove(); } 
        public float Z_negativeUp { get => (TimerGL.ElapsedTime * 500) * movimento[5].upMove(); } 
        public float Z_negativeDowm { get => (TimerGL.ElapsedTime * 500) * movimento[5].downMove(); } 
        

    }
}
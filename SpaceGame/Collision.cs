using OpenTK.Mathematics;

namespace MyGame
{
    class Collision
    {
        public static bool Detect(Vector4 boxA, Vector4 boxB)
        => (!(boxA.X > boxB.Z || boxA.Y < boxB.W || boxA.Z < boxB.X || boxA.W > boxB.Y));

        // public static Vector4 GetSizeBox(Matrix4 model, Vector2 position, Vector4 correctDif = new Vector4()) => new Vector4()
        // {
        //     X = position.X - model.M11 + correctDif.X,
        //     Z = position.X + model.M11 - correctDif.Z,

        //     Y = position.Y + model.M22 - correctDif.Y,
        //     W = position.Y - model.M22 + correctDif.W,
        // };
        private static Vector2 mousePos
        {
            get => new Vector2(Program.window.MousePosition.X, Uses.Height - Program.window.MousePosition.Y);
        }
        public static bool DetectMouse(Vector4 boxA)
        {
            Vector4 boxB  = new Vector4()
            {
                X = mousePos.X - 0.2f, 
                Y = mousePos.Y + 0.2f, 
                Z = mousePos.X + 0.2f, 
                W = mousePos.Y - 0.2f,
            };
            
            return (!(boxA.X > boxB.Z || boxA.Y < boxB.W || boxA.Z < boxB.X || boxA.W > boxB.Y));
        }
        public static Vector4 GetSizeBox(Matrix4 model, Vector2 position, Vector4 correctDif = new Vector4()) 
        {
            Vector4 box = new Vector4();
            
            if(model.M11 < 0)
            {
                box.X = position.X - model.M11 * -1 + correctDif.X;
                box.Z = position.X + model.M11 * -1 - correctDif.Z;
            }
            else
            {
                box.X = position.X - model.M11 + correctDif.X;
                box.Z = position.X + model.M11 - correctDif.Z;
            }

            if(model.M22 < 0)
            {
                box.Y = position.Y + model.M22 * -1 - correctDif.Y;
                box.W = position.Y - model.M22 * -1 + correctDif.W;
            }
            else
            {
                box.Y = position.Y + model.M22 - correctDif.Y;
                box.W = position.Y - model.M22 + correctDif.W;
            }
            
            return box;
        }
    }
}
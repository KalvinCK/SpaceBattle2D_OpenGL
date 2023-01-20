using Vector4 = System.Numerics.Vector4;
using System.Numerics;

namespace MyGame
{
    // the values 
    public struct Values
    {
        public static Vector4 lightColor = Vector4.One,
        fpsColor = Vector4.One,
        StencilColor = Vector4.One,
        crosshairColor = Vector4.One;

        public static float gammaBackground = 2.0f; 
        public static float interpolatedBack = 0.9f;


        public static float ForceLightScene = 25.0f;

        public static float stencilSize = 1.001f;


        // bloom
        public static bool isRenderBloom = true; 
        public static float new_bloom_exp = 1.0f;
        public static float new_bloom_streng = 0.4094488f;
        public static float new_bloom_gama = 0.446194f;
        public static float filterRadius = 0.0045801f;
        public static float new_bloom_filmGrain = 0.0475066f;
        public static float nitidezStrengh = 0.0246719f;
        public static int vibrance = 25;
        public static bool activeNegative = false;


        public static float posX = 0f;
        public static float posY = 0f;
        public static float scaleX = 1f;
        public static float scaleY = 1f;

        public static OpenTK.Mathematics.Vector4 Display;

        public struct playerVel
        {
            public static float velMax = 0.241f;
            public static float velMin = 0.187f;
            public static float velUp = 0.203f;
            public static float velDown = 0.011f;
        }

    }
}
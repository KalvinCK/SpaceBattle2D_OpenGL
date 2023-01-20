using ImGuiNET;

using Vector4 = System.Numerics.Vector4;

namespace MyGame
{
    public class ImGuiProgram
    {
        private static float hdr = 255f;
        private static Vector4 _lightColor = Vector4.One;
        public static void RenderFrame()
        {
            ImGui.StyleColorsClassic();

            ImGui.Begin("Scene Details");
            ImGui.Text($"Frames: {TimerGL.FramesForSecond} | Time: {TimerGL.Time.ToString("0.0")}");
            ImGui.NewLine();
            ImGui.ColorEdit4("Color Text Display", ref Values.fpsColor);
            
            ImGui.NewLine();
            ImGui.ColorEdit4("Color CrossHair", ref Values.crosshairColor);

            ImGui.NewLine();
            ImGui.SliderFloat("CubeMap Gamma", ref Values.gammaBackground, 0.1818182f, 2.0f, "%.7f");
            ImGui.SliderFloat("CubeMap Visibility", ref Values.interpolatedBack, 0.0f, 1.0f);


            ImGui.NewLine();
            ImGui.ColorEdit4("Color Light", ref _lightColor);
            ImGui.SliderFloat("Lights Scene Instencity", ref Values.ForceLightScene, 1.0f, 100f);
            Values.lightColor  = new Vector4(_lightColor.X * hdr, _lightColor.Y * hdr, 
                                            _lightColor.Z * hdr, _lightColor.W * hdr);

            
            ImGui.NewLine();
            ImGui.Checkbox("Enable Bloom", ref Values.isRenderBloom);

            if(Values.isRenderBloom)
            {
                ImGui.SliderFloat("Bloom Exposure", ref Values.new_bloom_exp, 0.0f, 1.0f);
                ImGui.SliderFloat("Bloom Strength", ref Values.new_bloom_streng, 0.0f, 1.0f, "%.7f");
                ImGui.SliderFloat("Bloom Gamma", ref Values.new_bloom_gama, 0.0f, 1.0f, "%.7f");
                ImGui.SliderFloat("Bloom Spacing Filter", ref Values.filterRadius, 0.0f, 0.01f, "%.7f");
                ImGui.SliderFloat("Bloom Film Grain", ref Values.new_bloom_filmGrain, -0.1f, 0.1f, "%.7f");
                ImGui.SliderFloat("Bloom Nitidez Strength", ref Values.nitidezStrengh, 0.0f, 0.2f, "%.7f");
                ImGui.SliderInt("Bloom Vibrance", ref Values.vibrance, -255, 100);
                ImGui.Checkbox("Active Negative", ref Values.activeNegative);

            }
            ImGui.End();



            ImGui.Begin("Position & Scale");
            ImGui.SliderFloat("Position X", ref Values.posX, 0f, 1500f, "%.2f");
            ImGui.SliderFloat("Position Y", ref Values.posY, 0f, 1f, "%.2f");
            ImGui.SliderFloat("Scale X", ref Values.scaleX, 0f, 1.0f, "%.7f");
            ImGui.SliderFloat("Scale Y", ref Values.scaleY, 0f, 1.0f, "%.7f");
            ImGui.End();

            ImGui.Begin("Player");
            ImGui.SliderFloat("Vel Max", ref Values.playerVel.velMax, 0.0f, 1.0f, "%.7f");
            ImGui.SliderFloat("Vel Min", ref Values.playerVel.velMin, 0.0f, 1.0f, "%.7f");
            ImGui.SliderFloat("Vel UP", ref Values.playerVel.velUp, 0.0f, 1.0f, "%.7f");
            ImGui.SliderFloat("Vel Down", ref Values.playerVel.velDown, 0.0f, 1.0f, "%.7f");
            ImGui.End();

            ImGui.Begin("Display Coliide");
            ImGui.SliderFloat("X", ref Values.Display.X, 0.0f, 100.0f);
            ImGui.SliderFloat("Y", ref Values.Display.Y, 0.0f, 100.0f);
            ImGui.SliderFloat("Z", ref Values.Display.Z, 0.0f, 100.0f);
            ImGui.SliderFloat("W", ref Values.Display.W, 0.0f, 100.0f);
            ImGui.End();
        }
    }
}
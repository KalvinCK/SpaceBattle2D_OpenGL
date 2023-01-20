using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MyGame
{
    public class Game : IDisposable
    {
        private Bloom bloom = new Bloom();
        private Uses ?uses = new Uses();
        private Menu menu = new Menu();
        private List<IEngine> engine = new List<IEngine>();
        public unsafe Game()
        {
            uses!.Init();

            engine.Add( new Background());
            engine.Add( new Player());
            engine.Add( new ShotsPlayer());
            engine.Add( new Enemies());
            engine.Add( new LinesEffect());
        }
        public void RenderFrame()
        {
            if(!Program.window.IsFocused)
                return;
            
            bloom.Active();

            if(Menu.renderMenu)
            {
                menu.RenderFrame();
            }
            else
            {
       
                foreach(var item in engine)
                    item.RenderFrame();

                var fpsPos = new Vector2(10.0f, Program.Size.Y - 50.0f);
                Text.RenderText($"{TimerGL.FramesForSecond.ToString()}", fpsPos, 1f, Values.fpsColor);
            }

            bloom.RenderFrame();


        }
        public void UpdateFrame()
        {
            if(!Program.window.IsFocused)
                return;


            if(Program.window.IsFocused)
            {
                Values.isRenderBloom = true;
            }
            else
            {
                Values.isRenderBloom = false;

            }

            menu.ReturnMenu();

            
            if(Menu.renderMenu)
            {
                menu.UpdateFrame();
            }
            else
            {
                foreach(var item in engine)
                    item.UpdateFrame();
            }
        }
        public void ResizedFrame()
        {
            if(!Program.window.IsFocused)
                return;
                
            bloom.ResizedFrameBuffer();
        }
        public void Dispose()
        {   
            uses!.Dispose();

            menu.Dispose();

            foreach(var item in engine)
                item.Dispose();
        }
    }
}


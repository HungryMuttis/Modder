using Modder;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Main
{
    public class Main : IGameMod
    {
        public Int16 Type { get { return 0; } } // This is a mod that adds the rendering
        public string Name { get { return "Test"; } } // The real name of the mod
        public string DisplayName { get { return "Test Game"; } } // The name of the mod(it will be used as the name of the application)
        public string Description { get { return "This is a test game"; } } // The description of the mod
        public string Version { get { return "1.0.0"; } } // The version of the mod
        public string Patch { get { return ""; } } // No patch for this version
        public(string,(string, string[]?)[])[] Required { get { return []; } } // No required mods or libraries
        public(string,(string, string[]?)[])[] Prohibited { get { return []; } } // No prohibited mods or libraries
    }
}

namespace PFG_Render
{
    public static class TestMod
    {

        public static void Main()
        {
            using GameWindow game = new(GameWindowSettings.Default, NativeWindowSettings.Default);
            game.RenderFrame +=(FrameEventArgs e) =>
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();

                GL.Begin(PrimitiveType.Quads);

                GL.Color3(1.0f, 0.0f, 0.0f); // Red
                GL.Vertex2(-0.5f, -0.5f); // Bottom left
                GL.Vertex2(0.5f, -0.5f); // Bottom right
                GL.Vertex2(0.5f, 0.5f); // Top right
                GL.Vertex2(-0.5f, 0.5f); // Top left

                GL.End();

                game.SwapBuffers();
            };

            game.Run();
        }
    }
}
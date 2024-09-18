using Modder;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Main
{
    public class Main : IGameMod
    {
        public ushort Type { get { return 4; } } // This is a mod that adds content to the game
        public string RealName { get { return "TestMod"; } } // The real name of the mod
        public string Name { get { return "Test mod"; } } // The name of the mod
        public string Description { get { return "This is a test mod."; } } // The description of the mod
        public string Version { get { return "1.0.0"; } } // The version of the mod
        public string Patch { get { return ""; } } // No patch for this version
        public (string, (string, string[]?)[])[] Required { get { return []; } } // No required mods or libraries
        public (string, (string, string[]?)[])[] Prohibited { get { return []; } } // No prohibited mods or libraries
    }
}

namespace PFG_Render
{
    public static class TestMod
    {

        public static void Main()
        {
            using GameWindow game = new(GameWindowSettings.Default, NativeWindowSettings.Default);
            game.RenderFrame += (FrameEventArgs e) =>
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
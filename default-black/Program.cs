using Modder;
using default_black;

namespace Main
{
    public class Main : IDesign
    {
        public event EventHandler<string>? AddLog;
        public event EventHandler<string>? NewLog;
        public event EventHandler<List<string>>? StartProgram;

        public void Start(List<Mod> mods, Dictionary<string, string> settings)
        {
            MainForm mainForm = new(mods, settings, AddLog, NewLog, StartProgram);
            Application.Run(mainForm);
        }
    }
}

/*namespace _
{
    public class _
    {
        /// <summary>
        /// The entry point of the program, just for it to be able to build
        /// </summary>
        [STAThread]
        public static void Main()
        {
            MessageBox.Show("This design has to be satred using 'Modder' https://github.com/HungryMuttis/Modder", "Failed to start");
        }
    }
}

/*
namespace default_black
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new frmMain([], []));
        }
    }
}
*/
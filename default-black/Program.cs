using Modder;
using default_black;

namespace Main
{
    public class Main : IDesign
    {
        public event EventHandler<string>? AddLog;
        public event EventHandler<string>? NewLog;
        public event EventHandler<List<string>>? StartProgram;

        public void Start(List<object> mods, Dictionary<string, string> settings)
        {
            frmMain mainForm = new(mods, settings);
            //ApplicationConfiguration.Initialize();
            Application.Run(mainForm);
        }
    }
}

namespace _
{
    public class _
    {
        [STAThread]
        public static void Main() { }
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
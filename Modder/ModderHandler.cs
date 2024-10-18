using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Modder
{
    internal partial class ModderHandler
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private IntPtr Ptr { get; }

        private LogHandler LogHandle { get; }
        private Dictionary<string, string> Settings { get; set; }
        private Form? DesignForm { get; set; } = null;
        private IDesign? Design { get; set; } = null;

        public ModderHandler(LogHandler logHandle, Dictionary<string, string> settings, IntPtr ptr)
        {
            this.LogHandle = logHandle;
            this.Settings = settings;
            this.Ptr = ptr;
            this.LogHandle.New("ModderHandler loaded", LogType.OK);
        }
        public void Launch(Type[] mods, Type designType)
        {
            this.LogHandle.New("Launching the Design", LogType.Info);
            ShowWindow(this.Ptr, Program.CMD_HIDE);
            try
            {
                try
                {
                    this.Design = (IDesign?)Activator.CreateInstance(designType);
                    if (this.Design == null)
                    {
                        Print($"Failed to build the found design", LogType.Fatal);
                        Utils.Error("Failed to build the found design");
                        Environment.Exit(0);
                    }
                    this.DesignForm = this.Design.Start(mods, this.Settings);
                }
                catch (Exception ex)
                {
                    Print($"Failed to build the found design\nError: {ex}", LogType.Fatal);
                    Utils.Error("Failed to build the found design");
                    Environment.Exit(0);
                }

                while (!this.DesignForm.Created)
                    Thread.Sleep(10);

                RichTextBox? rtb;

                while ((rtb = this.Design.GetTextBox()) == null)
                {
                    Console.WriteLine("RichTextBox was null, trying again after 10ms");
                    //Print("RichTextBox was null, trying again after 10ms", LogType.Info);
                    Thread.Sleep(10);
                }

                Print("Adding RichTextBox to log handler", LogType.Info);
                this.LogHandle.NewRichTextBox(rtb);
                Print("RichTextBox was added", LogType.OK);
            }
            catch (Exception e)
            {
                ShowWindow(this.Ptr, Program.CMD_SHOW);
                Print($"Design error: {e}", LogType.Fatal);
                Utils.Error("Fatal design error");
                //Console.ReadLine();
                throw;
            }
        }
        private static void Print(object txt, LogType type, string end = "\n")
        {
            Loader.Print(txt, type, end);
        }
    }
}

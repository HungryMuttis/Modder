namespace Modder
{
    internal partial class ModderHandler
    {
        private IntPtr Ptr { get; }

        private LogHandler LogHandle { get; }
        private Dictionary<string, string> Settings { get; set; }
        private Form? DesignForm { get; set; } = null;
        private IDesign? Design { get; set; } = null;
        private string? DesignSettings { get; set; }

        public ModderHandler(LogHandler logHandle, Dictionary<string, string> settings, IntPtr ptr)
        {
            this.LogHandle = logHandle;
            this.Settings = settings;
            this.Ptr = ptr;
            this.LogHandle.New("ModderHandler loaded", LogType.OK);
        }
        public void Launch(Type designType)
        {
            this.LogHandle.New("Launching the Design", LogType.Info);
            Data.ShowWindow(this.Ptr, Data.CMD_HIDE);
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
                    this.Design.Log += DesignLog;

                    if (Utils.CheckInvalidChars(this.Design.Name))
                    {
                        Print("The Design name has invalid characters", LogType.Fatal);
                        Utils.Error("The Design name has invalid characters");
                        Environment.Exit(0);
                    }

                    this.DesignSettings = Path.Combine(this.Settings["PATH:ModData"], this.Design.Name) + '\\';

                    this.DesignForm = this.Design.Start(this.Settings, this.DesignSettings);
                }
                catch (Exception ex)
                {
                    Print($"Failed to build the found design\nError: {ex}", LogType.Fatal);
                    Utils.Error("Failed to build the found design");
                    Environment.Exit(0);
                }

                while (!this.DesignForm.Created)
                    Thread.Sleep(10);

                this.DesignForm.Invoke(() => {
                    this.DesignForm.Text = $"Modder {Data.Version}";
                });

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
                Data.ShowWindow(this.Ptr, Data.CMD_SHOW);
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

        private void DesignLog(object obj, LogType type)
        {
            Print(obj, type);
        }
    }
}

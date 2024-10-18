using System.Data;
using System.Runtime.InteropServices;
using System.Xml;

namespace Modder
{
    internal static class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Loader main = new(GetConsoleWindow());
            main.Start();
        }
    }
    public partial class Loader : Form
    {
        private static LogHandler LogHandle { get; } = new();
        private ModderHandler ModderHandle { get; }

        internal IntPtr Ptr { get; }

        public string PATH { get; }
        public string HERE { get; }
        public Dictionary<string, string> Settings { get; set; }

        private Dictionary<int, List<Tuple<LogType, object?>>> HoldList { get; set; } = [];
        private SplashScreen? SplashScreen { get; set; }

        delegate int Test(int x);
        public Loader(IntPtr ptr)
        {
            Print("INIT started", LogType.Info);

            this.Ptr = ptr;
            Data.ShowWindow(this.Ptr, Data.CMD_SHOW);

            Print("Showing splash screen", LogType.Info);
            this.SplashScreenShow("Loading...");

            //Setting PATH and HERE values
            Print("Setting up DEFAULT variables", LogType.Info);
            this.PATH = Utils.SetupPATH();
            if (!Directory.Exists(this.PATH))
                Directory.CreateDirectory(this.PATH);
            this.HERE = AppDomain.CurrentDomain.BaseDirectory;

            string Xml = Data.DefaultXml;

            this.CheckXmlFile(Xml);

            //Config loading
            Print("Loading configs", LogType.Info);
            bool useDefSettings = false;

            XmlDocument doc = new();
            try
            {
                doc.Load(this.PATH + "main.xml");
            }
            catch (Exception e)
            {
                Print($"Failed to load user's XML settings due to an error:\n{e}", LogType.Error);
                Utils.Warn("Failed to load main.xml\nDefault XML will be loaded", "XML Error");
                doc.LoadXml(Xml);
                useDefSettings = true;
            }

            if (doc.DocumentElement == null)
            {
                if (useDefSettings)
                {
                    Print("Default settings are corrupted", LogType.Fatal);
                    Utils.Error("Default settings are corrupted\nFixing this issue requires rebuilding the application", "XML Error");
                    throw new XmlException("Default XML is corrupted");
                }
                Print("User settings file does not contain any settings", LogType.Warning);
                Print(" Using default settings", LogType.Info);
                useDefSettings = true;
                Utils.Warn("User settings file does not contain any settings. The default settings will be used", "XML Error");
            }

            XmlDocument defDoc = new();
            defDoc.LoadXml(Xml);

            if (defDoc.DocumentElement == null)
            {
                Print("Default settings are corrupted", LogType.Fatal);
                Utils.Error("Default settings are corrupted.\nFixing this issue requires rebuilding the application", "XML Error");
                throw new XmlException("Default settings are corrupted");
            }

            if (!useDefSettings)
            {
                if (doc.DocumentElement == null)
                {
                    Print("Unexpected error occoured at checking 'doc'", LogType.Fatal);
                    Utils.Error("Unexpected error occoured at checking 'doc'", "XML Error");
                    throw new XmlException("Unexpected error occoured at checking 'doc'");
                }

                if (!Utils.CheckXml(defDoc.DocumentElement, doc.DocumentElement))
                {
                    Print("User settings do not match the template", LogType.Warning);
                    Print("Using default settings", LogType.Info);
                    useDefSettings = true;
                    Utils.Warn("User settings do not match the template\nDefault settings will be used", "XML Warning");
                }
            }

            XmlDocument xmlSettings;

            if (useDefSettings)
                xmlSettings = defDoc;
            else
                xmlSettings = doc;
            
            if (xmlSettings.DocumentElement == null)
            {
                Print("Unexpected error occoured at checking 'xmlSettings'", LogType.Fatal);
                Utils.Error("Unexpected error occoured at checking 'xmlSettings'", "XML Error");
                throw new XmlException("Unexpected error occoured at checking 'xmlSettings'");
            }

            XMLInterpolator interpolator = new(this.PATH, this.HERE);

            Print("Interpolating settings", LogType.Info);
            this.Settings = interpolator.Interpolate(xmlSettings);

            Print("Interpolated settings:", LogType.None);
            foreach(KeyValuePair<string, string> kvp in this.Settings.AsEnumerable())
            {
                Print($" {kvp.Key} {kvp.Value}", LogType.None);

                if (kvp.Key.StartsWith("PATH:") && !Directory.Exists(kvp.Value))
                {
                    Directory.CreateDirectory(kvp.Value);
                    Print($" Created directory {kvp.Value}", LogType.Info, 0);
                }
            }
            PrintHold(0, "");

            this.ModderHandle = new(Loader.LogHandle, this.Settings, this.Ptr);

            Loader.LogHandle.NewFolder(this.Settings["PATH:Logs"], "", true, LogsRestoreMethod.LogFile);

            SplashScreenHide(true, true);
        }

        public void Start()
        {
            Print("Loading design", LogType.Info);
            Type design = this.GetDesign();
            Print("Design loaded", LogType.OK);
            Print("Loading mods", LogType.Info);
            Tuple<Type string>[] mods = this.GetMods();
            Print("Mods loaded", LogType.OK);
            Print("Starting the loaded design", LogType.Info);
            this.ModderHandle.Launch(mods, design);
        }

        private Type GetDesign()
        {
            Type? design = null;

            if (File.Exists(this.Settings["Params:Design"]))
                design = Utils.Load<IDesign>(this.Settings["Params:Design"]);

            if (design == null)
            {
                Print($"The selected desing was not found in {this.Settings["Params:Design"]}", LogType.Warning);
                foreach(string path in Directory.GetFiles(this.Settings["PATH:Designs"]))
                {
                    if (!path.EndsWith(".dll"))
                        continue;

                    Print($"Checking if {path} is a design", LogType.Info);

                    design = Utils.Load<IDesign>(path);

                    if (design == null)
                        continue;

                    break;
                }
            }

            if (design == null)
            {
                Print($"No design was found in {this.Settings["PATH:Designs"]}", LogType.Warning);
                foreach(string path in Directory.GetFiles(this.Settings["DEFAULT:HERE"]))
                {
                    if (!path.EndsWith(".dll"))
                        continue;

                    Print($"Checking if {path} is a design", LogType.Info);

                    Type? des = Utils.Load<IDesign>(path);

                    if (des == null)
                        continue;

                    design = des;

                    bool del = true;

                    if (File.Exists(this.Settings["PATH:Designs"] + path.Split(@"\")[^1]))
                        del = false;

                    File.Move(path, this.Settings["PATH:Designs"] + path.Split(@"\")[^1], del);

                    if (del)
                        File.Delete(path);

                    break;
                }
            }

            if (design == null)
            {
                Print("No design was found", LogType.Fatal);
                Utils.Error($"No viable design DLL was found\nGet the DLL and add it to the {this.Settings["PATH:Designs"]} directory, or to the EXE directory", "No design found");
                Environment.Exit(0);
            }

            return design;
        }

        private Tuple<Type, string>[] GetMods()
        {
            List<Tuple<Type?, string>> mods = [];

            foreach(string path in Directory.GetFiles(this.Settings["PATH:Mods"]))
            {
                if (!path.EndsWith(".dll"))
                    continue;

                mods.Add(new(Utils.Load<IGameMod>(path), path));
            }

            foreach(string path in Directory.GetFiles(this.Settings["DEFAULT:HERE"]))
            {
                if (!path.EndsWith(".dll"))
                    continue;

                mods.Add(new(Utils.Load<IGameMod>(path), path));
            }

            List<Tuple<Type, string>> realMods = [];

            foreach(Tuple<Type?, string> mod in mods)
                if (mod.Item1 != null)
                {
                    /*if (mod.Item1.RealName == "no_name")
                    {
                        Print("A potentially broken mod was found without a name", LogType.Warning);
                        if (Utils.Load<IDesign>(mod.Item2) != null)
                        {
                            Print("A design was detected as a mod. Ignoring it", LogType.Info);
                            continue;
                        }
                        DialogResult dRes = Utils.Warn("A potentially broken mod was found\nStill add it?", "Broken Mod", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                        if (dRes == DialogResult.Yes)
                        {
                            Print($" The mod was added located in {mod.Item2}", LogType.Info);
                            realMods.Add(new(mod.Item1, mod.Item2));
                        }
                    }
                    else
                    {*/
                    if (!File.Exists(this.Settings["PATH:Mods"] + mod.Item2.Split(@"\")[^1]))
                    {
                        File.Move(mod.Item2, this.Settings["PATH:Mods"] + mod.Item2.Split(@"\")[^1]);
                        File.Delete(mod.Item2);
                    }

                    Print($"Added mod {mod.Item1.Name} in {mod.Item2}", LogType.Info);

                    realMods.Add(new(mod.Item1, mod.Item2));
                    //}
                }

            return [.. realMods];
        }

        internal void SplashScreenShow(string bottomText, string name = "MODDER")
        {
            new Thread(() =>
            {
                SplashScreen = new(name, bottomText);
                Application.Run(SplashScreen);
            }).Start();
        }

        internal void SplashScreenHide(bool tryUntilSuccess = true, bool tryUntilNotNull = false)
        {
            while(true)
            {
                try
                {
                    if (SplashScreen != null)
                    {
                        SplashScreen.Invoke(new Action(SplashScreen.Close));
                        break;
                    }
                    else if (!tryUntilNotNull)
                        break;
                }
                catch
                {
                    if (!tryUntilSuccess)
                        break;
                }
            }
        }

        internal void CheckXmlFile(string Xml)
        {
            Print("Checking main.xml file", LogType.Info);
            if (!File.Exists(this.PATH + "main.xml"))
                File.WriteAllText(this.PATH + "main.xml", Xml);
        }
        internal static void Print(object? txt, LogType type, string? end = "\n")
        {
            if (end == "\n")
                end = Environment.NewLine;
            if (Loader.LogHandle != null && Loader.LogHandle.Usable)
                Loader.LogHandle.New($"{txt}{end}", type);
            Console.Write(txt);
            Console.Write(end);
        }
        internal void Print(object? txt, LogType type, int holdID, string? end = "\n")
        {
            if (end == "\n")
                end = Environment.NewLine;

            if (HoldList.TryGetValue(holdID, out List<Tuple<LogType, object?>>? value))
                value.Add(new(type, txt));
            else
                this.HoldList.Add(holdID, [new(type, txt + end)]);
        }

        internal void PrintHold(int holdID, string? end)
        {
            if (!this.HoldList.TryGetValue(holdID, out List<Tuple<LogType, object?>>? value))
                return;

            foreach(Tuple<LogType, object?> tpl in value)
                Print(tpl.Item2, tpl.Item1, end);

            this.HoldList.Remove(holdID);
        }
    }
}
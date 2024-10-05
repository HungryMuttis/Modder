using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using System.Xml.Xsl;
using Newtonsoft.Json;

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
            Main main = new(GetConsoleWindow());
            main.Start();
        }
    }
    public partial class Main : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        internal int CMD_HIDE { get; } = 0;
        internal int CMD_SHOW { get; } = 5;

        internal IntPtr Ptr { get; }

        public string PATH { get; }
        public string HERE { get; }
        public Dictionary<string, string> Settings { get; set; }

        private Dictionary<int, List<object?>> HoldList { get; set; } = [];
        private SplashScreen? SplashScreen { get; set; }
        private LogHandler LogHandle { get; }
        public Main(IntPtr ptr)
        {
            Print("INIT started");

            this.LogHandle = new();
            this.Ptr = ptr;
            ShowWindow(this.Ptr, 4);

            Print("Showing splash screen");
            this.SplashScreenShow("Loading...");

            //Setting PATH and HERE values
            Print("Setting up DEFAULT variables");
            this.PATH = Utils.SetupPATH();
            if (!Directory.Exists(this.PATH))
                Directory.CreateDirectory(this.PATH);
            this.HERE = AppDomain.CurrentDomain.BaseDirectory;

            string Xml =
"""
<?xml version="1.0"?>
<!--The main configuration for "Modder"-->
<!--At runtime "COMMON" will have "PATH" and "HERE-->
<!--"PATH" is the folder where this file should be stored(saved in an enviorment variable "MODDER_PATH")-->
<!--"HERE" is the folder in which the EXE is located-->
<xml>
    <PATH>
        <Designs>{DEFAULT:PATH}Designs\</Designs>
        <Settings>{DEFAULT:PATH}Settings\</Settings>
        <Mods>{DEFAULT:PATH}Mods\</Mods>
        <ModData>{DEFAULT:PATH}ModData\</ModData>
        <ModLists>{PATH:ModData}ModLists\</ModLists>
        <Logs>{DEFAULT:PATH}Logs\</Logs>
    </PATH>
    <Params>
        <Design>{PATH:Designs}default-black.dll</Design>
        <ModList>{PATH:ModLists}default.xml</ModList>
        <ModWarn>true</ModWarn>
    </Params>
</xml>
""";

            this.CheckXmlFile(Xml);

            //Config loading
            Print("Loading configs");
            bool useDefSettings = false;

            XmlDocument doc = new();
            try
            {
                doc.Load(this.PATH + "main.xml");
            }
            catch (Exception e)
            {
                Print("Failed to load default XML due to an error:");
                Print(e);
                Utils.Warn("Failed to load main.xml\nDefault XML will be loaded", "XML Error");
                doc.LoadXml(Xml);
                useDefSettings = true;
            }

            if (doc.DocumentElement == null)
            {
                if (useDefSettings)
                {
                    Print("Default settings are corrupted");
                    Utils.Error("Default settings are corrupted\nFixing this issue requires rebuilding the application", "XML Error");
                    throw new XmlException("Default XML is corrupted");
                }
                Print("User settings file does not contain any settings");
                Print("Using default settings");
                useDefSettings = true;
                Utils.Warn("User settings file does not contain any settings. The default settings will be used", "XML Error");
            }

            XmlDocument defDoc = new();
            defDoc.LoadXml(Xml);

            if (defDoc.DocumentElement == null)
            {
                Print("Default settings are corrupted");
                Utils.Error("Default settings are corrupted.\nFixing this issue requires rebuilding the application", "XML Error");
                throw new XmlException("Default settings are corrupted");
            }

            if (!useDefSettings)
            {
                if (doc.DocumentElement == null)
                {
                    Print("Unexpected error occoured at checking 'doc'");
                    Utils.Error("Unexpected error occoured at checking 'doc'", "XML Error");
                    throw new XmlException("Unexpected error occoured at checking 'doc'");
                }

                if (!Utils.CheckXml(defDoc.DocumentElement, doc.DocumentElement))
                {
                    Print("User settings do not match the template");
                    Print("Using default settings");
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
                Print("Unexpected error occoured at checking 'xmlSettings'");
                Utils.Error("Unexpected error occoured at checking 'xmlSettings'", "XML Error");
                throw new XmlException("Unexpected error occoured at checking 'xmlSettings'");
            }

            XMLInterpolator interpolator = new(this.PATH, this.HERE);

            Print("Interpolating settings");
            this.Settings = interpolator.Interpolate(xmlSettings);

            Print("Interpolated settings:");
            foreach(KeyValuePair<string, string> kvp in this.Settings.AsEnumerable())
            {
                Print($" {kvp.Key} {kvp.Value}");

                if (kvp.Key.StartsWith("PATH:") &&
                    !Directory.Exists(kvp.Value))
                {
                    Directory.CreateDirectory(kvp.Value);
                    Print($" Created directory {kvp.Value}", 0);
                }
            }

            Print(0, "\n");

            SplashScreenHide(true, true);
        }

        public void Start()
        {
            Print("Loading design");
            Design design = this.GetDesign();
            Print("Loading mods");
            List<Mod> mods = this.GetMods();
            Print("Starting the loaded design");
            this.StartDesign(mods, design);
        }

        private Design GetDesign()
        {
            Design? design = null;

            if (File.Exists(this.Settings["Params:Design"]))
                design = Utils.Load<Design>(this.Settings["Params:Design"]);

            if (design == null)
            {
                Print($"The selected desing was not found in {this.Settings["Params:Design"]}");
                foreach(string path in Directory.GetFiles(this.Settings["PATH:Designs"]))
                {
                    if (!path.EndsWith(".dll"))
                        continue;

                    Print($"Checking if {path} is a design");

                    design = Utils.Load<Design>(path);

                    if (design == null)
                        continue;

                    break;
                }
            }

            if (design == null)
            {
                Print($"No design was found in {this.Settings["PATH:Designs"]}");
                foreach(string path in Directory.GetFiles(this.Settings["DEFAULT:HERE"]))
                {
                    if (!path.EndsWith(".dll"))
                        continue;

                    Print($"Checking if {path} is a design");

                    Design? des = Utils.Load<Design>(path);

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
                Print("No design was found");
                Utils.Error("No viable design DLL was found\nGet the DLL and add it to the PATH:DESIGNS directory, or to the EXE directory", "No design found");
                throw new DesignException("No design was found");
            }

            return design;
        }

        private List<Mod> GetMods()
        {
            List<Mod?> mods = [];

            foreach(string path in Directory.GetFiles(this.Settings["PATH:Mods"]))
            {
                if (!path.EndsWith(".dll"))
                    continue;

                mods.Add(Utils.Load<Mod>(path));
            }

            foreach(string path in Directory.GetFiles(this.Settings["DEFAULT:HERE"]))
            {
                if (!path.EndsWith(".dll"))
                    continue;

                mods.Add(Utils.Load<Mod>(path));
            }

            List<Mod> realMods = [];

            foreach(Mod? mod in mods)
                if (mod != null)
                    if (mod.RealName == "no_name")
                    {
                        Print("A potentially broken mod was found without a name");
                        if (Utils.Load<Design>(mod.Path) != null)
                        {
                            Print("A design was detected as a mod. Ignoring it");
                            continue;
                        }
                        DialogResult dRes = Utils.Warn("A potentially broken mod was found\nStill add it?", "Broken Mod", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                        if (dRes == DialogResult.Yes)
                        {
                            Print($" The mod was added located in {mod.Path}");
                            realMods.Add(mod);
                        }
                    }
                    else
                    {
                        if (!File.Exists(this.Settings["PATH:Mods"] + mod.Path.Split(@"\")[^1]))
                        {
                            File.Move(mod.Path, this.Settings["PATH:Mods"] + mod.Path.Split(@"\")[^1]);
                            File.Delete(mod.Path);
                        }

                        Print($"Added mod {mod.Name} in {mod.Path}");

                        realMods.Add(mod);
                    }

            return realMods;
        }
        private void StartDesign(List<Mod> mods, Design design)
        {
            LogHandle.New(LogType.Info, "test");
            LogHandle.NewRichTextBox(new(), true);
            Console.ReadLine(); ///////////////////////////////////////////////////////////////////////////
            ShowWindow(this.Ptr, 0);
            try
            {
                design.Start(mods, this.Settings);
            }
            catch (Exception e)
            {
                ShowWindow(this.Ptr, 4);
                Print($"Design error: {e}");
                Utils.Error("Fatal design error");
                throw;
            }
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
            Print("Checking main.xml file");
            if (!File.Exists(this.PATH + "main.xml"))
                File.WriteAllText(this.PATH + "main.xml", Xml);
        }

        internal static void Print(object? txt, string? end = "\n", bool _ = false)
        {
            Console.Write(txt);
            Console.Write(end);
        }
        internal void Print(object? txt, int holdID, string _ = "")
        {
            if (this.HoldList.TryGetValue(holdID, out List<object?>? list))
            {
                list.Add(txt);
                this.HoldList[holdID] = list;
            }
            else
                this.HoldList.Add(holdID, [txt]);
        }

        internal void Print(int holdID, string? end, int _ = 0)
        {
            if (!this.HoldList.TryGetValue(holdID, out List<object?>? list))
                return;

            foreach(object? txt in list)
                Print(txt, end);

            this.HoldList.Remove(holdID);
        }
    }
}
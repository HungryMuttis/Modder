using System.Data;
using System.Runtime.InteropServices;
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
            Main main = new(GetConsoleWindow());
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
        public Main(IntPtr ptr)
        {
            //INIT started
            Print("INIT started");

            Ptr = ptr;

            //Starting the splach screen
            Print("Showing splash screen");
            SplashScreen? splashScreen = null;
            new Thread(() =>
            {
                splashScreen = new();
                Application.Run(splashScreen);
            }).Start();

            //Setting up
            Print("Setting up ENV variables");
            string? path = Environment.GetEnvironmentVariable("MODDER_PATH", EnvironmentVariableTarget.User);

            if (path == null)
            {
                path = @"C:\ProgramData\Modder\";
                Environment.SetEnvironmentVariable("MODDER_PATH", path, EnvironmentVariableTarget.User);
            }

            //Setting PATH and HERE values
            Print("Setting up DEFAULT variables");
            this.PATH = path;
            if (!Directory.Exists(this.PATH))
                Directory.CreateDirectory(this.PATH);
            this.HERE = AppDomain.CurrentDomain.BaseDirectory;

            string Xml =
"""
<?xml version="1.0"?>
<!--The main configuration for "Modder"-->
<!--At runtime "COMMON" will have "PATH" and "HERE-->
<!--"PATH" is the folder where this file should be stored (saved in an enviorment variable "MODDER_PATH")-->
<!--"HERE" is the folder in which the EXE is located-->
<xml>
    <PATH>
        <Designs>{DEFAULT:PATH}Designs\</Designs>
        <Settings>{DEFAULT:PATH}Settings\</Settings>
        <Mods>{DEFAULT:PATH}Mods\</Mods>
        <ModData>{DEFAULT:PATH}ModData\</ModData>
        <Logs>{DEFAULT:PATH}Logs\</Logs>
    </PATH>
    <Params>
        <Design>{PATH:Designs}default-black.dll</Design>
    </Params>
</xml>
""";

            //Check if main.xml file exists
            Print("Checking main.xml file");
            if (!File.Exists(this.PATH + "main.xml"))
                File.WriteAllText(this.PATH + "main.xml", Xml);

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

                if (!CheckXml(defDoc.DocumentElement, doc.DocumentElement))
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
            foreach (KeyValuePair<string, string> kvp in this.Settings.AsEnumerable())
                Print($" {kvp.Key} {kvp.Value}");

            // Closing the splash screen
            while (true)
            {
                try
                {
                    if (splashScreen != null)
                    {
                        splashScreen.Invoke(new Action(splashScreen.Close));
                        break;
                    }
                }
                catch { }
            }
        }

        public static void Print(object? txt, string? end = "\n")
        {
            Console.Write(txt);
            Console.Write(end);
        }

        static bool CheckXml(XmlNode defDoc, XmlNode doc)
        {
            foreach (XmlNode defNode in defDoc.ChildNodes)
            {
                bool ex = false;
                foreach (XmlNode node in doc.ChildNodes)
                {
                    if (defNode.Name == node.Name)
                    {
                        ex = true;
                        if (defNode.ChildNodes.Count > 0)
                            ex = CheckXml(defNode, node);
                        break;
                    }
                }
                if (!ex)
                    return false;
            }
            return true;
        }

        static void LoadMods(string dir)
        {
            try
            {
                foreach (string mod in Directory.GetFiles(dir))
                {
                    Utils.LoadMod(mod);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in loading mods. Error stack: {e}", "Loading Error");
            }
        }
        /*static void InterpolateXmlSettings(XmlNode doc)
        {
            foreach (XmlNode node in doc.ChildNodes)
            {
                if (node.ChildNodes.Count > 0)
                {
                    InterpolateXmlSettings(node);
                }
                if (node.Value != null)
                {
                    string val = Utils.Replace(kvp.Value, settings);
                    settings.Add(kvp.Key, val);
                    Console.WriteLine($"key: {kvp.Key}, val: {val}");
                }
            }
        }*/
    }
}
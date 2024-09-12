using System.Data;
using System.Xml;
using Newtonsoft.Json;

namespace Modder
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Main _ = new();
        }
    }
    public partial class Main
    {
        public string PATH { get; }
        public Dictionary<string, string> Settings { get; set; }
        public Main()
        {
            //Starting the splach screen
            SplashScreen? splashScreen = null;
            new Thread(() =>
            {
                splashScreen = new();
                Application.Run(splashScreen);
            }).Start();

            //Setting up
            string? path = Environment.GetEnvironmentVariable("MODDER_PATH", EnvironmentVariableTarget.User);

            if (path == null)
            {
                path = @"C:\ProgramData\Modder\";
                Environment.SetEnvironmentVariable("MODDER_PATH", path, EnvironmentVariableTarget.User);
            }

            this.PATH = path;
            if (!Directory.Exists(this.PATH))
                Directory.CreateDirectory(this.PATH);

            string Xml =
"""
<?xml version="1.0"?>
<!--The main configuration for "Modder"-->
<!--At runtime "COMMON" will have "PATH" and "HERE-->
<!--"PATH" is the folder where this file should be stored (saved in an enviorment variable "MODDER_PATH")-->
<!--"HERE" is the folder in which the EXE is located-->
<xml>
    <COMMON>
    </COMMON>
    <PATH>
        <Designs>
            {DEFAULT:PATH}Designs\
        </Designs>
        <Settings>
            {DEFAULT:PATH}Settings\
        </Settings>
        <Mods>
            {DEFAULT:PATH}Mods\
        </Mods>
        <ModData>
            {DEFAULT:PATH}ModData\
        </ModData>
        <Logs>
            {DEFAULT:PATH}Logs\
        </Logs>
    </PATH>
    <Params>
        <Desing>
            {PATH:DESIGNS}default-black.dll
        </Desing>
    </Params>
</xml>
""";

            if (!File.Exists(this.PATH + "main.xml"))
                File.WriteAllText(this.PATH + "main.xml", Xml);

            //Config loading
            bool useDefSettings = false;

            XmlDocument doc = new();
            try
            {
                doc.Load(this.PATH + "main.xml");
            }
            catch
            {
                Utils.Warn("Failed to load main.xml\nDefault XML will be loaded", "XML Error");
                doc.LoadXml(Xml);
                useDefSettings = true;
            }

            if (doc.DocumentElement == null)
            {
                if (useDefSettings)
                {
                    Utils.Error("Default settings are corrupted\nFixing this issue requires rebuilding the application", "XML Error");
                    throw new XmlException("Default XML is corrupted");
                }
                Utils.Warn("Settings file does not contain any settings. The default settings will be used", "XML Error");
            }

            XmlDocument defDoc = new();
            defDoc.LoadXml(Xml);


            if (defDoc.DocumentElement == null)
            {
                Utils.Error("Default settings are corrupted.\nFixing this issue requires rebuilding the application", "XML Error");
                throw new XmlException("Default settings are corrupted");
            }

            if (!useDefSettings)
            {
                if (doc.DocumentElement == null)
                {
                    Utils.Error("Unexpected error occoured at checking settings", "XML Error");
                    throw new XmlException("Unexpected error occoured ate checking settins");
                }

                if (!CheckXml(defDoc.DocumentElement, doc.DocumentElement))
                {
                    Utils.Warn("Settings do not match the template\nDefault settings will be used", "XML Warning");
                    useDefSettings = true;
                }
            }

            if (useDefSettings)
            {

            }

            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            
            /*
            try
            {

                foreach (KeyValuePair<string, string?> kvp in settingsIni.AsEnumerable())
                {
                    if (kvp.Value != null)
                    {
                        string val = Utils.Replace(kvp.Value, settings);
                        settings.Add(kvp.Key, val);
                        Console.WriteLine($"key: {kvp.Key}, val: {val}");
                    }
                }

                Console.WriteLine();

                foreach (KeyValuePair<string, string> kvp in settings.AsEnumerable())
                {
                    if (kvp.Key.StartsWith("PATH:") &&
                        !Directory.Exists(kvp.Value))
                    {
                        Console.WriteLine($"Creating directory: {kvp.Value}");
                        Directory.CreateDirectory(kvp.Value);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error in loading configs. Error stack: {e}", "Loading Error");
            }

            this.Settings = settings;

            // Checking if the design exists (default: default_black.dll)
            //if (this.Settings.)*/

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
    }
}
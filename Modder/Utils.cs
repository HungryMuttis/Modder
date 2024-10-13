using System.Reflection;
using System.Text;
using System.Xml;

namespace Modder
{
    public static class Utils
    {
        public static object? RunFunction(Type mod, string funcName, params object[]? parameters)
        {
            return mod.GetMethod(funcName)?.Invoke(Activator.CreateInstance(mod), parameters);
        }
        public static object? GetProperty(Type mod, string propName)
        {
            return mod.GetProperty(propName)?.GetValue(Activator.CreateInstance(mod));
        }
        public static void Error(string message, string caption = "Error")
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void Warn(string message, string caption = "Warning")
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static DialogResult Warn(string message, string caption = "Warning", MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK, MessageBoxDefaultButton messageBoxDefaultButton = MessageBoxDefaultButton.Button1)
        {
            return MessageBox.Show(message, caption, messageBoxButtons, MessageBoxIcon.Warning, messageBoxDefaultButton);
        }

        /// <summary>
        /// Interpolates the given value with the given replacements
        /// </summary>
        /// <param name="value">string that will be interpolated</param>
        /// <param name="replacements">all the different possible replacements</param>
        /// <returns></returns>
        public static string Interpolate(string value, Dictionary<string, string> replacements)
        {
            StringBuilder newValue = new();
            StringBuilder toInterp = new();

            bool interp = false;
            bool cl = false;
            foreach(char c in value)
            {
                if (c == '{')
                {
                    if (!interp)
                        interp = true;
                    else
                    {
                        interp = false;
                        newValue.Append('{');
                    }
                }
                else if (c == '}')
                {
                    if (interp)
                    {
                        interp = false;
                        string word = toInterp.ToString();
                        foreach(KeyValuePair<string, string> kvp in replacements.AsEnumerable())
                            if (word == kvp.Key)
                            {
                                word = kvp.Value;
                                break;
                            }

                        newValue.Append(word);
                        toInterp.Clear();
                    }
                    else if (cl)
                        cl = false;
                    else
                    {
                        cl = true;
                        newValue.Append('}');
                    }
                }
                else if (interp)
                    toInterp.Append(c);
                else
                    newValue.Append(c);
            }

            return newValue.ToString();
        }
        public static bool CheckXml(XmlNode defDoc, XmlNode doc)
        {
            foreach(XmlNode defNode in defDoc.ChildNodes)
            {
                bool ex = false;
                foreach(XmlNode node in doc.ChildNodes)
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
        public static string SetupPATH()
        {
            string? path = Environment.GetEnvironmentVariable("MODDER_PATH", EnvironmentVariableTarget.User);

            if (path != null)
                return path;

            // First time introduction
            MessageBox.Show("Hello there!\nAs I see, this is your first time trying Modder", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MessageBox.Show("This program is made to be used by a community\nIt can do anything with the right mod(s) installed\nAnd sadly, that anything means malicuos mods can harm your PC", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult rs = MessageBox.Show("Press NO if:\n  You don't know what mods you are installing\n  Someone else wants you to run specific design and/or mod(s)\n  You wish to stop Modder from running\nPress YES if:\n  You know what you are doing\n  You have checked that the mod(s)/design you are about to load doesn't conatin any viruses\n\nNote: after starting Modder, this popup will never appear again", "Do you wish to run Modder", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (rs == DialogResult.No)
                Environment.Exit(0);

            Main.Print("Setting up ENV variables", LogType.Info);
            path = @"C:\ProgramData\Modder\";
            Environment.SetEnvironmentVariable("MODDER_PATH", path, EnvironmentVariableTarget.User);

            return path;

        }
        /*public static Mod? LoadMod(string path)
        {
            Assembly loadedAssembly = Assembly.LoadFile(path);

            Type selfIGameMod = typeof(IGameMod);
            Type? modType = loadedAssembly.GetType("Main.Main");

            if (modType == null)
                return null;

            if (!Utils.CheckInterface(selfIGameMod, modType))
                return null;

            Main.Print($"{path} implements {selfIGameMod}");

            return new Mod(modType, path);
        }
        public static Design? LoadDesign(string design)
        {
            Assembly loadedAssembly = Assembly.LoadFile(design);

            Type selfIDesign = typeof(IDesign);
            Type? designType = loadedAssembly.GetType("Main.Main");

            if (designType == null)
                return null;

            if (!Utils.CheckInterface(selfIDesign, designType))
                return null;

            Main.Print($"{design} implements {selfIDesign}");
            
            return new Design(designType);
        }*/
        public static Type? Load<T>(string path) where T : class
        {
            try
            {
                Type? dllType;
                if ((dllType = Assembly.LoadFile(path).GetTypes().FirstOrDefault(t => typeof(T).IsAssignableFrom(t))) == null)
                    return null;
                return dllType;
            }
            catch (Exception ex)
            {
                Main.Print(ex, LogType.Warning);
                Main.Print(path, LogType.Warning);
                return null;
            }
        }

        /// <summary>
        /// Splits the given string by the given chars, also, keeps the split chars in the string (at the end)
        /// </summary>
        /// <param name="chars">The chars that will split the string</param>
        /// <returns></returns>
        public static string[] SplitFull(string str, params char[] chars)
        {
            StringBuilder sb = new();
            List<string> strs = [];

            foreach (char ch in str)
            {
                if (chars.Contains(ch))
                {
                    sb.Append(ch);
                    strs.Add(sb.ToString());
                    sb.Clear();
                }
                else
                    sb.Append(ch);
            }

            if (sb.Length > 0)
                strs.Add(sb.ToString());

            return [.. strs];
        }
    }
}
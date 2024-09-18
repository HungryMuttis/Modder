using System.Reflection;
using System.Runtime.CompilerServices;
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
            foreach (char c in value)
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
                        foreach (KeyValuePair<string, string> kvp in replacements.AsEnumerable())
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
        public static string SetupPATH()
        {
            Main.Print("Setting up ENV variables");
            string? path = Environment.GetEnvironmentVariable("MODDER_PATH", EnvironmentVariableTarget.User);

            if (path == null)
            {
                path = @"C:\ProgramData\Modder\";
                Environment.SetEnvironmentVariable("MODDER_PATH", path, EnvironmentVariableTarget.User);
            }

            return path;
        }
        public static bool CheckInterface(Type selfInterface, Type interfaceType)
        {
            if (!selfInterface.GetProperties().Select(p => p).SequenceEqual(selfInterface.GetProperties().Select(p => p)) ||
                !selfInterface.GetMethods().Select(m => m).SequenceEqual(selfInterface.GetMethods().Select(m => m)))
                return false;

            Main.Print($"{interfaceType} implements {selfInterface}");
            return true;
        }
        public static Mod? LoadMod(string mod)
        {
            Assembly loadedAssembly = Assembly.LoadFile(mod);

            Type selfIGameMod = typeof(IGameMod);
            Type? modType = loadedAssembly.GetType("Main.Main");

            if (modType == null)
                return null;

            if (!Utils.CheckInterface(selfIGameMod, modType))
                return null;

            if (Activator.CreateInstance(modType) == null)
                return null;

            return new Mod(modType);
        }
        public static Design? LoadDesign(string design)
        {
            Assembly loadedAssembly = Assembly.LoadFile(design);

            Type selfIDesign = typeof(IGameMod);
            Type? designType = loadedAssembly.GetType("Main.Main");

            if (designType == null)
                return null;

            if (!Utils.CheckInterface(selfIDesign, designType))
                return null;

            if (Activator.CreateInstance(designType) == null)
                return null;

            return new Design(designType);
        }
    }
}
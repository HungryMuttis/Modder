using System.Linq;
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
        public static bool CheckInterface(Type selfInterface, Type dllType, string? path = null)
        {
            foreach (MemberInfo requiredMember in selfInterface.GetMembers())
                if (dllType.GetMember(requiredMember.Name).Length == 0)
                {
                    Console.WriteLine($"{(string.IsNullOrEmpty(path) ? dllType : path)} does not implement the required member: {requiredMember.Name}");
                    return false;
                }

            return true;
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
        public static T? Load<T>(string path) where T : class
        {
            Type[] selfInterfaces = typeof(T).GetInterfaces();
            Type selfInterface = selfInterfaces[0];

            if (selfInterface == null)
            {
                Main.Print($"Given class does not implement any intercafes");
                throw new ArgumentException("Given class does not implement any interfaces");
            }

            Assembly loadedAssembly = Assembly.LoadFile(path);
            Type? dllType = loadedAssembly.GetType("Main.Main");

            if (dllType == null)
                return null;

            if (!Utils.CheckInterface(selfInterface, dllType, path))
                return null;

            return (T)Activator.CreateInstance(typeof(T), dllType, path)!;/////////////////////////////////////////////////////////////////////////////////////
        }
    }
}
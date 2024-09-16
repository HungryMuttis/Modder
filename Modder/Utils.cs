using System.Reflection;
using System.Text;

namespace Modder
{
    public static class Utils
    {
        public static object? RunFunction(Type mod, string funcName, object[]? parameters = null)
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
        public static (bool, Mod?) LoadMod(string mod)
        {
            Assembly loadedAssembly = Assembly.LoadFile(mod);

            Type selfIGameMod = typeof(IGameMod);
            Type? modIGameMod = loadedAssembly.GetType("Main.IGameMod");
            Type? modType = loadedAssembly.GetType("Main.Main");

            if (modIGameMod == null || modType == null)
                return (false, null);

            if (!selfIGameMod.GetProperties().Select(p => p.Name).SequenceEqual(modIGameMod.GetProperties().Select(p => p.Name)) || !selfIGameMod.GetMethods().Select(m => m.Name).SequenceEqual(modIGameMod.GetMethods().Select(m => m.Name)))
                return (false, null);

            if (!modIGameMod.IsAssignableFrom(modType))
                return (false, null);

            Console.WriteLine("The class fully implements the IGameMod interface.");

            object? instance = Activator.CreateInstance(modType);

            if (instance == null)
                return (false, null);

            Console.WriteLine((string?)RunFunction(modType, "DoSomething"));
            return (true, new Mod(modType));
        }
    }
}
using System.Reflection;

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
        /// **NOT PERFECT** (fails to correctly format variables surrounded by more than two {})
        /// <br></br>
        /// Modifies the string like string.Format(), but with correct variable selection system
        /// </summary>
        /// <param name="input">
        /// string that is going to be modified
        /// </param>
        /// <param name="replacements">
        /// array containing all of the replacements
        /// </param>
        /// <returns></returns>
        public static string Replace(string input, Dictionary<string, string> replacements)
        {
            string placeholder = Guid.NewGuid().ToString();
            input = input.Replace("{{", "{" + placeholder).Replace("}}", placeholder + "}");
            foreach (KeyValuePair<string, string> kvp in replacements)
                input = input.Replace("{" + kvp.Key + "}", kvp.Value);
            return input.Replace("{" + placeholder, "{").Replace(placeholder + "}", "}");
        }
        /*public static string Interpolate(string str, Dictionary<string, string> replacements)
        {

        }*/
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
namespace Modder
{
    public enum LogType
    {
        None,
        Info,
        OK,
        Warning,
        Error,
        Critical,
        Fatal
    }
    public delegate void NewLog(string message, LogType logType);
    public interface IGameMod
    {
        Int16 Type { get; } // A variable to know what type of .dll is this
        /*
         0. Core render     + only 1 can be mounted at a time(should not include any contend, only the rendering)
         1. Core content    + a mod that adds the core features of a game(can be multiple mounted at once)
         2. Library         - a library that others mods use
         3. Core render mod - a mod that expands the rendering
         4. Mod             - a mod that adds content to the game
         */
        string RealName { get; } // The real name of the mod(can be anything. The real difference between [Name] is that this will be the name program refers to this mod between other mods)
        string Name { get; } // The name of the mod. IF THE MOD TYPE IS `Core render` THIS WILL BE USED AS THE APPLICATION NAME
        string Description { get; } // The description of the mod or a library
        string Version { get; } // The version of the mod
        string Patch { get; } // In some cases changing version might mess up other mods that might depend on this one(patch should not include the version itself)
       (string,(string, string[]?)[])[] Required { get; } // Mods might depend on other mods or libraries. This is a list of tuples containing the name, versions and maybe a patch of the required mods or libraries(all data has to be an exact match. If not, the mod will count as not found)
       (string,(string, string[]?)[])[] Prohibited { get; } // A patch or a version of a mod cannot be present? This tuple is used to prohibit the user of using some mods while also having this one.(RealName,(Version, Patch[]?)[])[]
    }
}
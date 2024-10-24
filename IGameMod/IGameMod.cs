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
         0. Core render     + only 1 can be mounted at a time
         1. Core content    + a mod that adds the core features of a game (can be multiple mounted at once)
         2. Library         - a library that others mods use
         3. Core render mod - a mod that expands the rendering
         4. Mod             - a mod that adds content to the game
         */
        string Name { get; } // The internal name of the mod (this will not be displayed to the user unless two of the mods have the same name. The name should not contain any characters that are not allowed in filenames)
        string DisplayName { get; } // The name of the mod. IF THE MOD TYPE IS `Core render` THIS WILL BE USED AS THE APPLICATION NAME
        string Description { get; } // The description of the mod or a library
        string Version { get; } // The version of the mod
        string Patch { get; } // In some cases changing version might mess up other mods that might depend on this one (patch should not include the version itself)
        (string,(string, string[]?)[])[] Required { get; } // Mods might depend on other mods or libraries. This is a list of tuples containing the name, versions and maybe a patch of the required mods or libraries (all data has to be an exact match. If not, the mod will count as not found)
        (string,(string, string[]?)[])[] Prohibited { get; } // A patch or a version of a mod cannot be present? This tuple is used to prohibit the user of using some mods while also having this one. (Name,(Version, Patch[]?)[])[]
    }
}
namespace Main
{
    public interface IGameMod
    {
        Int16 Type { get; } // A variable to know what type of .dll is this
        /*
         * 0. Core         - only 1 can be mounted at a time (should not include any contend, only the rendering)
         * 1. Core mod     - a mod that expands the core 
         * 2. Core content - a mod that adds the core features of a game
         * 3. Library      - a library that others mods use
         * 4. Mod          - a mod that adds content to the game
         */
        string RealName { get; } // The real name of the mod (should be similar to the [Name], but can't be duplicate)
        string Name { get; } // The name of the mod
        string Description { get; } // The description of the mod or a library
        string Version { get; } // The version of the mod
        string Patch { get; } // In some cases changing version might mess up other mods that might depend on this one (patch should not include the version itself)
        (string, (string, string[]?)[])[] Required { get; } // Mods might depend on other mods or libraries. This is a list of tuples containing the name, versions and maybe a patch of the required mods or libraries (all data has to be an exact match. If not, the mod will count as not found)
        (string, (string, string[]?)[])[] Prohibited { get; } // A patch or a version of a mod cannot be present? This tuple is used to prohibit the user of using some mods while also having this one. (RealName, (Version, Patch[]?)[])[]
        string DoSomething();
    }
}
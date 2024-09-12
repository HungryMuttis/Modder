namespace Modder
{
    public class Mod : IGameMod
    {
        public ushort Type { get; } // A variable to know what type of .dll is this
        /*
         * 0. Unknown      - a mod should not contain this. This is for the modloader to know if something went wrong. Unknown mods cannot be used
         * 1. Core         - only 1 can be mounted at a time (should not include any contend, only the rendering)
         * 2. Core mod     - a mod that expands the core 
         * 3. Core content - a mod that adds the core features of a game
         * 4. Library      - a library that others mods use
         * 5. Mod          - a mod that adds content to the game
         */
        public string RealName { get; } // The real name of the mod (should be similar to the [Name], but can't be duplicate)
        public string Name { get; } // The name of the mod
        public string Description { get; } // The description of the mod or a library
        public string Version { get; } // The version of the mod
        public string Patch { get; } // In some cases changing version might mess up other mods that might depend on this one (patch should not include the version itself)
        public (string, (string, string[]?)[])[] Required { get; } // Mods might depend on other mods or libraries. This is a list of tuples containing the name, versions and maybe a patch of the required mods or libraries (all data has to be an exact match. If not, the mod will count as not found)
        public (string, (string, string[]?)[])[] Prohibited { get; } // A patch or a version of a mod cannot be present? This tuple is used to prohibit the user of using some mods while also having this one. (RealName, (Version, Patch[]?)[])[]
        Type ModType { get; }
        public Mod(Type mod)
        {
            ModType = mod;

            ushort? type = (ushort?)Utils.GetProperty(ModType, "Type");
            Type = type ?? 0;

            string? realName = (string?)Utils.GetProperty(ModType, "RealName");
            RealName = realName ?? "";
            
            string? name = (string?)Utils.GetProperty(ModType, "Name");
            Name = name ?? "";

            string? description = (string?)Utils.GetProperty(ModType, "Description");
            Description = description ?? "";

            string? version = (string?)Utils.GetProperty(ModType, "Version");
            Version = version ?? "";

            string? patch = (string?)Utils.GetProperty(ModType, "Patch");
            Patch = patch ?? "";

            (string, (string, string[]?)[])[]? required = ((string, (string, string[]?)[])[]?)Utils.GetProperty(ModType, "Required");
            Required = required ?? [];

            (string, (string, string[]?)[])[]? prohibited = ((string, (string, string[]?)[])[]?)Utils.GetProperty(ModType, "Prohibited");
            Prohibited = prohibited ?? [];
        }
        object? RunFunc(string funcName, object[]? parameters = null)
        {
            return Utils.RunFunction(ModType, funcName, parameters);
        }
    }
}

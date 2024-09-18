namespace Modder
{
    public class Mod : IGameMod
    {
        public ushort Type { get; } // A variable to know what type of .dll is this
        public string RealName { get; } // The real name of the mod (should be similar to the [Name], but can't be duplicate between other mods)
        public string Name { get; } // The name of the mod
        public string Description { get; } // The description of the mod or a library
        public string Version { get; } // The version of the mod
        public string Patch { get; } // In some cases changing version might mess up other mods that might depend on this one (patch should not include the version itself)
        public (string, (string, string[]?)[])[] Required { get; } // Mods might depend on other mods or libraries. This is a list of tuples containing the name, versions and maybe a patch of the required mods or libraries (all data has to be an exact match. If not, the mod will count as not found)
        public (string, (string, string[]?)[])[] Prohibited { get; } // A patch or a version of a mod cannot be present? This tuple is used to prohibit the user of using some mods while also having this one. (RealName, (Version, Patch[]?)[])[]
        Type ModType { get; }
        public Mod(Type mod)
        {
            this.ModType = mod;

            ushort? type = (ushort?)Utils.GetProperty(ModType, "Type");
            this.Type = type ?? 0;

            string? realName = (string?)Utils.GetProperty(ModType, "RealName");
            this.RealName = realName ?? "no_name";
            
            string? name = (string?)Utils.GetProperty(ModType, "Name");
            this.Name = name ?? "";

            string? description = (string?)Utils.GetProperty(ModType, "Description");
            this.Description = description ?? "";

            string? version = (string?)Utils.GetProperty(ModType, "Version");
            this.Version = version ?? "";

            string? patch = (string?)Utils.GetProperty(ModType, "Patch");
            this.Patch = patch ?? "";

            (string, (string, string[]?)[])[]? required = ((string, (string, string[]?)[])[]?)Utils.GetProperty(ModType, "Required");
            this.Required = required ?? [];

            (string, (string, string[]?)[])[]? prohibited = ((string, (string, string[]?)[])[]?)Utils.GetProperty(ModType, "Prohibited");
            this.Prohibited = prohibited ?? [];
        }
    }
}

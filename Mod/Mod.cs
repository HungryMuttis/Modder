using System.Reflection;

namespace Modder
{
    public class Mod : IGameMod
    {
        public Int16 Type { get; } // A variable to know what type of .dll is this
        public string RealName { get; } // The real name of the mod(should be similar to the [Name], but can't be duplicate between other mods)
        public string Name { get; } // The name of the mod
        public string Description { get; } // The description of the mod or a library
        public string Version { get; } // The version of the mod
        public string Patch { get; } // In some cases changing version might mess up other mods that might depend on this one(patch should not include the version itself)
        public(string,(string, string[]?)[])[] Required { get; } // Mods might depend on other mods or libraries. This is a list of tuples containing the name, versions and maybe a patch of the required mods or libraries(all data has to be an exact match. If not, the mod will count as not found)
        public(string,(string, string[]?)[])[] Prohibited { get; } // A patch or a version of a mod cannot be present? This tuple is used to prohibit the user of using some mods while also having this one.(RealName,(Version, Patch[]?)[])[]

        public string Path { get; set; }
        Type ModType { get; }
        public Mod(Type mod, string path)
        {
            this.Path = path;

            this.ModType = mod;

            object? instance = Activator.CreateInstance(mod) ?? throw new Exception("Mod instance is null");

            Int16? type =(Int16?)this.GetPropValue("Type", instance);
            this.Type = type ?? 5;

            string? realName =(string?)this.GetPropValue("RealName", instance);
            this.RealName = realName ?? "no_name";

            string? name =(string?)this.GetPropValue("Name", instance);
            this.Name = name ?? "";

            string? description =(string?)this.GetPropValue("Description", instance);
            this.Description = description ?? "";

            string? version =(string?)this.GetPropValue("Version", instance);
            this.Version = version ?? "";

            string? patch =(string?)this.GetPropValue("Patch", instance);
            this.Patch = patch ?? "";

            (string,(string, string[]?)[])[]? required =((string,(string, string[]?)[])[]?)this.GetPropValue("Required", instance);
            this.Required = required ?? [];

            (string,(string, string[]?)[])[]? prohibited =((string,(string, string[]?)[])[]?)this.GetPropValue("Prohibited", instance);
            this.Prohibited = prohibited ?? [];
        }

        private PropertyInfo GetProp(string propName)
        {
            PropertyInfo? propertyInfo = this.ModType.GetProperty(propName);
            return propertyInfo ?? throw new ArgumentException($"Property '{propName}' not found in type '{this.ModType.FullName}'.");
        }

        private object? GetPropValue(string propName, object instance)
        {
            return this.GetProp(propName).GetValue(instance);
        }
    }
}

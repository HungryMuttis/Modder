using System.Diagnostics.CodeAnalysis;

namespace Modder
{
    public class ModHandler : IDisposable
    {
        private bool AllowModsWithoutPaths { get; }

        /// <summary>
        /// Value indicating if this object is disposed
        /// </summary>
        public bool IsDisposed { get; private set; } = false;

        /// <summary>
        /// All stored mod Types
        /// </summary>
        public List<Tuple<Type, string>> ModTypes
        {
            get
            {
                if (IsDisposed)
                    throw new ObjectDisposedException("ModHandler");

                return _ModTypes;
            }
            private set
            {
                _ModTypes = value;
            }
        }
        private List<Tuple<Type, string>> _ModTypes = [];
        /// <summary>
        /// All stored loaded mods
        /// </summary>
        public List<Tuple<IGameMod, string>> LoadedMods
        {
            get
            {
                if (IsDisposed)
                    throw new ObjectDisposedException("ModHandler");

                return _LoadedMods;
            }
            private set
            {
                _LoadedMods = value;
            }
        }
        private List<Tuple<IGameMod, string>> _LoadedMods = [];

        /// <summary>
        /// <c>True</c>: No new mod types will be stored, they will be loaded automatically
        /// <br></br>
        /// <c>False</c>: [DEFAULT] New mod types will be stored
        /// </summary>
        public bool AutoLoadNewMods { get; set; } = false;

        /// <summary>
        /// Creates a new instance of ModHandler
        /// </summary>
        /// <param name="allowModsWithoutPaths">Are mods without paths allowed</param>
        public ModHandler(bool allowModsWithoutPaths = false)
        {
            this.AllowModsWithoutPaths = allowModsWithoutPaths;
            this.ModTypes = [];
        }
        /// <summary>
        /// Creates a new instance of ModHandler
        /// </summary>
        /// <param name="modTypes">Tuples of Type of IGameMod and string containing the path of the mod</param>
        /// <param name="allowModsWithoutPaths">Are mods without paths allowed</param>
        public ModHandler(IEnumerable<Tuple<Type, string>> modTypes, bool allowModsWithoutPaths = false)
        {
            this.AllowModsWithoutPaths = allowModsWithoutPaths;
            this.ModTypes = new(modTypes);
            AddModTypes(modTypes);
        }
        /// <summary>
        /// Creates a new instance of ModHandler
        /// </summary>
        /// <param name="modTypes">Mod Types</param>
        /// <param name="allowModsWithoutPaths">Are mods without paths allowed</param>
        /// <exception cref="ArgumentException">Thrown if allowModsWithoutPaths is set to false</exception>
        public ModHandler(IEnumerable<Type> modTypes, bool allowModsWithoutPaths = true)
        {
            if (!allowModsWithoutPaths)
                throw new ArgumentException("This constructor cannot be used if 'allowModsWithoutPaths' is set to false", nameof(allowModsWithoutPaths));

            this.AllowModsWithoutPaths = allowModsWithoutPaths;
            this.ModTypes = [];
            AddModTypes(modTypes);
        }

        /// <summary>
        /// Adds the given Types and paths to ModHandler
        /// </summary>
        /// <param name="modTypes">Tuples of Type of IGameMod and string containing the path of the mod</param>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed</exception>
        public void AddModTypes(IEnumerable<Tuple<Type, string>> modTypes)
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);

            if (!this.AllowModsWithoutPaths)
                if (CheckPaths(modTypes, false))
                {
                    if (this.AutoLoadNewMods)
                        this.LoadedMods.AddRange(LoadModTypes(modTypes));
                    else
                        this.ModTypes.AddRange(modTypes);

                    return;
                }

            foreach (Tuple<Type, string> modType in modTypes)
                if (CheckPath(modType.Item2, false))
                    AddModType(modType);
        }
        /// <summary>
        /// Adds the given Types to ModHandler
        /// </summary>
        /// <param name="modTypes">Types of IGameMod</param>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed</exception>
        /// <exception cref="InvalidOperationException">Thrown if AllowModsWithoutPaths is set to false</exception>
        public void AddModTypes(IEnumerable<Type> modTypes)
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            if (!this.AllowModsWithoutPaths)
                throw new InvalidOperationException($"Function {nameof(AddModTypes)} canot be called if {nameof(this.AllowModsWithoutPaths)} is set to false");

            foreach (Type modType in modTypes)
                AddModType(modType);
        }
        /// <summary>
        /// Adds the given mod Type to ModHandler
        /// </summary>
        /// <param name="modType">Mod Type</param>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed</exception>
        /// <exception cref="InvalidPathException">Thrown if AllowModsWithoutPaths is set to false and the given modType path is invalid</exception>
        public void AddModType(Tuple<Type, string> modType)
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            if (!this.AllowModsWithoutPaths)
                CheckPath(modType.Item2);

            if (this.AutoLoadNewMods)
            {
                IGameMod? loadedMod = LoadModType(modType.Item1);
                if (loadedMod != null)
                    this.LoadedMods.Add(new(loadedMod, modType.Item2));
            }
            else
                this.ModTypes.Add(modType);
        }
        /// <summary>
        /// Adds the given mod Type to ModHandler
        /// </summary>
        /// <param name="modType">Mod Type</param>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed</exception>
        /// <exception cref="InvalidOperationException">Thrown if AllowModsWithoutPaths is set to false</exception>
        public void AddModType(Type modType)
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            if (!this.AllowModsWithoutPaths)
                throw new InvalidOperationException($"Function {nameof(AddModType)} canot be called if {nameof(this.AllowModsWithoutPaths)} is set to false");

            AddModType(new Tuple<Type, string>(modType, ""));
        }

        /// <summary>
        /// Loads all stored mod types
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed</exception>
        public void LoadModTypes()
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);

            this.LoadedMods.AddRange(LoadModTypes(this.ModTypes));
            this.ModTypes.Clear();
        }
        /// <summary>
        /// Loads the given mod type
        /// </summary>
        /// <param name="modType">Tuple of the mod Type and a string path to the mod</param>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed</exception>
        /// <exception cref="InvalidPathException">Thrown if AllowModsWithoutPaths is set to false and the given modType path is invalid</exception>
        public void LoadModType(Tuple<Type, string> modType)
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            if (!this.AllowModsWithoutPaths)
                CheckPath(modType.Item2);

            IGameMod? loadedMod = LoadModType(modType.Item1);
            if (loadedMod != null)
                this.LoadedMods.Add(new(loadedMod, modType.Item2));
        }
        /// <summary>
        /// Clears all stored mod Types
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed</exception>
        public void ClearModTypes()
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);

            this.ModTypes.Clear();
        }
        /// <summary>
        /// Clears all loaded mods
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed</exception>
        public void ClearLoadedMods()
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);

            this.LoadedMods.Clear();
        }

        public void AddLoadedMod(IGameMod loadedMod)
        {
            this.LoadedMods.Add(new(loadedMod, ""));
        }

        private static List<Tuple<IGameMod, string>> LoadModTypes(IEnumerable<Tuple<Type, string>> modTypes)
        {
            List<Tuple<IGameMod, string>> loadedMods = [];

            foreach (Tuple<Type, string> modType in modTypes)
            {
                IGameMod? loadedMod = LoadModType(modType.Item1);
                if (loadedMod != null)
                    loadedMods.Add(new(loadedMod, modType.Item2));
            }

            return loadedMods;
        }
        private static List<IGameMod?> LoadModTypes(IEnumerable<Type> modTypes)
        {
            List<IGameMod?> loadedMods = [];

            foreach (Type modType in modTypes)
                loadedMods.Add(LoadModType(modType));

            return loadedMods;
        }
        private static IGameMod? LoadModType(Type modType)
        {
            try
            {
                return (IGameMod?)Activator.CreateInstance(modType);
            }
            catch
            {
                return null;
            }
        }

        private static bool CheckPaths(IEnumerable<Tuple<Type, string>> paths, bool error = true)
        {
            foreach (Tuple<Type, string> path in paths)
                if (!CheckPath(path.Item2, error))
                    return false;

            return true;
        }
        private static bool CheckPaths(IEnumerable<string> paths, bool error = true)
        {
            foreach (string path in paths)
                if (!CheckPath(path, error))
                    return false;

            return true;
        }
        private static bool CheckPath(string path, bool error = true)
        {
            if (!Path.IsPathRooted(path))
            {
                InvalidPathException.ThrowIf(error, path, "Path was not rooted");
                return false;
            }
            if (!path.Any(c => Path.GetInvalidPathChars().Contains(c)))
            {
                InvalidPathException.ThrowIf(error, path, "Path contains invalid characters");
                return false;
            }
            if (!File.Exists(path))
            {
                InvalidPathException.ThrowIf(error, path, "Path does not exist");
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);

            IsDisposed = true;
            this.ModTypes.Clear();
            this.LoadedMods.Clear();
            GC.SuppressFinalize(this);
        }
    }
    public class InvalidPathException : Exception
    {
        public string Path { get; }

        public InvalidPathException(string path) : base()
        {
            this.Path = path;
        }
        public InvalidPathException(string path, string message) : base(message)
        {
            this.Path = path;
        }
        public InvalidPathException(string path, string message, Exception? innerException) : base(message, innerException)
        {
            this.Path = path;
        }

        public static void ThrowIf([DoesNotReturnIf(true)] bool condition, string path)
        {
            if (!condition)
                return;

            throw new InvalidPathException(path);
        }
        public static void ThrowIf([DoesNotReturnIf(true)] bool condition, string path, string messgae)
        {
            if (!condition)
                return;

            throw new InvalidPathException(path, messgae);
        }
        public static void ThrowIf([DoesNotReturnIf(true)] bool condition, string path, string messgae, Exception? innerException)
        {
            if (!condition)
                return;

            throw new InvalidPathException(path, messgae, innerException);
        }
    }
}

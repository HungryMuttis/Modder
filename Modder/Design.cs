namespace Modder
{
    public class Design(Type design, string path) : IDesign
    {

        public event EventHandler<string>? AddLog;
        public event EventHandler<string>? NewLog;
        public event EventHandler<List<string>>? StartProgram;
        public string Path { get; set; } = path;
        Type DesignType { get; } = design;

        public void Start(List<Mod> mods, Dictionary<string, string> settings)
        {
            Utils.RunFunction(this.DesignType, "Start", mods, settings);
        }
    }

    public class DesignException : Exception
    {
        public DesignException() : base() { }
        public DesignException(string? message) : base(message) { }
        public DesignException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}

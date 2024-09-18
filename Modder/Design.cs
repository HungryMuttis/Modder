namespace Modder
{
    public class Design(Type design) : IDesign
    {

        public event EventHandler<string>? AddLog;
        public event EventHandler<string>? NewLog;
        public event EventHandler<List<string>>? StartProgram;
        Type DesignType { get; } = design;

        public RichTextBox GetLogOutput()
        {
            object? output = Utils.RunFunction(this.DesignType, "GetLogOutput");

            if (output == null)
            {
                Main.Print($"Current applied design returns NULL when GetLogOutput is called");
                Utils.Warn("The current applied design is flawed", "Design Warning");
                return new RichTextBox();
            }

            if (output.GetType() != typeof(RichTextBox))
            {
                Main.Print($"Current applied design does not return RichTextBox when GetLogOutput function is called");
                Utils.Warn("The current applied design is flawed", "Design Warning");
                return new RichTextBox();
            }

            return (RichTextBox)output;
        }

        public void Start(List<object> mods, Dictionary<string, string> settings)
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

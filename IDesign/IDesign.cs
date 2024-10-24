namespace Modder
{
    public interface IDesign
    {
        public delegate void LogEvent(object obj, LogType type);
        public delegate void StartProgramEvent(IEnumerable<IGameMod> modNames);

        abstract event IDesign.LogEvent? Log; // (Fired by Design) the Designs way of telling the program to log something
        abstract event IDesign.StartProgramEvent? StartProgram; // (Fired by Design) when fired, all the given mods will be executed (the program will be started)

        abstract public string Name { get; }

        abstract public RichTextBox? GetTextBox();
        abstract public Form Start(Dictionary<string, string> settings, string designSettingsPath); // The function ran to start the form
    }
}
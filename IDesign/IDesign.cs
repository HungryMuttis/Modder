namespace Modder
{
    public interface IDesign
    {
        public delegate void LogEvent(string message);
        public delegate void StartProgramEvent(string[] modNames);

        abstract event IDesign.LogEvent? Log; // (Fired by Design) the Designs way of telling the program to log something
        abstract event IDesign.StartProgramEvent? StartProgram; // (Fired by Design) when fired, all the given mods will be executed (the program will be started)

        abstract public RichTextBox? GetTextBox();
        abstract public Form Start(Type[] mods, Dictionary<string, string> settings); //the function ran to start the form
    }
}
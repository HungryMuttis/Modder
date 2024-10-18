namespace Modder
{
    public interface IDesign
    {
        public delegate void LogEvent(object obj, LogType type);
        public delegate void StartProgramEvent(Type[] modNames);

        abstract event IDesign.LogEvent? Log; // (Fired by Design) the Designs way of telling the program to log something
        abstract event IDesign.StartProgramEvent? StartProgram; // (Fired by Design) when fired, all the given mods will be executed (the program will be started)

        abstract public RichTextBox? GetTextBox();
        abstract public Form Start(Tuple<Type, string>[] mods, Dictionary<string, string> settings); //the function ran to start the form
    }
}
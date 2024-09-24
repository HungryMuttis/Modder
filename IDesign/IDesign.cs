namespace Modder
{
    public interface IDesign
    {
        abstract event EventHandler<string>? AddLog; //should add the line to wherever the form shows the logs
        abstract event EventHandler<string>? NewLog; //write a new log (fired by the Design)
                                                    // note: AddLog will be fired after the log is saved to a fil
        abstract event EventHandler<List<string>>? StartProgram; // when the Design fires this method, the program will be excecuted
        abstract void Start(List<Mod> mods, Dictionary<string, string> settings); //the function ran to start the form
    }
}
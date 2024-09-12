namespace Modder
{
    public interface IDesign
    {
        abstract void Start(List<object> mods, List<object> settings);
        abstract RichTextBox GetLogOutput();
    }
}
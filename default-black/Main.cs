using Modder;

namespace default_black
{
    public partial class MainForm : Form
    {
        public event IDesign.LogEvent? Log;
        public event IDesign.StartProgramEvent? StartProgram;

        private Type[] Mods { get; set; } = [];
        private Dictionary<string, string> Settings { get; set; } = [];

        public MainForm(Type[] mods, Dictionary<string, string> settings)
        {
            this.Mods = mods;
            this.Settings = settings;
            InitializeComponent();
        }

        private void MainForm_ClientSizeChanged(object sender, EventArgs e)
        {
            //grbWindow
            grbWindow.Height = this.Height - 63;
            pnlWindow.Height = grbWindow.Height - 14;

            //grbMain
            grbMain.Height = this.Height - 63;
            grbMain.Width = this.Width - 146;
            pnlMain.Height = grbMain.Height - 17;
            pnlMain.Width = grbMain.Width - 2;
        }

        private void radMain_CheckedChanged(object sender, EventArgs e)
        {
            grbMain.Visible = radMain.Checked;
        }

        private void radLogs_CheckedChanged(object sender, EventArgs e)
        {
            grbLogs.Visible = radLogs.Checked;
        }
        
        public RichTextBox GetTextBox()
        {
            return this.rtbLogs;
        }
    }
    public class Main : IDesign
    {
        private MainForm? mForm;
        public event IDesign.LogEvent? Log;
        public event IDesign.StartProgramEvent? StartProgram;
        public Form Start(Type[] mods, Dictionary<string, string> settings)
        {
            new Thread(() => { Application.Run(this.mForm = new(mods, settings)); }).Start();

            while (this.mForm == null)
                Thread.Sleep(10);

            return this.mForm;
        }
        public RichTextBox? GetTextBox()
        {
            return this.mForm?.GetTextBox();
        }
    }
}
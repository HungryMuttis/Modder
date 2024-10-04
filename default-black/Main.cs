using Modder;

namespace default_black
{
    public partial class MainForm : Form
    {
        public event EventHandler<string>? AddLog;
        public event EventHandler<string>? NewLog;
        public event EventHandler<List<string>>? StartProgram;
        List<Mod> Mods { get; set; }
        Dictionary<string, string> Settings { get; set; }
        public MainForm(List<Mod> mods, Dictionary<string, string> settings, EventHandler<string>? addLog, EventHandler<string>? newLog, EventHandler<List<string>>? startProgram)
        {
            this.AddLog = addLog;
            this.NewLog = newLog;
            this.StartProgram = startProgram;
            InitializeComponent();
            this.Mods = mods;
            this.Settings = settings;
        }

        public RichTextBox GetLogOutput()
        {
            return rtbLogs;
        }

        private void MainForm_ClientSizeChanged(object sender, EventArgs e)
        {
            //grbWindow
            grbWindow.Height = this.Height - 63;
            pnlWindow.Height = grbWindow.Height - 14;

            //grbMain
            grbMain.Height = this.Height - 63;
            grbMain.Width = this.Width - 146;
            pnlMain.Height = grbMain.Height - 14;
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
    }
}
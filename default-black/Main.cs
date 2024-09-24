using Modder;

namespace default_black
{
    public partial class frmMain : Form
    {
        List<Mod> Mods { get; set; }
        Dictionary<string, string> Settings { get; set; }
        public frmMain(List<Mod> mods, Dictionary<string, string> settings)
        {
            InitializeComponent();
            Mods = mods;
            Settings = settings;
        }

        public RichTextBox GetLogOutput()
        {
            return rtbLogs;
        }

        private void frmMain_ClientSizeChanged(object sender, EventArgs e)
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
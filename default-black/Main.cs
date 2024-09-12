using System.Security.Cryptography;

namespace default_black
{
    public partial class frmMain : Form
    {
        List<object> Mods { get; set; }
        List<object> Settings { get; set; }
        public frmMain(List<object> mods, List<object> settings)
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

namespace Modder
{
    public class Main : IDesign
    {
        default_black.frmMain? mainForm;
        public void Start(List<object> mods, List<object> settings)
        {
            this.mainForm = new default_black.frmMain(mods, settings);
            //ApplicationConfiguration.Initialize();
            Application.Run(this.mainForm);
        }

        public RichTextBox GetLogOutput()
        {
            if (mainForm == null)
                throw new Exception("Form not started");
            return mainForm.GetLogOutput();
        }
    }
}
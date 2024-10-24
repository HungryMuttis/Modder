using Modder;
using System;
using System.CodeDom;
using System.Windows.Forms;

namespace default_black
{
    public partial class MainForm : Form
    {
        public event IDesign.LogEvent? Log;
        public event IDesign.StartProgramEvent? StartProgram;

        private bool ShowingUnloaded = false;

        private Type[] Mods { get; set; } = [];
        private Dictionary<string, string> Settings { get; set; } = [];

        private void Print(object data, LogType type)
        {
            Log?.Invoke(data, type);
        }

        public MainForm(Dictionary<string, string> settings, string _)
        {
            this.Settings = settings;
            InitializeComponent();

            // Temporary
            this.Height = 400;

            grbLogs.Visible = false;
            grbLogs.Location = new(118, 12);

            grbSettings.Visible = false;
            grbSettings.Location = new(118, 12);

            /*
            DataGridViewRow row = new();
            row.CreateCells(dgvLoad, "🡠");
            dgvLoad.Rows.Add(row);

            DataGridViewRow rw = new();
            rw.CreateCells(dgvUnloaded, "jdhsdjikfhjfhjgkfhsfjhsdfujishfjk", "🡢");
            dgvUnloaded.Rows.Add(rw);*/
        }

        private void MainForm_ClientSizeChanged(object sender, EventArgs e)
        {
            //grbWindow
            this.grbWindow.Height = this.Height - 63;
            this.pnlWindow.Height = this.grbWindow.Height - 17;

            //grbMain
            this.grbMain.Size = new(this.Width - 146, this.grbWindow.Height);
            this.pnlMain.Size = new(this.grbMain.Width - 2, this.pnlWindow.Height);

            this.btnStartApp.Location = new(this.btnStartApp.Location.X, this.pnlMain.Height - 29);

            ResizeDGV();

            //grbLogs
            this.grbLogs.Size = this.grbMain.Size;
            this.rtbLogs.Size = this.pnlMain.Size;

            //grbSettings
            this.grbSettings.Size = this.grbMain.Size;
            this.pnlSettings.Size = this.pnlMain.Size;
        }

        private void radMain_CheckedChanged(object sender, EventArgs e)
        {
            this.grbMain.Visible = this.radMain.Checked;
        }

        private void radLogs_CheckedChanged(object sender, EventArgs e)
        {
            this.grbLogs.Visible = this.radLogs.Checked;
        }
        private void radSettings_CheckedChanged(object sender, EventArgs e)
        {
            this.grbSettings.Visible = this.radSettings.Checked;
        }

        public RichTextBox GetTextBox()
        {
            return this.rtbLogs;
        }

        private void dgvUnloaded_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (this.dgvUnloaded.Rows.Count < 1)
            {
                this.ShowingUnloaded = false;
                ResizeDGV();
            }
        }

        private void dgvUnloaded_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (!this.ShowingUnloaded)
                this.ShowingUnloaded = true;
            ResizeDGV();
        }

        private void ResizeDGV()
        {
            this.grbUnloaded.Visible = this.ShowingUnloaded;

            Tuple<GroupBox, DataGridView>[] conts;

            if (this.ShowingUnloaded)
                conts = [
                    new(this.grbUnloaded, this.dgvUnloaded),
                    new(this.grbLoaded, this.dgvLoaded),
                    new(this.grbApp, this.dgvLoad)
                ];
            else
                conts = [
                    new(this.grbLoaded, this.dgvLoaded),
                    new(this.grbApp, this.dgvLoad)
                ];

            foreach (Tuple<GroupBox, DataGridView> cont in conts)
                if (cont != null)
                    cont.Item1.Width = 0;

            int def;
            int left = this.grbMain.Width - this.grbUnloaded.Location.X - 15;
            int notFull = conts.Length;

            for (int i = 0; i < conts.Length; i++)
            {
                def = left / conts.Length;
                left = 0;

                for (int j = 0; j < conts.Length; j++)
                {
                    if (def < conts[j].Item2.PreferredSize.Width - 30)
                        conts[j].Item1.Width += def;
                    else if (notFull == 0)
                        conts[j].Item1.Width += def;
                    else
                    {
                        conts[j].Item1.Width += conts[j].Item2.PreferredSize.Width - 30;
                        left += def - (conts[j].Item2.PreferredSize.Width - 30);
                        notFull--;
                    }
                    conts[j].Item2.Width = conts[j].Item1.Width - 2;

                    if (j != 0)
                        conts[j].Item1.Location = new(conts[j - 1].Item1.Location.X + conts[j - 1].Item1.Width + 5, conts[j].Item1.Location.Y);
                    else
                        conts[j].Item1.Location = new(this.grbUnloaded.Location.X, conts[j].Item1.Location.Y);
                }

                if (left == 0)
                    break;
            }

            foreach (Tuple<GroupBox, DataGridView> tp in conts)
            {
                tp.Item1.Height = pnlMain.Height - 7;
                tp.Item2.Height = tp.Item1.Height - 17;
            }

            // Extend the last groupbox to not have gaps
            conts.Last().Item1.Width += this.pnlMain.Width - conts.Last().Item1.Width - conts.Last().Item1.Location.X - 5;
            conts.Last().Item2.Width = conts.Last().Item1.Width - 2;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ResizeDGV();
        }

        private void btnReadPaths_Click(object sender, EventArgs e)
        {
            if (this.Settings.TryGetValue("PATH:Mods", out string? path))
                LoadMods(path);
            else
            {
                Print("Can not find key 'PATH:Mods' in the settings dictionary", LogType.Error);
                MessageBox.Show("Failed to find the path of the folder containing the mods", "Failed to load mods", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMods(string path)
        {
            /////////////////////////////////////////////////////load the mods/////////////////////////////////////////////////////
        }

        private void LoadMod(string path)
        {

        }
    }
    public class Main : IDesign
    {
        private MainForm? mForm;
        public event IDesign.LogEvent? Log;
        public event IDesign.StartProgramEvent? StartProgram;
        public string Name { get; } = "default_black";
        public Form Start(Dictionary<string, string> settings, string designSettingsPath)
        {
            new Thread(() => { Application.Run(this.mForm = new(settings, designSettingsPath)); }).Start();

            while (this.mForm == null)
                Thread.Sleep(10);

            this.mForm.Log += Log;
            this.mForm.StartProgram += StartProgram;

            return this.mForm;
        }
        public RichTextBox? GetTextBox()
        {
            return this.mForm?.GetTextBox();
        }
    }
}
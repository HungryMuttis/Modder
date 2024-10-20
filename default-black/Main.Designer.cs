namespace default_black
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing &&(components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            grbWindow = new GroupBox();
            pnlWindow = new Panel();
            radLogs = new RadioButton();
            radMain = new RadioButton();
            radSettings = new RadioButton();
            button1 = new Button();
            grbMain = new GroupBox();
            pnlMain = new Panel();
            grbUnloaded = new GroupBox();
            dgvUnloaded = new DataGridView();
            Path = new DataGridViewTextBoxColumn();
            LoadType = new DataGridViewButtonColumn();
            grbLoaded = new GroupBox();
            dgvLoaded = new DataGridView();
            dataGridViewButtonColumn1 = new DataGridViewButtonColumn();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            grbApp = new GroupBox();
            dgvLoad = new DataGridView();
            MoveToUnloaded = new DataGridViewButtonColumn();
            CName = new DataGridViewTextBoxColumn();
            MoveDown = new DataGridViewButtonColumn();
            MoveUp = new DataGridViewButtonColumn();
            grbLogs = new GroupBox();
            rtbLogs = new RichTextBox();
            grbSettings = new GroupBox();
            pnlSettings = new Panel();
            grbWindow.SuspendLayout();
            pnlWindow.SuspendLayout();
            grbMain.SuspendLayout();
            pnlMain.SuspendLayout();
            grbUnloaded.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUnloaded).BeginInit();
            grbLoaded.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLoaded).BeginInit();
            grbApp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLoad).BeginInit();
            grbLogs.SuspendLayout();
            grbSettings.SuspendLayout();
            SuspendLayout();
            // 
            // grbWindow
            // 
            grbWindow.Controls.Add(pnlWindow);
            grbWindow.ForeColor = Color.White;
            grbWindow.Location = new Point(12, 12);
            grbWindow.Name = "grbWindow";
            grbWindow.Size = new Size(100, 337);
            grbWindow.TabIndex = 0;
            grbWindow.TabStop = false;
            grbWindow.Text = "Window";
            // 
            // pnlWindow
            // 
            pnlWindow.AutoScroll = true;
            pnlWindow.Controls.Add(radLogs);
            pnlWindow.Controls.Add(radMain);
            pnlWindow.Controls.Add(radSettings);
            pnlWindow.ForeColor = Color.White;
            pnlWindow.Location = new Point(1, 15);
            pnlWindow.Name = "pnlWindow";
            pnlWindow.Size = new Size(98, 320);
            pnlWindow.TabIndex = 2;
            // 
            // radLogs
            // 
            radLogs.AutoSize = true;
            radLogs.Location = new Point(5, 57);
            radLogs.Name = "radLogs";
            radLogs.Size = new Size(50, 19);
            radLogs.TabIndex = 3;
            radLogs.Text = "Logs";
            radLogs.UseVisualStyleBackColor = true;
            radLogs.CheckedChanged += radLogs_CheckedChanged;
            // 
            // radMain
            // 
            radMain.AutoSize = true;
            radMain.Checked = true;
            radMain.Location = new Point(5, 7);
            radMain.Name = "radMain";
            radMain.Size = new Size(52, 19);
            radMain.TabIndex = 1;
            radMain.TabStop = true;
            radMain.Text = "Main";
            radMain.UseVisualStyleBackColor = true;
            radMain.CheckedChanged += radMain_CheckedChanged;
            // 
            // radSettings
            // 
            radSettings.AutoSize = true;
            radSettings.Location = new Point(5, 32);
            radSettings.Name = "radSettings";
            radSettings.Size = new Size(67, 19);
            radSettings.TabIndex = 2;
            radSettings.Text = "Settings";
            radSettings.UseVisualStyleBackColor = true;
            radSettings.CheckedChanged += radSettings_CheckedChanged;
            // 
            // button1
            // 
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = Color.White;
            button1.Location = new Point(5, 292);
            button1.Name = "button1";
            button1.Size = new Size(77, 23);
            button1.TabIndex = 1;
            button1.Text = "Launch";
            button1.UseVisualStyleBackColor = true;
            // 
            // grbMain
            // 
            grbMain.Controls.Add(pnlMain);
            grbMain.ForeColor = Color.White;
            grbMain.Location = new Point(118, 12);
            grbMain.Name = "grbMain";
            grbMain.Size = new Size(554, 337);
            grbMain.TabIndex = 2;
            grbMain.TabStop = false;
            grbMain.Text = "Main";
            // 
            // pnlMain
            // 
            pnlMain.AutoScroll = true;
            pnlMain.Controls.Add(grbUnloaded);
            pnlMain.Controls.Add(grbLoaded);
            pnlMain.Controls.Add(grbApp);
            pnlMain.Controls.Add(button1);
            pnlMain.Location = new Point(1, 15);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(552, 320);
            pnlMain.TabIndex = 2;
            // 
            // grbUnloaded
            // 
            grbUnloaded.Controls.Add(dgvUnloaded);
            grbUnloaded.ForeColor = Color.White;
            grbUnloaded.Location = new Point(85, 3);
            grbUnloaded.Name = "grbUnloaded";
            grbUnloaded.Size = new Size(151, 313);
            grbUnloaded.TabIndex = 6;
            grbUnloaded.TabStop = false;
            grbUnloaded.Text = "Unloaded";
            // 
            // dgvUnloaded
            // 
            dgvUnloaded.AllowUserToAddRows = false;
            dgvUnloaded.AllowUserToDeleteRows = false;
            dgvUnloaded.AllowUserToResizeRows = false;
            dgvUnloaded.BackgroundColor = Color.Black;
            dgvUnloaded.BorderStyle = BorderStyle.None;
            dgvUnloaded.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.Black;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.Black;
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvUnloaded.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvUnloaded.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUnloaded.Columns.AddRange(new DataGridViewColumn[] { Path, LoadType });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.Black;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.Black;
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvUnloaded.DefaultCellStyle = dataGridViewCellStyle2;
            dgvUnloaded.EnableHeadersVisualStyles = false;
            dgvUnloaded.GridColor = Color.DimGray;
            dgvUnloaded.Location = new Point(1, 15);
            dgvUnloaded.MultiSelect = false;
            dgvUnloaded.Name = "dgvUnloaded";
            dgvUnloaded.ReadOnly = true;
            dgvUnloaded.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.Black;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.White;
            dataGridViewCellStyle3.SelectionBackColor = Color.Black;
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvUnloaded.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvUnloaded.RowHeadersVisible = false;
            dgvUnloaded.ShowEditingIcon = false;
            dgvUnloaded.Size = new Size(149, 296);
            dgvUnloaded.TabIndex = 3;
            dgvUnloaded.RowsAdded += dgvUnloaded_RowsAdded;
            dgvUnloaded.RowsRemoved += dgvUnloaded_RowsRemoved;
            // 
            // Path
            // 
            Path.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Path.HeaderText = "Path";
            Path.Name = "Path";
            Path.ReadOnly = true;
            Path.Resizable = DataGridViewTriState.False;
            Path.Width = 55;
            // 
            // Load
            // 
            LoadType.HeaderText = "🡢";
            LoadType.Name = "Load";
            LoadType.ReadOnly = true;
            LoadType.Resizable = DataGridViewTriState.False;
            LoadType.Width = 25;
            // 
            // grbLoaded
            // 
            grbLoaded.Controls.Add(dgvLoaded);
            grbLoaded.ForeColor = Color.White;
            grbLoaded.Location = new Point(241, 3);
            grbLoaded.Name = "grbLoaded";
            grbLoaded.Size = new Size(151, 313);
            grbLoaded.TabIndex = 5;
            grbLoaded.TabStop = false;
            grbLoaded.Text = "Loaded";
            // 
            // dgvLoaded
            // 
            dgvLoaded.AllowUserToAddRows = false;
            dgvLoaded.AllowUserToDeleteRows = false;
            dgvLoaded.AllowUserToResizeRows = false;
            dgvLoaded.BackgroundColor = Color.Black;
            dgvLoaded.BorderStyle = BorderStyle.None;
            dgvLoaded.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.Black;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = Color.White;
            dataGridViewCellStyle4.SelectionBackColor = Color.Black;
            dataGridViewCellStyle4.SelectionForeColor = Color.White;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvLoaded.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvLoaded.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLoaded.Columns.AddRange(new DataGridViewColumn[] { dataGridViewButtonColumn1, dataGridViewTextBoxColumn1 });
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.Black;
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle5.ForeColor = Color.White;
            dataGridViewCellStyle5.SelectionBackColor = Color.Black;
            dataGridViewCellStyle5.SelectionForeColor = Color.White;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dgvLoaded.DefaultCellStyle = dataGridViewCellStyle5;
            dgvLoaded.EnableHeadersVisualStyles = false;
            dgvLoaded.GridColor = Color.DimGray;
            dgvLoaded.Location = new Point(1, 15);
            dgvLoaded.MultiSelect = false;
            dgvLoaded.Name = "dgvLoaded";
            dgvLoaded.ReadOnly = true;
            dgvLoaded.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.Black;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = Color.White;
            dataGridViewCellStyle6.SelectionBackColor = Color.Black;
            dataGridViewCellStyle6.SelectionForeColor = Color.White;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            dgvLoaded.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dgvLoaded.RowHeadersVisible = false;
            dgvLoaded.ShowEditingIcon = false;
            dgvLoaded.Size = new Size(149, 296);
            dgvLoaded.TabIndex = 3;
            // 
            // dataGridViewButtonColumn1
            // 
            dataGridViewButtonColumn1.Frozen = true;
            dataGridViewButtonColumn1.HeaderText = "🡠";
            dataGridViewButtonColumn1.Name = "dataGridViewButtonColumn1";
            dataGridViewButtonColumn1.ReadOnly = true;
            dataGridViewButtonColumn1.Resizable = DataGridViewTriState.False;
            dataGridViewButtonColumn1.Text = "";
            dataGridViewButtonColumn1.Width = 25;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Name";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // grbApp
            // 
            grbApp.Controls.Add(dgvLoad);
            grbApp.ForeColor = Color.White;
            grbApp.Location = new Point(397, 3);
            grbApp.Name = "grbApp";
            grbApp.Size = new Size(151, 313);
            grbApp.TabIndex = 4;
            grbApp.TabStop = false;
            grbApp.Text = "Application";
            // 
            // dgvLoad
            // 
            dgvLoad.AllowUserToAddRows = false;
            dgvLoad.AllowUserToDeleteRows = false;
            dgvLoad.AllowUserToResizeRows = false;
            dgvLoad.BackgroundColor = Color.Black;
            dgvLoad.BorderStyle = BorderStyle.None;
            dgvLoad.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = Color.Black;
            dataGridViewCellStyle7.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle7.ForeColor = Color.White;
            dataGridViewCellStyle7.SelectionBackColor = Color.Black;
            dataGridViewCellStyle7.SelectionForeColor = Color.White;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            dgvLoad.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            dgvLoad.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLoad.Columns.AddRange(new DataGridViewColumn[] { MoveToUnloaded, CName, MoveDown, MoveUp });
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = Color.Black;
            dataGridViewCellStyle8.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle8.ForeColor = Color.White;
            dataGridViewCellStyle8.SelectionBackColor = Color.Black;
            dataGridViewCellStyle8.SelectionForeColor = Color.White;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            dgvLoad.DefaultCellStyle = dataGridViewCellStyle8;
            dgvLoad.EnableHeadersVisualStyles = false;
            dgvLoad.GridColor = Color.DimGray;
            dgvLoad.Location = new Point(1, 15);
            dgvLoad.MultiSelect = false;
            dgvLoad.Name = "dgvLoad";
            dgvLoad.ReadOnly = true;
            dgvLoad.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = Color.Black;
            dataGridViewCellStyle9.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle9.ForeColor = Color.White;
            dataGridViewCellStyle9.SelectionBackColor = Color.Black;
            dataGridViewCellStyle9.SelectionForeColor = Color.White;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            dgvLoad.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            dgvLoad.RowHeadersVisible = false;
            dgvLoad.ShowEditingIcon = false;
            dgvLoad.Size = new Size(149, 296);
            dgvLoad.TabIndex = 3;
            // 
            // MoveToUnloaded
            // 
            MoveToUnloaded.Frozen = true;
            MoveToUnloaded.HeaderText = "🡠";
            MoveToUnloaded.Name = "MoveToUnloaded";
            MoveToUnloaded.ReadOnly = true;
            MoveToUnloaded.Resizable = DataGridViewTriState.False;
            MoveToUnloaded.Text = "";
            MoveToUnloaded.Width = 25;
            // 
            // CName
            // 
            CName.HeaderText = "Name";
            CName.Name = "CName";
            CName.ReadOnly = true;
            // 
            // MoveDown
            // 
            MoveDown.HeaderText = "🡣";
            MoveDown.Name = "MoveDown";
            MoveDown.ReadOnly = true;
            MoveDown.Resizable = DataGridViewTriState.False;
            MoveDown.SortMode = DataGridViewColumnSortMode.Automatic;
            MoveDown.Width = 25;
            // 
            // MoveUp
            // 
            MoveUp.HeaderText = "🡡";
            MoveUp.Name = "MoveUp";
            MoveUp.ReadOnly = true;
            MoveUp.Resizable = DataGridViewTriState.False;
            MoveUp.SortMode = DataGridViewColumnSortMode.Automatic;
            MoveUp.Width = 25;
            // 
            // grbLogs
            // 
            grbLogs.Controls.Add(rtbLogs);
            grbLogs.ForeColor = Color.White;
            grbLogs.Location = new Point(118, 350);
            grbLogs.Name = "grbLogs";
            grbLogs.Size = new Size(554, 337);
            grbLogs.TabIndex = 3;
            grbLogs.TabStop = false;
            grbLogs.Text = "Logs";
            // 
            // rtbLogs
            // 
            rtbLogs.BackColor = Color.Black;
            rtbLogs.BorderStyle = BorderStyle.None;
            rtbLogs.ForeColor = Color.White;
            rtbLogs.Location = new Point(1, 15);
            rtbLogs.Name = "rtbLogs";
            rtbLogs.ReadOnly = true;
            rtbLogs.Size = new Size(552, 320);
            rtbLogs.TabIndex = 0;
            rtbLogs.Text = "";
            rtbLogs.WordWrap = false;
            // 
            // grbSettings
            // 
            grbSettings.Controls.Add(pnlSettings);
            grbSettings.ForeColor = Color.White;
            grbSettings.Location = new Point(118, 693);
            grbSettings.Name = "grbSettings";
            grbSettings.Size = new Size(554, 337);
            grbSettings.TabIndex = 3;
            grbSettings.TabStop = false;
            grbSettings.Text = "Settings";
            // 
            // pnlSettings
            // 
            pnlSettings.AutoScroll = true;
            pnlSettings.Location = new Point(1, 15);
            pnlSettings.Name = "pnlSettings";
            pnlSettings.Size = new Size(552, 320);
            pnlSettings.TabIndex = 2;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(684, 1039);
            Controls.Add(grbSettings);
            Controls.Add(grbLogs);
            Controls.Add(grbMain);
            Controls.Add(grbWindow);
            ForeColor = Color.White;
            MinimumSize = new Size(230, 160);
            Name = "MainForm";
            Text = "Modder";
            Load += this.MainForm_Load;
            ClientSizeChanged += MainForm_ClientSizeChanged;
            grbWindow.ResumeLayout(false);
            pnlWindow.ResumeLayout(false);
            pnlWindow.PerformLayout();
            grbMain.ResumeLayout(false);
            pnlMain.ResumeLayout(false);
            grbUnloaded.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUnloaded).EndInit();
            grbLoaded.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLoaded).EndInit();
            grbApp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLoad).EndInit();
            grbLogs.ResumeLayout(false);
            grbSettings.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grbWindow;
        private Panel pnlWindow;
        private RadioButton radMain;
        private RadioButton radLogs;
        private RadioButton radSettings;
        private Button button1;
        private GroupBox grbMain;
        private Panel pnlMain;
        private GroupBox grbLogs;
        private RichTextBox rtbLogs;
        private GroupBox grbSettings;
        private Panel pnlSettings;
        private DataGridView dgvLoad;
        private GroupBox grbApp;
        private GroupBox grbUnloaded;
        private DataGridView dgvUnloaded;
        private GroupBox grbLoaded;
        private DataGridView dgvLoaded;
        private DataGridViewButtonColumn dataGridViewButtonColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn Path;
        private DataGridViewButtonColumn LoadType;
        private DataGridViewButtonColumn MoveToUnloaded;
        private DataGridViewTextBoxColumn CName;
        private DataGridViewButtonColumn MoveDown;
        private DataGridViewButtonColumn MoveUp;
    }
}

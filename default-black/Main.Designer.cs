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
            if(disposing &&(components != null))
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
            grbWindow = new GroupBox();
            pnlWindow = new Panel();
            radLogs = new RadioButton();
            radSettings = new RadioButton();
            radMain = new RadioButton();
            button1 = new Button();
            grbMain = new GroupBox();
            pnlMain = new Panel();
            grbLogs = new GroupBox();
            rtbLogs = new RichTextBox();
            grbWindow.SuspendLayout();
            pnlWindow.SuspendLayout();
            grbMain.SuspendLayout();
            pnlMain.SuspendLayout();
            grbLogs.SuspendLayout();
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
            pnlWindow.Controls.Add(radSettings);
            pnlWindow.Controls.Add(radMain);
            pnlWindow.ForeColor = Color.White;
            pnlWindow.Location = new Point(1, 12);
            pnlWindow.Name = "pnlWindow";
            pnlWindow.Size = new Size(98, 323);
            pnlWindow.TabIndex = 2;
            // 
            // radLogs
            // 
            radLogs.AutoSize = true;
            radLogs.Location = new Point(5, 60);
            radLogs.Name = "radLogs";
            radLogs.Size = new Size(50, 19);
            radLogs.TabIndex = 3;
            radLogs.Text = "Logs";
            radLogs.UseVisualStyleBackColor = true;
            radLogs.CheckedChanged += radLogs_CheckedChanged;
            // 
            // radSettings
            // 
            radSettings.AutoSize = true;
            radSettings.Location = new Point(5, 35);
            radSettings.Name = "radSettings";
            radSettings.Size = new Size(67, 19);
            radSettings.TabIndex = 2;
            radSettings.Text = "Settings";
            radSettings.UseVisualStyleBackColor = true;
            // 
            // radMain
            // 
            radMain.AutoSize = true;
            radMain.Checked = true;
            radMain.Location = new Point(5, 10);
            radMain.Name = "radMain";
            radMain.Size = new Size(52, 19);
            radMain.TabIndex = 1;
            radMain.TabStop = true;
            radMain.Text = "Main";
            radMain.UseVisualStyleBackColor = true;
            radMain.CheckedChanged += radMain_CheckedChanged;
            // 
            // button1
            // 
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = Color.White;
            button1.Location = new Point(3, 293);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
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
            pnlMain.Controls.Add(button1);
            pnlMain.Location = new Point(1, 15);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(552, 320);
            pnlMain.TabIndex = 2;
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
            rtbLogs.Location = new Point(1, 15);
            rtbLogs.Name = "rtbLogs";
            rtbLogs.Size = new Size(552, 320);
            rtbLogs.TabIndex = 0;
            rtbLogs.Text = "";
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(684, 858);
            Controls.Add(grbLogs);
            Controls.Add(grbMain);
            Controls.Add(grbWindow);
            ForeColor = Color.White;
            MinimumSize = new Size(230, 160);
            Name = "frmMain";
            Text = "Modder";
            ClientSizeChanged += MainForm_ClientSizeChanged;
            grbWindow.ResumeLayout(false);
            pnlWindow.ResumeLayout(false);
            pnlWindow.PerformLayout();
            grbMain.ResumeLayout(false);
            pnlMain.ResumeLayout(false);
            grbLogs.ResumeLayout(false);
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
    }
}

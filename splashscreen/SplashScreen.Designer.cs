namespace Modder
{
    partial class SplashScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblName = new Label();
            lbl = new Label();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Font = new Font("Impact", 48F);
            lblName.Location = new Point(-12, -10);
            lblName.Name = "lblName";
            lblName.Size = new Size(0, 80);
            lblName.TabIndex = 7;
            lblName.UseWaitCursor = true;
            // 
            // lbl
            // 
            lbl.FlatStyle = FlatStyle.Flat;
            lbl.Font = new Font("Impact", 24F);
            lbl.Location = new Point(-2, 60);
            lbl.Name = "lbl";
            lbl.Size = new Size(219, 39);
            lbl.TabIndex = 8;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.UseWaitCursor = true;
            // 
            // SplashScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(215, 96);
            Controls.Add(lbl);
            Controls.Add(lblName);
            FormBorderStyle = FormBorderStyle.None;
            Name = "SplashScreen";
            Opacity = 0.5D;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SplashScreen";
            UseWaitCursor = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblName;
        private Label lbl;
    }
}
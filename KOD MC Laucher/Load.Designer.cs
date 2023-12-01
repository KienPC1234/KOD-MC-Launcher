namespace KOD_MC_Laucher
{
    partial class Load
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
            if (disposing && (components != null))
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Load));
            kryptonPictureBox1 = new Krypton.Toolkit.KryptonPictureBox();
            kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            kryptonProgressBar1 = new Krypton.Toolkit.KryptonProgressBar();
            kryptonHeader1 = new Krypton.Toolkit.KryptonHeader();
            ((System.ComponentModel.ISupportInitialize)kryptonPictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)kryptonPanel1).BeginInit();
            kryptonPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // kryptonPictureBox1
            // 
            kryptonPictureBox1.Image = Properties.Resources.game;
            kryptonPictureBox1.Location = new Point(19, 19);
            kryptonPictureBox1.Name = "kryptonPictureBox1";
            kryptonPictureBox1.Size = new Size(104, 100);
            kryptonPictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            kryptonPictureBox1.TabIndex = 0;
            kryptonPictureBox1.TabStop = false;
            kryptonPictureBox1.Click += kryptonPictureBox1_Click;
            // 
            // kryptonPanel1
            // 
            kryptonPanel1.Controls.Add(kryptonProgressBar1);
            kryptonPanel1.Controls.Add(kryptonHeader1);
            kryptonPanel1.Controls.Add(kryptonPictureBox1);
            kryptonPanel1.Location = new Point(-7, -7);
            kryptonPanel1.Name = "kryptonPanel1";
            kryptonPanel1.Size = new Size(344, 140);
            kryptonPanel1.TabIndex = 1;
            // 
            // kryptonProgressBar1
            // 
            kryptonProgressBar1.Location = new Point(148, 80);
            kryptonProgressBar1.Name = "kryptonProgressBar1";
            kryptonProgressBar1.Size = new Size(160, 26);
            kryptonProgressBar1.StateCommon.Back.Color1 = Color.Green;
            kryptonProgressBar1.StateDisabled.Back.ColorStyle = Krypton.Toolkit.PaletteColorStyle.OneNote;
            kryptonProgressBar1.StateNormal.Back.ColorStyle = Krypton.Toolkit.PaletteColorStyle.OneNote;
            kryptonProgressBar1.Style = ProgressBarStyle.Marquee;
            kryptonProgressBar1.TabIndex = 4;
            kryptonProgressBar1.Text = "Loading...";
            kryptonProgressBar1.Values.Text = "Loading...";
            // 
            // kryptonHeader1
            // 
            kryptonHeader1.Location = new Point(148, 28);
            kryptonHeader1.Name = "kryptonHeader1";
            kryptonHeader1.Size = new Size(160, 31);
            kryptonHeader1.TabIndex = 3;
            kryptonHeader1.Values.Description = "";
            kryptonHeader1.Values.Heading = "KOD MC Laucher";
            kryptonHeader1.Values.Image = null;
            // 
            // Load
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(330, 124);
            ControlBox = false;
            Controls.Add(kryptonPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Load";
            StartPosition = FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)kryptonPictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)kryptonPanel1).EndInit();
            kryptonPanel1.ResumeLayout(false);
            kryptonPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Krypton.Toolkit.KryptonPictureBox kryptonPictureBox1;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private Krypton.Toolkit.KryptonProgressBar kryptonProgressBar1;
    }
}
namespace KOD_MC_Laucher
{
    partial class FileChoice
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
            kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            SuspendLayout();
            // 
            // kryptonLabel1
            // 
            kryptonLabel1.Location = new Point(314, 44);
            kryptonLabel1.Name = "kryptonLabel1";
            kryptonLabel1.Size = new Size(88, 20);
            kryptonLabel1.TabIndex = 0;
            kryptonLabel1.Values.Text = "kryptonLabel1";
            // 
            // FileChoice
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(745, 455);
            Controls.Add(kryptonLabel1);
            Name = "FileChoice";
            Text = "FileChoice";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
    }
}
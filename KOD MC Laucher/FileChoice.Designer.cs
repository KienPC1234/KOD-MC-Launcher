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
            dataGridView1 = new DataGridView();
            kryptonButton1 = new Krypton.Toolkit.KryptonButton();
            fhileName = new DataGridViewTextBoxColumn();
            GamewVer = new DataGridViewTextBoxColumn();
            ModLoader22Type = new DataGridViewTextBoxColumn();
            File_Date = new DataGridViewTextBoxColumn();
            Urlee = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // kryptonLabel1
            // 
            kryptonLabel1.LabelStyle = Krypton.Toolkit.LabelStyle.TitleControl;
            kryptonLabel1.Location = new Point(206, 12);
            kryptonLabel1.Name = "kryptonLabel1";
            kryptonLabel1.Size = new Size(328, 29);
            kryptonLabel1.TabIndex = 0;
            kryptonLabel1.Values.Text = "Please Choose Version To Download";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.BackgroundColor = SystemColors.ButtonFace;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { fhileName, GamewVer, ModLoader22Type, File_Date, Urlee });
            dataGridView1.Location = new Point(40, 41);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(657, 324);
            dataGridView1.TabIndex = 1;
            // 
            // kryptonButton1
            // 
            kryptonButton1.Location = new Point(231, 371);
            kryptonButton1.Name = "kryptonButton1";
            kryptonButton1.Size = new Size(241, 25);
            kryptonButton1.TabIndex = 2;
            kryptonButton1.Values.Text = "kryptonButton1";
            kryptonButton1.Click += kryptonButton1_Click;
            // 
            // fhileName
            // 
            fhileName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            fhileName.HeaderText = "File Name";
            fhileName.Name = "fhileName";
            fhileName.ReadOnly = true;
            // 
            // GamewVer
            // 
            GamewVer.HeaderText = "Minecraft Version";
            GamewVer.Name = "GamewVer";
            GamewVer.ReadOnly = true;
            GamewVer.Resizable = DataGridViewTriState.True;
            GamewVer.SortMode = DataGridViewColumnSortMode.NotSortable;
            GamewVer.Width = 122;
            // 
            // ModLoader22Type
            // 
            ModLoader22Type.HeaderText = "Mod Loader Type";
            ModLoader22Type.Name = "ModLoader22Type";
            ModLoader22Type.ReadOnly = true;
            ModLoader22Type.Resizable = DataGridViewTriState.True;
            ModLoader22Type.SortMode = DataGridViewColumnSortMode.NotSortable;
            ModLoader22Type.Width = 121;
            // 
            // File_Date
            // 
            File_Date.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            File_Date.HeaderText = "File Date";
            File_Date.Name = "File_Date";
            File_Date.ReadOnly = true;
            // 
            // Urlee
            // 
            Urlee.HeaderText = "Url";
            Urlee.Name = "Urlee";
            Urlee.ReadOnly = true;
            Urlee.Visible = false;
            // 
            // FileChoice
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(745, 416);
            Controls.Add(kryptonButton1);
            Controls.Add(dataGridView1);
            Controls.Add(kryptonLabel1);
            Name = "FileChoice";
            Text = "FileChoice";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private DataGridView dataGridView1;
        private Krypton.Toolkit.KryptonButton kryptonButton1;
        private DataGridViewTextBoxColumn fhileName;
        private DataGridViewTextBoxColumn GamewVer;
        private DataGridViewTextBoxColumn ModLoader22Type;
        private DataGridViewTextBoxColumn File_Date;
        private DataGridViewTextBoxColumn Urlee;
    }
}
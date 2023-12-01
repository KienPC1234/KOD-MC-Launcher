using System.Data;
using System.Text.Json;


namespace KOD_MC_Laucher
{
    public partial class Add : Form
    {
        public DataTable dt = new DataTable();
        public Image icon = Properties.Resources.game;
        public async void LoadDataIntoDataGridView()
        {
            kryptonButton2.Enabled = false;
            string url = "https://meta.multimc.org/v1/net.minecraft/";
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string accountDirectory = Path.Combine(appDirectory, "Data");
            Directory.CreateDirectory(accountDirectory); // Create the directory if it doesn't exist
            string filePath = Path.Combine(accountDirectory, "net.minecraft.json");

            // Download the JSON file
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    await stream.CopyToAsync(fileStream);
                }
            }
            // Read the JSON file
            string jsonResponse = await File.ReadAllTextAsync(filePath);

            JsonDocument doc = JsonDocument.Parse(jsonResponse);
            JsonElement root = doc.RootElement;
            JsonElement versionsArray = root.GetProperty("versions");
            dataGridView1.DataSource = dt;
            dt.Columns.Add("Version", typeof(string));
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Release", typeof(DateTime));
            dataGridView1.Columns["Version"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["Release"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            foreach (JsonElement versionElement in versionsArray.EnumerateArray())
            {
                string versionNumber = versionElement.GetProperty("version").GetString();
                string type = versionElement.GetProperty("type").GetString();
                DateTime releaseTime = versionElement.GetProperty("releaseTime").GetDateTime();
                dt.Rows.Add(versionNumber, type, releaseTime);
            }
            dataGridView1.ReadOnly = true;
            kryptonButton2.Enabled = true;
        }
        public Add()
        {
            InitializeComponent();
            LoadDataIntoDataGridView();
        }
        private List<string> activeFilters = new List<string>();
        private void ApplyFilters()
        {
            if (activeFilters.Count > 0)
            {
                dt.DefaultView.RowFilter = string.Join(" OR ", activeFilters);
            }
            else
            {
                dt.DefaultView.RowFilter = string.Empty;
            }
        }
        private void AddOrRemoveFilter(bool isChecked, string filter)
        {
            if (isChecked)
            {
                activeFilters.Add(filter);
            }
            else
            {
                activeFilters.Remove(filter);
            }
            ApplyFilters();
        }
        private void kryptonCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            AddOrRemoveFilter(kryptonCheckBox1.Checked, string.Format("Type LIKE '%{0}%'", "snapshot"));
        }

        private void kryptonCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            AddOrRemoveFilter(kryptonCheckBox2.Checked, string.Format("Type LIKE '%{0}%'", "release"));
        }

        private void kryptonCheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            AddOrRemoveFilter(kryptonCheckBox3.Checked, string.Format("Type LIKE '%{0}%'", "old_alpha"));
        }

        private void kryptonCheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            AddOrRemoveFilter(kryptonCheckBox4.Checked, string.Format("Type LIKE '%{0}%'", "old_beta"));
        }

        private void kryptonCheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            AddOrRemoveFilter(kryptonCheckBox5.Checked, string.Format("Type LIKE '%{0}%'", "experiment"));
        }

        private async void kryptonButton2_Click(object sender, EventArgs e)
        {
            dt.Rows.Clear();
            kryptonButton2.Enabled = false;
            string url = "https://meta.multimc.org/v1/net.minecraft/";
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string accountDirectory = Path.Combine(appDirectory, "Data");
            Directory.CreateDirectory(accountDirectory); // Create the directory if it doesn't exist
            string filePath = Path.Combine(accountDirectory, "net.minecraft.json");

            // Download the JSON file
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    await stream.CopyToAsync(fileStream);
                }
            }
            // Read the JSON file
            string jsonResponse = await File.ReadAllTextAsync(filePath);

            JsonDocument doc = JsonDocument.Parse(jsonResponse);
            JsonElement root = doc.RootElement;
            JsonElement versionsArray = root.GetProperty("versions");
            dataGridView1.DataSource = dt;
            foreach (JsonElement versionElement in versionsArray.EnumerateArray())
            {
                string versionNumber = versionElement.GetProperty("version").GetString();
                string type = versionElement.GetProperty("type").GetString();
                DateTime releaseTime = versionElement.GetProperty("releaseTime").GetDateTime();
                dt.Rows.Add(versionNumber, type, releaseTime);
            }
            dataGridView1.ReadOnly = true;
            kryptonButton2.Enabled = true;
        }
        private void MinecraftInstall()
        {
            var AppDictory = AppDomain.CurrentDomain.BaseDirectory;
            var modpackDirectory = Path.Combine(AppDictory, "Modpacks");
            if (dataGridView1.SelectedRows.Count == 1)
            {
                string version = dataGridView1.SelectedRows[0].Cells["Version"].Value.ToString();
                if (string.IsNullOrEmpty(kryptonTextBox1.Text))
                {
                    MessageBox.Show("Please Enter Name!", "!!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }
                else
                {
                    var target = Path.Combine(modpackDirectory, kryptonTextBox1.Text);
                    if (Directory.Exists(target))
                    {
                        MessageBox.Show("This modpack already exists!", "!!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Directory.CreateDirectory(target);
                        var mcdatapath = target + "\\minecraft";
                        Directory.CreateDirectory(mcdatapath);
                        var jsonObject = new
                        {
                            displayname = kryptonTextBox1.Text,
                            type = "Vanila",
                            minecraftver = version,
                            data = mcdatapath
                        };
                        var iconPath = Path.Combine(target, "icon.png");
                        icon.Save(iconPath, System.Drawing.Imaging.ImageFormat.Png);
                        // Chuyển đổi đối tượng thành chuỗi JSON
                        var jsonString = JsonSerializer.Serialize(jsonObject);

                        // Lưu chuỗi JSON vào tệp
                        var jsonFilePath = Path.Combine(target, "kcdmpinfo.json");
                        File.WriteAllText(jsonFilePath, jsonString);
                    }
                    this.Close();
                }
            }
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            MinecraftInstall();
        }
        private void kryptonPictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PNG Files (*.png)|*.png";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Image image = Image.FromFile(openFileDialog.FileName);
                kryptonPictureBox1.Image = image;
                icon = image;
            }
        }
    }
}

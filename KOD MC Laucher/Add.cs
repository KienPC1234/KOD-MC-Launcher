using Krypton.Toolkit;
using System.Data;
using System.Diagnostics;
using System.Drawing.Imaging;
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
        public class Result
        {
            public string name { get; set; }
            public int id { get; set; }
            public string url { get; set; }
            public string authorNames { get; set; }
            public int totaldownload { get; set; }
        }

        public async void FetchingCFAPI(string mode, int classid, string slug, string search)
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var Nodepath = Path.Combine(appDirectory, "node", "node.exe");
            var apipath = Path.Combine(appDirectory, "cf-api");
            if (mode == "mpfind")
            {
                kryptonButton4.Enabled = false;
                // Delete files
                File.Delete(Path.Combine(apipath, "searchinfo.json"));
                File.Delete(Path.Combine(apipath, "simplifiedResults.json"));

                // Create searchinfo.json
                var searchInfo = new
                {
                    slug = slug,
                    searchFilter = search,
                    classid = classid
                };
                File.WriteAllText(Path.Combine(apipath, "searchinfo.json"), JsonSerializer.Serialize(searchInfo));

                // Run node.js script
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    WorkingDirectory = apipath,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    UseShellExecute = false
                };
                Process process = new Process { StartInfo = startInfo };
                process.Start();
                using (StreamWriter writer = process.StandardInput)
                {
                    if (writer.BaseStream.CanWrite)
                    {
                        writer.WriteLine($"cd {apipath}");
                        writer.WriteLine($@"""{Nodepath}"" search.js");
                        writer.WriteLine("exit");
                    }
                }
                process.WaitForExit();

                // Process results
                var resultsPath = Path.Combine(apipath, "simplifiedResults.json");
                if (!File.Exists(resultsPath))
                {
                    MessageBox.Show("Can't Fetching API!");
                }
                else
                {
                    var results = JsonSerializer.Deserialize<List<Result>>(File.ReadAllText(resultsPath));
                    if (results.Count == 0)
                    {
                        MessageBox.Show("Can't Find This Pack!");
                    }
                    else
                    {
                        foreach (var result in results)
                        {
                            DataGridViewRow row = new DataGridViewRow();
                            row.Height += 6;
                            using (var httpClient = new HttpClient())
                            {
                                var response = await httpClient.GetAsync(result.url);
                                byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                                using (var ms = new MemoryStream(imageBytes))
                                {
                                    Image image = Image.FromStream(ms);
                                    Image resizedImage = image.GetThumbnailImage(32, 32, null, IntPtr.Zero);
                                    string tempDirectory = Path.Combine(appDirectory, "Temps");
                                    Directory.CreateDirectory(tempDirectory); // Tạo thư mục "Temps" nếu nó chưa tồn tại
                                    string imagePath = Path.Combine(tempDirectory, $"{result.id}.png"); // Tạo đường dẫn cho hình ảnh
                                    resizedImage.Save(imagePath, ImageFormat.Png); // Lưu hình ảnh vào đường dẫn đã tạo
                                    var imageCell = new DataGridViewImageCell();
                                    imageCell.Value = Image.FromFile(imagePath); // Tải hình ảnh từ đường dẫn
                                    row.Cells.Add(imageCell);
                                }
                            }
                            row.Cells.Add(new DataGridViewTextBoxCell { Value = result.name });
                            row.Cells.Add(new DataGridViewTextBoxCell { Value = result.id.ToString() });
                            row.Cells.Add(new DataGridViewTextBoxCell { Value = result.authorNames });
                            row.Cells.Add(new DataGridViewTextBoxCell { Value = result.totaldownload.ToString() });
                            dataGridView2.Rows.Add(row);
                            kryptonButton4.Enabled = true;
                        }
                    }
                }
               
            }
            else if (mode == "allfile")
            {
                string allFileVerPath = Path.Combine(appDirectory, "cf-api\\allfilever.json");
                string filesPath = Path.Combine(appDirectory, "cf-api\\files.json");
                kryptonButton3.Enabled = false;
                // Xóa file "allfilever.json" nếu tồn tại
                if (File.Exists(allFileVerPath))
                {
                    File.Delete(allFileVerPath);
                }

                // Xóa file "files.json" nếu tồn tại
                if (File.Exists(filesPath))
                {
                    File.Delete(filesPath);
                }

                // Tạo file "allfilever.json" với nội dung yêu cầu
                if (dataGridView2.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please Select Pack!");
                }
                else if (dataGridView2.SelectedRows.Count > 1)
                {
                    // Không làm gì nếu người dùng chọn nhiều hơn một hàng
                }
                else
                {
                    int modidValue = int.Parse(dataGridView2.SelectedRows[0].Cells["ID"].Value.ToString());
                    var jsonContent = new { modid = modidValue };

                    using (var streamWriter = File.CreateText(allFileVerPath))
                    {
                        var json = JsonSerializer.Serialize(jsonContent);
                        streamWriter.Write(json);
                    }
                    ProcessStartInfo startInfo1 = new ProcessStartInfo
                    {
                        WorkingDirectory = apipath,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = "cmd.exe",
                        RedirectStandardInput = true,
                        UseShellExecute = false
                    };
                    Process process1 = new Process { StartInfo = startInfo1 };
                    process1.Start();
                    using (StreamWriter writer = process1.StandardInput)
                    {
                        if (writer.BaseStream.CanWrite)
                        {
                            writer.WriteLine($"cd {apipath}");
                            writer.WriteLine($@"""{Nodepath}"" getfile.js");
                            writer.WriteLine("exit");
                        }
                    }
                    process1.WaitForExit();
                    FileChoice Form = new FileChoice();
                    Form.ShowDialog();
                }
            }
        }

        public Add()
        {
            InitializeComponent();
            LoadDataIntoDataGridView();
            FetchingCFAPI("mpfind", 4471, "", "");
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
                Image image2 = Image.FromFile(openFileDialog.FileName);
                kryptonPictureBox1.Image = image2;
                icon = image2;
            }
        }

        private void kryptonButton4_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            kryptonButton4.Enabled = false;
            FetchingCFAPI("mpfind", 4471, kryptonTextBox3.Text, kryptonTextBox2.Text);
            kryptonButton4.Enabled = true;
        }

        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            
            FetchingCFAPI("allfile", 4471, "", "");
            this.Close();
        }
    }
}

using CmlLib.Core.Installer.Forge;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Downloader;
using System.ComponentModel;
using System.Text.Json;
using Krypton.Toolkit;
using CmlLib.Core.Auth.Microsoft;

namespace KOD_MC_Laucher
{


    public partial class Main : Form
    {
        void fileChanged(DownloadFileChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => fileChanged(e)));
                return;
            }

            toolStripStatusLabel1.Text = $"[{e.FileKind.ToString()}]:{e.FileName}";
            kryptonProgressBarToolStripItem1.Text = $"{e.ProgressedFileCount}/{e.TotalFileCount}";
        }
        void progressChanged(object? sender, ProgressChangedEventArgs e)
        {
            kryptonProgressBarToolStripItem1.Value = e.ProgressPercentage;
        }
        private void MainFormClosing(object? sender, FormClosingEventArgs e)
        {
            // Thoát ứng dụng khi nhấn nút đóng
            Application.Exit();
        }
        public Main()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(MainFormClosing);
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Tạo thư mục "Account" nếu nó chưa tồn tại
            var accountDirectory = Path.Combine(appDirectory, "Account");
            Directory.CreateDirectory(accountDirectory);

            // Lấy tất cả các tệp JSON trong thư mục "Account"
            var jsonFiles = Directory.GetFiles(accountDirectory, "*.json");

            foreach (var jsonFile in jsonFiles)
            {
                // Đọc nội dung của tệp JSON
                var jsonString = File.ReadAllText(jsonFile);

                // Chuyển đổi chuỗi JSON thành đối tượng
                var jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);

                // Lấy giá trị của "name" và "type"
                var name = jsonObject.ContainsKey("name") ? jsonObject["name"].ToString() : null;
                var type = jsonObject.ContainsKey("type") ? jsonObject["type"].ToString() : null;

                // Tạo một ToolStripMenuItem mới
                var toolStripMenuItem = new ToolStripMenuItem
                {
                    Name = name,
                    Size = new Size(180, 22),
                    Text = name,
                };

                // Nếu type là "offline", thêm hình ảnh
                if (type == "offline")
                {
                    toolStripMenuItem.Image = Properties.Resources.cloud__1_;
                }
                else
                {
                    toolStripMenuItem.Image = Properties.Resources.Microsoft_Logo_icon_png_Transparent_Background;
                }

                // Thêm sự kiện Click cho toolStripMenuItem
                toolStripMenuItem.Click += (sender, e) =>
                {
                    // Đặt toolStripSplitButton1.Text là tên tài khoản
                    toolStripSplitButton1.Text = name;
                };

                // Thêm ToolStripMenuItem vào toolStripSplitButton1
                toolStripSplitButton1.DropDownItems.Add(toolStripMenuItem);
            }

            // Nếu toolStripSplitButton1.Text không phải là một tên tài khoản hiện có, đặt nó thành một tên tài khoản ngẫu nhiên hoặc "None"
            if (!jsonFiles.Any(jf => Path.GetFileNameWithoutExtension(jf) == toolStripSplitButton1.Text))
            {
                var randomAccountFile = jsonFiles.OrderBy(jf => Guid.NewGuid()).FirstOrDefault();
                if (randomAccountFile != null)
                {
                    var jsonString = File.ReadAllText(randomAccountFile);
                    var jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);
                    var name = jsonObject.ContainsKey("name") ? jsonObject["name"].ToString() : null;
                    toolStripSplitButton1.Text = name;
                }
                else
                {
                    toolStripSplitButton1.Text = "None";
                }
            }
            ChangeToolSpirit();
        }

        public async Task<MSession> GetSessionsAsync()
        {
            var loginHandler = JELoginHandlerBuilder.BuildDefault();
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var accountDirectory = Path.Combine(appDirectory, "Account");
            Directory.CreateDirectory(accountDirectory);
            MSession session = null;

            foreach (var file in Directory.EnumerateFiles(accountDirectory, "*.json"))
            {
                var json = await File.ReadAllTextAsync(file);
                var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                if (Path.GetFileNameWithoutExtension(file) == toolStripSplitButton1.Text)
                {
                    if (data.ContainsKey("type") && data["type"].ToString() == "online")
                    {
                        var accounts = loginHandler.AccountManager.GetAccounts();
                        var selectedAccount = accounts.GetAccount(data["Identifier"].ToString());
                        session = await loginHandler.Authenticate(selectedAccount);
                    }
                    else
                    {
                        session = MSession.CreateOfflineSession(toolStripSplitButton1.Text);
                    }
                }
            }

            return session;
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }
        public async Task<MLaunchOption> LaunchGameAsync()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var launchOption = new MLaunchOption
            {
                MaximumRamMb = 4024,
                Session = await GetSessionsAsync()
            };

            return launchOption;
        }
        public string BasePath;
        public string mcver;
        public string type;
        private async void toolStripSplitButton2_ButtonClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(BasePath) && toolStripSplitButton3.Text == "None")
            {
                MessageBox.Show("Please Seleted Ver");
            }
            else
            {
                toolStripSplitButton2.Enabled = false;
                // Initialize CMLauncher
                var path = AppDomain.CurrentDomain.BaseDirectory;
                MinecraftPath myPath = new MinecraftPath(path + "\\MinecraftDir");
                var launcher = new CMLauncher(myPath);
                launcher.FileChanged += fileChanged;
                launcher.ProgressChanged += progressChanged;
                // Define BasePath from kcdmpinfo.json
                var modpackDirectory = Path.Combine(path, "Modpacks");
                var jsonFiles = Directory.GetFiles(modpackDirectory, "kcdmpinfo.json", SearchOption.AllDirectories);
                foreach (var jsonFile in jsonFiles)
                {
                    var jsonData = File.ReadAllText(jsonFile);
                    var jsonObj = JsonDocument.Parse(jsonData).RootElement;
                    if (jsonObj.GetProperty("displayname").GetString() == toolStripSplitButton3.Text)
                    {
                        BasePath = jsonObj.GetProperty("data").GetString();
                        mcver = jsonObj.GetProperty("minecraftver").GetString();
                        type = jsonObj.GetProperty("type").GetString();
                        break;
                    }
                }
                if (type == "Vanila")
                {
                    myPath.BasePath = BasePath;
                    var launchOption = await LaunchGameAsync();
                    var process = await launcher.CreateProcessAsync(mcver, launchOption);
                    this.Hide();
                    process.Start();
                    process.WaitForExit();
                    toolStripSplitButton2.Enabled = true;
                    this.Show();
                }
            }
        }

        private void forgeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void createMoreAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Account accountForm = new Account();
            accountForm.ShowDialog();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            // Lấy thư mục chứa ứng dụng
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Tạo thư mục "Account" nếu nó chưa tồn tại
            var accountDirectory = Path.Combine(appDirectory, "Account");
            Directory.CreateDirectory(accountDirectory);

            // Lấy tất cả các tệp JSON trong thư mục "Account"
            var jsonFiles = Directory.GetFiles(accountDirectory, "*.json");

            // Lấy danh sách tên của tất cả các toolStripMenuItem hiện có
            var existingMenuItems = toolStripSplitButton1.DropDownItems.OfType<ToolStripMenuItem>().Select(t => t.Name).ToList();

            foreach (var jsonFile in jsonFiles)
            {
                // Đọc nội dung của tệp JSON
                var jsonString = File.ReadAllText(jsonFile);

                // Chuyển đổi chuỗi JSON thành đối tượng
                var jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);

                // Lấy giá trị của "name"
                var name = jsonObject.ContainsKey("name") ? jsonObject["name"].ToString() : null;
                var type = jsonObject.ContainsKey("type") ? jsonObject["type"].ToString() : null;
                // Kiểm tra xem có toolStripMenuItem nào có tên giống với "name" không
                if (!existingMenuItems.Contains(name))
                {
                    // Nếu không, tạo một toolStripMenuItem mới
                    var toolStripMenuItem = new ToolStripMenuItem
                    {
                        Name = name,
                        Size = new Size(180, 22),
                        Text = name,
                    };

                    // Nếu type là "offline", thêm hình ảnh
                    if (type == "offline")
                    {
                        toolStripMenuItem.Image = Properties.Resources.cloud__1_;
                    }
                    else
                    {
                        toolStripMenuItem.Image = Properties.Resources.Microsoft_Logo_icon_png_Transparent_Background;
                    }

                    // Thêm sự kiện Click cho toolStripMenuItem
                    toolStripMenuItem.Click += (sender, e) =>
                    {
                        // Đặt toolStripSplitButton1.Text là tên tài khoản
                        toolStripSplitButton1.Text = name;
                    };

                    // Thêm ToolStripMenuItem vào toolStripSplitButton1
                    toolStripSplitButton1.DropDownItems.Add(toolStripMenuItem);
                }

                // Nếu toolStripSplitButton1.Text không phải là một tên tài khoản hiện có, đặt nó thành một tên tài khoản ngẫu nhiên hoặc "None"
                if (!jsonFiles.Any(jf => Path.GetFileNameWithoutExtension(jf) == toolStripSplitButton1.Text))
                {
                    var randomAccountFile = jsonFiles.OrderBy(jf => Guid.NewGuid()).FirstOrDefault();
                    if (randomAccountFile != null)
                    {
                        jsonString = File.ReadAllText(randomAccountFile);
                        jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);
                        name = jsonObject.ContainsKey("name") ? jsonObject["name"].ToString() : null;
                        toolStripSplitButton1.Text = name;
                    }
                    else
                    {
                        toolStripSplitButton1.Text = "None";
                    }
                }

                // Xóa tên khỏi danh sách
                existingMenuItems.Remove(name);
            }

            // Xóa tất cả các toolStripMenuItem còn lại
            foreach (var menuItemName in existingMenuItems)
            {
                if (menuItemName != "createMoreAccountsToolStripMenuItem")
                {
                    var menuItem = toolStripSplitButton1.DropDownItems.OfType<ToolStripMenuItem>().FirstOrDefault(t => t.Name == menuItemName);
                    if (menuItem != null)
                    {
                        toolStripSplitButton1.DropDownItems.Remove(menuItem);
                    }
                }
            }
        }
        private void ChangeToolSpirit()
        {
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var modpackDirectory = Path.Combine(appDirectory, "Modpacks");
            var directories = Directory.GetDirectories(modpackDirectory);

            foreach (var directory in directories)
            {
                var jsonFile = Path.Combine(directory, "kcdmpinfo.json");
                if (File.Exists(jsonFile))
                {
                    var jsonContent = File.ReadAllText(jsonFile);
                    using (JsonDocument doc = JsonDocument.Parse(jsonContent))
                    {
                        JsonElement root = doc.RootElement;
                        string displayName = root.GetProperty("displayname").GetString();

                        var existingItem = toolStripSplitButton3.DropDownItems.Cast<ToolStripMenuItem>().FirstOrDefault(item => item != null && item.Tag != null && item.Text != null && item.Tag.ToString() == "mp" && item.Text == displayName);
                        if (existingItem != null)
                        {
                            toolStripSplitButton3.DropDownItems.Remove(existingItem);
                        }

                        var menuItem = new ToolStripMenuItem
                        {
                            Text = displayName,
                            Image = Image.FromFile(Path.Combine(directory, "icon.png")),
                            Tag = "mp"
                        };
                        menuItem.Click += (sender, e) => { toolStripSplitButton3.Text = ((ToolStripMenuItem)sender).Text; };
                        toolStripSplitButton3.DropDownItems.Add(menuItem);
                    }
                }
            }
        }




        private void createNewVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add AddForm = new Add();
            AddForm.ShowDialog();
        }


        private void versionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            ChangeToolSpirit();
        }
    }
}
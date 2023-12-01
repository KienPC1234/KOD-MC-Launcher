using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Sessions;
using KOD_MC_Laucher.Properties;
using Krypton.Toolkit;
using System.Text.Json;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.CodeFlow;
using XboxAuthNet.XboxLive;

namespace KOD_MC_Laucher
{
    public partial class Account : Form
    {
        private int toolStripCount = 2;
        public Account()
        {
            InitializeComponent();
            kryptonButton2_Click(null,null);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }
        private void kryptonButton1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(kryptonTextBox2.Text))
            {
                MessageBox.Show("Please Enter Name!", "!!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            else
            {
                CreateOfflineAcc();
            }
        }
        public void CreateOfflineAcc()
        {
            // Lấy thư mục chứa ứng dụng
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Tạo thư mục "Account" nếu nó chưa tồn tại
            var accountDirectory = Path.Combine(appDirectory, "Account");

            // Tạo đối tượng JSON
            var jsonObject = new
            {
                type = "offline",
                name = kryptonTextBox2.Text
            };

            // Chuyển đổi đối tượng thành chuỗi JSON
            var jsonString = JsonSerializer.Serialize(jsonObject);

            // Lưu chuỗi JSON vào tệp
            var jsonFilePath = Path.Combine(accountDirectory, $"{kryptonTextBox2.Text}.json");
            File.WriteAllText(jsonFilePath, jsonString);
        }

        private void kryptonPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void kryptonLabel1_Click(object sender, EventArgs e)
        {

        }
        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Tạo thư mục "Account" nếu nó chưa tồn tại
            var accountDirectory = Path.Combine(appDirectory, "Account");
            Directory.CreateDirectory(accountDirectory);

            // Lấy tất cả các tệp JSON trong thư mục "Account"
            var jsonFiles = Directory.GetFiles(accountDirectory, "*.json");

            // Lấy danh sách tên của tất cả các kryptonToolStrip hiện có
            var existingToolStrips = kryptonPanel2.Controls.OfType<Krypton.Toolkit.KryptonToolStrip>().Select(t => t.Name).ToList();

            foreach (var jsonFile in jsonFiles)
            {
                // Đọc nội dung của tệp JSON
                var jsonString = File.ReadAllText(jsonFile);

                // Chuyển đổi chuỗi JSON thành đối tượng
                var jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);

                // Lấy giá trị của "name"
                var name = jsonObject.ContainsKey("name") ? jsonObject["name"].ToString() : null;
                var type = jsonObject.ContainsKey("type") ? jsonObject["type"].ToString() : null;
                // Kiểm tra xem có kryptonToolStrip nào có tên giống với "name" không
                if (!existingToolStrips.Contains(name))
                {
                    if (type == "online")
                    {
                        CreateOnlineToolStrip(name);
                    }
                    if (type == "offline")
                    {
                        CreateToolStrip(name);
                    }
                    
                }

                // Xóa tên khỏi danh sách
                existingToolStrips.Remove(name);
            }

            // Xóa tất cả các kryptonToolStrip còn lại
            foreach (var toolStripName in existingToolStrips)
            {
                var toolStrip = kryptonPanel2.Controls.OfType<Krypton.Toolkit.KryptonToolStrip>().FirstOrDefault(t => t.Name == toolStripName);
                if (toolStrip != null)
                {
                    kryptonPanel2.Controls.Remove(toolStrip);
                }
            }
        }




        private void CreateToolStrip(string name)
        {
            // Tạo một kryptonToolStrip1 mới
            var kryptonToolStrip1 = new Krypton.Toolkit.KryptonToolStrip
            {
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                Location = new Point(0, 0),
                Name = $"kryptonToolStrip{toolStripCount++}",
                Size = new Size(245, 25),
                TabIndex = 4,
                Text = "kryptonToolStrip1",
            };

            // Tạo một toolStripLabel1 mới
            var toolStripLabel1 = new ToolStripLabel
            {
                Name = $"toolStripLabel1{toolStripCount++}",
                Size = new Size(39, 22),
                Text = name,
            };

            // Tạo một toolStripButton1 mới
            var toolStripButton1 = new ToolStripButton
            {
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Image = Properties.Resources.delete,
                ImageTransparentColor = Color.Magenta,
                Name = $"toolStripButton{toolStripCount++}",
                Size = new Size(23, 22),
                Text = "toolStripButton1",
            };

            // Thêm sự kiện Click cho toolStripButton1
            toolStripButton1.Click += (sender, e) =>
            {
                // Xóa kryptonToolStrip1 khỏi kryptonPanel2
                kryptonPanel2.Controls.Remove(kryptonToolStrip1);

                // Xóa tệp JSON tương ứng
                var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var accountDirectory = Path.Combine(appDirectory, "Account");
                var jsonFilePath = Path.Combine(accountDirectory, $"{name}.json");
                File.Delete(jsonFilePath);
            };

            // Thêm toolStripLabel1 và toolStripButton1 vào kryptonToolStrip1
            kryptonToolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, toolStripButton1 });

            // Thêm kryptonToolStrip1 vào kryptonPanel2
            kryptonPanel2.Controls.Add(kryptonToolStrip1);
        }

        private void CreateOnlineToolStrip(string name)
        {
            // Tạo một kryptonToolStrip1 mới
            var kryptonToolStrip1 = new Krypton.Toolkit.KryptonToolStrip
            {
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                Location = new Point(0, 0),
                Name = $"kryptonToolStrip{toolStripCount++}",
                Size = new Size(245, 25),
                TabIndex = 4,
                Text = "kryptonToolStrip1",
            };

            var toolStripButton0 = new ToolStripButton
            {
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Image = Properties.Resources.Microsoft_Logo_icon_png_Transparent_Background,
                ImageTransparentColor = Color.Magenta,
                Name = $"toolStripButton{toolStripCount++}",
                Size = new Size(23, 22),
                Text = "toolStripButton1",
            };
            // Tạo một toolStripLabel1 mới
            var toolStripLabel1 = new ToolStripLabel
            {
                Name = $"toolStripLabel1{toolStripCount++}",
                Size = new Size(39, 22),
                Text = name,
            };

            // Tạo một toolStripButton1 mới
            var toolStripButton1 = new ToolStripButton
            {
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Image = Properties.Resources.logout,
                ImageTransparentColor = Color.Magenta,
                Name = $"toolStripButton{toolStripCount++}",
                Size = new Size(23, 22),
                Text = "toolStripButton1",
            };

            // Thêm sự kiện Click cho toolStripButton1
            toolStripButton1.Click += async (sender, e) =>
            {

                // Xóa kryptonToolStrip1 khỏi kryptonPanel2
                kryptonPanel2.Controls.Remove(kryptonToolStrip1);
                var loginHandler = JELoginHandlerBuilder.BuildDefault();
                // Xóa tệp JSON tương ứng
                var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var accountDirectory = Path.Combine(appDirectory, "Account");
                var jsonFilePath = Path.Combine(accountDirectory, $"{name}.json");
                var json = await File.ReadAllTextAsync(jsonFilePath);
                var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                File.Delete(jsonFilePath);
                var accounts = loginHandler.AccountManager.GetAccounts();
                var selectedAccount = accounts.GetAccount(data["Identifier"].ToString());
                await loginHandler.SignoutWithBrowser(selectedAccount);
            };

            // Thêm toolStripLabel1 và toolStripButton1 vào kryptonToolStrip1
            kryptonToolStrip1.Items.AddRange(new ToolStripItem[] {toolStripButton0,toolStripLabel1, toolStripButton1 });

            // Thêm kryptonToolStrip1 vào kryptonPanel2
            kryptonPanel2.Controls.Add(kryptonToolStrip1);
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private async void kryptonButton3_Click(object sender, EventArgs e)
        {
            try
            {
                var loginHandler = JELoginHandlerBuilder.BuildDefault();
                var accounts = loginHandler.AccountManager.GetAccounts();
                var session = await loginHandler.AuthenticateInteractively();
                int index = 0;

                // Lấy thư mục của ứng dụng
                var appDirectory = AppDomain.CurrentDomain.BaseDirectory;

                // Tạo thư mục "Account" nếu nó chưa tồn tại
                var accountDirectory = Path.Combine(appDirectory, "Account");
                Directory.CreateDirectory(accountDirectory);

                foreach (var account in accounts)
                {
                    if (account is not JEGameAccount jeAccount)
                        continue;
                    CreateOnlineToolStrip(jeAccount.XboxTokens?.XstsToken?.XuiClaims?.Gamertag);
                    // Tạo một đối tượng JSON mới với thông tin từ jeAccount
                    var json = new
                    {
                        type = "online",
                        Identifier = jeAccount.Identifier,
                        name = jeAccount.XboxTokens?.XstsToken?.XuiClaims?.Gamertag
                    };

                    // Ghi đối tượng JSON vào tệp trong thư mục "Account"
                    var jsonString = JsonSerializer.Serialize(json);
                    var filePath = Path.Combine(accountDirectory, jeAccount.XboxTokens?.XstsToken?.XuiClaims?.Gamertag + ".json");
                    File.WriteAllText(filePath, jsonString);

                    index++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "!!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }
    }
}

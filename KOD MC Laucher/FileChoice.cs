using Newtonsoft.Json;
using System.Net;
using System.IO.Compression;
namespace KOD_MC_Laucher
{
    public partial class FileChoice : Form
    {
        public async Task ReadDataFile()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var apipath = Path.Combine(appDirectory, "cf-api");

            // Read the JSON file
            var jsonFilePath = Path.Combine(apipath, "files.json");
            var jsonData = await File.ReadAllTextAsync(jsonFilePath);

            // Deserialize the JSON data to dynamic object
            dynamic jsonObject = JsonConvert.DeserializeObject(jsonData);

            // Clear the DataGridView
            dataGridView1.Rows.Clear();

            // Loop through each item in the JSON object
            foreach (var item in jsonObject)
            {
                // Extract the required information
                string fileName = item.fileName;
                DateTime fileDate = DateTime.Parse(item.fileDate.ToString());
                string ver = item.ver[0];
                string forge = item.ver[1];
                string downloadUrl = item.downloadUrl;
                string fileid = item.fileid;

                // Add a new row to the DataGridView
                dataGridView1.Rows.Add(fileName, ver, forge, fileDate, downloadUrl);
            }
        }

        public FileChoice()
        {
            InitializeComponent();
            ReadDataFile();
        }

        private async void kryptonButton1_Click(object sender, EventArgs e)
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var prspath = Path.Combine(appDirectory, "Packprs");

            // If 'prspath' contains any files, delete them except for 'buildinfo.json' and 'packicon.png'
            foreach (var file in Directory.GetFiles(prspath, "*.*", SearchOption.AllDirectories))
            {
                if (!file.EndsWith("buildinfo.json") && !file.EndsWith("packicon.png"))
                {
                    File.Delete(file);
                }
            }

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please Select File!");
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                // Do nothing if the user selects more than one row
            }
            else
            {
                int fileidValue = int.Parse(dataGridView1.SelectedRows[0].Cells["File ID"].Value.ToString());
                string filenamevar = dataGridView1.SelectedRows[0].Cells["File Name"].Value.ToString();
                string downloadlink = dataGridView1.SelectedRows[0].Cells["Url"].Value.ToString();

                // Download the file
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(downloadlink);

                    using (var fileStream = new FileStream(Path.Combine(prspath, filenamevar), FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await response.Content.CopyToAsync(fileStream);
                    }
                }

                // Unzip the file
                ZipFile.ExtractToDirectory(Path.Combine(prspath, filenamevar), prspath);

                // Delete the zip file
                File.Delete(Path.Combine(prspath, filenamevar));

                // Move all directories and files from the 'overrides' directory to 'prspath'
                var overridesPath = Path.Combine(prspath, "overrides");
                if (Directory.Exists(overridesPath))
                {
                    foreach (var dirPath in Directory.GetDirectories(overridesPath, "*", SearchOption.AllDirectories))
                    {
                        Directory.CreateDirectory(dirPath.Replace(overridesPath, prspath));
                    }

                    foreach (var newPath in Directory.GetFiles(overridesPath, "*.*", SearchOption.AllDirectories))
                    {
                        File.Move(newPath, newPath.Replace(overridesPath, prspath));
                    }

                    // Delete the 'overrides' directory
                    Directory.Delete(overridesPath, true);
                }
            }
        }



    }

}



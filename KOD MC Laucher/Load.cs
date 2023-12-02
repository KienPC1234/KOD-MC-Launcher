
namespace KOD_MC_Laucher
{
    public partial class Load : Form
    {
        public Load()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            var AppDictory = AppDomain.CurrentDomain.BaseDirectory;
            Directory.CreateDirectory(AppDictory + "\\Account");
            Directory.CreateDirectory(AppDictory + "\\Modpacks");
            Directory.CreateDirectory(AppDictory + "\\Data");
            Directory.CreateDirectory(AppDictory + "\\Temps");
            Directory.CreateDirectory(AppDictory + "\\Packprs");
            YourMethod();
        }
        private void YourMethod()
        {
            // Tạo một thread mới để chờ trong 6 giây
            Thread newThread = new Thread(new ThreadStart(WaitSixSeconds));
            newThread.Start();
        }

        private void WaitSixSeconds()
        {
            // Chờ trong 6 giây
            Thread.Sleep(6000);

            // Mở Form Main
            this.Invoke(new Action(() =>
            {
                Main mainForm = new Main();
                mainForm.Show();

                // Ẩn form hiện tại
                this.Hide();
            }));
        }
        private void kryptonPictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}

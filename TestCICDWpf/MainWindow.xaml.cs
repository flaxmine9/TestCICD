using AutoUpdaterDotNET;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Threading;

namespace TestCICDWpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AutoUpdater.InstalledVersion = new Version("1.1");

            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(10) };
            timer.Tick += delegate
            {
                AutoUpdater.Start("ftp:eu-west-1.sftpcloud.io/UpdateInfo.xml", new NetworkCredential("a4fb7a14b0d14773b0b68c41434ec015", "riVgMWA3Y93J1fV2Vw8NlnZmCPYJgmzj"));

                var currentDirectory = new DirectoryInfo(@"C:\Users\Flax\Documents\GitHub\TestCICD\TestCICDWpf");
                if (currentDirectory.Parent != null)
                {
                    AutoUpdater.InstallationPath = currentDirectory.Parent.FullName;
                }
            };
            timer.Start();
        }
    }
}
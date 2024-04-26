using FluentFTP;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace TestCICDWpf
{
    public partial class MainWindow : Window
    {
        private string Url { get; set; } = "eu-central-1.sftpcloud.io";
        private string UserName { get; set; } = "ea066c0c2dde4e91b2d8b8b881df1304";
        private string PassWord { get; set; } = "JpADT8knYnuULHwDfSdTUd8oioPGF25E";

        private FtpClient _client;

        private bool _isOpen = false;

        public MainWindow()
        {
            InitializeComponent();

            _client = new FtpClient(Url, UserName, PassWord);
            _client.AutoConnect();

            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            timer.Tick += delegate
            {
                if (Process.GetProcessesByName("AutoUpdater").Any())
                    return;

                Version newVersion = CheckNewVersion("VersionProject/version.txt");
                Version versionApp = Assembly.GetEntryAssembly()!.GetName().Version!;

                if (newVersion != versionApp)
                {
                    var msg = MessageBox.Show("Новое обновление", "Обновление", MessageBoxButton.YesNo);
                    if (msg == MessageBoxResult.Yes)
                    {
                        var currentProcess = Process.GetCurrentProcess();

                        // Запускаем процесс автоапдейтера (.exe)
                        Process.Start(currentProcess.MainModule!.FileName!.Replace(currentProcess.MainModule.ModuleName!, @"AutoUpdater/AutoUpdater.exe"));

                        currentProcess.CloseMainWindow();
                    }
                }
            };
            timer.Start();
        }

        public Version CheckNewVersion(string remotePath)
        {
            var boolDownload = _client.DownloadBytes(out var bytes, remotePath: remotePath);
            if (boolDownload)
            {
                var splittedVersions = Encoding.UTF8.GetString(bytes).Split('.');
                if (splittedVersions.Any())
                {
                    return new Version(Convert.ToInt32(splittedVersions[0]), Convert.ToInt32(splittedVersions[1]), Convert.ToInt32(splittedVersions[2]), 0);
                }
            }

            return new Version();
        }
    }
}
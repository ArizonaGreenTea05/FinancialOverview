using CommunityToolkit.Maui.Alerts;
using MauiMoneyMate.Pages;
using MauiMoneyMate.Popups;
using MauiMoneyMate.Translations;
using MauiMoneyMate.Utils;

namespace MauiMoneyMate
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(FilePage), typeof(FilePage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(DetailedSalesPage), typeof(DetailedSalesPage));

            if (CommonProperties.DownloadUpdatesAutomatically) new Thread(t =>                {
                CommonProperties.UpdateAvailable = CommonFunctions.CheckForUpdates();
                if (!CommonProperties.UpdateAvailable) return;
                Toast.Make(
                        $"{LanguageResource.NewAppVersionDetected}\n{LanguageResource.UpdateWillBeInstalledOnClosingTheApplication}")
                    .Show();
                Toast.Make($"{LanguageResource.DownloadingNewestVersion}").Show();
                CommonProperties.DownloadThread.Start();
            }).Start();

            if (!CommonProperties.CheckForUpdatesOnStart) return;
            CommonProperties.UpdateAvailable = CommonFunctions.CheckForUpdates();
        }
    }
}
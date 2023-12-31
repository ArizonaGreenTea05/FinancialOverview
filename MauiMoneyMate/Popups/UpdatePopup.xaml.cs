using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using MauiMoneyMate.Translations;
using MauiMoneyMate.Utils;

namespace MauiMoneyMate.Popups;

public partial class UpdatePopup : Popup
{
    private readonly Func<bool> _downloadFunction;
    private readonly Func<bool> _installFunction;
    private readonly Thread _updateThread;
    private bool _updateFinishedSuccessful = false;

    public UpdatePopup(Func<bool> downloadFunction, Func<bool> installFunction)
    {
        _updateThread = new Thread(RunUpdate);
        _downloadFunction = downloadFunction;
        _installFunction = installFunction;
        InitializeComponent();
        UpdateTitleLbl.LoadFromResource(nameof(UpdateTitleLbl));
        UpdateInfoLbl.LoadFromResource(nameof(UpdateInfoLbl));
        UpdateBtn.LoadFromResource(nameof(UpdateBtn));
    }

    protected override Task OnDismissedByTappingOutsideOfPopup()
    {
        base.OnDismissedByTappingOutsideOfPopup();
        if (!_updateThread.IsAlive) return Task.CompletedTask;
        _updateThread.Join();
        if (!_updateFinishedSuccessful) return Task.CompletedTask;
        if (Window != null) Application.Current?.CloseWindow(Window);
        Environment.Exit(1);
        return Task.CompletedTask;
    }

    private void Update_Clicked(object sender, EventArgs e)
    {
        UpdateBtn.IsEnabled = false;
        ActivityIndicator.IsRunning = true;
        _updateThread.Start();
    }
    
    private void RunUpdate()
    {
        if (!RunDownload()) return;
        if (!RunInstallation())
        {
            if (!RunDownload()) return;
            if (!RunInstallation()) return;
        }

        Toast.Make(LanguageResource.InstallationComplete).Show();
        _updateFinishedSuccessful = true;
    }

    private bool RunDownload()
    {
        Toast.Make(LanguageResource.DownloadingNewestVersion).Show();
        if (_downloadFunction.Invoke()) return true;
        Toast.Make(
                $"{LanguageResource.CouldNotDownloadUpdate}\n{LanguageResource.PleaseCheckYourInternetConnectionAndTryAgainLater}")
            .Show();
        return false;
    }

    private bool RunInstallation()
    {
        Toast.Make(LanguageResource.InstallingNewVersion).Show();
        if (_installFunction.Invoke()) return true;
        Toast.Make(LanguageResource.CouldNotInstallUpdate).Show();
        return false;
    }
}
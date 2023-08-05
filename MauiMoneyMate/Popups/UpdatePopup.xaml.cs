using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using MauiMoneyMate.Resources.Languages;
using MauiMoneyMate.Utils.ResourceItemTemplates;

namespace MauiMoneyMate.Popups;

public partial class UpdatePopup : Popup
{
    private readonly Func<bool> _downloadFunction;
    private readonly Func<bool> _installFunction;
    private ResourceLabel UpdateTitleLbl;
    private ResourceLabel UpdateInfoLbl;
    private ResourceButton UpdateBtn;
    private readonly Thread _updateThread;

    public UpdatePopup(Func<bool> downloadFunction, Func<bool> installFunction)
    {
        _updateThread = new Thread(RunUpdate);
        _downloadFunction = downloadFunction;
        _installFunction = installFunction;
        InitializeComponent();
        UpdateTitleLbl = new ResourceLabel(nameof(UpdateTitleLbl), Title);
        UpdateInfoLbl = new ResourceLabel(nameof(UpdateInfoLbl), Info);
        UpdateBtn = new ResourceButton(nameof(UpdateBtn), Update);
    }

    protected override void OnDismissedByTappingOutsideOfPopup()
    {
        if (_updateThread.IsAlive)
        {
            _updateThread.Join();
            Application.Current?.CloseWindow(Application.Current.MainPage.Window);
            Environment.Exit(1);
        }
        base.OnDismissedByTappingOutsideOfPopup();
    }

    private void Update_Clicked(object sender, EventArgs e)
    {
        Update.IsEnabled = false;
        ActivityIndicator.IsRunning = true;
        _updateThread.Start();
    }

    private void RunUpdate()
    {
        Toast.Make(LanguageResource.DownloadingNewestVersion).Show();
        if (!_downloadFunction.Invoke())
        {
            Toast.Make($"{LanguageResource.CouldNotDownloadUpdate}\n{LanguageResource.PleaseCheckYourInternetConnectionAndTryAgainLater}").Show();
            return;
        }
        Toast.Make(LanguageResource.InstallingNewVersion).Show();
        if (!_installFunction.Invoke())
        {
            Toast.Make(LanguageResource.CouldNotInstallUpdate).Show();
            return;
        }
        Toast.Make(LanguageResource.InstallationComplete).Show();
    }
}
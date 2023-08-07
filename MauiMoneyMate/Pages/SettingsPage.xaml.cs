using BusinessLogic;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using MauiMoneyMate.Popups;
using MauiMoneyMate.Resources.Languages;
using MauiMoneyMate.Utils;
using MauiMoneyMate.ViewModels;

namespace MauiMoneyMate.Pages;

public partial class SettingsPage : ContentPage
{
    #region private Properties

    private bool PageLoaded { get; set; } = false;

    #endregion

    private readonly SettingsViewModel _viewModel;

	public SettingsPage(SettingsViewModel fileViewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = fileViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing();
        PageLoaded = true;
    }

    private void CheckForUpdatesOnStartChk_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!PageLoaded) return;
        CommonProperties.CheckForUpdatesOnStart = _viewModel.CheckForUpdatesOnStart;
        _viewModel.LoadSettings();
    }

    private void DownloadUpdatesAutomaticallyChk_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!PageLoaded) return;
        CommonProperties.DownloadUpdatesAutomatically = _viewModel.DownloadUpdatesAutomatically;
        _viewModel.LoadSettings();
    }

    private void SearchForUpdateBtn_OnClicked(object sender, EventArgs e)
    {
        CommonProperties.UpdateAvailable = CommonFunctions.CheckForUpdates();
        if (!CommonProperties.UpdateAvailable)
        {
            Toast.Make(LanguageResource.NoUpdatesAvailable).Show();
            return;
        }
        this.ShowPopup(new UpdatePopup(CommonFunctions.DownloadLatestRelease, CommonFunctions.InstallDownloadedRelease));
    }

    private void ShowFilePathInTitleBarChk_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!PageLoaded) return;
        CommonProperties.ShowFilePathInTitleBar = _viewModel.ShowFilePathInTitleBar;
        _viewModel.LoadSettings();
        CommonFunctions.DisplayFilePathInTitleBar();
    }

    private void ThemePkr_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (!PageLoaded) return;
        CommonProperties.CurrentAppTheme = ((Picker)sender).SelectedIndex;
    }

    private void ExportSettingsBtn_OnClicked(object sender, EventArgs e)
    {
        if(CommonFunctions.ExportSettings()) return;
    }

    private void ImportSettingsBtn_OnClicked(object sender, EventArgs e)
    {
        if (CommonFunctions.ImportSettings())
        {
            _viewModel.LoadSettings();
            return;
        }
    }
}
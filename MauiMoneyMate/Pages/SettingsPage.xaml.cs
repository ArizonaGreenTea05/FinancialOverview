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
}
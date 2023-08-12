using MauiMoneyMate.ViewModels;

namespace MauiMoneyMate.Pages;

public partial class SettingsPage : ContentPage
{

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
    }

    private void CheckForUpdatesOnStartChk_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        => _viewModel.CheckForUpdatesOnStartChk_OnCheckedChanged(sender, e);

    private void DownloadUpdatesAutomaticallyChk_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        => _viewModel.DownloadUpdatesAutomaticallyChk_OnCheckedChanged(sender, e);

    private void SearchForUpdateBtn_OnClicked(object sender, EventArgs e)
        => _viewModel.SearchForUpdateBtn_OnClicked(sender, e);

    private void ShowFilePathInTitleBarChk_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        => _viewModel.ShowFilePathInTitleBarChk_OnCheckedChanged(sender, e);

    private void ThemePkr_OnSelectedIndexChanged(object sender, EventArgs e)
        => _viewModel.ThemePkr_OnSelectedIndexChanged(sender, e);

    private void ExportSettingsBtn_OnClicked(object sender, EventArgs e)
        => _viewModel.ExportSettingsBtn_OnClicked(sender, e);

    private void ImportSettingsBtn_OnClicked(object sender, EventArgs e)
        => _viewModel.ImportSettingsBtn_OnClicked(sender, e);

    private void DeleteTemporaryFilesBtn_OnClicked(object sender, EventArgs e)
        => _viewModel.DeleteTemporaryFilesBtn_OnClicked(sender, e);
}
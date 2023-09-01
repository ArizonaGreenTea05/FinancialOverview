using MauiMoneyMate.ViewModels;

namespace MauiMoneyMate.Pages;

public partial class FilePage : ContentPage
{
    private readonly FileViewModel _viewModel;

	public FilePage(FileViewModel fileViewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = fileViewModel;
        _viewModel.OnInitialized(this);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing();
    }

    private void ExitMnuFlt_OnClicked(object sender, EventArgs e) 
        => _viewModel.ExitMnuFlt_OnClicked(sender, e);

    private void GoToWebsiteMnuFlt_OnClicked(object sender, EventArgs e)
        => _viewModel.GoToWebsiteMnuFlt_OnClicked(sender, e);

    private void WriteTicketMnuFlt_OnClicked(object sender, EventArgs e)
        => _viewModel.WriteTicketMnuFlt_OnClicked(sender, e);

    private void OpenSettingsMnuFlt_OnClicked(object sender, EventArgs e)
        => _viewModel.OpenSettingsMnuFlt_OnClicked(sender, e);

    private void SystemThemeMnuFlt_OnClicked(object sender, EventArgs e)
        => _viewModel.SystemThemeMnuFlt_OnClicked(sender, e);

    private void LightThemeMnuFlt_OnClicked(object sender, EventArgs e)
        => _viewModel.LightThemeMnuFlt_OnClicked(sender, e);

    private void DarkMnuFlt_OnClicked(object sender, EventArgs e)
        => _viewModel.DarkMnuFlt_OnClicked(sender, e);

    private void BackMnuFlt_OnClicked(object sender, EventArgs e)
        => _viewModel.BackMnuFlt_OnClicked(sender, e);
}
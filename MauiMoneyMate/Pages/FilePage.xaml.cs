using MauiMoneyMate.ViewModels;

namespace MauiMoneyMate.Pages;

public partial class FilePage : ContentPage
{
    private readonly FileViewModel _viewModel;

	public FilePage(FileViewModel fileViewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = fileViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing();
    }
}
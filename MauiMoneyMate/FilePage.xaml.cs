using CommunityToolkit.Mvvm.Input;
using MauiMoneyMate.ViewModels;

namespace MauiMoneyMate;

public partial class FilePage : ContentPage
{
	public FilePage(FileViewModel fileViewModel)
	{
		InitializeComponent();
		BindingContext = fileViewModel;
    }
}
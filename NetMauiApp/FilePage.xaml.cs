using CommunityToolkit.Mvvm.Input;
using NetMauiApp.ViewModels;

namespace NetMauiApp;

public partial class FilePage : ContentPage
{
	public FilePage(FileViewModel fileViewModel)
	{
		InitializeComponent();
		BindingContext = fileViewModel;
    }
}
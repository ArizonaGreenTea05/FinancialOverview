using Microsoft.Maui.Controls.Internals;
using NetMauiApp.ViewModels;

namespace NetMauiApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
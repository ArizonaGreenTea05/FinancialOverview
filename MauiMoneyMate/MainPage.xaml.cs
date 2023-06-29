using MauiMoneyMate.ViewModels;
using Microsoft.Maui.Controls.Internals;

namespace MauiMoneyMate
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel;

        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void TimeUnitPkr_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            _viewModel.TimeUnitChanged();
        }
    }
}
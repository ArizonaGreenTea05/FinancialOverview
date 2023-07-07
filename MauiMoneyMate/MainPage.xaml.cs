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

        private void MainPage_OnLoaded(object sender, EventArgs e)
        {
            _viewModel.OnLoaded();
        }

        private void MonthlySalesEntry_OnCompleted(object sender, EventArgs e)
        {
            MonthlyNameEntry.Focus();
        }

        private void MonthlyNameEntry_OnCompleted(object sender, EventArgs e)
        {
            MonthlyAdditionEntry.Focus();
        }

        private void MonthlyAdditionEntry_OnCompleted(object sender, EventArgs e)
        {
            _viewModel.AddMonthly();
        }

        private void YearlySalesEntry_OnCompleted(object sender, EventArgs e)
        {
            YearlyNameEntry.Focus();
        }

        private void YearlyNameEntry_OnCompleted(object sender, EventArgs e)
        {
            YearlyAdditionEntry.Focus();
        }

        private void YearlyAdditionEntry_OnCompleted(object sender, EventArgs e)
        {
            _viewModel.AddYearly();
        }
    }
}
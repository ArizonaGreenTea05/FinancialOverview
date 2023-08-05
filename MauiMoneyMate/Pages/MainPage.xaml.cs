using CommonLibrary;
using CommunityToolkit.Maui.Views;
using MauiMoneyMate.Popups;
using MauiMoneyMate.Utils;
using MauiMoneyMate.ViewModels;

namespace MauiMoneyMate.Pages
{
    public partial class MainPage
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
            if (!_viewModel.ShowUpdatePopup) return;
            this.ShowPopup(new UpdatePopup(CommonFunctions.DownloadLatestRelease,
                CommonFunctions.InstallDownloadedRelease));
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

        private void ShowHelp(object sender, EventArgs e)
        {
            var popup = new HelpPopup();
            this.ShowPopup(popup);
        }
    }
}
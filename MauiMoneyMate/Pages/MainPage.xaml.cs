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
            Application.Current.MainPage.Window.Title = nameof(MauiMoneyMate);
            Application.Current.MainPage.Window.MinimumWidth = 800;
            Application.Current.MainPage.Window.MinimumHeight = 450;
            Application.Current.MainPage.Window.Width = 1600;
            Application.Current.MainPage.Window.Height = 900;
            Application.Current.MainPage.Window.X = 50;
            Application.Current.MainPage.Window.Y = 50;
            _viewModel.OnPageInitialized();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void TimeUnitPkr_OnSelectedIndexChanged(object sender, EventArgs e) 
            => _viewModel.TimeUnitPkr_OnSelectedIndexChanged(sender, e);

        private void MainPage_OnLoaded(object sender, EventArgs e)
            => _viewModel.OnLoaded(sender, e);

        private void MonthlySalesEntry_OnCompleted(object sender, EventArgs e)
        {
            MonthlyNameEntry.Focus();
        }

        private void MonthlyNameEntry_OnCompleted(object sender, EventArgs e)
        {
            MonthlyAdditionEntry.Focus();
        }

        private void MonthlyAdditionEntry_OnCompleted(object sender, EventArgs e)
            => _viewModel.MonthlyAdditionEntry_OnCompleted(sender, e);

        private void YearlySalesEntry_OnCompleted(object sender, EventArgs e)
        {
            YearlyNameEntry.Focus();
        }

        private void YearlyNameEntry_OnCompleted(object sender, EventArgs e)
        {
            YearlyAdditionEntry.Focus();
        }

        private void YearlyAdditionEntry_OnCompleted(object sender, EventArgs e)
            => _viewModel.YearlyAdditionEntry_OnCompleted(sender, e);

        private void HelpBtn_OnClicked(object sender, EventArgs e)
            => _viewModel.HelpBtn_OnClicked(sender, e);
    }
}
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
            Application.Current.MainPage.Window.MinimumWidth = 1080;
            Application.Current.MainPage.Window.MinimumHeight = 600;
            Application.Current.MainPage.Window.Width = 1600;
            Application.Current.MainPage.Window.Height = 900;
            Application.Current.MainPage.Window.X = 50;
            Application.Current.MainPage.Window.Y = 50;
            _viewModel.OnPageInitialized(this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing(this);
        }

        private void MonthPkr_OnSelectedIndexChanged(object sender, EventArgs e) 
            => _viewModel.MonthPkr_OnSelectedIndexChanged(sender, e);

        private void YearPkr_OnSelectedIndexChanged(object sender, EventArgs e)
            => _viewModel.YearPkr_OnSelectedIndexChanged(sender, e);

        private void MainPage_OnLoaded(object sender, EventArgs e)
            => _viewModel.OnLoaded(sender, e);

        private void NewSaleBtn_OnClicked(object sender, EventArgs e)
            => _viewModel.NewSaleBtn_OnClicked(sender, e);
    }
}
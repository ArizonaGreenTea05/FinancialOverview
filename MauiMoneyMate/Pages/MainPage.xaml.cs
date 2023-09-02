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

        private void OpenFilePageMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.OpenFilePageMnuFlt_OnClicked(sender, e);

        private void NewMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.NewMnuFlt_OnClicked(sender, e);

        private void OpenMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.OpenMnuFlt_OnClicked(sender, e);

        private void SaveMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.SaveMnuFlt_OnClicked(sender, e);

        private void SaveAsMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.SaveAsMnuFlt_OnClicked(sender, e);

        private void ExitMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.ExitMnuFlt_OnClicked(sender, e);

        private void OpenSettingsMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.OpenSettingsMnuFlt_OnClicked(sender, e);

        private void UndoMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.UndoMnuFlt_OnClicked(sender, e);

        private void RedoMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.RedoMnuFlt_OnClicked(sender, e);

        private void GoToWebsiteMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.GoToWebsiteMnuFlt_OnClicked(sender, e);

        private void WriteTicketMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.WriteTicketMnuFlt_OnClicked(sender, e);

        private void DetailedSalesMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.DetailedSalesMnuFlt_OnClicked(sender, e);

        private void SystemThemeMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.SystemThemeMnuFlt_OnClicked(sender, e);

        private void LightThemeMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.LightThemeMnuFlt_OnClicked(sender, e);

        private void DarkMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.DarkMnuFlt_OnClicked(sender, e);

        private void RefreshMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.RefreshMnuFlt_OnClicked(sender, e);

        private void AppInfoMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.AppInfoMnuFlt_OnClicked(sender, e);
    }
}
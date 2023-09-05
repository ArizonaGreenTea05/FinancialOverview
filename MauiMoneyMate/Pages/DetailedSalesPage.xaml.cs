using MauiMoneyMate.ViewModels;

namespace MauiMoneyMate.Pages
{
    public partial class DetailedSalesPage
    {
        private readonly DetailedSalesViewModel _viewModel;

        public DetailedSalesPage(DetailedSalesViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
            _viewModel.OnPageInitialized(this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing(this);
        }

        private void DetailedSalesPage_OnLoaded(object sender, EventArgs e)
            => _viewModel.DetailedSalesPage_OnLoaded(sender, e);

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

        private void SystemThemeMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.SystemThemeMnuFlt_OnClicked(sender, e);

        private void LightThemeMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.LightThemeMnuFlt_OnClicked(sender, e);

        private void DarkMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.DarkMnuFlt_OnClicked(sender, e);

        private void OverviewMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.OverviewMnuFlt_OnClicked(sender, e);

        private void BackMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.BackMnuFlt_OnClicked(sender, e);

        private void RefreshMnuFlt_OnClicked(object sender, EventArgs e)
            => _viewModel.RefreshMnuFlt_OnClicked(sender, e);
    }
}
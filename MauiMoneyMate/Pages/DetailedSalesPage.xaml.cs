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
    }
}
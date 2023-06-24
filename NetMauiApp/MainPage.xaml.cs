using Microsoft.Maui.Controls.Internals;

namespace NetMauiApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnUpdateClicked(object sender, EventArgs e)
        {
            UpdateBtn.Text = "boo";
        }

        private void OnMonthlyAddClicked(object sender, EventArgs e)
        {
            MonthlyAddBtn.Text = "boo";
        }

        private void OnYearlyAddClicked(object sender, EventArgs e)
        {
            YearlyAddBtn.Text = "boo";
        }

        private void OnMainPageLoaded(object sender, EventArgs e)
        {
            TimeUnitPkr.SelectedIndex = 0;
        }

        private void OnDeleteEntryBtnClicked(object sender, EventArgs e)
        {
            
        }
    }
}
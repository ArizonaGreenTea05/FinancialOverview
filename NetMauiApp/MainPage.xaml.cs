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
            UpdateBtn.Text = "helloo";
            
            SemanticScreenReader.Announce(UpdateBtn.Text);
        }

        private void OnMainPageLoaded(object sender, EventArgs e)
        {
            TimeUnitPkr.SelectedIndex = 0;
        }
    }
}
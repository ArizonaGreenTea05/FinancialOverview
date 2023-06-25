using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinancialOverview;

namespace NetMauiApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {

        [ObservableProperty]
        private ObservableCollection<string> _monthlySales;

        [ObservableProperty]
        private ObservableCollection<string> _yearlySales;

        [ObservableProperty]
        private ObservableCollection<string> _allSales;

        [ObservableProperty]
        private string _updateBtnText;

        [ObservableProperty]
        private string _addBtnText;

        [ObservableProperty] 
        private string _monthlySalesEntryText;

        [ObservableProperty]
        private string _monthlyNameEntryText;

        [ObservableProperty]
        private string _monthlyAdditionEntryText;

        [ObservableProperty]
        private string _yearlySalesEntryText;

        [ObservableProperty]
        private string _yearlyNameEntryText;

        [ObservableProperty]
        private string _yearlyAdditionEntryText;

        private FinancialOverview.FinancialOverview _financialOverview;

        public MainViewModel(FinancialOverview.FinancialOverview financialOverview)
        {
            _financialOverview = financialOverview;
            UpdateBtnText = "Update";
            AddBtnText = "Add";
            MonthlySales = new ObservableCollection<string>();
            YearlySales = new ObservableCollection<string>();
            AllSales = new ObservableCollection<string>();
        }

        [RelayCommand]
        void AddMonthly()
        {
            if (string.IsNullOrWhiteSpace(MonthlySalesEntryText) || string.IsNullOrWhiteSpace(MonthlyNameEntryText))
                return;
            MonthlySales.Add(
                $@"{MonthlySalesEntryText} | {MonthlyNameEntryText} {
                    (string.IsNullOrWhiteSpace(MonthlyAdditionEntryText) 
                    ? "" 
                    : $" | {MonthlyAdditionEntryText}"
                    )}");
            MonthlySalesEntryText = string.Empty;
            MonthlyNameEntryText = string.Empty;
            MonthlyAdditionEntryText = string.Empty;
        }

        [RelayCommand]
        void AddYearly()
        {
            if (string.IsNullOrWhiteSpace(YearlySalesEntryText) || string.IsNullOrWhiteSpace(YearlyNameEntryText))
                return;
            YearlySales.Add($@"{YearlySalesEntryText} | {YearlyNameEntryText} {(string.IsNullOrWhiteSpace(YearlyAdditionEntryText)
                    ? ""
                    : $" | {MonthlyAdditionEntryText}"
                )}");
            YearlySalesEntryText = string.Empty;
            YearlyNameEntryText = string.Empty;
            YearlyAdditionEntryText = string.Empty;
        }

        [RelayCommand]
        void DeleteMonthly(string s)
        {
            if (MonthlySales.Contains(s)) 
                MonthlySales.Remove(s);
        }

        [RelayCommand]
        void DeleteYearly(string s)
        {
            if (YearlySales.Contains(s))
                YearlySales.Remove(s);
        }

        [RelayCommand]
        void Update()
        {

        }
    }
}

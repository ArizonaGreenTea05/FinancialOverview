using System.Collections.ObjectModel;
using System.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BusinessLogic;

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
        private string _deleteBtnText;

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

        [ObservableProperty]
        private int _timeUnit;

        [ObservableProperty]
        private decimal _restMoney;

        private readonly FinancialOverview _financialOverview;

        public MainViewModel(FinancialOverview financialOverview)
        {
            AddBtnText = "Add";
            DeleteBtnText = "Delete";
            _financialOverview = financialOverview;
            _financialOverview.LoadData();

            MonthlySales = new ObservableCollection<string>();
            YearlySales = new ObservableCollection<string>();
            AllSales = new ObservableCollection<string>();
            foreach (DataRow row in _financialOverview.MonthlySales.Rows)
                Add(MonthlySales, Convert.ToDouble(row[0]), Convert.ToString(row[1]), Convert.ToString(row[2]));
            foreach (DataRow row in _financialOverview.YearlySales.Rows)
                Add(YearlySales, Convert.ToDouble(row[0]), Convert.ToString(row[1]), Convert.ToString(row[2]));
            UpdateAllSales();

            _timeUnit = (int)_financialOverview.UnitOfAll;
        }

        [RelayCommand]
        void AddMonthly()
        {
            if (string.IsNullOrWhiteSpace(MonthlySalesEntryText) || string.IsNullOrWhiteSpace(MonthlyNameEntryText))
                return;
            Add(MonthlySales, Convert.ToDouble(MonthlySalesEntryText), MonthlyNameEntryText, MonthlyAdditionEntryText);
            MonthlySalesEntryText = string.Empty;
            MonthlyNameEntryText = string.Empty;
            MonthlyAdditionEntryText = string.Empty;
        }

        [RelayCommand]
        void AddYearly()
        {
            if (string.IsNullOrWhiteSpace(YearlySalesEntryText) || string.IsNullOrWhiteSpace(YearlyNameEntryText))
                return;
            Add(MonthlySales, Convert.ToDouble(YearlySalesEntryText), YearlyNameEntryText, YearlyAdditionEntryText);
            YearlySalesEntryText = string.Empty;
            YearlyNameEntryText = string.Empty;
            YearlyAdditionEntryText = string.Empty;
        }

        [RelayCommand]
        void DeleteMonthly(string s)
        {
            if (MonthlySales.Contains(s)) 
                MonthlySales.Remove(s);
            UpdateAllSales();
        }

        [RelayCommand]
        void DeleteYearly(string s)
        {
            if (YearlySales.Contains(s))
                YearlySales.Remove(s);
            UpdateAllSales();
        }

        public void TimeUnitChanged()
        {
            _financialOverview.UnitOfAll = (FinancialOverview.Unit)_timeUnit;
            UpdateAllSales();
        }

        private void UpdateAllSales()
        {
            var tmp = _financialOverview.AllSales.Copy();
            AllSales.Clear();
            foreach (DataRow row in tmp.Rows)
                Add(AllSales, Convert.ToDouble(row[0]), Convert.ToString(row[1]), Convert.ToString(row[2]));
            RestMoney = _financialOverview.GetRest();
        }

        private void Add(ObservableCollection<string> collection, double sales, string name, string addition = null)
        {
            collection.Add(
                $@"{sales} | {name} {(string.IsNullOrWhiteSpace(addition)
                        ? ""
                        : $" | {addition}"
                    )}");
        }
    }
}

using System.Collections.ObjectModel;
using System.Data;
using System.Resources;
using BusinessLogic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NetMauiApp.Resources.Languages;

namespace NetMauiApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<string> _monthlySales;

    [ObservableProperty] private ObservableCollection<string> _yearlySales;

    [ObservableProperty] private ObservableCollection<string> _allSales;

    [ObservableProperty] private string _deleteBtnText;

    [ObservableProperty] private string _addBtnText;

    [ObservableProperty] private int _addBtnFontSize;

    [ObservableProperty] private string _namePlaceholder;

    [ObservableProperty] private string _additionPlaceholder;

    [ObservableProperty] private string _allSalesLblText;

    [ObservableProperty] private string _monthlySalesLblText;

    [ObservableProperty] private string _yearlySalesLblText;

    [ObservableProperty] private string _restLblText;

    [ObservableProperty] private string _unitLblText;

    [ObservableProperty] private string _monthlySalesEntryText;

    [ObservableProperty] private string _monthlyNameEntryText;

    [ObservableProperty] private string _monthlyAdditionEntryText;

    [ObservableProperty] private string _yearlySalesEntryText;

    [ObservableProperty] private string _yearlyNameEntryText;

    [ObservableProperty] private string _yearlyAdditionEntryText;

    [ObservableProperty] private ObservableCollection<string> _timeUnits;

    [ObservableProperty] private int _selectedTimeUnit;

    [ObservableProperty] private decimal _restMoney;

    private readonly FinancialOverview _financialOverview;
    private readonly ResourceManager _resources = MainViewResource.ResourceManager;

    public MainViewModel(FinancialOverview financialOverview)
    {
        TimeUnits = new ObservableCollection<string>();
        LoadResources();

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

        SelectedTimeUnit = (int)_financialOverview.UnitOfAll;
    }

    [RelayCommand]
    private void AddMonthly()
    {
        if (string.IsNullOrWhiteSpace(MonthlySalesEntryText) || string.IsNullOrWhiteSpace(MonthlyNameEntryText))
            return;
        Add(MonthlySales, Convert.ToDouble(MonthlySalesEntryText), MonthlyNameEntryText, MonthlyAdditionEntryText);
        MonthlySalesEntryText = string.Empty;
        MonthlyNameEntryText = string.Empty;
        MonthlyAdditionEntryText = string.Empty;
    }

    [RelayCommand]
    private void AddYearly()
    {
        if (string.IsNullOrWhiteSpace(YearlySalesEntryText) || string.IsNullOrWhiteSpace(YearlyNameEntryText))
            return;
        Add(MonthlySales, Convert.ToDouble(YearlySalesEntryText), YearlyNameEntryText, YearlyAdditionEntryText);
        YearlySalesEntryText = string.Empty;
        YearlyNameEntryText = string.Empty;
        YearlyAdditionEntryText = string.Empty;
    }

    [RelayCommand]
    private void DeleteMonthly(string s)
    {
        if (MonthlySales.Contains(s))
            MonthlySales.Remove(s);
        UpdateAllSales();
    }

    [RelayCommand]
    private void DeleteYearly(string s)
    {
        if (YearlySales.Contains(s))
            YearlySales.Remove(s);
        UpdateAllSales();
    }

    public void TimeUnitChanged()
    {
        _financialOverview.UnitOfAll = (FinancialOverview.Unit)_selectedTimeUnit;
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

    private void LoadResources()
    {
        AddBtnText = _resources.GetString("Add") ?? string.Empty;
        AddBtnFontSize = Convert.ToInt16(_resources.GetString("AddBtnFontSize"));
        DeleteBtnText = _resources.GetString("Delete") ?? string.Empty;
        AllSalesLblText = _resources.GetString("AllSales") ?? string.Empty;
        NamePlaceholder = _resources.GetString("NamePlaceholder") ?? string.Empty;
        AdditionPlaceholder = _resources.GetString("AdditionPlaceholder") ?? string.Empty;
        MonthlySalesLblText = _resources.GetString("MonthlySales") ?? string.Empty;
        YearlySalesLblText = _resources.GetString("YearlySales") ?? string.Empty;
        RestLblText = _resources.GetString("RestLblText") ?? string.Empty;
        UnitLblText = _resources.GetString("MoneyUnit") ?? string.Empty;
        TimeUnits.Clear();
        foreach (var name in Enum.GetNames(typeof(FinancialOverview.Unit)))
            TimeUnits.Add(_resources.GetString(name) ?? string.Empty);
    }
}
using System.Collections.ObjectModel;
using System.Data;
using BusinessLogic;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMoneyMate.Resources.Languages;
using MauiMoneyMate.Utils;

namespace MauiMoneyMate.ViewModels;

public partial class MainViewModel : ObservableObject
{
    #region private Observable Properties

    [ObservableProperty] private ObservableCollection<string> _monthlySales;

    [ObservableProperty] private ObservableCollection<string> _yearlySales;

    [ObservableProperty] private ObservableCollection<string> _allSales;

    [ObservableProperty] private string _financialOverviewTitle;

    [ObservableProperty] private string _deleteBtnText;

    [ObservableProperty] private string _addBtnText;

    [ObservableProperty] private string _filePageBtnText;

    [ObservableProperty] private string _mainPageBtnText;

    [ObservableProperty] private int _addBtnFontSize;

    [ObservableProperty] private string _namePlaceholder;

    [ObservableProperty] private string _additionPlaceholder;

    [ObservableProperty] private string _allSalesLblText;

    [ObservableProperty] private string _monthlySalesLblText;

    [ObservableProperty] private string _yearlySalesLblText;

    [ObservableProperty] private string _restLblText;

    [ObservableProperty] private string _monthlySalesEntryText;

    [ObservableProperty] private string _monthlyNameEntryText;

    [ObservableProperty] private string _monthlyAdditionEntryText;

    [ObservableProperty] private string _yearlySalesEntryText;

    [ObservableProperty] private string _yearlyNameEntryText;

    [ObservableProperty] private string _yearlyAdditionEntryText;

    [ObservableProperty] private ObservableCollection<string> _timeUnits;

    [ObservableProperty] private int _selectedTimeUnit;

    [ObservableProperty] private decimal _restMoney;

    [ObservableProperty] private string _moneyUnit;

    #endregion

    #region private Members

    private readonly FinancialOverview _financialOverview;
    private readonly CommonVariables _commonVariables;
    private readonly Dictionary<string, DataRow> _monthlySalesDict;
    private readonly Dictionary<string, DataRow> _yearlySalesDict;
    private readonly string _appDataFilePath = Path.Combine(Environment.GetFolderPath(
        Environment.SpecialFolder.ApplicationData) + @"\MauiMoneyMate", "MauiMoneyMate.AppData");

    #endregion

    #region public Constructors

    public MainViewModel(FinancialOverview financialOverview, CommonVariables commonVariables)
    {
        TimeUnits = new ObservableCollection<string>();
        LoadResources();

        _commonVariables = commonVariables;
        _financialOverview = financialOverview;
        _financialOverview.DefaultFilePath = LoadStringFromAppData().Split("\r\n")[0];
        DataIsSaved = File.Exists(_financialOverview.DefaultFilePath);
        _financialOverview.OnDefaultFilePathChanged += OnDefaultFilePathChanged;

        _monthlySalesDict = new Dictionary<string, DataRow>();
        _yearlySalesDict = new Dictionary<string, DataRow>();

        MonthlySales = new ObservableCollection<string>();
        YearlySales = new ObservableCollection<string>();
        AllSales = new ObservableCollection<string>();

        SelectedTimeUnit = (int)_financialOverview.UnitOfAll;
    }

    #endregion

    #region private EventHandlers

    private void OnDefaultFilePathChanged(object sender, string path)
    {
        SaveStringToAppData(path);
    }

    #endregion

    #region private Relay Commands

    [RelayCommand]
    private async Task AddMonthly(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(MonthlySalesEntryText) || string.IsNullOrWhiteSpace(MonthlyNameEntryText))
            return;
        var tmp = ConvertToLabelText(Convert.ToDouble(MonthlySalesEntryText), MonthlyNameEntryText,
            MonthlyAdditionEntryText);
        if (MonthlySales.Contains(tmp))
        {
            await Toast.Make(string.Format(TextResource.AlreadyContainsEntry, MonthlySalesLblText, tmp))
                .Show(cancellationToken);
            return;
        }

        _monthlySalesDict[tmp] =
            _financialOverview.MonthlySales.Rows.Add(MonthlySalesEntryText, MonthlyNameEntryText,
                MonthlyAdditionEntryText);
        MonthlySales.Add(tmp);
        MonthlySalesEntryText = string.Empty;
        MonthlyNameEntryText = string.Empty;
        MonthlyAdditionEntryText = string.Empty;
        UpdateAllSales();
        DataIsSaved = false;
    }

    [RelayCommand]
    private async Task AddYearly(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(YearlySalesEntryText) || string.IsNullOrWhiteSpace(YearlyNameEntryText))
            return;
        var tmp = ConvertToLabelText(Convert.ToDouble(YearlySalesEntryText), YearlyNameEntryText,
            YearlyAdditionEntryText);
        if (YearlySales.Contains(tmp))
        {
            await Toast.Make(string.Format(TextResource.AlreadyContainsEntry, YearlySalesLblText, tmp))
                .Show(cancellationToken);
            return;
        }

        _yearlySalesDict[tmp] =
            _financialOverview.YearlySales.Rows.Add(YearlySalesEntryText, YearlyNameEntryText,
                YearlyAdditionEntryText);
        YearlySales.Add(tmp);
        YearlySalesEntryText = string.Empty;
        YearlyNameEntryText = string.Empty;
        YearlyAdditionEntryText = string.Empty;
        UpdateAllSales();
        DataIsSaved = false;
    }

    [RelayCommand]
    private void DeleteMonthly(string s)
    {
        if (MonthlySales.Contains(s))
        {
            MonthlySales.Remove(s);
            _monthlySalesDict[s].Delete();
        }

        UpdateAllSales();
        DataIsSaved = false;
    }

    [RelayCommand]
    private void DeleteYearly(string s)
    {
        if (YearlySales.Contains(s))
        {
            YearlySales.Remove(s);
            _yearlySalesDict[s].Delete();
        }

        UpdateAllSales();
        DataIsSaved = false;
    }

    [RelayCommand]
    private async Task SwitchToFilePage()
    {
        await Shell.Current.GoToAsync(nameof(FilePage));
    }

    #endregion

    #region public Methods

    public void OnAppearing()
    {
        UpdateSales();
        DisplaySavingState();
    }

    public void OnLoaded()
    {
        if (!_financialOverview.LoadData())
        {
            var path = FileHandler.OpenFileDialog();
            if (null == path)
            {
                Toast.Make(TextResource.CouldNotOpenFile).Show();
                return;
            }
            _financialOverview.LoadData(path);
        }
        UpdateSales();
    }

    public void TimeUnitChanged()
    {
        _financialOverview.UnitOfAll = (FinancialOverview.Unit)_selectedTimeUnit;
        UpdateAllSales();
    }

    #endregion

    #region private Methods

    private void SaveStringToAppData(string s)
    {
        if (!FileHandler.WriteTextToFile(s, _appDataFilePath))
            return;
    }

    private string LoadStringFromAppData()
    {
        var text = FileHandler.ReadTextFile(_appDataFilePath);
        return text ?? string.Empty;
    }

    private bool DataIsSaved
    {
        set
        {
            _commonVariables.DataIsSaved = value;
            DisplaySavingState();
        }
        get => _commonVariables.DataIsSaved;
    }

    private void DisplaySavingState()
    {
        FinancialOverviewTitle = FinancialOverviewTitle.TrimEnd('*');
        if (!DataIsSaved)
            FinancialOverviewTitle += '*';
    }

    private void UpdateSales()
    {
        UpdateMonthlySales();
        UpdateYearlySales();
        UpdateAllSales();
    }

    private void UpdateMonthlySales()
    {
        MonthlySales.Clear();
        foreach (DataRow row in _financialOverview.MonthlySales.Rows)
        {
            var tmp = ConvertToLabelText(Convert.ToDouble(row[0]), Convert.ToString(row[1]), Convert.ToString(row[2]));
            MonthlySales.Add(tmp);
            _monthlySalesDict[tmp] = row;
        }
    }

    private void UpdateYearlySales()
    {
        YearlySales.Clear();
        foreach (DataRow row in _financialOverview.YearlySales.Rows)
        {
            var tmp = ConvertToLabelText(Convert.ToDouble(row[0]), Convert.ToString(row[1]), Convert.ToString(row[2]));
            YearlySales.Add(tmp);
            _yearlySalesDict[tmp] = row;
        }
    }

    private void UpdateAllSales()
    {
        var tmp = _financialOverview.AllSales.Copy();
        AllSales.Clear();
        foreach (DataRow row in tmp.Rows)
            AllSales.Add(ConvertToLabelText(Convert.ToDouble(row[0]), Convert.ToString(row[1]),
                Convert.ToString(row[2])));
        RestMoney = _financialOverview.GetRest();
    }

    private string ConvertToLabelText(double sales, string name, string addition = null)
    {
        return $@"{sales} | {name} {(string.IsNullOrWhiteSpace(addition)
                ? ""
                : $" | {addition}"
            )}";
    }

    private void LoadResources()
    {
        FinancialOverviewTitle = TextResource.FinancialOverviewTitle ?? string.Empty;
        FilePageBtnText = TextResource.FilePageTitle ?? string.Empty;
        AddBtnText = TextResource.Add ?? string.Empty;
        AddBtnFontSize = Convert.ToInt16(TextResource.AddBtnFontSize);
        DeleteBtnText = TextResource.Delete ?? string.Empty;
        AllSalesLblText = TextResource.AllSales ?? string.Empty;
        NamePlaceholder = TextResource.NamePlaceholder ?? string.Empty;
        AdditionPlaceholder = TextResource.AdditionPlaceholder ?? string.Empty;
        MonthlySalesLblText = TextResource.MonthlySales ?? string.Empty;
        YearlySalesLblText = TextResource.YearlySales ?? string.Empty;
        RestLblText = TextResource.RestLblText ?? string.Empty;
        MoneyUnit = TextResource.MoneyUnit ?? string.Empty;
        TimeUnits.Clear();
        foreach (var name in Enum.GetNames(typeof(FinancialOverview.Unit)))
            TimeUnits.Add(TextResource.ResourceManager.GetString(name) ?? string.Empty);
    }

    #endregion
}
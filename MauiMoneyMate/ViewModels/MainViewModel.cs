﻿using System.Collections.ObjectModel;
using System.Data;
using BusinessLogic;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMoneyMate.Resources.Languages;
using MauiMoneyMate.Utils;
using MauiMoneyMate.Utils.ResourceItemTemplates;

namespace MauiMoneyMate.ViewModels;

public partial class MainViewModel : ObservableObject
{
    #region private Observable Properties

    [ObservableProperty] private ObservableCollection<string> _monthlySales;

    [ObservableProperty] private ObservableCollection<string> _yearlySales;

    [ObservableProperty] private ObservableCollection<string> _allSales;

    [ObservableProperty] private string _financialOverviewTitle;

    [ObservableProperty] private string _mainPageBtnText;

    [ObservableProperty] private int _addBtnFontSize;

    [ObservableProperty] private string _namePlaceholder;

    [ObservableProperty] private string _additionPlaceholder;

    [ObservableProperty] private decimal _restMoney;

    [ObservableProperty] private ResourceLabel _allSalesLbl;

    [ObservableProperty] private ResourceLabel _monthlySalesLbl;

    [ObservableProperty] private ResourceLabel _yearlySalesLbl;

    [ObservableProperty] private ResourceLabel _restLbl;

    [ObservableProperty] private ResourceLabel _restMoneyLbl;

    [ObservableProperty] private ResourceLabel _moneyUnitLbl;

    [ObservableProperty] private ResourceButton _filePageBtn;

    [ObservableProperty] private ResourceButton _yearlyAddBtn;

    [ObservableProperty] private ResourceButton _monthlyAddBtn;

    [ObservableProperty] private ResourceButton _deleteBtn;

    [ObservableProperty] private ResourceEntry _monthlySalesEntry;

    [ObservableProperty] private ResourceEntry _monthlyNameEntry;

    [ObservableProperty] private ResourceEntry _monthlyAdditionEntry;

    [ObservableProperty] private ResourceEntry _yearlySalesEntry;

    [ObservableProperty] private ResourceEntry _yearlyNameEntry;

    [ObservableProperty] private ResourceEntry _yearlyAdditionEntry;

    [ObservableProperty] private ObservableCollection<string> _timeUnits;

    [ObservableProperty] private int _selectedTimeUnit;

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
        if (string.IsNullOrWhiteSpace(MonthlySalesEntry.Text) || string.IsNullOrWhiteSpace(MonthlyNameEntry.Text))
            return;
        var tmp = ConvertToLabelText(Convert.ToDouble(MonthlySalesEntry.Text), MonthlyNameEntry.Text,
            MonthlyAdditionEntry.Text);
        if (MonthlySales.Contains(tmp))
        {
            await Toast.Make(string.Format(LanguageResource.AlreadyContainsEntry, MonthlySalesLbl.Text, tmp))
                .Show(cancellationToken);
            return;
        }

        _monthlySalesDict[tmp] =
            _financialOverview.MonthlySales.Rows.Add(MonthlySalesEntry.Text, MonthlyNameEntry.Text,
                MonthlyAdditionEntry.Text);
        MonthlySales.Add(tmp);
        MonthlySalesEntry.Text = string.Empty;
        MonthlyNameEntry.Text = string.Empty;
        MonthlyAdditionEntry.Text = string.Empty;
        UpdateAllSales();
        DataIsSaved = false;
    }

    [RelayCommand]
    private async Task AddYearly(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(YearlySalesEntry.Text) || string.IsNullOrWhiteSpace(YearlyNameEntry.Text))
            return;
        var tmp = ConvertToLabelText(Convert.ToDouble(YearlySalesEntry.Text), YearlyNameEntry.Text,
            YearlyAdditionEntry.Text);
        if (YearlySales.Contains(tmp))
        {
            await Toast.Make(string.Format(LanguageResource.AlreadyContainsEntry, YearlySalesLbl.Text, tmp))
                .Show(cancellationToken);
            return;
        }

        _yearlySalesDict[tmp] =
            _financialOverview.YearlySales.Rows.Add(YearlySalesEntry.Text, YearlyNameEntry.Text,
                YearlyAdditionEntry.Text);
        YearlySales.Add(tmp);
        YearlySalesEntry.Text = string.Empty;
        YearlyNameEntry.Text = string.Empty;
        YearlyAdditionEntry.Text = string.Empty;
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
                Toast.Make(LanguageResource.CouldNotOpenFile).Show();
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
        if (App.Window == null) return;
        App.Window.Title = App.Window.Title?.Split('-')[0].TrimEnd();
        App.Window.Title += $" - {_financialOverview.DefaultFilePath ?? "none"}";
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
        AllSalesLbl = new ResourceLabel(nameof(AllSalesLbl));
        MonthlySalesLbl = new ResourceLabel(nameof(MonthlySalesLbl));
        YearlySalesLbl = new ResourceLabel(nameof(YearlySalesLbl));
        RestLbl = new ResourceLabel(nameof(RestLbl));
        RestMoneyLbl = new ResourceLabel(nameof(RestMoneyLbl));
        RestMoney = Convert.ToDecimal(RestMoneyLbl.Text);
        MoneyUnitLbl = new ResourceLabel(nameof(MoneyUnitLbl));
        FilePageBtn = new ResourceButton(nameof(FilePageBtn));
        MonthlyAddBtn = new ResourceButton(nameof(MonthlyAddBtn));
        YearlyAddBtn = new ResourceButton(nameof(YearlyAddBtn));
        DeleteBtn = new ResourceButton(nameof(DeleteBtn));
        MonthlySalesEntry = new ResourceEntry(nameof(MonthlySalesEntry));
        MonthlyNameEntry = new ResourceEntry(nameof(MonthlyNameEntry));
        MonthlyAdditionEntry = new ResourceEntry(nameof(MonthlyAdditionEntry));
        YearlySalesEntry = new ResourceEntry(nameof(YearlySalesEntry));
        YearlyNameEntry = new ResourceEntry(nameof(YearlyNameEntry));
        YearlyAdditionEntry = new ResourceEntry(nameof(YearlyAdditionEntry));
        FinancialOverviewTitle = LanguageResource.FinancialOverviewTitle ?? string.Empty;
        TimeUnits.Clear();
        foreach (var name in Enum.GetNames(typeof(FinancialOverview.Unit)))
            TimeUnits.Add(LanguageResource.ResourceManager.GetString(name) ?? string.Empty);
    }

    #endregion
}
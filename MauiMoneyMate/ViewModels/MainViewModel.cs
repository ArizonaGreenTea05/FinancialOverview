using System;
using System.Collections.ObjectModel;
using System.Data;
using BusinessLogic;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMoneyMate.Pages;
using MauiMoneyMate.Popups;
using MauiMoneyMate.Translations;
using MauiMoneyMate.Utils;
using MauiMoneyMate.Utils.ResourceItemTemplates;

namespace MauiMoneyMate.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public sealed class SalesItem
    {
        public decimal Value { get; }
        public string Name { get; }
        public string Comment { get; }
        public object[] DataRow { get; }

        public SalesItem(DataRow dataRow)
        {
            DataRow = dataRow.ItemArray;
            Value = Convert.ToDecimal(dataRow[0]);
            Name = Convert.ToString(dataRow[1]);
            Comment = Convert.ToString(dataRow[2]);
        }

        public SalesItem(object[] dataRow)
        {
            DataRow = dataRow;
            Value = Convert.ToDecimal(dataRow[0]);
            Name = Convert.ToString(dataRow[1]);
            Comment = Convert.ToString(dataRow[2]);
        }

        public SalesItem(decimal value, string name, string comment)
        {
            Value = value;
            Name = name;
            Comment = comment;
            DataRow = new object[] { Value, Name, Comment };
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(SalesItem)) return false;
            var si = obj as SalesItem;
            return Value == si.Value && Name == si.Name && Comment == si.Comment;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DataRow, Value, Name, Comment);
        }
    }

    #region private Observable Properties

    [ObservableProperty] private ObservableCollection<SalesItem> _monthlySales;

    [ObservableProperty] private ObservableCollection<SalesItem> _yearlySales;

    [ObservableProperty] private ObservableCollection<SalesItem> _allSales;

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

    [ObservableProperty] private ResourceButton _undoBtn;

    [ObservableProperty] private ResourceButton _redoBtn;

    [ObservableProperty] private ResourceButton _settingsPageBtn;

    [ObservableProperty] private ResourceButton _helpBtn;

    [ObservableProperty] private ResourceButton _yearlyAddBtn;

    [ObservableProperty] private ResourceButton _monthlyAddBtn;

    [ObservableProperty] private ResourceButton _deleteBtn;

    [ObservableProperty] private ResourceButton _editBtn;

    [ObservableProperty] private ResourceEntry _monthlySalesEntry;

    [ObservableProperty] private ResourceEntry _monthlyNameEntry;

    [ObservableProperty] private ResourceEntry _monthlyAdditionEntry;

    [ObservableProperty] private ResourceEntry _yearlySalesEntry;

    [ObservableProperty] private ResourceEntry _yearlyNameEntry;

    [ObservableProperty] private ResourceEntry _yearlyAdditionEntry;

    [ObservableProperty] private string _monthlySalesEntryInput;

    [ObservableProperty] private string _monthlyNameEntryInput;

    [ObservableProperty] private string _monthlyAdditionEntryInput;

    [ObservableProperty] private string _yearlySalesEntryInput;

    [ObservableProperty] private string _yearlyNameEntryInput;

    [ObservableProperty] private string _yearlyAdditionEntryInput;

    [ObservableProperty] private ObservableCollection<string> _timeUnits;

    [ObservableProperty] private int _selectedTimeUnit;

    #endregion

    #region private Properties

    private MainPage MainPage
    {
        get => _mainPage;
        set => _mainPage ??= value;
    }

    #endregion

    #region private Members
    
    private static readonly Thread DownloadThread = new(() => CommonFunctions.DownloadLatestRelease());
    private MainPage _mainPage;

    #endregion

    #region public Constructors

    public MainViewModel()
    {
        TimeUnits = new ObservableCollection<string>();
        LoadResources();

        var tmpHistory = LoadStringFromAppData().Replace("\r", "").Split("\n").ToList();
        if (tmpHistory.Count >= 1 && string.IsNullOrEmpty(tmpHistory[^1])) tmpHistory.RemoveAt(tmpHistory.Count - 1);
        CommonProperties.FinancialOverview.FileHistory = tmpHistory;
        DataIsSaved = File.Exists(CommonProperties.FinancialOverview.FilePath);
        CommonProperties.FinancialOverview.OnDefaultFilePathChanged += OnDefaultFilePathChanged;

        MonthlySales = new ObservableCollection<SalesItem>();
        YearlySales = new ObservableCollection<SalesItem>();
        AllSales = new ObservableCollection<SalesItem>();

        SelectedTimeUnit = (int)CommonProperties.FinancialOverview.UnitOfAll;
    }

    #endregion

    #region public Relay Commands

    [RelayCommand]
    public void AddMonthly()
    {
        if (string.IsNullOrWhiteSpace(MonthlySalesEntryInput) || string.IsNullOrWhiteSpace(MonthlyNameEntryInput))
            return;
        if (!decimal.TryParse(MonthlySalesEntryInput, out var monthlySalesEntry))
        {
            Toast.Make(string.Format(LanguageResource.IsNoValidDecimal, MonthlySalesEntryInput))
                .Show();
            return;
        }

        var tmp = new SalesItem(monthlySalesEntry, MonthlyNameEntryInput, MonthlyAdditionEntryInput);
        if (MonthlySales.Contains(tmp))
        {
            Toast.Make(string.Format(LanguageResource.AlreadyContainsEntry, MonthlySalesLbl.Text, tmp))
                .Show();
            return;
        }

        CommonProperties.FinancialOverview.MonthlySales.Rows.Add(tmp.DataRow);
        MonthlySales.Add(tmp);
        MonthlySalesEntryInput = string.Empty;
        MonthlyNameEntryInput = string.Empty;
        MonthlyAdditionEntryInput = string.Empty;
        UpdateAllSales();
        DataIsSaved = false;
    }

    [RelayCommand]
    public void AddYearly()
    {
        if (string.IsNullOrWhiteSpace(YearlySalesEntryInput) || string.IsNullOrWhiteSpace(YearlyNameEntryInput))
            return;
        if (!decimal.TryParse(YearlySalesEntryInput, out var yearlySalesEntry))
        {
            Toast.Make(string.Format(LanguageResource.IsNoValidDecimal, MonthlySalesEntryInput))
                .Show();
            return;
        }

        var tmp = new SalesItem(yearlySalesEntry, YearlyNameEntryInput, YearlyAdditionEntryInput);
        if (YearlySales.Contains(tmp))
        {
            Toast.Make(string.Format(LanguageResource.AlreadyContainsEntry, YearlySalesLbl.Text, tmp))
                .Show();
            return;
        }

        CommonProperties.FinancialOverview.YearlySales.Rows.Add(tmp.DataRow);
        YearlySales.Add(tmp);
        YearlySalesEntryInput = string.Empty;
        YearlyNameEntryInput = string.Empty;
        YearlyAdditionEntryInput = string.Empty;
        UpdateAllSales();
        DataIsSaved = false;
    }

    #endregion

    #region private Relay Commands

    [RelayCommand]
    private void EditMonthly(string s)
    {
        /*if (!MonthlySales.Contains(s)) return;
        var tmp = s.Split('|');
        MonthlySalesEntryInput = tmp[0].Trim();
        MonthlyNameEntryInput = tmp[1].Trim();
        if (tmp.Length > 2) MonthlyAdditionEntryInput = tmp[2].Trim();
        DeleteMonthly(s);*/
    }

    [RelayCommand]
    private void DeleteMonthly(string s)
    {
        /*if (MonthlySales.Contains(s))
        {
            MonthlySales.Remove(s);
            _monthlySalesDict[s].Delete();
        }

        UpdateAllSales();
        DataIsSaved = false;*/
    }

    [RelayCommand]
    private void EditYearly(string s)
    {
        /*if (!YearlySales.Contains(s)) return;
        var tmp = s.Split('|');
        YearlySalesEntryInput = tmp[0].Trim();
        YearlyNameEntryInput = tmp[1].Trim();
        if (tmp.Length > 2) YearlyAdditionEntryInput = tmp[2].Trim();
        DeleteYearly(s);*/
    }

    [RelayCommand]
    private void DeleteYearly(string s)
    {
        /*if (YearlySales.Contains(s))
        {
            YearlySales.Remove(s);
            _yearlySalesDict[s].Delete();
        }

        UpdateAllSales();
        DataIsSaved = false;*/
    }

    [RelayCommand]
    private async Task SwitchToFilePage()
    {
        await Shell.Current.GoToAsync(nameof(FilePage));
    }

    [RelayCommand]
    private async Task SwitchToSettingsPage()
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }

    [RelayCommand]
    private void Undo()
    {
        CommonProperties.FinancialOverview.Undo();
        UpdateSales();
    }

    [RelayCommand]
    private void Redo()
    {
        CommonProperties.FinancialOverview.Redo();
        UpdateSales();
    }

    #endregion

    #region private EventHandlers

    private void OnDefaultFilePathChanged(object sender, string path)
    {
        SaveListToAppData(CommonProperties.FinancialOverview.FileHistory);
    }

    #endregion

    #region internal Event Handlers

    internal void OnAppearing()
    {
        CommonFunctions.UpdateAppTheme(CommonProperties.CurrentAppTheme);
        UpdateSales();
        DisplaySavingState();
    }

    internal void OnLoaded(object sender, EventArgs e)
    {
        CommonProperties.FinancialOverview.LoadData();
        UpdateSales();
        CommonProperties.FinancialOverview.ClearHistory();
        CommonProperties.FinancialOverview.AddCurrentStateToHistory();

        Application.Current!.MainPage!.Window.Destroying += (sender, args) =>
        {
            if (CommonProperties.UpdateAvailable && CommonProperties.DownloadUpdatesAutomatically)
                new Thread(UpdateProgram).Start();
        };

        if (CommonProperties.CheckForUpdatesOnStart || CommonProperties.DownloadUpdatesAutomatically)
            CommonProperties.UpdateAvailable = CommonFunctions.CheckForUpdates();
        if (!CommonProperties.UpdateAvailable) return;
        if (!CommonProperties.DownloadUpdatesAutomatically)
        {
            MainPage ??= (sender as Element).GetAncestor<MainPage>();
            MainPage.ShowPopup(new UpdatePopup(CommonFunctions.DownloadLatestRelease,
                CommonFunctions.InstallDownloadedRelease));
            return;
        }

        Toast.Make(
                $"{LanguageResource.NewAppVersionDetected}\n{LanguageResource.UpdateWillBeInstalledOnClosingTheApplication}")
            .Show();
        Toast.Make($"{LanguageResource.DownloadingNewestVersion}").Show();
        DownloadThread.Start();
    }

    internal void TimeUnitPkr_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CommonProperties.FinancialOverview.UnitOfAll = (FinancialOverview.Unit)_selectedTimeUnit;
        UpdateAllSales();
    }

    internal void MonthlyAdditionEntry_OnCompleted(object sender, EventArgs e)
    {
        AddMonthly();
    }

    internal void YearlyAdditionEntry_OnCompleted(object sender, EventArgs e)
    {
        AddYearly();
    }

    internal void HelpBtn_OnClicked(object sender, EventArgs e)
    {
        var popup = new HelpPopup();
        MainPage ??= (sender as Element).GetAncestor<MainPage>();
        MainPage.ShowPopup(popup);
    }

    #endregion

    #region private Methods

    private static void UpdateProgram()
    {
        if (DownloadThread.IsAlive) DownloadThread.Join();
        if (!CommonFunctions.DownloadLatestRelease())
        {
            Toast.Make(
                    $"{LanguageResource.CouldNotDownloadUpdate}\n{LanguageResource.PleaseCheckYourInternetConnectionAndTryAgainLater}")
                .Show();
            return;
        }

        if (!CommonFunctions.InstallDownloadedRelease())
        {
            Toast.Make(LanguageResource.CouldNotInstallUpdate).Show();
            if (!CommonFunctions.DownloadLatestRelease())
            {
                Toast.Make(
                        $"{LanguageResource.CouldNotDownloadUpdate}\n{LanguageResource.PleaseCheckYourInternetConnectionAndTryAgainLater}")
                    .Show();
                return;
            }

            if (!CommonFunctions.InstallDownloadedRelease())
                Toast.Make(LanguageResource.CouldNotInstallUpdate).Show();
        }

        Toast.Make(LanguageResource.InstallationComplete).Show();
    }

    private void SaveListToAppData(List<string> content)
    {
        if (!FileHandler.WriteTextToFile(content, CommonProperties.FileHistoryFilePath))
            return;
    }

    private string LoadStringFromAppData()
    {
        var text = FileHandler.ReadTextFile(File.Exists(CommonProperties.FileHistoryFilePath)
            ? CommonProperties.FileHistoryFilePath
            : CommonProperties.FileHistoryFilePath.Replace("FileHistory", "AppData"));
        return text ?? string.Empty;
    }

    private bool DataIsSaved
    {
        set
        {
            CommonProperties.DataIsSaved = value;
            DisplaySavingState();
        }
        get => CommonProperties.DataIsSaved;
    }

    private void DisplaySavingState()
    {
        FinancialOverviewTitle = FinancialOverviewTitle.TrimEnd('*');
        if (!DataIsSaved)
            FinancialOverviewTitle += '*';
        CommonFunctions.DisplayFilePathInTitleBar();
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
        foreach (DataRow row in CommonProperties.FinancialOverview.MonthlySales.Rows)
            MonthlySales.Add(new SalesItem(Convert.ToDecimal(row[0]), Convert.ToString(row[1]), Convert.ToString(row[2])));
    }

    private void UpdateYearlySales()
    {
        YearlySales.Clear();
        foreach (DataRow row in CommonProperties.FinancialOverview.YearlySales.Rows)
            YearlySales.Add(new SalesItem(row));
    }

    private void UpdateAllSales()
    {
        AllSales.Clear();
        foreach (DataRow row in CommonProperties.FinancialOverview.AllSales.Rows)
            AllSales.Add(new SalesItem(row));
        RestMoney = CommonProperties.FinancialOverview.GetRest();
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
        SettingsPageBtn = new ResourceButton(nameof(SettingsPageBtn));
        HelpBtn = new ResourceButton(nameof(HelpBtn));
        MonthlyAddBtn = new ResourceButton(nameof(MonthlyAddBtn));
        YearlyAddBtn = new ResourceButton(nameof(YearlyAddBtn));
        DeleteBtn = new ResourceButton(nameof(DeleteBtn));
        EditBtn = new ResourceButton(nameof(EditBtn));
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
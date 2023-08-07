using System.Collections.ObjectModel;
using System.Data;
using BusinessLogic;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMoneyMate.Pages;
using MauiMoneyMate.Popups;
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

    #region private Members
    
    private readonly Dictionary<string, DataRow> _monthlySalesDict;
    private readonly Dictionary<string, DataRow> _yearlySalesDict;
    private static readonly Thread DownloadThread = new(() => CommonFunctions.DownloadLatestRelease());

    #endregion

    #region public Constructors

    public MainViewModel()
    {
        TimeUnits = new ObservableCollection<string>();
        LoadResources();
        
        var tmpHistory = LoadStringFromAppData().Replace("\r", "").Split("\n").ToList();
        if (tmpHistory.Count >= 1 && string.IsNullOrEmpty(tmpHistory[^1])) tmpHistory.RemoveAt(tmpHistory.Count-1);
        CommonProperties.FinancialOverview.FileHistory = tmpHistory;
        DataIsSaved = File.Exists(CommonProperties.FinancialOverview.FilePath);
        CommonProperties.FinancialOverview.OnDefaultFilePathChanged += OnDefaultFilePathChanged;

        _monthlySalesDict = new Dictionary<string, DataRow>();
        _yearlySalesDict = new Dictionary<string, DataRow>();

        MonthlySales = new ObservableCollection<string>();
        YearlySales = new ObservableCollection<string>();
        AllSales = new ObservableCollection<string>();

        SelectedTimeUnit = (int)CommonProperties.FinancialOverview.UnitOfAll;
    }

    #endregion

    #region private EventHandlers

    private void OnDefaultFilePathChanged(object sender, string path)
    {
        SaveListToAppData(CommonProperties.FinancialOverview.FileHistory);
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
        var tmp = ConvertToLabelText(monthlySalesEntry, MonthlyNameEntryInput,
            MonthlyAdditionEntryInput);
        if (MonthlySales.Contains(tmp))
        {
            Toast.Make(string.Format(LanguageResource.AlreadyContainsEntry, MonthlySalesLbl.Text, tmp))
                .Show();
            return;
        }

        _monthlySalesDict[tmp] =
            CommonProperties.FinancialOverview.MonthlySales.Rows.Add(MonthlySalesEntryInput, MonthlyNameEntryInput,
                MonthlyAdditionEntryInput);
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
        var tmp = ConvertToLabelText(yearlySalesEntry, YearlyNameEntryInput,
            YearlyAdditionEntryInput);
        if (YearlySales.Contains(tmp))
        {
            Toast.Make(string.Format(LanguageResource.AlreadyContainsEntry, YearlySalesLbl.Text, tmp))
                .Show();
            return;
        }

        _yearlySalesDict[tmp] =
            CommonProperties.FinancialOverview.YearlySales.Rows.Add(YearlySalesEntryInput, YearlyNameEntryInput,
                YearlyAdditionEntryInput);
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
        if (!MonthlySales.Contains(s)) return;
        var tmp = s.Split('|');
        MonthlySalesEntryInput = tmp[0].Trim();
        MonthlyNameEntryInput = tmp[1].Trim();
        if (tmp.Length > 2) MonthlyAdditionEntryInput = tmp[2].Trim();
        DeleteMonthly(s);
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
    private void EditYearly(string s)
    {
        if (!YearlySales.Contains(s)) return;
        var tmp = s.Split('|');
        YearlySalesEntryInput = tmp[0].Trim();
        YearlyNameEntryInput = tmp[1].Trim();
        if (tmp.Length > 2) YearlyAdditionEntryInput = tmp[2].Trim();
        DeleteYearly(s);
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

    #region public Methods

    public void OnAppearing()
    {
        UpdateSales();
        DisplaySavingState();
    }

    public void OnLoaded()
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
        ShowUpdatePopup = !CommonProperties.DownloadUpdatesAutomatically;
        if (CommonProperties.DownloadUpdatesAutomatically)
        {
            Toast.Make(
                    $"{LanguageResource.NewAppVersionDetected}\n{LanguageResource.UpdateWillBeInstalledOnClosingTheApplication}")
                .Show();
            Toast.Make($"{LanguageResource.DownloadingNewestVersion}").Show();
            DownloadThread.Start();
        }
    }

    public void TimeUnitChanged()
    {
        CommonProperties.FinancialOverview.UnitOfAll = (FinancialOverview.Unit)_selectedTimeUnit;
        UpdateAllSales();
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

    public bool ShowUpdatePopup { get; private set; }

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
        {
            var tmp = ConvertToLabelText(Convert.ToDecimal(row[0]), Convert.ToString(row[1]), Convert.ToString(row[2]));
            MonthlySales.Add(tmp);
            _monthlySalesDict[tmp] = row;
        }
    }

    private void UpdateYearlySales()
    {
        YearlySales.Clear();
        foreach (DataRow row in CommonProperties.FinancialOverview.YearlySales.Rows)
        {
            var tmp = ConvertToLabelText(Convert.ToDecimal(row[0]), Convert.ToString(row[1]), Convert.ToString(row[2]));
            YearlySales.Add(tmp);
            _yearlySalesDict[tmp] = row;
        }
    }

    private void UpdateAllSales()
    {
        var tmp = CommonProperties.FinancialOverview.AllSales.Copy();
        AllSales.Clear();
        foreach (DataRow row in tmp.Rows)
            AllSales.Add(ConvertToLabelText(Convert.ToDecimal(row[0]), Convert.ToString(row[1]),
                Convert.ToString(row[2])));
        RestMoney = CommonProperties.FinancialOverview.GetRest();
    }

    private string ConvertToLabelText(decimal sales, string name, string addition = null)
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
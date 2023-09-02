using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using System.Xml.Linq;
using FinancialOverview;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMoneyMate.Pages;
using MauiMoneyMate.Popups;
using MauiMoneyMate.Translations;
using MauiMoneyMate.Utils;
using MauiMoneyMate.Utils.ResourceItemTemplates;
#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
#endif
using static CommonLibrary.Functions;

namespace MauiMoneyMate.ViewModels;

public partial class MainViewModel : ObservableObject
{
    #region private Observable Properties

    [ObservableProperty] private ObservableCollection<SalesObject> _incomeCollection;

    [ObservableProperty] private ObservableCollection<SalesObject> _expensesCollection;

    [ObservableProperty] private string _financialOverviewTitle;

    [ObservableProperty] private string _mainPageBtnText;

    [ObservableProperty] private string _namePlaceholder;

    [ObservableProperty] private string _additionPlaceholder;

    [ObservableProperty] private string _restMoney;

    [ObservableProperty] private string _income;

    [ObservableProperty] private string _expenses;

    [ObservableProperty] private ResourceLabel _incomeTextLbl;

    [ObservableProperty] private ResourceLabel _expensesTextLbl;

    [ObservableProperty] private ResourceLabel _salesLbl;

    [ObservableProperty] private ResourceLabel _restLbl;

    [ObservableProperty] private ResourceLabel _restMoneyLbl;

    [ObservableProperty] private ResourceButton _detailsBtn;

    [ObservableProperty] private ResourceButton _editBtn;

    [ObservableProperty] private ResourceButton _deleteBtn;

    [ObservableProperty] private ResourceButton _newSalesBtn;

    [ObservableProperty] private ObservableCollection<string> _monthsCollection;

    [ObservableProperty] private int _selectedMonth;

    [ObservableProperty] private ObservableCollection<string> _yearsCollection;

    [ObservableProperty] private int _selectedYear;

    #endregion

    #region private Properties

    private MainPage MainPage
    {
        get => _mainPage;
        set => _mainPage ??= value;
    }
    private Enums.Month CurrentMonth { get; set; } = Enums.Month.WholeYear;
    private int CurrentYear { get; set; } = DateTime.Now.Year;

    #endregion

    #region private Members

    private static readonly Thread DownloadThread = new(() => CommonFunctions.DownloadLatestRelease());
    private static Rect _startUpBounds;
    private MainPage _mainPage;

    #endregion

    #region public Constructors

    public MainViewModel()
    {
        CommonFunctions.RemoveNonZipFiles(CommonProperties.UpdateDirectory);

        MonthsCollection = new ObservableCollection<string>();
        YearsCollection = new ObservableCollection<string>();
        IncomeCollection = new ObservableCollection<SalesObject>();
        ExpensesCollection = new ObservableCollection<SalesObject>();

        var tmpHistory = LoadStringFromAppData().Replace("\r", "").Split("\n").ToList();
        if (tmpHistory.Count >= 1 && string.IsNullOrEmpty(tmpHistory[^1])) tmpHistory.RemoveAt(tmpHistory.Count - 1);
        CommonProperties.FinancialOverview.FileHistory = tmpHistory;
        CommonProperties.FinancialOverview.OnDefaultFilePathChanged += OnDefaultFilePathChanged;

        SelectedMonth = -1;
    }

    #endregion

    #region private Relay Commands

    [RelayCommand]
    private void SortDescending(ObservableCollection<SalesObject> collection)
    {
        CommonFunctions.BubbleSortDescending(collection, MoveEntryDown);
        UpdateSales();
        CommonProperties.FinancialOverview.AddCurrentStateToHistory();
    }

    [RelayCommand]
    private void SortAscending(ObservableCollection<SalesObject> collection)
    {
        CommonFunctions.BubbleSortAscending(collection, MoveEntryDown);
        UpdateSales();
        CommonProperties.FinancialOverview.AddCurrentStateToHistory();
    }

    [RelayCommand]
    private void MoveEntryDown(SalesObject so)
    {
        MoveEntryDown(so, IncomeCollection.Contains(so) ? IncomeCollection : ExpensesCollection);
        CommonProperties.FinancialOverview.AddCurrentStateToHistory();
    }

    [RelayCommand]
    private void MoveEntryUp(SalesObject so)
    {
        MoveEntryUp(so, IncomeCollection.Contains(so) ? IncomeCollection : ExpensesCollection);
        CommonProperties.FinancialOverview.AddCurrentStateToHistory();
    }

    [RelayCommand]
    private void EditEntry(SalesObject so)
    {
        OpenNewSalesPopup(so.Parent ?? so);
    }

    [RelayCommand]
    private void DeleteEntry(SalesObject so)
    {
        CommonProperties.FinancialOverview.Sales.Remove(so.Parent ?? so);
        UpdateSales();
        DataIsSaved = false;
        CommonProperties.FinancialOverview.AddCurrentStateToHistory();
    }

    [RelayCommand]
    private void ShowDetails(SalesObject so)
    {
        MainPage.ShowPopup(new DetailsPopup(so.Parent ?? so, EditEntry, DeleteEntry));
    }

    #endregion

    #region private Event Handlers

    private ICommand OpenRecentMnuFlt_OnClicked(MainPage mainPage, string path)
    {
        return new Command(a =>
        {
            if (!CommonProperties.FinancialOverview.LoadData(path))
            {
                Toast.Make(LanguageResource.CouldNotOpenFile).Show();
                return;
            }

            CommonProperties.FinancialOverview.ClearHistory();
            DataIsSaved = true;
            UpdateSales();
            UpdateRecentlyOpenedFiles(mainPage);
        });
    }

    private void OnDefaultFilePathChanged(object sender, string path)
    {
        CommonFunctions.DisplayFilePathInTitleBar();
        SaveListToAppData(CommonProperties.FinancialOverview.FileHistory);
    }

    private void Window_OnDestroying(object sender, EventArgs e)
    {
#if WINDOWS
        SaveWindowState();
#endif
        if (CommonProperties.UpdateAvailable && CommonProperties.DownloadUpdatesAutomatically)
            UpdateProgram();
    }

    #endregion

    #region internal Event Handlers

    internal void RefreshMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        UpdateSales();
    }

    public void SystemThemeMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        CommonProperties.CurrentAppTheme = (int)AppTheme.Unspecified;
    }

    public void LightThemeMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        CommonProperties.CurrentAppTheme = (int)AppTheme.Light;
    }

    public void DarkMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        CommonProperties.CurrentAppTheme = (int)AppTheme.Dark;
    }

    internal void OpenFilePageMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        Shell.Current.GoToAsync(nameof(FilePage));
    }

    internal void DetailedSalesMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        Shell.Current.GoToAsync(nameof(DetailedSalesPage));
    }

    internal void NewMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        CommonFunctions.NewDocumentAction();
        DataIsSaved = false;
        UpdateSales();
    }

    internal void OpenMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        DataIsSaved = CommonFunctions.OpenFileAction() || DataIsSaved;
        UpdateSales();
    }

    internal void SaveMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        DataIsSaved = CommonFunctions.SaveFileAction();
    }

    internal void SaveAsMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        DataIsSaved = CommonFunctions.SaveFileAsAction();
    }

    internal void ExitMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {

        CommonFunctions.ExitAction();
    }

    internal void OpenSettingsMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        Shell.Current.GoToAsync(nameof(SettingsPage));
    }

    internal void UndoMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        DataIsSaved = !CommonProperties.FinancialOverview.Undo();
        UpdateSales();
    }

    internal void RedoMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        DataIsSaved = !CommonProperties.FinancialOverview.Redo();
        UpdateSales();
    }

    internal void GoToWebsiteMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        Browser.Default.OpenAsync(CommonProperties.WebsiteUrl, BrowserLaunchMode.SystemPreferred);
    }

    internal void WriteTicketMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        Browser.Default.OpenAsync(CommonProperties.NewIssueUrl, BrowserLaunchMode.SystemPreferred);
    }

    internal void OnPageInitialized(MainPage mainPage)
    {
        LoadSettings();
        LoadResources(mainPage);
        DataIsSaved = File.Exists(CommonProperties.FinancialOverview.FilePath);
#if WINDOWS
        RestoreWindowState();
#endif
    }

    internal void OnAppearing(MainPage mainPage)
    {
        UpdateSales();
        DisplaySavingState();
        UpdateRecentlyOpenedFiles(mainPage);
    }

    private void UpdateRecentlyOpenedFiles(MainPage mainPage)
    {
        mainPage.OpenRecentMnuFlt.Clear();
        mainPage.OpenRecentMnuFlt.IsEnabled = CommonProperties.FinancialOverview.FileHistory.Count != 0;
        foreach (var item in CommonProperties.FinancialOverview.FileHistory)
            mainPage.OpenRecentMnuFlt.Add(new MenuFlyoutItem
                { Text = Path.GetFileName(item), Command = OpenRecentMnuFlt_OnClicked(mainPage, item) });
    }

    internal void OnLoaded(object sender, EventArgs e)
    {
        MainPage ??= (sender as Element).GetAncestor<MainPage>();
        CommonProperties.FinancialOverview.LoadData();
        UpdateSales();
        CommonProperties.FinancialOverview.ClearHistory();
        CommonProperties.FinancialOverview.AddCurrentStateToHistory();

        Application.Current!.MainPage!.Window.Destroying += Window_OnDestroying;

        if (CommonProperties.DownloadUpdatesAutomatically)
        {
            new Thread(t =>
            {
                CommonProperties.UpdateAvailable = CommonFunctions.CheckForUpdates();
                if (!CommonProperties.UpdateAvailable) return;
                Toast.Make(
                        $"{LanguageResource.NewAppVersionDetected}\n{LanguageResource.UpdateWillBeInstalledOnClosingTheApplication}")
                    .Show();
                Toast.Make($"{LanguageResource.DownloadingNewestVersion}").Show();
                DownloadThread.Start();
            }).Start();
        }

        if (CommonProperties.CheckForUpdatesOnStart) CommonProperties.UpdateAvailable = CommonFunctions.CheckForUpdates();
        if (CommonProperties.UpdateAvailable) MainPage.ShowPopup(new UpdatePopup(CommonFunctions.DownloadLatestRelease, CommonFunctions.InstallDownloadedRelease));
    }

    internal void MonthPkr_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CurrentMonth = (Enums.Month)((Picker)sender).SelectedIndex;
        UpdateSales();
    }

    internal void YearPkr_OnSelectedIndexChanged(object sender, EventArgs eventArgs)
    {
        var yearBackup = CurrentYear;
        CurrentYear = Convert.ToInt32(YearsCollection[((Picker)sender).SelectedIndex]);
        if (CurrentYear == yearBackup + 1 && CurrentMonth == Enums.Month.December)
            SelectedMonth = (int)Enums.Month.January;
        else if (CurrentYear == yearBackup - 1 && CurrentMonth == Enums.Month.January)
            SelectedMonth = (int)Enums.Month.December;
        UpdateSales();
    }

    internal void NewSaleBtn_OnClicked(object sender, EventArgs e)
    {
        MainPage ??= (sender as Element).GetAncestor<MainPage>();
        OpenNewSalesPopup();
    }

    #endregion

    #region private Methods

    private void MoveEntryDown(SalesObject so, ICollection<SalesObject> collection)
    {
        if (collection.Count <= 1) return;
        SalesObject[] tmp;
        int index;
        do
        {
            tmp = collection.Select(item => item.Copy()).ToArray();
            index = CommonProperties.FinancialOverview.Sales.IndexOf(so.Parent ?? so);
            if (index >= CommonProperties.FinancialOverview.Sales.Count - 1) break;
            CommonProperties.FinancialOverview.Sales.Move(index, index + 1);
            UpdateSales();
        } while (AreEqual(tmp, collection));

        if (index >= CommonProperties.FinancialOverview.Sales.Count - 1) return;
        DataIsSaved = false;
    }

    private void MoveEntryUp(SalesObject so, ICollection<SalesObject> collection)
    {
        if (collection.Count <= 1) return;
        SalesObject[] tmp;
        int index;
        do
        {
            tmp = collection.Select(item => item.Copy()).ToArray();
            index = CommonProperties.FinancialOverview.Sales.IndexOf(so.Parent ?? so);
            if (index <= 0) break;
            CommonProperties.FinancialOverview.Sales.Move(index, index - 1);
            UpdateSales();
        } while (AreEqual(tmp, collection));

        if (index <= 0) return;
        DataIsSaved = false;
    }

    private void OpenNewSalesPopup(SalesObject salesObject = null)
    {
        var tmp = CommonProperties.FinancialOverview.Sales.Select(item => item.Copy()).ToArray();
        var newSalePopup = new NewSalePopup(CommonProperties.FinancialOverview.Sales.IndexOf(salesObject))
            {
                Size = new Size(600, MainPage.Height + 4)
            };
        newSalePopup.Closed += (sender, args) =>
        {
            if (AreEqual(tmp, CommonProperties.FinancialOverview.Sales)) return;
            CommonProperties.FinancialOverview.AddCurrentStateToHistory();
            UpdateSales();
            DataIsSaved = false;
        };
        MainPage.ShowPopup(newSalePopup);
    }

#if WINDOWS
    private void SaveWindowState()
    {
        var state = 2;
        var nativeWindowHandle = ((MauiWinUIWindow)Application.Current.Windows[0].Handler.PlatformView).WindowHandle;
        var win32WindowsId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
        if (AppWindow.GetFromWindowId(win32WindowsId).Presenter is OverlappedPresenter p)
            state = Convert.ToInt32(p.State);
        var isMaximized = state == Convert.ToInt32(OverlappedPresenterState.Maximized);
        var doc = new XDocument(
            new XElement("WindowState",
                new XElement("State", state),
                new XElement("X", isMaximized ? _startUpBounds.X : Application.Current.MainPage.Window.X),
                new XElement("Y", isMaximized ? _startUpBounds.Y : Application.Current.MainPage.Window.Y),
                new XElement("Width", isMaximized ? _startUpBounds.Width : Application.Current.MainPage.Window.Width),
                new XElement("Height", isMaximized ? _startUpBounds.Height : Application.Current.MainPage.Window.Height)));
        if (!Directory.Exists(Path.GetDirectoryName(CommonProperties.WindowStateFilePath)))
            Directory.CreateDirectory(CommonProperties.WindowStateFilePath);
        doc.Save(CommonProperties.WindowStateFilePath);
    }

    private void RestoreWindowState()
    {
        if (!File.Exists(CommonProperties.WindowStateFilePath)) return;
        var doc = XDocument.Load(CommonProperties.WindowStateFilePath);
        var state = Convert.ToInt32(doc.Descendants().Where(x => x.Name.LocalName == "State").ToList()[0].Value
            .Split('.')[0]);
        _startUpBounds.X = Application.Current.MainPage.Window.X =
            Convert.ToDouble(doc.Descendants().Where(x => x.Name.LocalName == "X").ToList()[0].Value.Split('.')[0]);
        _startUpBounds.Y = Application.Current.MainPage.Window.Y =
            Convert.ToDouble(doc.Descendants().Where(x => x.Name.LocalName == "Y").ToList()[0].Value.Split('.')[0]);
        _startUpBounds.Width = Application.Current.MainPage.Window.Width =
            Convert.ToDouble(
                doc.Descendants().Where(x => x.Name.LocalName == "Width").ToList()[0].Value.Split('.')[0]);
        _startUpBounds.Height = Application.Current.MainPage.Window.Height = Convert.ToDouble(
            doc.Descendants().Where(x => x.Name.LocalName == "Height").ToList()[0].Value.Split('.')[0]);
        var nativeWindowHandle = ((MauiWinUIWindow)App.Current.Windows[0].Handler.PlatformView).WindowHandle;
        var win32WindowsId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
        if (AppWindow.GetFromWindowId(win32WindowsId).Presenter is OverlappedPresenter p
            && state == Convert.ToInt32(OverlappedPresenterState.Maximized)) p.Maximize();
    }
#endif

    private static void LoadSettings()
    {
        CommonFunctions.UpdateAppTheme(CommonProperties.CurrentAppTheme);
        CommonFunctions.UpdateAppLanguage(CommonProperties.CurrentAppLanguage);
    }

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
        IncomeCollection.Clear();
        ExpensesCollection.Clear();
        foreach (var item in CommonProperties.FinancialOverview.GetSales(CurrentMonth, CurrentYear).ToList())
            if (item.Value > 0) IncomeCollection.Add(item);
            else ExpensesCollection.Add(item);
        RestMoney = string.Format(LanguageResource.MoneyFormat,
            CommonProperties.FinancialOverview.GetRestAsString(CurrentMonth, CurrentYear));
        Income = string.Format(LanguageResource.MoneyFormat,
            string.Format(CultureInfo.CurrentCulture, "{0:#,##0.00}",
                Math.Round(IncomeCollection.Sum(item => item.Value), 2)));
        Expenses = string.Format(LanguageResource.MoneyFormat,
            string.Format(CultureInfo.CurrentCulture, "{0:#,##0.00}",
                Math.Round(ExpensesCollection.Sum(item => item.Value), 2)));
    }

    private void LoadResources(MainPage mainPage)
    {
        new ResourceMenuBarItem(nameof(mainPage.FileMnu), mainPage.FileMnu);
        new ResourceMenuFlyout(nameof(mainPage.OpenFilePageMnuFlt), mainPage.OpenFilePageMnuFlt);
        new ResourceMenuFlyout(nameof(mainPage.NewMnuFlt), mainPage.NewMnuFlt);
        new ResourceMenuFlyout(nameof(mainPage.OpenMnuFlt), mainPage.OpenMnuFlt);
        new ResourceMenuFlyout(nameof(mainPage.OpenFileMnuFlt), mainPage.OpenFileMnuFlt);
        new ResourceMenuFlyout(nameof(mainPage.OpenRecentMnuFlt), mainPage.OpenRecentMnuFlt);
        new ResourceMenuFlyout(nameof(mainPage.SaveMnuFlt), mainPage.SaveMnuFlt);
        new ResourceMenuFlyout(nameof(mainPage.SaveAsMnuFlt), mainPage.SaveAsMnuFlt);
        new ResourceMenuFlyout(nameof(mainPage.ExitMnuFlt), mainPage.ExitMnuFlt);
        new ResourceMenuBarItem(nameof(mainPage.SettingsMnu), mainPage.SettingsMnu);
        new ResourceMenuFlyout(nameof(mainPage.OpenSettingsMnuFlt), mainPage.OpenSettingsMnuFlt);
        new ResourceMenuBarItem(nameof(mainPage.EditMnu), mainPage.EditMnu);
        new ResourceMenuFlyout(nameof(mainPage.UndoMnuFlt), mainPage.UndoMnuFlt);
        new ResourceMenuFlyout(nameof(mainPage.RedoMnuFlt), mainPage.RedoMnuFlt);
        new ResourceMenuBarItem(nameof(mainPage.HelpMnu), mainPage.HelpMnu);
        new ResourceMenuFlyout(nameof(mainPage.GoToWebsiteMnuFlt), mainPage.GoToWebsiteMnuFlt);
        new ResourceMenuFlyout(nameof(mainPage.WriteTicketMnuFlt), mainPage.WriteTicketMnuFlt);
        new ResourceMenuBarItem(nameof(mainPage.ViewMnu), mainPage.ViewMnu);
        new ResourceMenuFlyout(nameof(mainPage.DetailedSalesMnuFlt), mainPage.DetailedSalesMnuFlt);
        new ResourceMenuFlyout(nameof(mainPage.RefreshMnuFlt), mainPage.RefreshMnuFlt);
        new ResourceMenuFlyout(nameof(mainPage.AppThemeMnuFlt), mainPage.AppThemeMnuFlt);
        new ResourceMenuFlyout(nameof(mainPage.SystemThemeMnuFlt), mainPage.SystemThemeMnuFlt);
        new ResourceMenuFlyout(nameof(mainPage.LightThemeMnuFlt), mainPage.LightThemeMnuFlt);
        new ResourceMenuFlyout(nameof(mainPage.DarkMnuFlt), mainPage.DarkMnuFlt);
        IncomeTextLbl = new ResourceLabel(nameof(IncomeTextLbl));
        ExpensesTextLbl = new ResourceLabel(nameof(ExpensesTextLbl));
        SalesLbl = new ResourceLabel(nameof(SalesLbl));
        RestLbl = new ResourceLabel(nameof(RestLbl));
        RestMoneyLbl = new ResourceLabel(nameof(RestMoneyLbl));
        RestMoney = RestMoneyLbl.Text;
        DetailsBtn = new ResourceButton(nameof(DetailsBtn));
        EditBtn = new ResourceButton(nameof(EditBtn));
        DeleteBtn = new ResourceButton(nameof(DeleteBtn));
        NewSalesBtn = new ResourceButton(nameof(NewSalesBtn));
        FinancialOverviewTitle = LanguageResource.FinancialOverviewTitle ?? string.Empty;
        MonthsCollection.Clear();
        foreach (var month in Enums.GetLocalizedList(typeof(Enums.Month))) MonthsCollection.Add(month);
        SelectedMonth = DateTime.Now.Month;
        YearsCollection.Clear();
        var maxYear = DateTime.Now.Year + 15;
        for (var i = DateTime.Now.Year-15; i <= maxYear; ++i)
            YearsCollection.Add(i.ToString());
        SelectedYear = 15;
    }

    #endregion
}
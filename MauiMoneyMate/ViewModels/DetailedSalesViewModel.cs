using System.Collections.ObjectModel;
using FinancialOverview;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMoneyMate.Pages;
using MauiMoneyMate.Popups;
using MauiMoneyMate.Translations;
using MauiMoneyMate.Utils;
using MauiMoneyMate.Utils.ResourceItemTemplates;
using static CommonLibrary.Functions;


namespace MauiMoneyMate.ViewModels;

public partial class DetailedSalesViewModel : ObservableObject
{
    #region private Observable Properties

    [ObservableProperty] private string _detailedSalesTitle;

    [ObservableProperty] private ObservableCollection<SalesObject> _salesCollection;

    [ObservableProperty] private ResourceLabel _salesLbl;

    [ObservableProperty] private ResourceButton _detailsBtn;

    [ObservableProperty] private ResourceButton _editBtn;

    [ObservableProperty] private ResourceButton _deleteBtn;

    [ObservableProperty] private ResourceButton _newSalesBtn;

    [ObservableProperty] private ResourceLabel _salesCountLbl;

    [ObservableProperty] private string _salesCount;

    [ObservableProperty] private ResourceLabel _incomeCountLbl;

    [ObservableProperty] private string _incomeCount;

    [ObservableProperty] private ResourceLabel _expensesCountLbl;

    [ObservableProperty] private string _expensesCount;

    [ObservableProperty] private DateTime _selectedDeleteDate;

    [ObservableProperty] private string _deleteDatePattern;

    [ObservableProperty] private ResourceButton _deleteExpiredSalesBtn;

    #endregion

    #region private Properties

    private DetailedSalesPage DetailedSalesPage
    {
        get => _detailedSalesPage;
        set => _detailedSalesPage ??= value;
    }

    #endregion

    #region private Members

    private DetailedSalesPage _detailedSalesPage;

    #endregion

    #region public Constructors

    public DetailedSalesViewModel()
    {
        SalesCollection = new ObservableCollection<SalesObject>();
    }

    #endregion

    #region private Relay Commands

    [RelayCommand]
    private void DeleteExpiredSales()
    {
        var tmp = CommonProperties.FinancialOverview.Sales.Select(item => item.Copy()).ToArray();
        CommonProperties.FinancialOverview.Sales.Clear();
        foreach (var sale in tmp.Where(item => item.EndDate > SelectedDeleteDate))
            CommonProperties.FinancialOverview.Sales.Add(sale);
        if (AreEqual(tmp, CommonProperties.FinancialOverview.Sales)) return;
        DataIsSaved = false;
        UpdateSales();
        CommonProperties.FinancialOverview.AddCurrentStateToHistory();
    }

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
        MoveEntryDown(so, SalesCollection);
        CommonProperties.FinancialOverview.AddCurrentStateToHistory();
    }

    [RelayCommand]
    private void MoveEntryUp(SalesObject so)
    {
        MoveEntryUp(so, SalesCollection);
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
    private void AddNewSales()
    {
        OpenNewSalesPopup();
    }

    [RelayCommand]
    private void ShowDetails(SalesObject so)
    {
        DetailedSalesPage.ShowPopup(new DetailsPopup(so.Parent ?? so, EditEntry, DeleteEntry));
    }

    #endregion

    #region internal Event Handlers

    internal void RefreshMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        UpdateSales();
    }

    internal void SystemThemeMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        CommonProperties.CurrentAppTheme = (int)AppTheme.Unspecified;
    }

    internal void LightThemeMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        CommonProperties.CurrentAppTheme = (int)AppTheme.Light;
    }

    internal void DarkMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        CommonProperties.CurrentAppTheme = (int)AppTheme.Dark;
    }

    internal void OpenFilePageMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        Shell.Current.GoToAsync(nameof(FilePage));
    }

    internal void OverviewMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        Shell.Current.GoToAsync("../../route");
    }

    internal void BackMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        Shell.Current.GoToAsync("..");
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
        CommonProperties.FinancialOverview.Undo();
        DataIsSaved = false;
        UpdateSales();
    }

    internal void RedoMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        CommonProperties.FinancialOverview.Redo();
        DataIsSaved = false;
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

    internal void OnPageInitialized(DetailedSalesPage detailedSalesPage)
    {
        LoadResources(detailedSalesPage);
    }

    internal void OnAppearing()
    {
        DisplaySavingState();
        UpdateSales();
        SelectedDeleteDate = DateTime.Now.AddYears(-15);
        DeleteDatePattern = CommonProperties.DatePickerFormat;
    }

    internal void DetailedSalesPage_OnLoaded(object sender, EventArgs eventArgs)
    {
        DetailedSalesPage ??= (sender as Element).GetAncestor<DetailedSalesPage>();
    }

    #endregion

    #region private Methods

    private void OpenNewSalesPopup(SalesObject salesObject = null)
    {
        var tmp = CommonProperties.FinancialOverview.Sales.Select(item => item.Copy()).ToArray();
        var newSalePopup = new NewSalePopup(CommonProperties.FinancialOverview.Sales.IndexOf(salesObject))
        {
            Size = new Size(600, DetailedSalesPage.Height + 4)
        };
        newSalePopup.Closed += (sender, args) =>
        {
            if (AreEqual(tmp, CommonProperties.FinancialOverview.Sales)) return;
            CommonProperties.FinancialOverview.AddCurrentStateToHistory();
            UpdateSales();
            DataIsSaved = false;
        };
        DetailedSalesPage.ShowPopup(newSalePopup);
    }

    private void UpdateSales()
    {
        var incomeCounter = 0;
        SalesCollection.Clear();
        foreach (var item in CommonProperties.FinancialOverview.Sales)
        {
            SalesCollection.Add(item);
            if (item.Value > 0) ++incomeCounter;
        }
        SalesCount = CommonProperties.FinancialOverview.Sales.Count.ToString();
        IncomeCount = incomeCounter.ToString();
        ExpensesCount = (CommonProperties.FinancialOverview.Sales.Count - incomeCounter).ToString();
    }

    private void MoveEntryDown(SalesObject so, ICollection<SalesObject> collection)
    {
        if (collection.Count <= 1) return;
        var index = CommonProperties.FinancialOverview.Sales.IndexOf(so.Parent ?? so);
        if (index >= CommonProperties.FinancialOverview.Sales.Count - 1) return;
        CommonProperties.FinancialOverview.Sales.Move(index, index + 1);
        UpdateSales();
        DataIsSaved = false;
    }

    private void MoveEntryUp(SalesObject so, ICollection<SalesObject> collection)
    {
        if (collection.Count <= 1) return;
        var index = CommonProperties.FinancialOverview.Sales.IndexOf(so.Parent ?? so);
        if (index <= 0) return;
        CommonProperties.FinancialOverview.Sales.Move(index, index - 1);
        UpdateSales();
        DataIsSaved = false;
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
        DetailedSalesTitle = DetailedSalesTitle.TrimEnd('*');
        if (!DataIsSaved)
            DetailedSalesTitle += '*';
        CommonFunctions.DisplayFilePathInTitleBar();
    }

    private void LoadResources(DetailedSalesPage detailedSalesPage)
    {
        DetailedSalesTitle = LanguageResource.DetailedSalesTitle ?? string.Empty;
        new ResourceMenuBarItem(nameof(detailedSalesPage.FileMnu), detailedSalesPage.FileMnu);
        new ResourceMenuFlyout(nameof(detailedSalesPage.OpenFilePageMnuFlt), detailedSalesPage.OpenFilePageMnuFlt);
        new ResourceMenuFlyout(nameof(detailedSalesPage.NewMnuFlt), detailedSalesPage.NewMnuFlt);
        new ResourceMenuFlyout(nameof(detailedSalesPage.OpenMnuFlt), detailedSalesPage.OpenMnuFlt);
        new ResourceMenuFlyout(nameof(detailedSalesPage.SaveMnuFlt), detailedSalesPage.SaveMnuFlt);
        new ResourceMenuFlyout(nameof(detailedSalesPage.SaveAsMnuFlt), detailedSalesPage.SaveAsMnuFlt);
        new ResourceMenuFlyout(nameof(detailedSalesPage.BackMnuFlt), detailedSalesPage.BackMnuFlt);
        new ResourceMenuFlyout(nameof(detailedSalesPage.ExitMnuFlt), detailedSalesPage.ExitMnuFlt);
        new ResourceMenuBarItem(nameof(detailedSalesPage.SettingsMnu), detailedSalesPage.SettingsMnu);
        new ResourceMenuFlyout(nameof(detailedSalesPage.OpenSettingsMnuFlt), detailedSalesPage.OpenSettingsMnuFlt);
        new ResourceMenuBarItem(nameof(detailedSalesPage.EditMnu), detailedSalesPage.EditMnu);
        new ResourceMenuFlyout(nameof(detailedSalesPage.UndoMnuFlt), detailedSalesPage.UndoMnuFlt);
        new ResourceMenuFlyout(nameof(detailedSalesPage.RedoMnuFlt), detailedSalesPage.RedoMnuFlt);
        new ResourceMenuBarItem(nameof(detailedSalesPage.HelpMnu), detailedSalesPage.HelpMnu);
        new ResourceMenuFlyout(nameof(detailedSalesPage.GoToWebsiteMnuFlt), detailedSalesPage.GoToWebsiteMnuFlt);
        new ResourceMenuFlyout(nameof(detailedSalesPage.WriteTicketMnuFlt), detailedSalesPage.WriteTicketMnuFlt);
        new ResourceMenuBarItem(nameof(detailedSalesPage.ViewMnu), detailedSalesPage.ViewMnu);
        new ResourceMenuFlyout(nameof(detailedSalesPage.OverviewMnuFlt), detailedSalesPage.OverviewMnuFlt);
        new ResourceMenuFlyout(nameof(detailedSalesPage.RefreshMnuFlt), detailedSalesPage.RefreshMnuFlt);
        new ResourceMenuFlyout(nameof(detailedSalesPage.AppThemeMnuFlt), detailedSalesPage.AppThemeMnuFlt);
        new ResourceMenuFlyout(nameof(detailedSalesPage.SystemThemeMnuFlt), detailedSalesPage.SystemThemeMnuFlt);
        new ResourceMenuFlyout(nameof(detailedSalesPage.LightThemeMnuFlt), detailedSalesPage.LightThemeMnuFlt);
        new ResourceMenuFlyout(nameof(detailedSalesPage.DarkMnuFlt), detailedSalesPage.DarkMnuFlt);

        SalesLbl = new ResourceLabel(nameof(SalesLbl));
        DetailsBtn = new ResourceButton(nameof(DetailsBtn));
        EditBtn = new ResourceButton(nameof(EditBtn));
        DeleteBtn = new ResourceButton(nameof(DeleteBtn));
        NewSalesBtn = new ResourceButton(nameof(NewSalesBtn));
        SalesCountLbl = new ResourceLabel(nameof(SalesCountLbl));
        IncomeCountLbl = new ResourceLabel(nameof(IncomeCountLbl));
        ExpensesCountLbl = new ResourceLabel(nameof(ExpensesCountLbl));
        DeleteExpiredSalesBtn = new ResourceButton(nameof(DeleteExpiredSalesBtn));
    }

    #endregion
}
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
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
    private readonly MenuBarItems _menuBarItems = new();

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

    #region private Event Handlers

    private void RefreshMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        UpdateSales();
    }

    private void NewMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        CommonFunctions.NewDocumentAction();
        DataIsSaved = false;
        UpdateSales();
    }

    private void OpenMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        if (!CommonProperties.DataIsSaved)
        {
            DetailedSalesPage.ShowPopup(new UnsavedChangesPopup(LanguageResource.DoYouWantToContinueOpeningAnotherFile, _ =>
            {
                DataIsSaved = CommonFunctions.OpenFileAction() || DataIsSaved;
                UpdateSales();
            }));
            return;
        }
        DataIsSaved = CommonFunctions.OpenFileAction() || DataIsSaved;
        UpdateSales();
    }

    private void SaveMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        DataIsSaved = CommonFunctions.SaveFileAction();
    }

    private void SaveAsMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        DataIsSaved = CommonFunctions.SaveFileAsAction();
    }

    private void UndoMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        CommonProperties.FinancialOverview.Undo();
        DataIsSaved = false;
        UpdateSales();
    }

    private void RedoMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        CommonProperties.FinancialOverview.Redo();
        DataIsSaved = false;
        UpdateSales();
    }

    #endregion

    #region internal Event Handlers

    internal void OnPageInitialized(DetailedSalesPage detailedSalesPage)
    {
        InitMenuBar(detailedSalesPage);
        LoadResources();
    }

    internal void OnAppearing(DetailedSalesPage page)
    {
        DisplaySavingState();
        UpdateSales();
        UpdateRecentlyOpenedFiles(page);
        SelectedDeleteDate = DateTime.Now.AddYears(-15);
        DeleteDatePattern = CommonProperties.DatePickerFormat;
    }

    internal void DetailedSalesPage_OnLoaded(object sender, EventArgs eventArgs)
    {
        DetailedSalesPage ??= (sender as Element).GetAncestor<DetailedSalesPage>();
    }

    #endregion

    #region private Methods

    private ICommand OpenRecentMnuFlt_OnClicked(Page page, string path)
    {
        return new Command(a =>
        {
            if (!CommonProperties.DataIsSaved)
            {
                page.ShowPopup(new UnsavedChangesPopup(LanguageResource.DoYouWantToContinueOpeningAnotherFile, _ => OpenRecent(page, path)));
                return;
            }
            OpenRecent(page, path);
        });
    }

    private void OpenRecent(Page page, string path)
    {
        if (!CommonProperties.FinancialOverview.LoadData(path))
        {
            Toast.Make(LanguageResource.CouldNotOpenFile).Show();
            return;
        }

        CommonProperties.FinancialOverview.ClearHistory();
        DataIsSaved = true;
        UpdateSales();
        UpdateRecentlyOpenedFiles(page);
    }

    private void UpdateRecentlyOpenedFiles(Page page)
    {
        _menuBarItems.OpenRecent.Clear();
        _menuBarItems.OpenRecent.IsEnabled = CommonProperties.FinancialOverview.FileHistory.Count != 0;
        foreach (var item in CommonProperties.FinancialOverview.FileHistory)
            _menuBarItems.OpenRecent.Add(new MenuFlyoutItem
                { Text = Path.GetFileName(item), Command = OpenRecentMnuFlt_OnClicked(page, item) });
    }

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

    private void InitMenuBar(Page mainPage)
    {
        _menuBarItems.New.Clicked += NewMnuFlt_OnClicked;
        _menuBarItems.Save.Clicked += SaveMnuFlt_OnClicked;
        _menuBarItems.SaveAs.Clicked += SaveAsMnuFlt_OnClicked;
        _menuBarItems.OpenFile.Clicked += OpenMnuFlt_OnClicked;
        mainPage.MenuBarItems.Add(_menuBarItems.FileCompleteWithBack);
        _menuBarItems.Undo.Clicked += UndoMnuFlt_OnClicked;
        _menuBarItems.Redo.Clicked += RedoMnuFlt_OnClicked;
        mainPage.MenuBarItems.Add(_menuBarItems.Edit);
        _menuBarItems.Refresh.Clicked += RefreshMnuFlt_OnClicked;
        mainPage.MenuBarItems.Add(_menuBarItems.ViewFromDetailedSalesPage);
        mainPage.MenuBarItems.Add(_menuBarItems.Settings);
        mainPage.MenuBarItems.Add(_menuBarItems.Help);
    }

    private void LoadResources()
    {
        DetailedSalesTitle = LanguageResource.DetailedSalesTitle ?? string.Empty;
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
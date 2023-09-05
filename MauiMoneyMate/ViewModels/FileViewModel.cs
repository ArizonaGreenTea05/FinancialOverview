using System.Collections.ObjectModel;
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

public partial class FileViewModel : ObservableObject
{
    #region public Structures

    public struct FileHistoryElement
    {
        public FileHistoryElement(string fullPath)
        {
            Filename = Path.GetFileName(fullPath);
            FullPath = fullPath;
        }

        public string Filename { get; set; }
        public string FullPath { get; set; }
    }

    #endregion

    #region private Observable Properties

    [ObservableProperty] private string _filePageTitle;

    [ObservableProperty] private ObservableCollection<FileHistoryElement> _fileHistory;

    [ObservableProperty] private ResourceLabel _historyLbl;

    [ObservableProperty] private ResourceLabel _openFileLbl;

    [ObservableProperty] private ResourceLabel _saveFileLbl;

    [ObservableProperty] private ResourceLabel _saveFileAsLbl;

    [ObservableProperty] private ResourceLabel _addNewLbl;

    [ObservableProperty] private ResourceButton _fileHistoryDeleteBtn;

    [ObservableProperty] private ResourceButton _fileHistoryOpenBtn;

    #endregion

    #region private Members

    private readonly MenuBarItems _menuBarItems = new();

    #endregion

    #region public Constructors

    public FileViewModel()
    {
    }

    #endregion

    #region private Relay Commands

    [RelayCommand]
    private void AddNewDocument()
    {
        CommonFunctions.NewDocumentAction();
        DataIsSaved = false;
        Shell.Current.GoToAsync("../../route");
    }

    [RelayCommand]
    private void OpenFileDialog()
    {
        if (!CommonProperties.DataIsSaved)
        {
            Shell.Current.CurrentPage.ShowPopup(new UnsavedChangesPopup(LanguageResource.DoYouWantToContinueOpeningAnotherFile, _ =>
            {
                DataIsSaved = CommonFunctions.OpenFileAction() || DataIsSaved;
                Shell.Current.GoToAsync("../../route");
            }));
            return;
        }
        DataIsSaved = CommonFunctions.OpenFileAction() || DataIsSaved;
        Shell.Current.GoToAsync("../../route");
    }

    [RelayCommand]
    private void SaveFile()
    {
        DataIsSaved = CommonFunctions.SaveFileAction();
        Shell.Current.GoToAsync("../../route");
    }

    [RelayCommand]
    private void SaveFileDialog()
    {
        DataIsSaved = CommonFunctions.SaveFileAsAction();
        Shell.Current.GoToAsync("../../route");
    }

    [RelayCommand]
    private void OpenFileFromHistory(FileHistoryElement fhe)
    {
        if (!CommonProperties.DataIsSaved)
        {
            Shell.Current.CurrentPage.ShowPopup(new UnsavedChangesPopup(LanguageResource.DoYouWantToContinueOpeningAnotherFile, _ => OpenRecent(fhe.FullPath)));
            return;
        }

        OpenRecent(fhe.FullPath);
    }

    private void OpenRecent(string path)
    {
        if (!CommonProperties.FinancialOverview.LoadData(path))
        {
            Toast.Make(LanguageResource.CouldNotOpenFile).Show();
            return;
        }
        CommonProperties.FinancialOverview.ClearHistory();
        DataIsSaved = true;
        Shell.Current.GoToAsync("../../route");
    }

    [RelayCommand]
    private void DeleteFileFromHistory(FileHistoryElement fhe)
    {
        FileHistory.Remove(fhe);
        CommonProperties.FinancialOverview.FileHistory.Remove(fhe.FullPath);
        if (CommonProperties.FinancialOverview.FileHistory.Count <= 0) CommonProperties.FinancialOverview.FilePath = fhe.FullPath;
        FileHandler.WriteTextToFile(CommonProperties.FinancialOverview.FileHistory, CommonProperties.FileHistoryFilePath);
    }

    #endregion

    #region internal Event Handlers

    internal void OnInitialized(FilePage filePage)
    {
        InitMenuBar(filePage);
        LoadResources();
    }

    internal void OnAppearing()
    {
        DisplaySavingState();
        FileHistory = new ObservableCollection<FileHistoryElement>();
        foreach (var item in CommonProperties.FinancialOverview.FileHistory)
            FileHistory.Add(new FileHistoryElement(item));
    }

    #endregion

    #region private Methods

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
        FilePageTitle = FilePageTitle.TrimEnd('*');
        if (!DataIsSaved)
            FilePageTitle += '*';
    }

    private void InitMenuBar(Page page)
    {
        page.MenuBarItems.Add(_menuBarItems.FileSmall);
        page.MenuBarItems.Add(_menuBarItems.View);
        page.MenuBarItems.Add(_menuBarItems.Settings);
        page.MenuBarItems.Add(_menuBarItems.Help);
    }

    private void LoadResources()
    {
        FilePageTitle = LanguageResource.FilePageTitle ?? string.Empty;
        HistoryLbl = new ResourceLabel(nameof(HistoryLbl));
        OpenFileLbl = new ResourceLabel(nameof(OpenFileLbl));
        SaveFileLbl = new ResourceLabel(nameof(SaveFileLbl));
        SaveFileAsLbl = new ResourceLabel(nameof(SaveFileAsLbl));
        AddNewLbl = new ResourceLabel(nameof(AddNewLbl));
        FileHistoryDeleteBtn = new ResourceButton(nameof(FileHistoryDeleteBtn));
        FileHistoryOpenBtn = new ResourceButton(nameof(FileHistoryOpenBtn));
    }

    #endregion
}
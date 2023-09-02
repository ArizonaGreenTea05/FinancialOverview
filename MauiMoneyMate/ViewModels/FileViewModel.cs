using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMoneyMate.Pages;
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
        if (!CommonProperties.FinancialOverview.LoadData(fhe.FullPath))
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

    internal void BackMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        Shell.Current.GoToAsync("..");
    }

    internal void OnInitialized(FilePage filePage)
    {
        LoadResources(filePage);
    }

    internal void OnAppearing()
    {
        DisplaySavingState();
        FileHistory = new ObservableCollection<FileHistoryElement>();
        foreach (var item in CommonProperties.FinancialOverview.FileHistory)
            FileHistory.Add(new FileHistoryElement(item));
    }

    internal void ExitMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        CommonFunctions.ExitAction();
    }

    internal void OpenSettingsMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        Shell.Current.GoToAsync(nameof(SettingsPage));
    }

    internal void GoToWebsiteMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        Browser.Default.OpenAsync(CommonProperties.WebsiteUrl, BrowserLaunchMode.SystemPreferred);
    }

    internal void WriteTicketMnuFlt_OnClicked(object sender, EventArgs eventArgs)
    {
        Browser.Default.OpenAsync(CommonProperties.NewIssueUrl, BrowserLaunchMode.SystemPreferred);
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

    private void LoadResources(FilePage filePage)
    {
        filePage.FileMnu.LoadFromResource(nameof(filePage.FileMnu));
        filePage.BackMnuFlt.LoadFromResource(nameof(filePage.BackMnuFlt));
        filePage.ExitMnuFlt.LoadFromResource(nameof(filePage.ExitMnuFlt));
        filePage.SettingsMnu.LoadFromResource(nameof(filePage.SettingsMnu));
        filePage.OpenSettingsMnuFlt.LoadFromResource(nameof(filePage.OpenSettingsMnuFlt));
        filePage.HelpMnu.LoadFromResource(nameof(filePage.HelpMnu));
        filePage.GoToWebsiteMnuFlt.LoadFromResource(nameof(filePage.GoToWebsiteMnuFlt));
        filePage.WriteTicketMnuFlt.LoadFromResource(nameof(filePage.WriteTicketMnuFlt));
        filePage.ViewMnu.LoadFromResource(nameof(filePage.ViewMnu));
        filePage.AppThemeMnuFlt.LoadFromResource(nameof(filePage.AppThemeMnuFlt));
        filePage.SystemThemeMnuFlt.LoadFromResource(nameof(filePage.SystemThemeMnuFlt));
        filePage.LightThemeMnuFlt.LoadFromResource(nameof(filePage.LightThemeMnuFlt));
        filePage.DarkMnuFlt.LoadFromResource(nameof(filePage.DarkMnuFlt));
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
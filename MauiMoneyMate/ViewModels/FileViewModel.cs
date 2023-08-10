using System.Collections.ObjectModel;
using BusinessLogic;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMoneyMate.Resources.Languages;
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
        LoadResources();
    }

    #endregion

    #region private Relay Commands

    [RelayCommand]
    private void AddNewDocument()
    {
        CommonProperties.FinancialOverview.ClearSales();
        CommonProperties.FinancialOverview.FilePath = null;
        CommonProperties.FinancialOverview.ClearHistory();
        DataIsSaved = false;
        Shell.Current.GoToAsync("../../route");
    }

    [RelayCommand]
    private void OpenFileDialog()
    {
        var path = FileHandler.OpenFileDialog();
        if (null == path)
        {
            Toast.Make(LanguageResource.CouldNotOpenFile).Show();
            return;
        }
        CommonProperties.FinancialOverview.LoadData(path);
        CommonProperties.FinancialOverview.ClearHistory();
        DataIsSaved = true;
        Shell.Current.GoToAsync("../../route");
    }

    [RelayCommand]
    private void SaveFile()
    {
        DataIsSaved = CommonProperties.FinancialOverview.SaveData();
        if (!DataIsSaved) SaveFileDialog();
        Shell.Current.GoToAsync("../../route");
    }

    [RelayCommand]
    private void SaveFileDialog()
    {
        var path = FileHandler.SaveFileDialog(CommonProperties.FinancialOverview.FileDirectory,
            CommonProperties.FinancialOverview.Filename ?? FinancialOverview.DefaultFilename);
        if (null == path)
        {
            Toast.Make(LanguageResource.CouldNotSaveFile).Show();
            return;
        }
        CommonProperties.FinancialOverview.SaveData(path);
        Toast.Make(string.Format(LanguageResource.SavedFile, path)).Show();
        DataIsSaved = true;
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
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

    #region private Members

    private readonly FinancialOverview _financialOverview;

    #endregion

    #region public Constructors

    public FileViewModel(FinancialOverview financialOverview)
    {
        _financialOverview = financialOverview;
        LoadResources();
    }

    #endregion

    #region private Relay Commands

    [RelayCommand]
    private void AddNewDocument()
    {
        _financialOverview.ClearSales();
        _financialOverview.FilePath = null;
        _financialOverview.ClearHistory();
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
        _financialOverview.LoadData(path);
        _financialOverview.ClearHistory();
        DataIsSaved = true;
        Shell.Current.GoToAsync("../../route");
    }

    [RelayCommand]
    private void SaveFile()
    {
        DataIsSaved = _financialOverview.SaveData();
        if (!DataIsSaved) SaveFileDialog();
        Shell.Current.GoToAsync("../../route");
    }

    [RelayCommand]
    private void SaveFileDialog()
    {
        var path = FileHandler.SaveFileDialog(_financialOverview.FileDirectory,
            _financialOverview.Filename ?? FinancialOverview.DefaultFilename);
        if (null == path)
        {
            Toast.Make(LanguageResource.CouldNotSaveFile).Show();
            return;
        }
        _financialOverview.SaveData(path);
        Toast.Make(string.Format(LanguageResource.SavedFile, path)).Show();
        DataIsSaved = true;
        Shell.Current.GoToAsync("../../route");
    }

    [RelayCommand]
    private void OpenFileFromHistory(FileHistoryElement fhe)
    {
        if (!_financialOverview.LoadData(fhe.FullPath))
        {
            Toast.Make(LanguageResource.CouldNotOpenFile).Show();
            return;
        }
        _financialOverview.ClearHistory();
        DataIsSaved = true;
        Shell.Current.GoToAsync("../../route");
    }

    [RelayCommand]
    private void DeleteFileFromHistory(FileHistoryElement fhe)
    {
        FileHistory.Remove(fhe);
        _financialOverview.FileHistory.Remove(fhe.FullPath);
        if (_financialOverview.FileHistory.Count <= 0) _financialOverview.FilePath = fhe.FullPath;
        FileHandler.WriteTextToFile(_financialOverview.FileHistory, CommonProperties.AppDataFilePath);
    }

    #endregion

    #region public Methods

    public void OnAppearing()
    {
        DisplaySavingState();
        FileHistory = new ObservableCollection<FileHistoryElement>();
        foreach (var item in _financialOverview.FileHistory)
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
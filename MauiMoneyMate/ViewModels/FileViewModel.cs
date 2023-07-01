using BusinessLogic;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMoneyMate.Resources.Languages;
using MauiMoneyMate.Utils;

namespace MauiMoneyMate.ViewModels;

public partial class FileViewModel : ObservableObject
{
    #region private Observable Properties

    [ObservableProperty] private string _filePageTitle;

    [ObservableProperty] private string _openFileText;

    [ObservableProperty] private string _saveFileText;

    [ObservableProperty] private string _saveFileAsText;

    [ObservableProperty] private int _openFileFontSize;

    [ObservableProperty] private int _saveFileFontSize;

    [ObservableProperty] private int _saveFileAsFontSize;

    #endregion

    #region private Members

    private readonly FinancialOverview _financialOverview;
    private readonly CommonVariables _commonVariables;

    #endregion

    #region public Constructors

    public FileViewModel(FinancialOverview financialOverview, CommonVariables commonVariables)
    {
        _commonVariables = commonVariables;
        _financialOverview = financialOverview;
        LoadResources();
    }

    #endregion

    #region private Relay Commands

    [RelayCommand]
    private void OpenFileDialog()
    {
        var path = FileHandler.OpenFileDialog();
        if (null == path)
        {
            Toast.Make(TextResource.CouldNotOpenFile).Show();
            return;
        }
        _financialOverview.LoadData(path);
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
        var path = FileHandler.SaveFileDialog(_financialOverview.DefaultDirectory, _financialOverview.DefaultFilename);
        if (null == path)
        {
            Toast.Make(TextResource.CouldNotSaveFile).Show();
            return;
        }
        _financialOverview.SaveData(path);
        Toast.Make(string.Format(TextResource.SavedFile, path)).Show();
        DataIsSaved = true;
        Shell.Current.GoToAsync("../../route");
    }

    #endregion

    #region public Methods

    public void OnAppearing()
    {
        DisplaySavingState();
    }

    #endregion

    #region private Methods

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
        FilePageTitle = FilePageTitle.TrimEnd('*');
        if (!DataIsSaved)
            FilePageTitle += '*';
    }

    private void LoadResources()
    {
        FilePageTitle = TextResource.FilePageTitle ?? string.Empty;
        OpenFileText = TextResource.OpenFile ?? string.Empty;
        SaveFileText = TextResource.SaveFile ?? string.Empty;
        SaveFileAsText = TextResource.SaveFileAs ?? string.Empty;
        OpenFileFontSize = Convert.ToInt32(TextResource.OpenFileFontSize);
        OpenFileFontSize = 0 == OpenFileFontSize ? 17 : OpenFileFontSize;
        SaveFileFontSize = Convert.ToInt32(TextResource.SaveFileFontSize);
        SaveFileFontSize = 0 == SaveFileFontSize ? 17 : SaveFileFontSize;
        SaveFileAsFontSize = Convert.ToInt32(TextResource.SaveFileAsFontSize);
        SaveFileAsFontSize = 0 == SaveFileAsFontSize ? 17 : SaveFileAsFontSize;
    }

    #endregion
}
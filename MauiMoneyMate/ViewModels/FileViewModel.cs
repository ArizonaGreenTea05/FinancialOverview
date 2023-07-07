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
    #region private Observable Properties

    [ObservableProperty] private string _filePageTitle;

    [ObservableProperty] private ResourceLabel _openFileLbl;

    [ObservableProperty] private ResourceLabel _saveFileLbl;

    [ObservableProperty] private ResourceLabel _saveFileAsLbl;

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
        var path = FileHandler.SaveFileDialog(_financialOverview.DefaultDirectory, _financialOverview.DefaultFilename);
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
        FilePageTitle = LanguageResource.FilePageTitle ?? string.Empty;
        OpenFileLbl = new ResourceLabel(nameof(OpenFileLbl));
        SaveFileLbl = new ResourceLabel(nameof(SaveFileLbl));
        SaveFileAsLbl = new ResourceLabel(nameof(SaveFileAsLbl));
    }

    #endregion
}
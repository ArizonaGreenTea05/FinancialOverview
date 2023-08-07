using System.Collections.ObjectModel;
using BusinessLogic;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMoneyMate.Popups;
using MauiMoneyMate.Resources.Languages;
using MauiMoneyMate.Utils;
using MauiMoneyMate.Utils.ResourceItemTemplates;
using Newtonsoft.Json.Linq;

namespace MauiMoneyMate.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    #region private Observable Properties

    [ObservableProperty] private string _settingsPageTitle;

    [ObservableProperty] private ResourceLabel _startupLbl;

    [ObservableProperty] private bool _checkForUpdatesOnStart;

    [ObservableProperty] private ResourceLabel _checkForUpdatesOnStartLbl;

    [ObservableProperty] private bool _downloadUpdatesAutomatically;

    [ObservableProperty] private ResourceLabel _downloadUpdatesAutomaticallyLbl;

    [ObservableProperty] private ResourceButton _searchForUpdateBtn;

    [ObservableProperty] private ResourceLabel _designLbl;

    [ObservableProperty] private bool _showFilePathInTitleBar;

    [ObservableProperty] private ResourceLabel _showFilePathInTitleBarLbl;

    #endregion

    #region private Members

    private readonly FinancialOverview _financialOverview;

    #endregion

    #region public Constructors

    public SettingsViewModel()
    {
        LoadResources();
    }

    #endregion

    #region internal Methods

    internal void OnAppearing()
    {
        DisplaySavingState();
        LoadSettings();
    }
    internal void LoadSettings()
    {
        CheckForUpdatesOnStart = CommonProperties.CheckForUpdatesOnStart;
        DownloadUpdatesAutomatically = CommonProperties.DownloadUpdatesAutomatically;
        ShowFilePathInTitleBar = CommonProperties.ShowFilePathInTitleBar;
    }

    #endregion

    #region private Methods

    private void DisplaySavingState()
    {
        SettingsPageTitle = SettingsPageTitle.TrimEnd('*');
        if (!CommonProperties.DataIsSaved) SettingsPageTitle += '*';
    }
    
    private void LoadResources()
    {
        SettingsPageTitle = LanguageResource.SettingsPageTitle ?? string.Empty;
        StartupLbl = new ResourceLabel(nameof(StartupLbl));
        CheckForUpdatesOnStartLbl = new ResourceLabel(nameof(CheckForUpdatesOnStartLbl));
        DownloadUpdatesAutomaticallyLbl = new ResourceLabel(nameof(DownloadUpdatesAutomaticallyLbl));
        SearchForUpdateBtn = new ResourceButton(nameof(SearchForUpdateBtn));
        DesignLbl = new ResourceLabel(nameof(DesignLbl));
        ShowFilePathInTitleBarLbl = new ResourceLabel(nameof(ShowFilePathInTitleBarLbl));
    }

    #endregion
}
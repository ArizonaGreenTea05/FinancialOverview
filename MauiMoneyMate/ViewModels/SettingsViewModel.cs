using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiMoneyMate.Pages;
using MauiMoneyMate.Popups;
using MauiMoneyMate.Translations;
using MauiMoneyMate.Utils;
using MauiMoneyMate.Utils.ResourceItemTemplates;

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

    [ObservableProperty] private ResourceLabel _themeLbl;

    [ObservableProperty] private ObservableCollection<string> _themes;

    [ObservableProperty] private int _currentTheme;

    [ObservableProperty] private ResourceLabel _languageLbl;

    [ObservableProperty] private ObservableCollection<string> _languages;

    [ObservableProperty] private int _currentLanguage;

    [ObservableProperty] private ResourceLabel _systemLbl;

    [ObservableProperty] private ResourceButton _exportSettingsBtn;

    [ObservableProperty] private ResourceButton _importSettingsBtn;

    [ObservableProperty] private ResourceButton _deleteTemporaryFilesBtn;

    #endregion

    #region private Properties

    private bool PageLoaded { get; set; }

    private SettingsPage SettingsPage
    {
        get => _settingsPage;
        set => _settingsPage ??= value;
    }

    #endregion

    #region private Members

    private SettingsPage _settingsPage;
    private readonly MenuBarItems _menuBarItems = new();

    #endregion

    #region public Constructors

    public SettingsViewModel()
    {
        CurrentTheme = -1;
        CurrentLanguage = -1;
    }

    #endregion

    #region internal Event Handlers

    internal void OnPageInitialized(SettingsPage settingsPage)
    {
        InitMenuBar(settingsPage);
        LoadResources();
        LoadSettings();
    }

    internal void OnAppearing()
    {
        DisplaySavingState();
        PageLoaded = true;
    }

    internal void SettingsPage_OnDisappearing(object sender, EventArgs eventArgs)
    {
        PageLoaded = false;
    }

    internal void CheckForUpdatesOnStartChk_OnCheckedChanged(object sender, CheckedChangedEventArgs checkedChangedEventArgs)
    {
        if (!PageLoaded) return;
        CommonProperties.CheckForUpdatesOnStart = CheckForUpdatesOnStart;
        LoadSettings();
    }

    internal void DownloadUpdatesAutomaticallyChk_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!PageLoaded) return;
        CommonProperties.DownloadUpdatesAutomatically = DownloadUpdatesAutomatically;
        LoadSettings();
    }

    internal void SearchForUpdateBtn_OnClicked(object sender, EventArgs e)
    {
        CommonProperties.UpdateAvailable = CommonFunctions.CheckForUpdates();
        if (!CommonProperties.UpdateAvailable)
        {
            Toast.Make(LanguageResource.NoUpdatesAvailable).Show();
            return;
        }

        SettingsPage ??= (sender as Button).GetAncestor<SettingsPage>();
        SettingsPage?.ShowPopup(new UpdatePopup(CommonFunctions.DownloadLatestRelease,
            CommonFunctions.InstallDownloadedRelease));
    }

    internal void ShowFilePathInTitleBarChk_OnCheckedChanged(object sender, CheckedChangedEventArgs checkedChangedEventArgs)
    {
        if (!PageLoaded) return;
        CommonProperties.ShowFilePathInTitleBar = ShowFilePathInTitleBar;
        LoadSettings();
        CommonFunctions.DisplayFilePathInTitleBar();
    }

    internal void ThemePkr_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (!PageLoaded) return;
        CommonProperties.CurrentAppTheme = ((Picker)sender).SelectedIndex;
    }

    public void LanguagePkr_OnSelectedIndexChanged(object sender, EventArgs eventArgs)
    {
        if (!PageLoaded) return;
        CommonProperties.CurrentAppLanguage = ((Picker)sender).SelectedIndex;
        Toast.Make(LanguageResource.RestartTheProgramToApplyTheChanges).Show();
    }

    internal void ExportSettingsBtn_OnClicked(object sender, EventArgs e)
    {
        if (CommonFunctions.ExportSettings()) return;
    }

    internal void ImportSettingsBtn_OnClicked(object sender, EventArgs e)
    {
        if (CommonFunctions.ImportSettings()) Toast.Make(LanguageResource.ImportFinishedSuccessfully).Show();
        else Toast.Make(LanguageResource.ImportFailed).Show();
        LoadSettings();
        
    }

    internal void DeleteTemporaryFilesBtn_OnClicked(object sender, EventArgs e)
    {
        Toast.Make(
            CommonFunctions.ClearTemporaryFiles()
                ? LanguageResource.CleanupFinishedSuccessfully
                : LanguageResource.ErrorWhileDeletingTemporaryFiles
                ).Show();
    }

    #endregion

    #region private Methods

    internal void LoadSettings()
    {
        CheckForUpdatesOnStart = CommonProperties.CheckForUpdatesOnStart;
        DownloadUpdatesAutomatically = CommonProperties.DownloadUpdatesAutomatically;
        ShowFilePathInTitleBar = CommonProperties.ShowFilePathInTitleBar;
        CurrentTheme = CommonProperties.CurrentAppTheme;
        CurrentLanguage = CommonProperties.CurrentAppLanguage;
    }

    private void DisplaySavingState()
    {
        SettingsPageTitle = SettingsPageTitle.TrimEnd('*');
        if (!CommonProperties.DataIsSaved) SettingsPageTitle += '*';
    }

    private void InitMenuBar(Page page)
    {
        page.MenuBarItems.Add(_menuBarItems.FileSmall);
        page.MenuBarItems.Add(_menuBarItems.Help);
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
        ThemeLbl = new ResourceLabel(nameof(ThemeLbl));
        Themes = new ObservableCollection<string>
        {
            LanguageResource.UseSystemTheme,
            LanguageResource.Light,
            LanguageResource.Dark
        };
        LanguageLbl = new ResourceLabel(nameof(LanguageLbl));
        Languages = new ObservableCollection<string>
        {
            LanguageResource.UseSystemLanguage
        };
        foreach (var lang in CommonProperties.Languages.Select(item => item.DisplayName))
            Languages.Add(lang);
        SystemLbl = new ResourceLabel(nameof(SystemLbl));
        ExportSettingsBtn = new ResourceButton(nameof(ExportSettingsBtn));
        ImportSettingsBtn = new ResourceButton(nameof(ImportSettingsBtn));
        DeleteTemporaryFilesBtn = new ResourceButton(nameof(DeleteTemporaryFilesBtn));
    }

    #endregion
}

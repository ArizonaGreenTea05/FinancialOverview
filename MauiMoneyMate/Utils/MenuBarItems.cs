using CommunityToolkit.Maui.Views;
using MauiMoneyMate.Pages;
using MauiMoneyMate.Popups;

namespace MauiMoneyMate.Utils
{
    internal class MenuBarItems
    {
        #region FileMnu

        #region Items
        
        private MenuBarItem _fileCompleteWithBack;
        public MenuBarItem FileCompleteWithBack
        {
            get
            {
                if (_fileCompleteWithBack != null) return _fileCompleteWithBack;
                _fileCompleteWithBack = new MenuBarItem();
                _fileCompleteWithBack.LoadFromResource("FileMnu");
                _fileCompleteWithBack.Add(OpenFilePage);
                _fileCompleteWithBack.Add(new MenuFlyoutSeparator());
                _fileCompleteWithBack.Add(New);
                _fileCompleteWithBack.Add(Open);
                _fileCompleteWithBack.Add(new MenuFlyoutSeparator());
                _fileCompleteWithBack.Add(Save);
                _fileCompleteWithBack.Add(SaveAs);
                _fileCompleteWithBack.Add(new MenuFlyoutSeparator());
                _fileCompleteWithBack.Add(Back);
                _fileCompleteWithBack.Add(Exit);
                return _fileCompleteWithBack;
            }
        }

        private MenuBarItem _fileCompleteWithoutBack;
        public MenuBarItem FileCompleteWithoutBack
        {
            get
            {
                if (_fileCompleteWithoutBack != null) return _fileCompleteWithoutBack;
                _fileCompleteWithoutBack = new MenuBarItem();
                _fileCompleteWithoutBack.LoadFromResource("FileMnu");
                _fileCompleteWithoutBack.Add(OpenFilePage);
                _fileCompleteWithoutBack.Add(new MenuFlyoutSeparator());
                _fileCompleteWithoutBack.Add(New);
                _fileCompleteWithoutBack.Add(Open);
                _fileCompleteWithoutBack.Add(new MenuFlyoutSeparator());
                _fileCompleteWithoutBack.Add(Save);
                _fileCompleteWithoutBack.Add(SaveAs);
                _fileCompleteWithoutBack.Add(new MenuFlyoutSeparator());
                _fileCompleteWithoutBack.Add(Exit);
                return _fileCompleteWithoutBack;
            }
        }

        private MenuBarItem _fileSmall;
        public MenuBarItem FileSmall
        {
            get
            {
                if (_fileSmall != null) return _fileSmall;
                _fileSmall = new MenuBarItem();
                _fileSmall.LoadFromResource("FileMnu");
                _fileSmall.Add(Back);
                _fileSmall.Add(Exit);
                return _fileSmall;
            }
        }

        private MenuFlyoutItem _openFilePage;
        public MenuFlyoutItem OpenFilePage
        {
            get
            {
                if (_openFilePage != null) return _openFilePage;
                _openFilePage = new MenuFlyoutItem();
                _openFilePage.LoadFromResource("OpenFilePageMnuFlt");
                _openFilePage.Clicked += OpenFilePageMnuFlt_OnClicked;
                return _openFilePage;
            }
        }

        private MenuFlyoutItem _new;
        public MenuFlyoutItem New
        {
            get
            {
                if (_new != null) return _new;
                _new = new MenuFlyoutItem();
                _new.LoadFromResource("NewMnuFlt");
                return _new;
            }
        }

        private MenuFlyoutSubItem _open;
        public MenuFlyoutSubItem Open
        {
            get
            {
                if (_open != null) return _open;
                _open = new MenuFlyoutSubItem();
                _open.LoadFromResource("OpenMnuFlt");
                _open.Add(OpenFile);
                _open.Add(OpenRecent);
                return _open;
            }
        }

        private MenuFlyoutItem _openFile;
        public MenuFlyoutItem OpenFile
        {
            get
            {
                if (_openFile != null) return _openFile;
                _openFile = new MenuFlyoutItem();
                _openFile.LoadFromResource("OpenFileMnuFlt");
                return _openFile;
            }
        }

        private MenuFlyoutSubItem _openRecent;
        public MenuFlyoutSubItem OpenRecent
        {
            get
            {
                if (_openRecent != null) return _openRecent;
                _openRecent = new MenuFlyoutSubItem();
                _openRecent.LoadFromResource("OpenRecentMnuFlt");
                return _openRecent;
            }
        }

        private MenuFlyoutItem _save;
        public MenuFlyoutItem Save
        {
            get
            {
                if (_save != null) return _save;
                _save = new MenuFlyoutItem();
                _save.LoadFromResource("SaveMnuFlt");
                return _save;
            }
        }

        private MenuFlyoutItem _saveAs;
        public MenuFlyoutItem SaveAs
        {
            get
            {
                if (_saveAs != null) return _saveAs;
                _saveAs = new MenuFlyoutItem();
                _saveAs.LoadFromResource("SaveAsMnuFlt");
                return _saveAs;
            }
        }

        private MenuFlyoutItem _back;
        public MenuFlyoutItem Back
        {
            get
            {
                if (_back != null) return _back;
                _back = new MenuFlyoutItem();
                _back.LoadFromResource("BackMnuFlt");
                _back.Clicked += BackMnuFlt_OnClicked;
                return _back;
            }
        }

        private MenuFlyoutItem _exit;
        public MenuFlyoutItem Exit
        {
            get
            {
                if (_exit != null) return _exit;
                _exit = new MenuFlyoutItem();
                _exit.LoadFromResource("ExitMnuFlt");
                _exit.Clicked += ExitMnuFlt_OnClicked;
                return _exit;
            }
        }

        #endregion

        #region Event Handlers

        private void OpenFilePageMnuFlt_OnClicked(object sender, EventArgs eventArgs)
        {
            Shell.Current.GoToAsync(nameof(FilePage));
        }

        internal void BackMnuFlt_OnClicked(object sender, EventArgs eventArgs)
        {
            Shell.Current.GoToAsync("..");
        }
        
        internal void ExitMnuFlt_OnClicked(object sender, EventArgs eventArgs)
        {
            CommonFunctions.ExitAction();
        }

        #endregion

        #endregion

        #region EditMnu

        private MenuBarItem _edit;
        public MenuBarItem Edit
        {
            get
            {
                if (_edit != null) return _edit;
                _edit = new MenuBarItem();
                _edit.LoadFromResource("EditMnu");
                _edit.Add(Undo);
                _edit.Add(Redo);
                return _edit;
            }
        }

        private MenuFlyoutItem _undo;
        public MenuFlyoutItem Undo
        {
            get
            {
                if (_undo != null) return _undo;
                _undo = new MenuFlyoutItem();
                _undo.LoadFromResource("UndoMnuFlt");
                return _undo;
            }
        }

        private MenuFlyoutItem _redo;
        public MenuFlyoutItem Redo
        {
            get
            {
                if (_redo != null) return _redo;
                _redo = new MenuFlyoutItem();
                _redo.LoadFromResource("RedoMnuFlt");
                return _redo;
            }
        }

        #endregion

        #region ViewMnu

        #region Items

        private MenuBarItem _view;
        public MenuBarItem View
        {
            get
            {
                if (_view != null) return _view;
                _view = new MenuBarItem();
                _view.LoadFromResource("ViewMnu");
                _view.Add(AppTheme);
                return _view;
            }
        }

        private MenuBarItem _viewMain;
        public MenuBarItem ViewFromMainPage
        {
            get
            {
                if (_viewMain != null) return _viewMain;
                _viewMain = new MenuBarItem();
                _viewMain.LoadFromResource("ViewMnu");
                _viewMain.Add(DetailedSales);
                _viewMain.Add(Refresh);
                _viewMain.Add(AppTheme);
                return _viewMain;
            }
        }

        private MenuBarItem _viewDetailed;
        public MenuBarItem ViewFromDetailedSalesPage
        {
            get
            {
                if (_viewDetailed != null) return _viewDetailed;
                _viewDetailed = new MenuBarItem();
                _viewDetailed.LoadFromResource("ViewMnu");
                _viewDetailed.Add(Overview);
                _viewDetailed.Add(Refresh);
                _viewDetailed.Add(AppTheme);
                return _viewDetailed;
            }
        }

        private MenuFlyoutItem _detailedSales;
        public MenuFlyoutItem DetailedSales
        {
            get
            {
                if (_detailedSales != null) return _detailedSales;
                _detailedSales = new MenuFlyoutItem();
                _detailedSales.LoadFromResource("DetailedSalesMnuFlt");
                _detailedSales.Clicked += DetailedSalesMnuFlt_OnClicked;
                return _detailedSales;
            }
        }

        private MenuFlyoutItem _overview;
        public MenuFlyoutItem Overview
        {
            get
            {
                if (_overview != null) return _overview;
                _overview = new MenuFlyoutItem();
                _overview.LoadFromResource("OverviewMnuFlt");
                _overview.Clicked += OverviewMnuFlt_OnClicked;
                return _overview;
            }
        }

        private MenuFlyoutItem _refresh;
        public MenuFlyoutItem Refresh
        {
            get
            {
                if (_refresh != null) return _refresh;
                _refresh = new MenuFlyoutItem();
                _refresh.LoadFromResource("RefreshMnuFlt");
                return _refresh;
            }
        }

        private MenuFlyoutSubItem _appTheme;
        public MenuFlyoutSubItem AppTheme
        {
            get
            {
                if (_appTheme != null) return _appTheme;
                _appTheme = new MenuFlyoutSubItem();
                _appTheme.LoadFromResource("AppThemeMnuFlt");
                _appTheme.Add(SystemTheme);
                _appTheme.Add(Light);
                _appTheme.Add(Dark);
                return _appTheme;
            }
        }

        private MenuFlyoutItem _systemTheme;
        public MenuFlyoutItem SystemTheme
        {
            get
            {
                if (_systemTheme != null) return _systemTheme;
                _systemTheme = new MenuFlyoutItem();
                _systemTheme.LoadFromResource("SystemThemeMnuFlt");
                _systemTheme.Clicked += SystemThemeMnuFlt_OnClicked;
                return _systemTheme;
            }
        }

        private MenuFlyoutItem _light;
        public MenuFlyoutItem Light
        {
            get
            {
                if (_light != null) return _light;
                _light = new MenuFlyoutItem();
                _light.LoadFromResource("LightThemeMnuFlt");
                _light.Clicked += LightThemeMnuFlt_OnClicked;
                return _light;
            }
        }

        private MenuFlyoutItem _dark;
        public MenuFlyoutItem Dark
        {
            get
            {
                if (_dark != null) return _dark;
                _dark = new MenuFlyoutItem();
                _dark.LoadFromResource("DarkMnuFlt");
                _dark.Clicked += DarkMnuFlt_OnClicked;
                return _dark;
            }
        }

        #endregion

        #region Event Handlers

        private void DetailedSalesMnuFlt_OnClicked(object sender, EventArgs eventArgs)
        {
            Shell.Current.GoToAsync(nameof(DetailedSalesPage));
        }

        private void OverviewMnuFlt_OnClicked(object sender, EventArgs eventArgs)
        {
            Shell.Current.GoToAsync("../../route");
        }

        private void SystemThemeMnuFlt_OnClicked(object sender, EventArgs eventArgs)
        {
            CommonProperties.CurrentAppTheme = (int)Microsoft.Maui.ApplicationModel.AppTheme.Unspecified;
        }

        private void LightThemeMnuFlt_OnClicked(object sender, EventArgs eventArgs)
        {
            CommonProperties.CurrentAppTheme = (int)Microsoft.Maui.ApplicationModel.AppTheme.Light;
        }

        private void DarkMnuFlt_OnClicked(object sender, EventArgs eventArgs)
        {
            CommonProperties.CurrentAppTheme = (int)Microsoft.Maui.ApplicationModel.AppTheme.Dark;
        }

        #endregion

        #endregion

        #region SettingsMnu

        #region Items

        private MenuBarItem _settings;
        public MenuBarItem Settings
        {
            get
            {
                if (_settings != null) return _settings;
                _settings = new MenuBarItem();
                _settings.LoadFromResource("SettingsMnu");
                _settings.Add(OpenSettings);
                return _settings;
            }
        }

        private MenuFlyoutItem _openSettings;
        public MenuFlyoutItem OpenSettings
        {
            get
            {
                if (_openSettings != null) return _openSettings;
                _openSettings = new MenuFlyoutItem();
                _openSettings.LoadFromResource("OpenSettingsMnuFlt");
                _openSettings.Clicked += OpenSettingsMnuFlt_OnClicked;
                return _openSettings;
            }
        }

        #endregion

        #region Event Handlers

        internal void OpenSettingsMnuFlt_OnClicked(object sender, EventArgs eventArgs)
        {
            Shell.Current.GoToAsync(nameof(SettingsPage));
        }

        #endregion

        #endregion

        #region HelpMnu

        #region Items

        private MenuBarItem _help;
        public MenuBarItem Help
        {
            get
            {
                if (_help != null) return _help;
                _help = new MenuBarItem();
                _help.LoadFromResource("HelpMnu");
                _help.Add(GoToWebsite);
                _help.Add(WriteTicket);
                _help.Add(AppInfo);
                return _help;
            }
        }

        private MenuFlyoutItem _goToWebsite;
        public MenuFlyoutItem GoToWebsite
        {
            get
            {
                if (_goToWebsite != null) return _goToWebsite;
                _goToWebsite = new MenuFlyoutItem();
                _goToWebsite.LoadFromResource("GoToWebsiteMnuFlt");
                _goToWebsite.Clicked += GoToWebsiteMnuFlt_OnClicked;
                return _goToWebsite;
            }
        }

        private MenuFlyoutItem _writeTicket;
        public MenuFlyoutItem WriteTicket
        {
            get
            {
                if (_writeTicket != null) return _writeTicket;
                _writeTicket = new MenuFlyoutItem();
                _writeTicket.LoadFromResource("WriteTicketMnuFlt");
                _writeTicket.Clicked += WriteTicketMnuFlt_OnClicked;
                return _writeTicket;
            }
        }

        private MenuFlyoutItem _appInfo;
        public MenuFlyoutItem AppInfo
        {
            get
            {
                if (_appInfo != null) return _appInfo;
                _appInfo = new MenuFlyoutItem();
                _appInfo.LoadFromResource("AppInfoMnuFlt");
                _appInfo.Clicked += AppInfoMnuFlt_OnClicked;
                return _appInfo;
            }
        }

        #endregion

        #region Event Handlers

        internal void GoToWebsiteMnuFlt_OnClicked(object sender, EventArgs eventArgs)
        {
            Browser.Default.OpenAsync(CommonProperties.WebsiteUrl, BrowserLaunchMode.SystemPreferred);
        }

        internal void WriteTicketMnuFlt_OnClicked(object sender, EventArgs eventArgs)
        {
            Browser.Default.OpenAsync(CommonProperties.NewIssueUrl, BrowserLaunchMode.SystemPreferred);
        }

        internal void AppInfoMnuFlt_OnClicked(object sender, EventArgs e)
        {
            Shell.Current.CurrentPage.ShowPopup(new AppInfoPopup());
        }

        #endregion

        #endregion
    }
}

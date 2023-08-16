using System.Data;
using System.Globalization;
using BusinessLogic;
using CommonLibrary;
using MauiMoneyMate.Translations;
using Path = System.IO.Path;

namespace MauiMoneyMate.Utils;

internal static class CommonProperties
{
    internal static FinancialOverview FinancialOverview = new ();
    internal static ReleaseInfo CurrentVersion { get; } = GetCurrentVersion();
    internal static string RepositoryOwner => "ArizonaGreenTea05";
    internal static string RepositoryName => "FinancialOverview";

    internal static ReleaseInfo LatestRelease { get; set; } = null;
    internal static string GeneralInstallationFolderName => "MauiMoneyMate_{0}.0_x64";

    internal static string InstallationFolderNameOfLatestRelease => string.Format(GeneralInstallationFolderName,
        null == LatestRelease ? CurrentVersion.VersionId : LatestRelease.VersionId);

    internal static string AppDataDirectory =>
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MauiMoneyMate";

    internal static string CommonAppDataDirectory =>
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\MauiMoneyMate";

    internal static string FileHistoryFilePath { get; } = Path.Combine(AppDataDirectory, "MauiMoneyMate.FileHistory");
    internal static string WindowStateFilePath { get; } = Path.Combine(AppDataDirectory, "MauiMoneyMate.WindowState");
    internal static string AppSettingsFileEnding => "AppSettings";

    internal static string SettingsFilePath { get; } = Path.Combine(AppDataDirectory, $"MauiMoneyMate.{AppSettingsFileEnding}");

    internal static string UpdateDirectory { get; } = CommonAppDataDirectory + @"\Update";
    internal static string GeneralAssetName { get; } = $"{GeneralInstallationFolderName}.zip";
    internal static string InstallerNameOfLatestRelease => $"{InstallationFolderNameOfLatestRelease}_Installer.cmd";
    internal static string InstallerPathOfLatestRelease => Path.Combine(UpdateDirectory, InstallerNameOfLatestRelease);
    internal static string UpdaterNameOfLatestRelease => $@"{InstallationFolderNameOfLatestRelease}_Updater.cmd";
    internal static string UpdaterPathOfLatestRelease => Path.Combine(UpdateDirectory, InstallationFolderNameOfLatestRelease, UpdaterNameOfLatestRelease);
    internal static string AssetNameOfLatestRelease => $"{InstallationFolderNameOfLatestRelease}.zip";
    private static readonly DataTable StartupSettings = new(nameof(StartupSettings))
    {
        Columns = { nameof(CheckForUpdatesOnStart), nameof(DownloadUpdatesAutomatically) },
        Rows = { new object[] { "True", "False" } }
    };
    private static readonly DataTable DesignSettings = new(nameof(DesignSettings))
    {
        Columns = { nameof(ShowFilePathInTitleBar), nameof(CurrentAppTheme), nameof(CurrentAppLanguage) },
        Rows = { new object[] { "True", "0", "0" } }
    };

    private static DataSet _settings;
    internal static DataSet Settings
    {
        get
        {
            if (_settings != null) return _settings;
            _settings = new DataSet();
            _settings.DataSetName = "SettingsSet";
            _settings.Tables.Add(StartupSettings);
            _settings.Tables.Add(DesignSettings);
            if (!File.Exists(SettingsFilePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsFilePath)!);
                return _settings;
            }
            _settings.Clear();
            _settings.ReadXml(SettingsFilePath);
            return _settings;
        }
    }
    internal static bool DataIsSaved { get; set; } = true;

    internal static bool CheckForUpdatesOnStart
    {
        get
        {
            if (Settings.Tables[nameof(StartupSettings)]!.Rows.Count <= 0)
                Settings.Tables[nameof(StartupSettings)]!.Rows.Add("False", "False");
            var tmp = Settings.Tables[nameof(StartupSettings)]!.Rows[0][nameof(CheckForUpdatesOnStart)];
            return tmp is DBNull || Convert.ToBoolean(tmp);
        }
        set
        {
            if (value) DownloadUpdatesAutomatically = false;
            UpdateSettings(StartupSettings, nameof(CheckForUpdatesOnStart), Convert.ToString(value));
        }
    }

    internal static bool DownloadUpdatesAutomatically
    {
        get
        {
            if (Settings.Tables[nameof(StartupSettings)]!.Rows.Count <= 0)
                Settings.Tables[nameof(StartupSettings)]!.Rows.Add("False", "False");
            var tmp = Settings.Tables[nameof(StartupSettings)]!.Rows[0][nameof(DownloadUpdatesAutomatically)];
            return tmp is DBNull || Convert.ToBoolean(tmp);
        }
        set
        {
            if (value) CheckForUpdatesOnStart = false;
            UpdateSettings(StartupSettings, nameof(DownloadUpdatesAutomatically), Convert.ToString(value));
        }
    }

    internal static bool ShowFilePathInTitleBar
    {
        get
        {
            if (Settings.Tables[nameof(DesignSettings)]!.Rows.Count <= 0)
                Settings.Tables[nameof(DesignSettings)]!.Rows.Add("True", "0", "0");
            var tmp = Settings.Tables[nameof(DesignSettings)]!.Rows[0][nameof(ShowFilePathInTitleBar)];
            return tmp is DBNull || Convert.ToBoolean(tmp);
        }
        set => UpdateSettings(DesignSettings, nameof(ShowFilePathInTitleBar), Convert.ToString(value));
    }

    internal static int CurrentAppTheme
    {
        get
        {
            if (Settings.Tables[nameof(DesignSettings)]!.Rows.Count <= 0)
                Settings.Tables[nameof(DesignSettings)]!.Rows.Add("True", "0", "0");
            var tmp = Settings.Tables[nameof(DesignSettings)]!.Rows[0][nameof(CurrentAppTheme)];
            return tmp is DBNull ? 0 : Convert.ToInt32(tmp);
        }
        set
        {
            CommonFunctions.UpdateAppTheme(value);
            UpdateSettings(DesignSettings, nameof(CurrentAppTheme), Convert.ToString(value));
        }
    }
    public static int CurrentAppLanguage
    {
        get
        {
            if (Settings.Tables[nameof(DesignSettings)]!.Rows.Count <= 0)
                Settings.Tables[nameof(DesignSettings)]!.Rows.Add("True", "0", "0");
            var tmp = Settings.Tables[nameof(DesignSettings)]!.Rows[0][nameof(CurrentAppLanguage)];
            return tmp is DBNull ? 0 : Convert.ToInt32(tmp);
        }
        set
        {
            CommonFunctions.UpdateAppLanguage(value);
            UpdateSettings(DesignSettings, nameof(CurrentAppLanguage), Convert.ToString(value));
        }
    }

    internal static List<string> DirectoriesWithTemporaryFiles { get; } = new()
    {
        UpdateDirectory,
        WindowStateFilePath
    };

    internal static bool UpdateAvailable { get; set; } = false;

    internal static List<CultureInfo> Languages { get; } = LanguageResource.ResourceManager.EnumSatelliteLanguages();

    #region helper methods

    private static void UpdateSettings(DataTable settings, string columnName, string value)
    {
        settings.Rows[0][columnName] = value;
        settings.AcceptChanges();
        Settings.WriteXml(SettingsFilePath);
    }

    private static ReleaseInfo GetCurrentVersion()
    {
        var currentVersion = AppInfo.Current.VersionString.Split('.');
        var build = Convert.ToInt32(currentVersion[2]);
        return new ReleaseInfo(
            $"MMM-v{Convert.ToInt32(currentVersion[0])}.{Convert.ToInt32(currentVersion[1])}{(build > 0 ? $".{build}" : string.Empty)}-beta"
            );
    }

    #endregion
}

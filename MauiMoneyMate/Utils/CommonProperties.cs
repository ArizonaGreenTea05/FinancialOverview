using System.Data;
using BusinessLogic;
using CommonLibrary;
using Version = CommonLibrary.Version;

namespace MauiMoneyMate.Utils;

internal static class CommonProperties
{
    internal static FinancialOverview FinancialOverview = new ();
    internal static Version CurrentVersion { get; } = GetCurrentVersion();
    internal static string RepositoryOwner => "ArizonaGreenTea05";
    internal static string RepositoryName => "FinancialOverview";

    internal static ReleaseInfo LatestRelease { get; set; } = null;
    internal static string GeneralInstallationFolderName => "MauiMoneyMate_{0}.0_x64";

    internal static string InstallationFolderNameOfLatestRelease => string.Format(GeneralInstallationFolderName,
        null == LatestRelease ? CurrentVersion.Id : LatestRelease.VersionId);

    internal static string AppDataDirectory =>
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MauiMoneyMate";

    internal static string CommonAppDataDirectory =>
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\MauiMoneyMate";

    internal static string FileHistoryFilePath { get; } = Path.Combine(AppDataDirectory, "MauiMoneyMate.FileHistory");
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
        Columns = { nameof(ShowFilePathInTitleBar), nameof(CurrentAppTheme)},
        Rows = { new object[] { "True", "0" } }
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
            return Convert.ToBoolean(Settings.Tables[nameof(StartupSettings)]!.Rows[0][nameof(CheckForUpdatesOnStart)]);
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
            return Convert.ToBoolean(Settings.Tables[nameof(StartupSettings)]!.Rows[0][nameof(DownloadUpdatesAutomatically)]);
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
                Settings.Tables[nameof(DesignSettings)]!.Rows.Add("True", "0");
            return Convert.ToBoolean(Settings.Tables[nameof(DesignSettings)]!.Rows[0][nameof(ShowFilePathInTitleBar)]);
        }
        set => UpdateSettings(DesignSettings, nameof(ShowFilePathInTitleBar), Convert.ToString(value));
    }

    internal static int CurrentAppTheme
    {
        get
        {
            if (Settings.Tables[nameof(DesignSettings)]!.Rows.Count <= 0)
                Settings.Tables[nameof(DesignSettings)]!.Rows.Add("True", "0");
            return Convert.ToInt32(Settings.Tables[nameof(DesignSettings)]!.Rows[0][nameof(CurrentAppTheme)]);
        }
        set
        {
            CommonFunctions.UpdateAppTheme(value);
            UpdateSettings(DesignSettings, nameof(CurrentAppTheme), Convert.ToString(value));
        }
    }
    internal static List<string> DirectoriesWithTemporaryFiles { get; } = new()
    {
        UpdateDirectory
    };

    internal static bool UpdateAvailable { get; set; } = false;

    #region helper methods

    private static void UpdateSettings(DataTable settings, string columnName, string value)
    {
        settings.Rows[0][columnName] = value;
        settings.AcceptChanges();
        Settings.WriteXml(SettingsFilePath);
    }

    private static Version GetCurrentVersion()
    {
        var currentVersion = AppInfo.Current.VersionString.Split('.');
        return new Version()
        {
            Prefix = "MMM-",
            Major = Convert.ToInt32(currentVersion[0]),
            Minor = Convert.ToInt32(currentVersion[1]),
            Build = Convert.ToInt32(currentVersion[2]),
            Suffix = "-beta",
        };
    }

    #endregion
}

using System.Data;
using CommonLibrary;
using Version = CommonLibrary.Version;

namespace MauiMoneyMate.Utils;

internal static class CommonProperties
{
    internal static Version CurrentVersion => new()
    {
        Prefix = "MMM-",
        MainVersion = 0,
        SubVersion = 2,
        SubSubVersion = 1,
        Suffix = "-beta"
    };

    internal static string RepositoryOwner => "ArizonaGreenTea05";
    internal static string RepositoryName => "FinancialOverview";
    internal static ReleaseInfo LatestRelease { get; set; } = null;
    internal static string GeneralInstallationFolderName => "MauiMoneyMate_{0}.0_x64";

    internal static string InstallationFolderNameOfLatestRelease => string.Format(GeneralInstallationFolderName,
        null == LatestRelease ? CurrentVersion.Id : LatestRelease.VersionId);

    internal static string AppDataDirectory =>
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MauiMoneyMate";

    internal static string FileHistoryFilePath => Path.Combine(AppDataDirectory, "MauiMoneyMate.FileHistory");

    internal static string SettingsFilePath => Path.Combine(AppDataDirectory, "MauiMoneyMate.AppSettings");

    internal static string UpdateDirectory =>
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\MauiMoneyMate\\Update";

    internal static string GeneralInstallerName => $"{GeneralInstallationFolderName}_Installer.cmd";
    internal static string GeneralInstallerPath => Path.Combine(UpdateDirectory, GeneralInstallerName);
    internal static string GeneralAssetName => $"{GeneralInstallationFolderName}.zip";
    internal static string InstallerNameOfLatestRelease => $"{InstallationFolderNameOfLatestRelease}_Installer.cmd";
    internal static string InstallerPathOfLatestRelease => Path.Combine(UpdateDirectory, InstallerNameOfLatestRelease);
    internal static string AssetNameOfLatestRelease => $"{InstallationFolderNameOfLatestRelease}.zip";
    private static readonly DataTable StartupSettings = new(nameof(StartupSettings))
    {
        Columns = { nameof(CheckForUpdatesOnStart), nameof(DownloadUpdatesAutomatically) },
        Rows = { new object[] { "True", "False" } }
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
            if (!File.Exists(SettingsFilePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsFilePath));
                return _settings;
            }
            _settings.Clear();
            _settings.ReadXml(SettingsFilePath);
            return _settings;
        }
    }
    internal static bool DataIsSaved { get; set; } = true;
    internal static GitHubAccessor GitHubAccessor { get; set; } = new();

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

    internal static bool UpdateAvailable { get; set; } = false;

    #region helper methods

    private static void UpdateSettings(DataTable settings, string columnName, string value)
    {
        settings.Rows[0][columnName] = value;
        settings.AcceptChanges();
        Settings.WriteXml(SettingsFilePath);
    }

    #endregion
}

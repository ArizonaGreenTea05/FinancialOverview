using CommonLibrary;
using MauiMoneyMate.Popups;
using Version = CommonLibrary.Version;

namespace MauiMoneyMate.Utils;

internal static class CommonProperties
{
    internal static Version CurrentVersion => new()
    {
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

    internal static string AppDataFilePath => Path.Combine(AppDataDirectory, "MauiMoneyMate.AppData");

    internal static string UpdateDirectory =>
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\MauiMoneyMate\\Update";

    internal static string GeneralInstallerName => $"{GeneralInstallationFolderName}_Installer.cmd";
    internal static string GeneralInstallerPath => Path.Combine(UpdateDirectory, GeneralInstallerName);
    internal static string GeneralAssetName => $"{GeneralInstallationFolderName}.zip";
    internal static string InstallerNameOfLatestRelease => $"{InstallationFolderNameOfLatestRelease}_Installer.cmd";
    internal static string InstallerPathOfLatestRelease => Path.Combine(UpdateDirectory, InstallerNameOfLatestRelease);
    internal static string AssetNameOfLatestRelease => $"{InstallationFolderNameOfLatestRelease}.zip";
    internal static bool DataIsSaved { get; set; } = true;
    internal static GitHubAccessor GitHubAccessor { get; set; } = new();
    internal static bool CheckForUpdatesOnStart { get; set; } = true;
    internal static bool DownloadUpdatesAutomatically { get; set; } = false;
    internal static bool UpdateAvailable { get; set; } = false;
}
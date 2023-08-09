using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using BusinessLogic;
using CommonLibrary;
using CommunityToolkit.Maui.Alerts;
using Newtonsoft.Json.Linq;

namespace MauiMoneyMate.Utils;

internal static class CommonFunctions
{
    public static bool CheckForUpdates()
    {
        var tmp = GitHubAccessor.GetLatestReleaseInfo(CommonProperties.RepositoryOwner,
            CommonProperties.RepositoryName);
        CommonProperties.LatestRelease = tmp.Tag != tmp.Tag.Replace("MMM", "")
            ? tmp // new MMM release available
            : new ReleaseInfo(CommonProperties.CurrentVersion, CommonProperties.AssetNameOfLatestRelease,
                CommonProperties.RepositoryOwner, CommonProperties.RepositoryName); // latest release is not an MMM, so it won't be recognized as new release
        return CommonProperties.LatestRelease.VersionId != CommonProperties.CurrentVersion.Id;
    }

    public static bool DownloadLatestRelease()
    {
        if (GitHubAccessor.DownloadReleaseAsset(CommonProperties.LatestRelease, CommonProperties.GeneralAssetName,
                CommonProperties.UpdateDirectory)) return true;
        CleanUpAfterFailedInstallation();
        return false;
    }

    public static bool InstallDownloadedRelease()
    {
        try
        {
            var zipFile = Path.Combine(CommonProperties.UpdateDirectory, CommonProperties.AssetNameOfLatestRelease);
            ZipFile.ExtractToDirectory(zipFile, CommonProperties.UpdateDirectory, true);
            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                FileName = GetFileToUpdateTheApp(),
                WindowStyle = ProcessWindowStyle.Normal
            };
            Process.Start(startInfo)!.WaitForExit();
            return true;
        }
        catch
        {
            CleanUpAfterFailedInstallation();
            return false;
        }
    }

    private static string GetFileToUpdateTheApp()
    {
        var filename = CommonProperties.UpdaterPathOfLatestRelease;
        if (File.Exists(filename)) return filename;
        filename = filename.Replace(".cmd", ".bat");
        if (File.Exists(filename)) return filename;
        return CommonProperties.InstallerPathOfLatestRelease; // has no updater, so the installer will be used
    }

    private static void CleanUpAfterFailedInstallation()
    {
        var zipFilePath = Path.Combine(CommonProperties.UpdateDirectory, CommonProperties.AssetNameOfLatestRelease);
        if (File.Exists(zipFilePath)) File.Delete(zipFilePath);
        RemoveNonZipFiles(CommonProperties.UpdateDirectory);
    }

    public static void RemoveNonZipFiles(string directoryPath)
    {
        try
        {
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Directory not found.");
                return;
            }

            var directory = new DirectoryInfo(directoryPath);
            foreach (var file in directory.GetFiles())
                if (!file.Extension.Equals(".zip", StringComparison.OrdinalIgnoreCase))
                    file.Delete();
            foreach (var subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
        }
    }

    public static void DisplayFilePathInTitleBar()
    {
        if (Application.Current == null) return;
        if (Application.Current.MainPage == null) return;
        var window = Application.Current.MainPage.Window;
        window.Title = window.Title?.Split('-')[0].TrimEnd();
        if (!CommonProperties.ShowFilePathInTitleBar) return;
        window.Title += $" - {CommonProperties.FinancialOverview.FilePath ?? "none"}";
    }

    public static void UpdateAppTheme(int value)
    {
        if (Application.Current == null) return;
        Application.Current.UserAppTheme = CommonProperties.ThemeDict.TryGetValue(value, out var theme)
            ? theme
            : Application.Current.UserAppTheme;
    }

    public static bool ExportSettings()
    {
        if (!File.Exists(CommonProperties.SettingsFilePath)) return false;
        var result = FileHandler.SaveFileDialog(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            $"MauiMoneyMate_{DateTime.Now:yyyyMMddHHmmss}.{CommonProperties.AppSettingsFileEnding}");
        if (null == result) return false;
        CommonProperties.Settings.WriteXml(result);
        return true;
    }

    public static bool ImportSettings()
    {
        var result = FileHandler.OpenFileDialog(new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
                { { DevicePlatform.WinUI, new[] { $".{CommonProperties.AppSettingsFileEnding}" } } }));
        if (null == result) return false;
        CommonProperties.Settings.Clear();
        CommonProperties.Settings.ReadXml(result);
        return true;
    }
}
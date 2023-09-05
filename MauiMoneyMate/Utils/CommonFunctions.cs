using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using CommonLibrary;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using FinancialOverview;
using MauiMoneyMate.Popups;
using MauiMoneyMate.Translations;

namespace MauiMoneyMate.Utils;

internal static class CommonFunctions
{
#if WINDOWS
    [DllImport("user32.dll")]
    internal static extern bool SetForegroundWindow(IntPtr hWnd);
#endif

    internal static void NewDocumentAction()
    {
        CommonProperties.FinancialOverview.Sales.Clear();
        CommonProperties.FinancialOverview.FilePath = null;
        CommonProperties.FinancialOverview.ClearHistory();
    }

    internal static bool OpenFileAction()
    {
        var path = FileHandler.OpenFileDialog();
        if (null == path)
        {
            Toast.Make(LanguageResource.CouldNotOpenFile).Show();
            return false;
        }
        CommonProperties.FinancialOverview.LoadData(path);
        CommonProperties.FinancialOverview.ClearHistory();
        return true;
    }

    internal static bool SaveFileAction()
    {
        if (CommonProperties.FinancialOverview.SaveData() || SaveFileAsAction()) return true;
        Toast.Make(LanguageResource.CouldNotSaveFile).Show();
        return false;
    }

    internal static bool SaveFileAsAction()
    {
        var path = FileHandler.SaveFileDialog(CommonProperties.FinancialOverview.FileDirectory,
            CommonProperties.FinancialOverview.Filename ?? FinancialOverview.FinancialOverview.DefaultFilename);
        if (null == path)
        {
            Toast.Make(LanguageResource.CouldNotSaveFile).Show();
            return false;
        }
        CommonProperties.FinancialOverview.SaveData(path);
        Toast.Make(string.Format(LanguageResource.SavedFile, path)).Show();
        return true;
    }

    internal static bool CheckForUpdates()
    {
        var releases = GitHubAccessor.GetAllReleaseInfos(CommonProperties.RepositoryOwner, CommonProperties.RepositoryName);
        foreach (var releaseInfo in releases)
        {
            if (releaseInfo.Tag == releaseInfo.Tag.Replace("MMM", "")) continue;
            CommonProperties.LatestRelease = releaseInfo;
            break;
        }
        if (null == CommonProperties.LatestRelease)
        {
            CommonProperties.LatestRelease = CommonProperties.CurrentVersion;
            return false;
        }
        return CommonProperties.LatestRelease.VersionId != CommonProperties.CurrentVersion.VersionId;
    }

    internal static bool DownloadLatestRelease()
    {
        if (GitHubAccessor.DownloadReleaseAsset(CommonProperties.LatestRelease, CommonProperties.GeneralAssetName,
                CommonProperties.UpdateDirectory)) return true;
        CleanUpAfterFailedInstallation();
        return false;
    }

    internal static bool InstallDownloadedRelease()
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

    internal static void CleanUpAfterFailedInstallation()
    {
        var zipFilePath = Path.Combine(CommonProperties.UpdateDirectory, CommonProperties.AssetNameOfLatestRelease);
        if (File.Exists(zipFilePath)) File.Delete(zipFilePath);
        RemoveNonZipFiles(CommonProperties.UpdateDirectory);
    }

    internal static void RemoveNonZipFiles(string directoryPath)
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

    internal static void DisplayFilePathInTitleBar()
    {
        if (Application.Current == null) return;
        if (Application.Current.MainPage == null) return;
        var window = Application.Current.MainPage.Window;
        window.Title = window.Title?.Split('-')[0].TrimEnd();
        if (!CommonProperties.ShowFilePathInTitleBar) return;
        window.Title += $" - {CommonProperties.FinancialOverview.FilePath ?? "none"}";
    }

    internal static void UpdateAppTheme(int value)
    {
        if (Application.Current == null) return;
        if (value >= Enum.GetValues(typeof(AppTheme)).Length) return;
        Application.Current.UserAppTheme = (AppTheme)value;
    }

    internal static void UpdateAppLanguage(int value)
    {
        if (value > CommonProperties.Languages.Count || value < 0) return;
        if (value == 0) return;
        Thread.CurrentThread.CurrentUICulture = CommonProperties.Languages[value - 1];
    }

    internal static bool ExportSettings()
    {
        if (!File.Exists(CommonProperties.SettingsFilePath)) return false;
        var result = FileHandler.SaveFileDialog(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            $"MauiMoneyMate_{DateTime.Now:yyyyMMddHHmmss}.{CommonProperties.AppSettingsFileEnding}");
        if (null == result) return false;
        CommonProperties.Settings.WriteXml(result);
        return true;
    }

    internal static bool ImportSettings()
    {
        var result = FileHandler.OpenFileDialog(new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
                { { DevicePlatform.WinUI, new[] { $".{CommonProperties.AppSettingsFileEnding}" } } }));
        if (null == result) return false;
        try
        {
            CommonProperties.Settings.Clear();
            CommonProperties.Settings.ReadXml(result);
            return true;
        }
        catch
        {
            return false;
        }
    }

    internal static bool ClearTemporaryFiles()
    {
        try
        {
            foreach (var directory in CommonProperties.DirectoriesWithTemporaryFiles)
            {
                if (!Directory.Exists(directory)) return true;
                Directory.Delete(directory, true);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    internal static void ExitAction(Page page = null)
    {
        page ??= Shell.Current.CurrentPage;
        if (!CommonProperties.DataIsSaved)
        {
            page.ShowPopup(new UnsavedChangesPopup(LanguageResource.DoYouWantToContinueClosingTheApplication, _ =>
            {
                Application.Current.CloseWindow(page.Window);
                Environment.Exit(1);
            }));
            return;
        }

        if (Application.Current != null) Application.Current.CloseWindow(page.Window);
        Environment.Exit(1);
    }

    internal static void BubbleSortDescending(IList<SalesObject> collection,
        Action<SalesObject, ICollection<SalesObject>> moveEntryDown)
    {
        for (var i = collection.Count - 1; i > 0; --i)
        for (var j = 0; j < i; ++j)
            if (collection[j].Value > collection[j + 1].Value)
                moveEntryDown(collection[j], collection);
    }

    internal static void BubbleSortAscending(IList<SalesObject> collection,
        Action<SalesObject, ICollection<SalesObject>> moveEntryDown)
    {
        for (var i = collection.Count - 1; i > 0; --i)
        for (var j = 0; j < i; ++j)
            if (collection[j].Value < collection[j + 1].Value)
                moveEntryDown(collection[j], collection);
    }
}
using System.Diagnostics;
using System.IO.Compression;
using CommonLibrary;
using CommunityToolkit.Maui.Alerts;

namespace MauiMoneyMate.Utils;

internal static class CommonFunctions
{
    public static bool CheckForUpdates()
    {
        var tmp = GitHubAccessor.GetLatestReleaseInfo(CommonProperties.RepositoryOwner,
            CommonProperties.RepositoryName);
        CommonProperties.LatestRelease = tmp.Tag != tmp.Tag.Replace("MMM", "")
            ? tmp // new MMM release available
            : new ReleaseInfo(VersionInfos.CurrentVersion, CommonProperties.AssetNameOfLatestRelease,
                CommonProperties.RepositoryOwner, CommonProperties.RepositoryName); // latest release is not an MMM, so it won't be recognized as new release
        return CommonProperties.LatestRelease.VersionId != VersionInfos.CurrentVersion.Id;
    }

    public static bool DownloadLatestRelease()
    {
        return GitHubAccessor.DownloadReleaseAsset(CommonProperties.LatestRelease,
            CommonProperties.GeneralAssetName, CommonProperties.UpdateDirectory);
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
                FileName = CommonProperties.InstallerPathOfLatestRelease,
                WindowStyle = ProcessWindowStyle.Normal
            };
            Process.Start(startInfo)!.WaitForExit();
            return true;
        }
        catch (Exception ex)
        {
            Toast.Make(Convert.ToString(ex) ?? ex.Message).Show();
            return false;
        }
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
}
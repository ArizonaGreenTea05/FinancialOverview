﻿using System.Diagnostics;
using System.IO.Compression;
using CommonLibrary;
using CommunityToolkit.Maui.Alerts;

namespace MauiMoneyMate.Utils;

internal static class CommonFunctions
{
    public static bool CheckForUpdates()
    {
        CommonProperties.LatestRelease =
            GitHubAccessor.GetLatestReleaseInfo(CommonProperties.RepositoryOwner,
                CommonProperties.RepositoryName);
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
            if (File.Exists(zipFile)) File.Delete(zipFile);
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
}
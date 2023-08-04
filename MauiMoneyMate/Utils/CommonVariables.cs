using CommonLibrary;

namespace MauiMoneyMate.Utils
{
    public class CommonVariables
    {
        public bool DataIsSaved = true;
        public readonly GitHubAssetDownloader GitHubAssetDownloader = new();
        public ReleaseInfo LatestRelease = null;
    }
}

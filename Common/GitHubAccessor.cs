namespace CommonLibrary
{
    using System;
    using System.IO;
    using System.Net;
    using Newtonsoft.Json;

    public class GitHubAccessor
    {
        private const string GitHubApiBaseUrl = "https://api.github.com";
        public const string BaseDownloadUrl = "https://github.com/{0}/{1}/releases/download/{2}/{3}";

        public static bool DownloadReleaseAsset(ReleaseInfo release, string generalAssetName, string targetDirectory)
        {
            try
            {
                if (null == release)
                {
                    Console.WriteLine("No releases found for the specified repository.");
                    return false;
                }

                var assetName = string.Format(generalAssetName, release.VersionId);

                if (File.Exists(Path.Join(targetDirectory, assetName))) return true;

                var targetAsset = release.Assets.Find(asset =>
                    asset.Name.Equals(assetName, StringComparison.OrdinalIgnoreCase));
                if (null == targetAsset)
                {
                    Console.WriteLine($"The asset '{assetName}' was not found in the latest release.");
                    return false;
                }

                return DownloadAsset(targetAsset.DownloadUrl, targetDirectory);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public static ReleaseInfo GetLatestReleaseInfo(string owner, string repo)
        {
            var apiUrl = $"{GitHubApiBaseUrl}/repos/{owner}/{repo}/releases/latest";
            var request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.UserAgent = "Tinky-Winky, Dipsy, Laa-Laa & Po";
            var responseJson = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
            var releaseInfo = JsonConvert.DeserializeObject<ReleaseInfo>(responseJson);
            if (releaseInfo == null) return null;
            var releaseUrl = responseJson.Split("html_url\":\"")[1].Split("\"")[0];
            var splitUrl = releaseUrl.Split("/");
            releaseInfo.Tag = splitUrl[^1];
            foreach (var asset in releaseInfo.Assets)
                asset.DownloadUrl = string.Format(BaseDownloadUrl, owner, repo, releaseInfo.Tag, asset.Name);
            return releaseInfo;
        }

        private static bool DownloadAsset(string downloadUrl, string targetDirectory)
        {
            if (!Directory.Exists(targetDirectory)) Directory.CreateDirectory(targetDirectory);
            var fileName = Path.GetFileName(new Uri(downloadUrl).AbsolutePath);
            var targetPath = Path.Combine(targetDirectory, fileName);
            new WebClient().DownloadFile(downloadUrl, targetPath);
            return true;
        }
    }

    public class ReleaseInfo
    {
        private string _tag = string.Empty;
        public List<ReleaseAsset> Assets { get; set; } = new();
        public string Tag
        {
            get => _tag;
            set
            {
                _tag = value;
                VersionId = _tag.Split("-")[1].Replace("v", "");
            }
        }
        public string VersionId { get; private set; } = string.Empty;

        public ReleaseInfo()
        {

        }

        public ReleaseInfo(Version version, string assetName, string owner, string repo)
        {
            Assets = new List<ReleaseAsset>
            {
                new()
                {
                    Name = assetName,
                    DownloadUrl = string.Format(GitHubAccessor.BaseDownloadUrl, owner, repo, version.Tag, assetName)
                }
            };
            Tag = version.Tag;
        }
    }

    public class ReleaseAsset
    {
        public string Name { get; set; } = string.Empty;
        public string DownloadUrl { get; set; } = string.Empty;
    }
}
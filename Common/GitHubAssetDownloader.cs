namespace CommonLibrary
{
    using System;
    using System.IO;
    using System.Net;
    using Newtonsoft.Json;

    public class GitHubAssetDownloader
    {
        private const string GitHubApiBaseUrl = "https://api.github.com";
        private const string BaseDownloadUrl = "https://github.com/{0}/{1}/releases/download/{2}/{3}";

        public void DownloadReleaseAsset(ReleaseInfo release, string assetName, string targetDirectory)
        {
            try
            {
                if (release == null)
                {
                    Console.WriteLine("No releases found for the specified repository.");
                    return;
                }

                assetName = string.Format(assetName, release.VersionId);

                var targetAsset = release.Assets.Find(asset => asset.Name.Equals(assetName, StringComparison.OrdinalIgnoreCase));
                if (targetAsset == null)
                {
                    Console.WriteLine($"The asset '{assetName}' was not found in the latest release.");
                    return;
                }
                
                DownloadAsset(targetAsset.DownloadUrl, targetDirectory);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public ReleaseInfo GetLatestReleaseInfo(string owner, string repo)
        {
            var apiUrl = $"{GitHubApiBaseUrl}/repos/{owner}/{repo}/releases/latest";
            var request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.UserAgent = "Tinky-Winky, Dipsy, Laa-Laa & Po";

            using (var response = request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var responseJson = reader.ReadToEnd();
                        var releaseInfo = JsonConvert.DeserializeObject<ReleaseInfo>(responseJson);
                        if (releaseInfo == null) return null;
                        var releaseUrl = responseJson.Split("html_url\":\"")[1].Split("\"")[0];
                        var splittedUrl = releaseUrl.Split("/");
                        releaseInfo.Tag = splittedUrl[^1];
                        foreach (var asset in releaseInfo.Assets)
                            asset.DownloadUrl = string.Format(BaseDownloadUrl, owner, repo, releaseInfo.Tag, asset.Name);
                        return releaseInfo;
                    }
                }
            }
        }

        private void DownloadAsset(string downloadUrl, string targetDirectory)
        {
            using (var client = new WebClient())
            {
                var fileName = Path.GetFileName(new Uri(downloadUrl).AbsolutePath);
                var targetPath = Path.Combine(targetDirectory, fileName);
                client.DownloadFile(downloadUrl, targetPath);
                Console.WriteLine($"Asset '{fileName}' downloaded successfully to '{targetPath}'.");
            }
        }
    }

    public class ReleaseInfo
    {
        private string _tag = string.Empty;
        public List<ReleaseAsset> Assets { get; set; }
        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
                VersionId = _tag.Split("-")[1].Replace("v", "");
            }
        }
        public string VersionId { get; private set; }
    }

    public class ReleaseAsset
    {
        public string Name { get; set; }
        public string DownloadUrl { get; set; }
    }
}
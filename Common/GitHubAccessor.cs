namespace CommonLibrary
{
    using System;
    using System.IO;
    using System.Net;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class GitHubAccessor
    {
        private const string GitHubApiBaseUrl = "https://api.github.com";
        internal const string BaseDownloadUrl = "https://github.com/{0}/{1}/releases/download/{2}/{3}";
        private const string TokenAsHex = "6700680070005F00480065003400520063006100490051006C004B003500620076006D0041006D0068007700700069004A004B004C006C0056004A0047004C0048004B00340044006D00780056006200";

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

        public static List<ReleaseInfo> GetAllReleaseInfos(string owner, string repo)
        {
            var releaseInfos = new List<ReleaseInfo>();
            foreach (var url in Task.Run(() => GetReleaseURLsAsync(owner, repo)).Result)
            {
                var tmp = GetReleaseInfo(url, owner, repo);
                if (null != tmp) releaseInfos.Add(tmp);
            }
            return releaseInfos;
        }

        static async Task<string[]> GetReleaseURLsAsync(string repoOwner, string repoName)
        {
            try
            {
                var apiUrl = $"{GitHubApiBaseUrl}/repos/{repoOwner}/{repoName}/releases";
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "GitHub API Client");
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Functions.FromHexString(TokenAsHex)}");
                    var tmp = await client.GetStringAsync(apiUrl);
                    var jsonArray = JArray.Parse(tmp);
                    var releaseURLs = new string[jsonArray.Count];
                    for (int i = 0; i < jsonArray.Count; i++)
                        releaseURLs[i] = jsonArray[i]["url"].ToString();
                    return releaseURLs;
                }
            }
            catch
            {
                return new string[] { };
            }
        }

        public static ReleaseInfo? GetReleaseInfo(string apiUrl, string owner, string repo)
        {
            try
            {
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
            catch
            {
                return null;
            }
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

        public ReleaseInfo(string tag)
        {
            Tag = tag;
        }
    }

    public class ReleaseAsset
    {
        public string Name { get; set; } = string.Empty;
        public string DownloadUrl { get; set; } = string.Empty;
    }
}
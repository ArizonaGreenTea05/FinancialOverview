namespace CommonLibrary
{
    public class Version
    {
        public string Prefix = string.Empty;
        public int MainVersion;
        public int SubVersion;
        public int SubSubVersion;
        public string Suffix = string.Empty;
        public string Tag => $"{Prefix}v{MainVersion}.{SubVersion}{(SubSubVersion > 0 ? $".{SubSubVersion}" : string.Empty)}{Suffix}";
        public string Id => $"{MainVersion}.{SubVersion}.{SubSubVersion}";
    }
}

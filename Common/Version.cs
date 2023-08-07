namespace CommonLibrary
{
    public class Version
    {
        public string Prefix = string.Empty;
        public int Major;
        public int Minor;
        public int Build;
        public string Suffix = string.Empty;
        public string Tag => $"{Prefix}v{Major}.{Minor}{(Build > 0 ? $".{Build}" : string.Empty)}{Suffix}";
        public string Id => $"{Major}.{Minor}.{Build}";
    }
}

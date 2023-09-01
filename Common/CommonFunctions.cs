using System.Text;

namespace CommonLibrary
{
    public static class Functions
    {
        public static string ToHexString(string str)
        {
            var sb = new StringBuilder();
            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
                sb.Append(t.ToString("X2"));
            return sb.ToString();
        }

        public static string FromHexString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return Encoding.Unicode.GetString(bytes);
        }

        public static bool AreEqual<T>(IEnumerable<T> item1, IEnumerable<T> item2)
        {
            var item1AsArray = item1 as T[] ?? item1.ToArray();
            var item2AsArray = item2 as T[] ?? item2.ToArray();
            if (item1AsArray.Count() != item2AsArray.Count()) return false;
            return !item1AsArray.Where((t, i) => !t.Equals(item2AsArray[i])).Any();
        }
    }
}

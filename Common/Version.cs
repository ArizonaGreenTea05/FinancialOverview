using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class Version
    {
        public int MainVersion;
        public int SubVersion;
        public int SubSubVersion;
        public string Suffix = string.Empty;
        public string Tag => $"MMM-v{MainVersion}.{SubVersion}{(SubSubVersion > 0 ? $".{SubSubVersion}" : string.Empty)}{Suffix}";
        public string Id => $"{MainVersion}.{SubVersion}.{SubSubVersion}";
    }
}

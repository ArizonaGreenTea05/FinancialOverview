using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMoneyMate.Utils
{
    internal class VersionInfos
    {
        internal static class CurrentVersion
        {
            internal const int MainVersion = 0;
            internal const int SubVersion = 2;
            internal const int SubSubVersion = 1;
            internal const string Suffix = "-beta";
            internal static string Name => $"MMM-v{MainVersion}.{SubVersion}.{SubSubVersion}{Suffix}";
            internal static string Id => $"{MainVersion}.{SubVersion}.{SubSubVersion}";
        }
    }
}

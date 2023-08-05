using System;
using System.Collections.Generic;
using System.Linq;
using Version = CommonLibrary.Version;

namespace MauiMoneyMate.Utils
{
    internal class VersionInfos
    {
        internal static Version CurrentVersion = new()
        {
            MainVersion = 0,
            SubVersion = 2,
            SubSubVersion = 1,
            Suffix = "-beta"
        };
    }
}

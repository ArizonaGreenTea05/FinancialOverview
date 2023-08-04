using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMoneyMate.Utils
{
    static class CommonConstants
    {
        public static readonly string AppDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MauiMoneyMate";

        public static readonly string AppDataFilePath = Path.Combine(AppDataDirectory, "MauiMoneyMate.AppData");

        public static readonly string UpdateDirectory = Path.Combine(AppDataDirectory, @"\Update");

        public const string GeneralAssetName = "MauiMoneyMate_{0}.0_x64.zip";

        public const string RepositoryOwner = "ArizonaGreenTea05";
        public const string RepositoryName = "FinancialOverview";

        public static class CurrentVersion
        {
            public const int MainVersion = 0;
            public const int SubVersion = 2;
            public const int SubSubVersoin = 1;
            public const string Suffix = "-beta";
            public static string Name => $"MMM-v{MainVersion}.{SubVersion}.{SubSubVersoin}{Suffix}";
            public static string Id => $"{MainVersion}.{SubVersion}.{SubSubVersoin}";
        }
    }
}

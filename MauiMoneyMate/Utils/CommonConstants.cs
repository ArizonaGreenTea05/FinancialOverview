using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMoneyMate.Utils
{
    static class CommonConstants
    {
        public static readonly string AppDataFilePath = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData) + @"\MauiMoneyMate", "MauiMoneyMate.AppData");
    }
}

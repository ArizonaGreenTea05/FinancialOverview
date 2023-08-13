using System.Globalization;
using ABI.Microsoft.Windows.ApplicationModel.Resources;
using System.Resources;
using ResourceManager = System.Resources.ResourceManager;
using System.Runtime.InteropServices;

namespace MauiMoneyMate.Utils
{
    internal static class Extensions
    {
        public static T GetAncestor<T>(this Element element) where T : class
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            var tmp = element;
            while (tmp.GetType() != typeof(T))
            {
                tmp = tmp.Parent;
                if (tmp == null) return null;
            }
            return tmp as T;
        }

        internal static List<CultureInfo> EnumSatelliteLanguages(this ResourceManager resourceManager)
        {
            var ret = new List<CultureInfo>();
            foreach (var directory in Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory))
            {
                CultureInfo culture;
                try
                {
                    culture = CultureInfo.GetCultureInfo(Path.GetFileNameWithoutExtension(directory));
                }
                catch
                {
                    continue;
                }

                if (!ret.Contains(culture) && null != resourceManager.GetResourceSet(culture, true, false))
                    ret.Add(culture);
            }

            return ret;
        }
    }
}

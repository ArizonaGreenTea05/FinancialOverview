using System.Globalization;
using MauiMoneyMate.Translations;
using ResourceManager = System.Resources.ResourceManager;

namespace MauiMoneyMate.Utils
{
    internal static class Extensions
    {
        public static void LoadFromResource(this Label label, string name)
        {
            var fontSize = Convert.ToInt32(LanguageResource.ResourceManager.GetString($"{name}.FontSize"));
            fontSize = fontSize == 0 ? 24 : fontSize;
            label.SetBinding(Label.TextProperty, new Binding("Text"));
            label.SetBinding(Label.FontSizeProperty, new Binding("FontSize"));
            label.SetBinding(Label.TextDecorationsProperty, new Binding("TextDecorations"));
            label.BindingContext = new
            {
                Text = LanguageResource.ResourceManager.GetString($"{name}.Text") ?? "",
                FontSize = fontSize,
                TextDecorations = LanguageResource.ResourceManager.GetString($"{name}.TextDecorations") ?? "None"
            };
        }

        public static void LoadFromResource(this Entry entry, string name)
        {
            var fontSize = Convert.ToInt32(LanguageResource.ResourceManager.GetString($"{name}.FontSize"));
            fontSize = fontSize == 0 ? 24 : fontSize;
            entry.SetBinding(Entry.TextProperty, new Binding("Text"));
            entry.SetBinding(Entry.FontSizeProperty, new Binding("FontSize"));
            entry.SetBinding(Entry.PlaceholderProperty, new Binding("Placeholder"));
            entry.BindingContext = new
            {
                Text = LanguageResource.ResourceManager.GetString($"{name}.Text") ?? "",
                FontSize = fontSize,
                Placeholder = LanguageResource.ResourceManager.GetString($"{name}.Placeholder") ?? "None"
            };
        }

        public static void LoadFromResource(this Button button, string name)
        {
            var fontSize = Convert.ToInt32(LanguageResource.ResourceManager.GetString($"{name}.FontSize"));
            fontSize = fontSize == 0 ? 24 : fontSize;
            button.SetBinding(Button.TextProperty, new Binding("Text"));
            button.SetBinding(Button.FontSizeProperty, new Binding("FontSize"));
            button.BindingContext = new
            {
                Text = LanguageResource.ResourceManager.GetString($"{name}.Text") ?? "",
                FontSize = fontSize
            };
        }

        public static void LoadFromResource(this MenuBarItem menuBarItem, string name)
        {
            menuBarItem.SetBinding(MenuBarItem.TextProperty, new Binding("Text"));
            menuBarItem.BindingContext = new
            {
                Text = LanguageResource.ResourceManager.GetString($"{name}.Text") ?? ""
            };
        }

        public static void LoadFromResource(this MenuFlyoutItem menuFlyoutItem, string name)
        {
            menuFlyoutItem.SetBinding(MenuItem.TextProperty, new Binding("Text"));
            menuFlyoutItem.BindingContext = new
            {
                Text = LanguageResource.ResourceManager.GetString($"{name}.Text") ?? ""
            };
        }

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

using System.Xml.Linq;
using static MauiMoneyMate.Resources.Languages.TextResource;

namespace MauiMoneyMate.Utils.LocalizableItemTemplates
{
    public class LocalizableItem
    {

    }

    public class LocalizableLabel : LocalizableItem
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public int FontSize { get; set; }

        public LocalizableLabel(string name)
        {
            Name = name;
            LoadResource();
        }

        public void LoadResource()
        {
            Text = ResourceManager.GetString($"{Name}.{nameof(Text)}");
            FontSize = Convert.ToInt32(ResourceManager.GetString($"{Name}.{nameof(FontSize)}"));
        }
    }

    public class LocalizableButton : LocalizableItem
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public int FontSize { get; set; }

        public LocalizableButton(string name)
        {
            Name = name;
            LoadResource();
        }

        public void LoadResource()
        {
            Text = ResourceManager.GetString($"{Name}.{nameof(Text)}");
            FontSize = Convert.ToInt32(ResourceManager.GetString($"{Name}.{nameof(FontSize)}"));
        }
    }

    public class LocalizableEntry : LocalizableItem
    {
        public string Name { get; set; }
        public string Placeholder { get; set; }
        public string Text { get; set; }
        public int FontSize { get; set; }

        public LocalizableEntry(string name)
        {
            Name = name;
            LoadResource();
        }

        public void LoadResource()
        {
            Text = ResourceManager.GetString($"{Name}.{nameof(Text)}");
            Placeholder = ResourceManager.GetString($"{Name}.{nameof(Placeholder)}");
            FontSize = Convert.ToInt32(ResourceManager.GetString($"{Name}.{nameof(FontSize)}"));
        }
    }
}

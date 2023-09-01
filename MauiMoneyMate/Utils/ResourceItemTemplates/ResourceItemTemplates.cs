using MauiMoneyMate.Translations;
using static System.Net.Mime.MediaTypeNames;

namespace MauiMoneyMate.Utils.ResourceItemTemplates;

public abstract class ResourceItem
{
    public string Name { get; set; }

    public abstract void LoadResource();
}

public sealed class ResourceLabel : ResourceItem
{
    public string Text { get; set; }
    public int FontSize { get; set; }
    public string TextDecorations { get; set; }

    public ResourceLabel(string name)
    {
        Name = name;
        LoadResource();
    }

    public ResourceLabel(string name, Label label)
    {
        Name = name;
        LoadResource();
        label.SetBinding(Label.TextProperty, new Binding("Text"));
        label.SetBinding(Label.FontSizeProperty, new Binding("FontSize"));
        label.SetBinding(Label.TextDecorationsProperty, new Binding("TextDecorations"));
        label.BindingContext = new
        {
            Text = Text,
            FontSize = FontSize,
            TextDecorations = TextDecorations
        };
    }

    public override void LoadResource()
    {
        Text = LanguageResource.ResourceManager.GetString($"{Name}.{nameof(Text)}") ?? "";
        FontSize = Convert.ToInt32(LanguageResource.ResourceManager.GetString($"{Name}.{nameof(FontSize)}"));
        FontSize = FontSize == 0 ? 24 : FontSize;
        TextDecorations = LanguageResource.ResourceManager.GetString($"{Name}.{nameof(TextDecorations)}") ?? "None";
    }
}

public sealed class ResourceButton : ResourceItem
{
    public string Text { get; set; }
    public int FontSize { get; set; }

    public ResourceButton(string name)
    {
        Name = name;
        LoadResource();
    }

    public ResourceButton(string name, Button button)
    {
        Name = name;
        LoadResource();
        button.SetBinding(Button.TextProperty, new Binding("Text"));
        button.SetBinding(Button.FontSizeProperty, new Binding("FontSize"));
        button.BindingContext = new
        {
            Text = Text,
            FontSize = FontSize
        };
    }

    public override void LoadResource()
    {
        Text = LanguageResource.ResourceManager.GetString($"{Name}.{nameof(Text)}") ?? "";
        FontSize = Convert.ToInt32(LanguageResource.ResourceManager.GetString($"{Name}.{nameof(FontSize)}"));
        FontSize = FontSize == 0 ? 24 : FontSize;
    }
}

public sealed class ResourceEntry : ResourceItem
{
    public string Text { get; set; }
    public string Placeholder { get; set; }
    public int FontSize { get; set; }

    public ResourceEntry(string name)
    {
        Name = name;
        LoadResource();
    }

    public ResourceEntry(string name, Entry entry)
    {
        Name = name;
        LoadResource();
        entry.SetBinding(Entry.TextProperty, new Binding("Text"));
        entry.SetBinding(Entry.FontSizeProperty, new Binding("FontSize"));
        entry.SetBinding(Entry.PlaceholderProperty, new Binding("Placeholder"));
        entry.BindingContext = new
        {
            Text = Text,
            FontSize = FontSize,
            Placeholder = Placeholder
        };
    }

    public override void LoadResource()
    {
        Placeholder = LanguageResource.ResourceManager.GetString($"{Name}.{nameof(Placeholder)}");
        FontSize = Convert.ToInt32(LanguageResource.ResourceManager.GetString($"{Name}.{nameof(FontSize)}"));
        FontSize = FontSize == 0 ? 24 : FontSize;
    }
}

public sealed class ResourceMenuBarItem : ResourceItem
{
    public string Text { get; set; }

    public ResourceMenuBarItem(string name)
    {
        Name = name;
        LoadResource();
    }

    public ResourceMenuBarItem(string name, MenuBarItem menuFlyout)
    {
        Name = name;
        LoadResource();
        menuFlyout.SetBinding(MenuBarItem.TextProperty, new Binding("Text"));
        menuFlyout.BindingContext = new
        {
            Text = Text
        };
    }

    public override void LoadResource()
    {
        Text = LanguageResource.ResourceManager.GetString($"{Name}.{nameof(Text)}") ?? string.Empty;
    }
}

public sealed class ResourceMenuFlyout : ResourceItem
{
    public string Text { get; set; }

    public ResourceMenuFlyout(string name)
    {
        Name = name;
        LoadResource();
    }

    public ResourceMenuFlyout(string name, MenuFlyoutItem menuFlyout)
    {
        Name = name;
        LoadResource();
        menuFlyout.SetBinding(MenuItem.TextProperty, new Binding("Text"));
        menuFlyout.BindingContext = new
        {
            Text = Text
        };
    }

    public override void LoadResource()
    {
        Text = LanguageResource.ResourceManager.GetString($"{Name}.{nameof(Text)}") ?? string.Empty;
    }
}
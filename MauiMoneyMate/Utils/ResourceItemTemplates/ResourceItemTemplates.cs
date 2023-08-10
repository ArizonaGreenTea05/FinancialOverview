using MauiMoneyMate.Translations;

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
    public string Placeholder { get; set; }
    public int FontSize { get; set; }

    public ResourceEntry(string name)
    {
        Name = name;
        LoadResource();
    }

    public override void LoadResource()
    {
        Placeholder = LanguageResource.ResourceManager.GetString($"{Name}.{nameof(Placeholder)}");
        FontSize = Convert.ToInt32(LanguageResource.ResourceManager.GetString($"{Name}.{nameof(FontSize)}"));
        FontSize = FontSize == 0 ? 24 : FontSize;
    }
}
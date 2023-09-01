using CommunityToolkit.Maui;
using MauiMoneyMate.Pages;
using MauiMoneyMate.ViewModels;

namespace MauiMoneyMate
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Arial-Rounded.ttf", "ArialRounded");
                    fonts.AddFont("Arial-RoundedBold.ttf", "ArialRoundedBold");
                }).UseMauiCommunityToolkit();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton(new MainViewModel());

            builder.Services.AddSingleton<FilePage>();
            builder.Services.AddSingleton(new FileViewModel());

            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddSingleton(new SettingsViewModel());

            builder.Services.AddSingleton<DetailedSalesPage>();
            builder.Services.AddSingleton(new DetailedSalesViewModel());

            return builder.Build();
        }
    }
}
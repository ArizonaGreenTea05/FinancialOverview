using CommonLibrary;
using CommunityToolkit.Maui;
using MauiMoneyMate.Pages;
using MauiMoneyMate.Popups;
using MauiMoneyMate.Utils;
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

            var financialOverview = new BusinessLogic.FinancialOverview();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton(new MainViewModel(financialOverview));

            builder.Services.AddTransient<FilePage>();
            builder.Services.AddSingleton(new FileViewModel(financialOverview));

            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddSingleton(new SettingsViewModel(financialOverview));

            return builder.Build();
        }
    }
}
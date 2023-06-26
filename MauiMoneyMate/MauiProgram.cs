using MauiMoneyMate.ViewModels;
using Microsoft.Extensions.Logging;

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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton(new MainViewModel(new BusinessLogic.FinancialOverview()));

            builder.Services.AddTransient<FilePage>();
            builder.Services.AddTransient<FileViewModel>();

            return builder.Build();
        }
    }
}
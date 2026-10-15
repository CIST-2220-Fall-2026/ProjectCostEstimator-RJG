using Microsoft.Extensions.Logging;
using ProjectCostEstimator.Services;
using ProjectCostEstimator.ViewModels;
using ProjectCostEstimator.Views;

namespace ProjectCostEstimator
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

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<IBomService, InMemoryBomService>();
            builder.Services.AddTransient<BillOfMaterialsViewModel>();
            builder.Services.AddTransient<BillOfMaterialsPage>();

            return builder.Build();
        }
    }
}

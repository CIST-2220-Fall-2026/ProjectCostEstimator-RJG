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

            builder.Services.AddSingleton<IBomService>(_ =>
                new JsonBomService(Path.Combine(FileSystem.AppDataDirectory, "bom.json")));
            builder.Services.AddSingleton<IBomInteractionService, MauiBomInteractionService>();
            builder.Services.AddTransient<BillOfMaterialsViewModel>();
            builder.Services.AddTransient<BillOfMaterialsPage>();
            builder.Services.AddTransient<ItemEditorViewModel>();
            builder.Services.AddTransient<ItemEditorPage>();

            return builder.Build();
        }
    }
}

using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using MauiAppArvoredo.Services;

namespace MauiAppArvoredo
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
                    fonts.AddFont("Gagalin-Regular.otf", "GagalinRegular");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // 🔹 REGISTRO DO SERVIÇO DA API ARVOREDO
            builder.Services.AddSingleton<ApiClient>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Remove apenas a borda/linha de foco
            EntryHandler.Mapper.AppendToMapping("NoFocusBorder", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.Background = null; // remove underline/focus color
#elif IOS || MACCATALYST
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None; // tira a borda azul
#elif WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            });

            return builder.Build();
        }
    }
}
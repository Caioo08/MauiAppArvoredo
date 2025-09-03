
using Microsoft.Extensions.Logging;

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

            // Configuração específica do Android
            #if ANDROID
                Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping("FullScreen", (handler, view) =>
                {
                    var activity = handler.PlatformView.Context as Android.App.Activity;
                    activity?.Window?.AddFlags(Android.Views.WindowManagerFlags.Fullscreen);
                    activity?.Window?.ClearFlags(Android.Views.WindowManagerFlags.ForceNotFullscreen);
                });
            #endif
            return builder.Build();
        }
    }
}
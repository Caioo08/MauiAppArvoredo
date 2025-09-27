using Android.App;
using Android.Content.PM;
using Android.OS;

namespace MauiAppArvoredo;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density,
    ScreenOrientation = ScreenOrientation.Portrait)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Define a Status Bar com a cor #fae6c2
        Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#fae6c2"));

        // Opcional: garante que o conteúdo não fique embaixo da Status Bar
        Window.DecorView.SystemUiVisibility = (Android.Views.StatusBarVisibility)Android.Views.SystemUiFlags.LayoutStable;
    }

}

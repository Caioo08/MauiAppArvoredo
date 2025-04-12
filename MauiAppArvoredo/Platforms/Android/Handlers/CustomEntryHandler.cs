using Android.Graphics.Drawables;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using MauiAppArvoredo.Controls;
using Android.Widget;
using MauiEditText = Microsoft.Maui.Platform.MauiEditText;

namespace MauiAppArvoredo.Platforms.Android.Handlers
{
    public class CustomEntryHandler : ViewHandler<CustomEntry, MauiEditText>
    {
        public CustomEntryHandler() : base(CustomEntryMapper)
        {
        }

        public static PropertyMapper<CustomEntry, IEntryHandler> CustomEntryMapper = new(ViewMapper)
        {
            [nameof(CustomEntry.CornerRadius)] = MapBackground,
            [nameof(CustomEntry.BorderColor)] = MapBackground,
            [nameof(CustomEntry.BorderWidth)] = MapBackground,
            [nameof(CustomEntry.CustomBackgroundColor)] = MapBackground
        };

        public static void MapBackground(IEntryHandler handler, IEntry entry)
        {
            if (entry is CustomEntry customEntry && handler.PlatformView is MauiEditText platformView)
            {
                platformView.Background = CreateBackground(customEntry);
            }
        }


        private static GradientDrawable CreateBackground(CustomEntry entry)
        {
            var gradientDrawable = new GradientDrawable();
            gradientDrawable.SetColor(entry.CustomBackgroundColor.ToPlatform());
            gradientDrawable.SetCornerRadius(entry.CornerRadius);
            gradientDrawable.SetStroke(entry.BorderWidth, entry.BorderColor.ToPlatform());
            return gradientDrawable;
        }

        protected override Microsoft.Maui.Platform.MauiEditText CreatePlatformView()
        {
            throw new NotImplementedException();
        }
    }
}

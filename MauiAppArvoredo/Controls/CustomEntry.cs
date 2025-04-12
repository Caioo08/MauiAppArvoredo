using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Windows.Input;


namespace MauiAppArvoredo.Controls
{
    public class CustomEntry : Entry
    {
        // Propriedades personalizadas
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(int), typeof(CustomEntry), 0);

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CustomEntry), Colors.Transparent);

        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create(nameof(BorderWidth), typeof(int), typeof(CustomEntry), 0);

        public static readonly BindableProperty CustomBackgroundColorProperty =
            BindableProperty.Create(nameof(CustomBackgroundColor), typeof(Color), typeof(CustomEntry), Colors.Transparent);

        // Getters e Setters
        public int CornerRadius
        {
            get => (int)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public int BorderWidth
        {
            get => (int)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }

        public Color CustomBackgroundColor
        {
            get => (Color)GetValue(CustomBackgroundColorProperty);
            set => SetValue(CustomBackgroundColorProperty, value);
        }

        public CustomEntry()
        {
            // Configuração padrão
            this.HeightRequest = 50;
        }
    }
}

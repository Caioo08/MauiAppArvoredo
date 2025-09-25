using MauiAppArvoredo.Models;

namespace MauiAppArvoredo.Views;

public partial class Estoque : ContentPage
{
    private List<Madeira> madeiras;
    private Dictionary<int, StackLayout> expanders = new Dictionary<int, StackLayout>();

    public Estoque()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Botão Sair no topo
        var btnSair = new Button
        {
            Text = "Sair",
            BackgroundColor = Colors.Transparent,
            BorderColor = Color.FromArgb("#391b01"),
            BorderWidth = 2,
            CornerRadius = 10,
            TextColor = Color.FromArgb("#391b01"),
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            WidthRequest = 150,
            HorizontalOptions = LayoutOptions.Start,
            Margin = new Thickness(0, 10, 0, 10)
        };

        btnSair.Clicked += async (s, e) =>
        {
            try
            {
                Navigation.PushAsync(new TelaInicial());
            }
            catch (Exception ex)
            {
                DisplayAlert("Não encontrado", ex.Message, "OK");
            }
        };

        // Adiciona no topo do StackPrincipal
        StackPrincipal.Children.Clear();
        StackPrincipal.Children.Add(btnSair);

        await CarregarMadeiras();
    }

    private async Task CarregarMadeiras()
    {
        // Remove expanders antigos, mas mantém o botão Sair no topo
        var btnSair = StackPrincipal.Children.FirstOrDefault() as Button;
        StackPrincipal.Children.Clear();
        expanders.Clear();
        if (btnSair != null)
            StackPrincipal.Children.Add(btnSair);

        madeiras = await App.Database.GetMadeirasAsync();

        foreach (var madeira in madeiras)
        {
            var btn = CriarBotaoMadeira(madeira);
            StackPrincipal.Children.Add(btn);
        }
    }

    private Button CriarBotaoMadeira(Madeira madeira)
    {
        var btn = new Button
        {
            Text = madeira.Nome,
            BackgroundColor = Color.FromArgb("#391b01"),
            TextColor = Colors.White,
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            WidthRequest = 320,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 5, 0, 0)
        };

        btn.Clicked += async (s, e) =>
        {
            if (expanders.ContainsKey(madeira.Id))
            {
                StackPrincipal.Children.Remove(expanders[madeira.Id]);
                expanders.Remove(madeira.Id);
                return;
            }

            var expandable = await CriarExpandable(madeira, btn);
            int index = StackPrincipal.Children.IndexOf(btn);
            StackPrincipal.Children.Insert(index + 1, expandable);
            expanders[madeira.Id] = expandable;
        };

        return btn;
    }

    private async Task<StackLayout> CriarExpandable(Madeira madeira, Button btn)
    {
        var expandable = new StackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            BackgroundColor = Color.FromArgb("#efd4ac"),
            WidthRequest = 320,
            Margin = new Thickness(0, -10, 0, 0),
            Padding = new Thickness(10),
            Spacing = 10
        };

        expandable.Children.Add(new Label
        {
            Text = $"Estoque de {madeira.Nome}",
            FontSize = 24,
            TextColor = Color.FromArgb("#391b01"),
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        });

        await AtualizarItens(expandable, madeira);

        var btnAdd = new Button
        {
            Text = "Adicionar Item",
            BackgroundColor = Color.FromArgb("#391b01"),
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            WidthRequest = 150,
            Margin = new Thickness(0, 10, 0, 0)
        };

        btnAdd.Clicked += async (s, e) =>
        {
            await Navigation.PushAsync(new EditarEstoque(madeira));
        };
        expandable.Children.Add(btnAdd);

        var btnFechar = new Button
        {
            Text = "Fechar",
            BackgroundColor = Color.FromArgb("#391b01"),
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            WidthRequest = 150,
            Margin = new Thickness(0, 10, 0, 5)
        };

        btnFechar.Clicked += (s, e) =>
        {
            StackPrincipal.Children.Remove(expandable);
            expanders.Remove(madeira.Id);
        };
        expandable.Children.Add(btnFechar);

        return expandable;
    }

    public static async Task AtualizarItens(StackLayout expandable, Madeira madeira)
    {
        expandable.Children.Clear();

        expandable.Children.Add(new Label
        {
            Text = $"Estoque de {madeira.Nome}",
            FontSize = 24,
            TextColor = Color.FromArgb("#391b01"),
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        });

        var itens = await App.Database.GetItensByMadeiraAsync(madeira.Id);
        foreach (var item in itens)
        {
            var frame = new Frame
            {
                BorderColor = Colors.Transparent,
                Padding = 10,
                Margin = new Thickness(0, 5),
                BackgroundColor = Colors.Transparent
            };
            var vstack = new VerticalStackLayout();
            vstack.Add(new Label { Text = $"Formato: {item.Formato}", FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#391b01") });
            vstack.Add(new Label { Text = $"Tamanho: {item.Tamanho}", TextColor = Color.FromArgb("#391b01") });
            vstack.Add(new Label { Text = $"Quantidade: {item.Quantidade}", TextColor = Color.FromArgb("#391b01") });
            frame.Content = vstack;
            expandable.Children.Add(frame);
        }
    }
}

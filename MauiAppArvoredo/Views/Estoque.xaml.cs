using MauiAppArvoredo.Models;
using MauiAppArvoredo.Services;

namespace MauiAppArvoredo.Views;

public partial class Estoque : ContentPage
{
    private readonly ApiClient _api;
    private List<Produto> _produtos = new();

    public Estoque(ApiClient apiClient)
    {
        InitializeComponent();
        _api = apiClient;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // chama fire-and-forget para não bloquear UI
        _ = CarregarProdutosAsync();
    }

    public async Task CarregarProdutosAsync()
    {
        try
        {
            var produtosAPI = await _api.GetProdutosAsync();
            _produtos = produtosAPI ?? new List<Produto>();

            StackPrincipal.Children.Clear();

            // topo: botões (garante que estejam sempre acima da lista)
            var topo = new HorizontalStackLayout { Spacing = 10, Margin = new Thickness(0, 0, 0, 10) };
            topo.Children.Add(new Button { Text = "Voltar", BackgroundColor = Color.FromArgb("#391b01"), TextColor = Colors.White, Command = new Command(async () => await Navigation.PopAsync()) });
            topo.Children.Add(new Button { Text = "Adicionar Produto", BackgroundColor = Color.FromArgb("#391b01"), TextColor = Colors.White, Command = new Command(async () => await Navigation.PushAsync(new EditarEstoque(new Produto(), _api))) });
            StackPrincipal.Children.Add(topo);

            foreach (var p in _produtos)
            {
                // use p.Descricao (mapeada para Nome)
                var produtoCard = new Frame
                {
                    BackgroundColor = Color.FromArgb("#fae6c2"),
                    CornerRadius = 15,
                    Padding = 15,
                    Margin = new Thickness(0, 0, 0, 10),
                    BorderColor = Color.FromArgb("#c68f56"),
                    Content = new VerticalStackLayout
                    {
                        Spacing = 5,
                        Children =
                        {
                            new Label { Text = p.Descricao, FontSize = 18, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#391b01") },
                            new Label { Text = $"Qtd: {p.Quantidade}", TextColor = Color.FromArgb("#391b01") },
                            new Label { Text = $"Qtd Min: {p.QuantidadeMin}", TextColor = Color.FromArgb("#391b01") },
                            new Label { Text = $"Unidade: {p.Unidade}", TextColor = Color.FromArgb("#391b01") },
                            new Label { Text = $"Valor: {p.Valor:C}", TextColor = Color.FromArgb("#391b01") },
                            new HorizontalStackLayout
                            {
                                Spacing = 10,
                                Children =
                                {
                                    new Button {
                                        Text = "Editar",
                                        BackgroundColor = Color.FromArgb("#c68f56"),
                                        TextColor = Colors.White,
                                        CornerRadius = 10,
                                        Command = new Command(async () => await Navigation.PushAsync(new EditarEstoque(p, _api)))
                                    }
                                }
                            }
                        }
                    }
                };

                StackPrincipal.Children.Add(produtoCard);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar o estoque.\n{ex.Message}", "OK");
        }
    }
    

}

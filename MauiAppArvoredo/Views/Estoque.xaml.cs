using MauiAppArvoredo.Services;

namespace MauiAppArvoredo.Views
{
    public partial class Estoque : ContentPage
    {
        private readonly ArvoredoApiService _apiService;

        public Estoque()
        {
            InitializeComponent();
            _apiService = new ArvoredoApiService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarEstoqueDaApi();
        }

        private async Task CarregarEstoqueDaApi()
        {
            try
            {
                // Mostra loading
                StackPrincipal.Children.Clear();

                var loading = new ActivityIndicator
                {
                    IsRunning = true,
                    Color = Colors.Brown,
                    HeightRequest = 50
                };
                StackPrincipal.Children.Add(loading);

                // Busca estoque da API
                var estoqueApi = await _apiService.GetEstoqueMadeirasAsync();

                // Remove loading
                StackPrincipal.Children.Clear();

                // Botão Voltar
                var btnVoltar = new Button
                {
                    Text = "Voltar",
                    BackgroundColor = Color.FromArgb("#391b01"),
                    TextColor = Colors.White,
                    CornerRadius = 10,
                    WidthRequest = 150,
                    Margin = new Thickness(0, 0, 0, 20)
                };
                btnVoltar.Clicked += OnVoltarClicked;
                StackPrincipal.Children.Add(btnVoltar);

                // Título
                var titulo = new Label
                {
                    Text = "ESTOQUE DE MADEIRAS",
                    FontSize = 24,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#391b01"),
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 0, 0, 20)
                };
                StackPrincipal.Children.Add(titulo);

                if (!estoqueApi.Any())
                {
                    var mensagem = new Label
                    {
                        Text = "Nenhum item no estoque",
                        FontSize = 18,
                        TextColor = Color.FromArgb("#391b01"),
                        HorizontalTextAlignment = TextAlignment.Center,
                        Margin = new Thickness(0, 50, 0, 0)
                    };
                    StackPrincipal.Children.Add(mensagem);
                    return;
                }

                // Agrupa por madeira
                var madeiraAgrupada = estoqueApi.GroupBy(e => e.Madeira?.Nome ?? "Sem nome");

                foreach (var grupo in madeiraAgrupada)
                {
                    // Frame para cada madeira
                    var frame = new Frame
                    {
                        BackgroundColor = Color.FromArgb("#efd4ac"),
                        CornerRadius = 15,
                        Padding = 15,
                        Margin = new Thickness(0, 10),
                        HasShadow = true
                    };

                    var stackMadeira = new VerticalStackLayout { Spacing = 10 };

                    // Nome da Madeira
                    var labelMadeira = new Label
                    {
                        Text = grupo.Key,
                        FontSize = 20,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Color.FromArgb("#391b01")
                    };
                    stackMadeira.Children.Add(labelMadeira);

                    // Separador
                    var separador = new BoxView
                    {
                        HeightRequest = 2,
                        BackgroundColor = Color.FromArgb("#391b01"),
                        Opacity = 0.3,
                        Margin = new Thickness(0, 5)
                    };
                    stackMadeira.Children.Add(separador);

                    // Itens da madeira
                    foreach (var item in grupo)
                    {
                        var itemFrame = new Frame
                        {
                            BackgroundColor = Colors.White,
                            CornerRadius = 10,
                            Padding = 12,
                            Margin = new Thickness(0, 5),
                            HasShadow = false
                        };

                        var stackItem = new VerticalStackLayout { Spacing = 5 };

                        var gridInfo = new Grid
                        {
                            ColumnDefinitions =
                            {
                                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                                new ColumnDefinition { Width = GridLength.Auto }
                            }
                        };

                        var labelTamanho = new Label
                        {
                            Text = $"Tamanho: {item.Tamanho?.Nome ?? "N/A"}",
                            FontSize = 16,
                            TextColor = Color.FromArgb("#391b01")
                        };
                        Grid.SetColumn(labelTamanho, 0);
                        gridInfo.Children.Add(labelTamanho);

                        var labelQuantidade = new Label
                        {
                            Text = $"{item.Quantidade} unidades",
                            FontSize = 16,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = item.Acabando ? Colors.Red : Color.FromArgb("#4CAF50")
                        };
                        Grid.SetColumn(labelQuantidade, 1);
                        gridInfo.Children.Add(labelQuantidade);

                        stackItem.Children.Add(gridInfo);

                        // Alerta de estoque baixo
                        if (item.Acabando)
                        {
                            var alerta = new Label
                            {
                                Text = "ESTOQUE BAIXO!",
                                FontSize = 14,
                                FontAttributes = FontAttributes.Bold,
                                TextColor = Colors.Red,
                                Margin = new Thickness(0, 5, 0, 0)
                            };
                            stackItem.Children.Add(alerta);
                        }

                        // Botão Editar
                        var btnEditar = new Button
                        {
                            Text = "Editar",
                            BackgroundColor = Color.FromArgb("#391b01"),
                            TextColor = Colors.White,
                            CornerRadius = 8,
                            HeightRequest = 35,
                            Margin = new Thickness(0, 5, 0, 0),
                            CommandParameter = item.Id
                        };
                        btnEditar.Clicked += OnEditarEstoqueClicked;
                        stackItem.Children.Add(btnEditar);

                        itemFrame.Content = stackItem;
                        stackMadeira.Children.Add(itemFrame);
                    }

                    frame.Content = stackMadeira;
                    StackPrincipal.Children.Add(frame);
                }

                // Resumo no final
                var resumoFrame = new Frame
                {
                    BackgroundColor = Color.FromArgb("#391b01"),
                    CornerRadius = 15,
                    Padding = 15,
                    Margin = new Thickness(0, 20, 0, 0)
                };

                var stackResumo = new VerticalStackLayout { Spacing = 8 };

                var totalItens = new Label
                {
                    Text = $"Total de itens: {estoqueApi.Count}",
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Colors.White
                };
                stackResumo.Children.Add(totalItens);

                var totalQuantidade = new Label
                {
                    Text = $"Quantidade total: {estoqueApi.Sum(e => e.Quantidade)} unidades",
                    FontSize = 16,
                    TextColor = Colors.White
                };
                stackResumo.Children.Add(totalQuantidade);

                var itensAcabando = estoqueApi.Count(e => e.Acabando);
                if (itensAcabando > 0)
                {
                    var alertaResumo = new Label
                    {
                        Text = $"{itensAcabando} itens com estoque baixo",
                        FontSize = 16,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Colors.Orange
                    };
                    stackResumo.Children.Add(alertaResumo);
                }

                resumoFrame.Content = stackResumo;
                StackPrincipal.Children.Add(resumoFrame);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Não foi possível carregar o estoque: {ex.Message}", "OK");
            }
        }

        private async void OnEditarEstoqueClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var estoqueId = button?.CommandParameter as string;

            if (estoqueId != null)
            {
                await DisplayAlert("Editar", $"Editar item ID: {estoqueId}", "OK");
                // await Navigation.PushAsync(new EditarEstoqueApi(estoqueId));
            }
        }

        private async void OnVoltarClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}

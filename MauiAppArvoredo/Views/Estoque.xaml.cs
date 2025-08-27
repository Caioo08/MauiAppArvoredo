using MauiAppArvoredo.Models;

namespace MauiAppArvoredo.Views;

public partial class Estoque : ContentPage
{
    private List<Button> todosBotoes = new List<Button>();
    public static string[] madeiras = { "Eucalipto", "Peroba", "Pau Brasil", "Carvalho", "Jatoba", "Nogueira" };
    public static string[] tipos = { "Viga", "Ripa", "Tábua" };
    public static string[] tamanhos_viga = { "6 metros", "7 metros", "9 metros" };
    public static string[] tamanhos_ripa = { "20x30", "12x07", "45x12" };
    public static string[] tamanhos_tabua = { "6 metros", "7 metros", "9 metros" };

    public Estoque()
    {
        InitializeComponent();
        CriarBotoesDinamicamente();
    }

    // M?todo chamado quando a p?gina aparece (para atualizar os dados)
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Atualizar a interface se necess?rio
    }

    private void voltar_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new TelaInicial());
        }
        catch (Exception ex)
        {
            DisplayAlert("Não encontrado", ex.Message, "OK");
        }
    }

    private void CriarBotoesDinamicamente()
    {
        // Cria bot?es em la?o com base na lista
        for (int i = 0; i < madeiras.Length; i++)
        {
            // Cria uma nova inst?ncia de bot?o
            Button novoBtn = new Button
            {
                Text = madeiras[i],
                BackgroundColor = Color.FromArgb("#efd4ac"),
                TextColor = Color.FromArgb("#391b01"),
                CornerRadius = 12,
                WidthRequest = 320,
                HeightRequest = 60,
                FontSize = 20,
                Margin = new Thickness(0, 5),
                BorderColor = Colors.Brown,
                BorderWidth = 1
            };

            // Adiciona um id ao bot?o para identifica??o
            int index = i; // Capturar o valor de i para o closure do evento

            // Adiciona o evento de clique
            novoBtn.Clicked += (sender, e) =>
            {
                EsconderBotoes();
                BotaoClicado(sender, index);
            };

            todosBotoes.Add(novoBtn);

            // Adiciona o bot?o ao container
            StackPrincipal.Add(novoBtn);
        }
    }

    private void EsconderBotoes()
    {
        // Esconde todos os bot?es
        foreach (Button btn in todosBotoes)
        {
            btn.IsVisible = false;
        }
        estoque.IsVisible = false;
    }

    private void MostrarBotoes()
    {
        // Torna todos os bot?es vis?veis novamente
        foreach (Button btn in todosBotoes)
        {
            btn.IsVisible = true;
        }
        estoque.IsVisible = true;
    }

    private void BotaoClicado(object sender, int index)
    {
        Button botaoClicado = (Button)sender;
        string madeiraSelecionada = botaoClicado.Text;

        // Criar um StackLayout personalizado
        StackLayout expandableSection = new StackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            BackgroundColor = Color.FromArgb("#efd4ac"),
            WidthRequest = 320,
            Margin = new Thickness(0, -10, 0, 0)
        };

        // Cabe?alho
        HorizontalStackLayout header = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center
        };

        header.Add(new Label
        {
            Text = $"Estoque de {botaoClicado.Text}",
            FontSize = 24,
            TextColor = Color.FromArgb("#391b01"),
            FontFamily = "Gagalin-Regular",
            HorizontalOptions = LayoutOptions.Center,
            FontAttributes = FontAttributes.Bold
        });

        expandableSection.Add(header);

        // MODIFICA??O: Criar linhas dinamicamente com dados salvos
        CriarLinhasEstoque(expandableSection, madeiraSelecionada);

        // Adicionar bot?o de fechar
        Button btnFechar = new Button
        {
            Text = "Fechar",
            BackgroundColor = Color.FromArgb("#391b01"),
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            Padding = new Thickness(10, 5),
            WidthRequest = 150,
            Margin = new Thickness(0, 10, 0, 5)
        };

        Button btnEditar = new Button
        {
            Text = "Editar",
            BackgroundColor = Color.FromArgb("#391b01"),
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            Padding = new Thickness(10, 5),
            WidthRequest = 150,
            Margin = new Thickness(0, 10, 0, 5)
        };

        btnFechar.Clicked += (s, e) =>
        {
            // Remover o StackLayout quando o bot?o de fechar for clicado
            StackPrincipal.Remove(expandableSection);
            MostrarBotoes();
        };

        // Passar o texto do bot?o clicado para EditarEstoque
        btnEditar.Clicked += (s, e) =>
        {
            try
            {
                Navigation.PushAsync(new EditarEstoque(botaoClicado.Text));
            }
            catch (Exception ex)
            {
                DisplayAlert("Não encontrado", ex.Message, "OK");
            }
        };

        expandableSection.Add(btnFechar);
        expandableSection.Add(btnEditar);

        // Adicionar o StackLayout ao mesmo container onde os bot?es est?o
        StackPrincipal.Add(expandableSection);
    }

    // NOVO M?TODO: Criar linhas do estoque com dados salvos
    private void CriarLinhasEstoque(StackLayout container, string madeira)
    {
        // Obter dados salvos para esta madeira
        var dadosSalvos = DadosEstoque.ObterQuantidadesPorMadeira(madeira);

        foreach (string tipo in tipos)
        {
            // Criar linha para cada tipo (Viga, Ripa, Tábua)
            HorizontalStackLayout linha = new HorizontalStackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 5)
            };

            // Label do tipo
            linha.Add(new Label
            {
                Text = tipo,
                FontFamily = "Gagalin-Regular",
                FontSize = 20,
                TextColor = Color.FromArgb("#391b01"),
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(10, 0, 0, 0)
            });

            // Criar se??o de quantidades para este tipo
            StackLayout secaoQuantidades = new StackLayout
            {
                HorizontalOptions = LayoutOptions.End,
                Spacing = 3
            };

            // Obter tamanhos baseado no tipo
            string[] tamanhos = tipo switch
            {
                "Viga" => tamanhos_viga,
                "Ripa" => tamanhos_ripa,
                "Tábua" => tamanhos_tabua,
                _ => new string[0]
            };

            // Criar labels para cada tamanho
            bool temDados = false;
            foreach (string tamanho in tamanhos)
            {
                int quantidade = DadosEstoque.ObterQuantidade(madeira, tipo, tamanho);
                if (quantidade > 0)
                {
                    temDados = true;
                    Label labelQuantidade = new Label
                    {
                        Text = $"   {tamanho}: {quantidade}",
                        FontFamily = "Gagalin-Regular",
                        FontSize = 18,
                        TextColor = Color.FromArgb("#391b01"),
                        HorizontalOptions = LayoutOptions.Center
                    };
                    secaoQuantidades.Add(labelQuantidade);
                }
            }

            // Se Não tem dados salvos, mostrar "QTD" como antes
            if (!temDados)
            {
                Label labelPadrao = new Label
                {
                    Text = " QTD",
                    FontFamily = "Gagalin-Regular",
                    FontSize = 20,
                    TextColor = Color.FromArgb("#391b01"),
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.Center
                };
                secaoQuantidades.Add(labelPadrao);
            }

            linha.Add(secaoQuantidades);
            container.Add(linha);
        }
    }
}
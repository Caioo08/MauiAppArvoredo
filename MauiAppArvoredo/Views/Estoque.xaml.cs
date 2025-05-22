using Microsoft.Windows.PushNotifications;

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
        // Exemplo de uma lista de dados para criar botões
        

        // Cria botões em laço com base na lista
        for (int i = 0; i < madeiras.Length; i++)
        {
            // Cria uma nova instância de botão
            Button novoBtn = new Button
            {
                Text = madeiras[i],
                BackgroundColor = Color.FromArgb("#efd4ac"),
                TextColor = Color.FromArgb("#391b01"),
                CornerRadius = 8,
                WidthRequest = 320,
                HeightRequest = 60,
                FontSize = 20,
                Margin = new Thickness(0, 5)
            };

            // Adiciona um id ao botão para identificação
            int index = i; // Capturar o valor de i para o closure do evento

            // Adiciona o evento de clique
            novoBtn.Clicked += (sender, e) =>
            {
                EsconderBotoes();
                BotaoClicado(sender, index);
            };

            todosBotoes.Add(novoBtn);

            // Adiciona o botão ao container
            StackPrincipal.Add(novoBtn);
        }
    }
    private void EsconderBotoes()
    {
        // Esconde todos os botões
        foreach (Button btn in todosBotoes)
        {
            btn.IsVisible = false;
        }
        estoque.IsVisible = false;
    }

    private void MostrarBotoes()
    {
        // Torna todos os botões visíveis novamente
        foreach (Button btn in todosBotoes)
        {
            btn.IsVisible = true;
        }
        estoque.IsVisible = true;
    }

    private void BotaoClicado(object sender, int index)
    {
        Button botaoClicado = (Button)sender;


        // Criar um StackLayout personalizado
        StackLayout expandableSection = new StackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            BackgroundColor = Color.FromArgb("#efd4ac"),
            WidthRequest = 320,
            Margin = new Thickness(0, -10, 0, 0)
        };

        // Adicionar itens ao StackLayout
        // Cabeçalho
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

        // Linha 1: Ripa
        HorizontalStackLayout linharipa = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center
        };

        linharipa.Add(new Label
        {
            Text = tipos[0],
            FontFamily = "Gagalin-Regular",
            FontSize = 24,
            TextColor = Color.FromArgb("#391b01"),
            HorizontalOptions = LayoutOptions.Start,
            Margin = new Thickness(0, 0, 100, 0)
        });

        linharipa.Add(new Label
        {
            Text = "QTD",
            FontFamily = "Gagalin-Regular",
            FontSize = 24,
            TextColor = Color.FromArgb("#391b01"),
            HorizontalOptions = LayoutOptions.End
        });

        expandableSection.Add(linharipa);

        // Linha 2: Viga
        HorizontalStackLayout linhaviga = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center
        };

        linhaviga.Add(new Label
        {
            Text = tipos[1],
            FontFamily = "Gagalin-Regular",
            FontSize = 24,
            TextColor = Color.FromArgb("#391b01"),
            HorizontalOptions = LayoutOptions.Start,
            Margin = new Thickness(0, 0, 100, 0)
        });

        linhaviga.Add(new Label
        {
            Text = "QTD",
            FontFamily = "Gagalin-Regular",
            FontSize = 24,
            TextColor = Color.FromArgb("#391b01"),
            HorizontalOptions = LayoutOptions.End
        });

        expandableSection.Add(linhaviga);

        // Linha 3: Tábua
        HorizontalStackLayout linhatabua = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center
        };

        linhatabua.Add(new Label
        {
            Text = tipos[2],
            FontFamily = "Gagalin-Regular",
            FontSize = 24,
            TextColor = Color.FromArgb("#391b01"),
            HorizontalOptions = LayoutOptions.Start,
            Margin = new Thickness(0, 0, 80, 0)
        });

        linhatabua.Add(new Label
        {
            Text = "QTD",
            FontFamily = "Gagalin-Regular",
            FontSize = 24,
            TextColor = Color.FromArgb("#391b01"),
            HorizontalOptions = LayoutOptions.End
        });

        expandableSection.Add(linhatabua);

        // Adicionar botão de fechar
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
            // Remover o StackLayout quando o botão de fechar for clicado
            StackPrincipal.Remove(expandableSection);
            MostrarBotoes();
        };

        btnEditar.Clicked += (s, e) =>
        {
            try
            {
                Navigation.PushAsync(new EditarEstoque());
            }
            catch (Exception ex)
            {
                DisplayAlert("Não encontrado", ex.Message, "OK");
            }
        };

        expandableSection.Add(btnFechar);
        expandableSection.Add(btnEditar);


        // Adicionar o StackLayout ao mesmo container onde os botões estão
        StackPrincipal.Add(expandableSection);
    }
}
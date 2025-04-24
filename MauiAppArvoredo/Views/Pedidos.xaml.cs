using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics.Text;
using static System.Net.Mime.MediaTypeNames;

namespace MauiAppArvoredo.Views;

public partial class Pedidos : ContentPage
{

    private List<Button> todosBotoes = new List<Button>();
    public Pedidos()
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
		catch(Exception ex)
		{
			DisplayAlert("Página não encontrada", ex.Message, "OK");
		}
    }


    private void CriarBotoesDinamicamente()
    {
        // Exemplo de uma lista de dados para criar botões
        string[] opcoes = { "João", "Pedro", "Marcos", "Tiago", "Lucas" };

        // Cria botões em laço com base na lista
        for (int i = 0; i < opcoes.Length; i++)
        {
            // Cria uma nova instância de botão
            Button novoBtn = new Button
            {
                Text = opcoes[i],
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
            myLayout.Add(novoBtn);
        }
    }
    private void EsconderBotoes()
    {
        // Esconde todos os botões
        foreach (Button btn in todosBotoes)
        {
            btn.IsVisible = false;
        }
        pedido.IsVisible = false;
    }

    private void MostrarBotoes()
    {
        // Torna todos os botões visíveis novamente
        foreach (Button btn in todosBotoes)
        {
            btn.IsVisible = true;
        }
        pedido.IsVisible = true;
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
            Text = $"Pedido de {botaoClicado.Text}",
            FontSize = 24,
            TextColor = Color.FromArgb("#391b01"),
            FontFamily = "Gagalin-Regular",
            HorizontalOptions = LayoutOptions.Center,
            FontAttributes = FontAttributes.Bold
        });

        expandableSection.Add(header);

        // Linha 1: Ripa
        HorizontalStackLayout linha1 = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center
        };

        linha1.Add(new Label
        {
            Text = "Ripa",
            FontFamily = "Gagalin-Regular",
            FontSize = 24,
            TextColor = Color.FromArgb("#391b01"),
            HorizontalOptions = LayoutOptions.Start,
            Margin = new Thickness(0, 0, 100, 0)
        });

        linha1.Add(new Label
        {
            Text = "QTD",
            FontFamily = "Gagalin-Regular",
            FontSize = 24,
            TextColor = Color.FromArgb("#391b01"),
            HorizontalOptions = LayoutOptions.End
        });

        expandableSection.Add(linha1);

        // Linha 2: Viga
        HorizontalStackLayout linha2 = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center
        };

        linha2.Add(new Label
        {
            Text = "Viga",
            FontFamily = "Gagalin-Regular",
            FontSize = 24,
            TextColor = Color.FromArgb("#391b01"),
            HorizontalOptions = LayoutOptions.Start,
            Margin = new Thickness(0, 0, 100, 0)
        });

        linha2.Add(new Label
        {
            Text = "QTD",
            FontFamily = "Gagalin-Regular",
            FontSize = 24,
            TextColor = Color.FromArgb("#391b01"),
            HorizontalOptions = LayoutOptions.End
        });

        expandableSection.Add(linha2);

        // Linha 3: Tábua
        HorizontalStackLayout linha3 = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center
        };

        linha3.Add(new Label
        {
            Text = "Tábua",
            FontFamily = "Gagalin-Regular",
            FontSize = 24,
            TextColor = Color.FromArgb("#391b01"),
            HorizontalOptions = LayoutOptions.Start,
            Margin = new Thickness(0, 0, 80, 0)
        });

        linha3.Add(new Label
        {
            Text = "QTD",
            FontFamily = "Gagalin-Regular",
            FontSize = 24,
            TextColor = Color.FromArgb("#391b01"),
            HorizontalOptions = LayoutOptions.End
        });

        expandableSection.Add(linha3);

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

        btnFechar.Clicked += (s, e) =>
        {
            // Remover o StackLayout quando o botão de fechar for clicado
            myLayout.Remove(expandableSection);
            MostrarBotoes();
        };

        expandableSection.Add(btnFechar);

        // Adicionar o StackLayout ao mesmo container onde os botões estão
        myLayout.Add(expandableSection);
    }
}
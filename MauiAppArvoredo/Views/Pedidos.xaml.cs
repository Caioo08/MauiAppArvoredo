using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;

namespace MauiAppArvoredo.Views;

public partial class Pedidos : ContentPage
{
    string[] tipos = Estoque.tipos;
    private List<Button> todosBotoes = new List<Button>();
    // Array de opções que será modificado quando um item for removido
    private string[] opcoes = { "João", "Pedro", "Marcos", "Tiago", "Lucas" };

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
        catch (Exception ex)
        {
            DisplayAlert("Página não encontrada", ex.Message, "OK");
        }
    }

    private void CriarBotoesDinamicamente()
    {
        // Limpa a lista de botões anterior, caso haja recriação
        todosBotoes.Clear();

        // Limpa o layout para recriar os botões
        foreach (var child in myLayout.Children.ToList())
        {
            if (child is Button)
                myLayout.Remove(child);
        }

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

    // Nova função para remover um item da array
    private void RemoverItemDoArray(string itemParaRemover)
    {
        // Remove o item usando LINQ e converte de volta para array
        opcoes = opcoes.Where(item => item != itemParaRemover).ToArray();

        // Recria os botões com o array atualizado
        CriarBotoesDinamicamente();
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

        // Adiciona botão para remover o item
        Button btnConcluir = new Button
        {
            Text = "Concluir Pedido",
            BackgroundColor = Color.FromArgb("#6BD14F"), // Cor vermelha para indicar remoção
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            Padding = new Thickness(10, 5),
            WidthRequest = 150,
            Margin = new Thickness(0, 5, 0, 5)
        };

        btnConcluir.Clicked += (s, e) =>
        {
            // Remove o item da array e fecha o painel
            string itemParaRemover = botaoClicado.Text;
            myLayout.Remove(expandableSection);
            RemoverItemDoArray(itemParaRemover);
            MostrarBotoes();
        };

        btnFechar.Clicked += (s, e) =>
        {
            // Remover o StackLayout quando o botão de fechar for clicado
            myLayout.Remove(expandableSection);
            MostrarBotoes();
        };

        expandableSection.Add(btnFechar);
        expandableSection.Add(btnConcluir); // Adiciona o botão de remover

        // Adicionar o StackLayout ao mesmo container onde os botões estão
        myLayout.Add(expandableSection);
    }
}
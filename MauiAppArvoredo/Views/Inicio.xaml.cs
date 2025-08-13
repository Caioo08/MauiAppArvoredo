using MauiAppArvoredo.Models;
namespace MauiAppArvoredo;

public partial class Inicio : ContentPage
{

    private List<EstoqueItem> itensEmMemoria = new();
    public Inicio()
	{
		InitializeComponent();
        tempo();
    }

    private void tempo()
    {
        Label horaLabel = new Label
        {
            Text = "Sincronizado desde:" + DateTime.Now.ToString("HH:mm"),
            FontSize = 16,
            TextColor = Colors.Black,
            HorizontalOptions = LayoutOptions.Center
        };
        
        Footer.Add(horaLabel);
        
    }

    private void login_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Login());
        }
        catch (Exception ex)
        {
            DisplayAlert("Não encontrado", ex.Message, "OK");
        }
    }

    private async void sincronizar_Clicked(object sender, EventArgs e)
    {
        foreach (var item in itensEmMemoria)
        {
            await App.Database.SalvarItemAsync(item);
        }

        await DisplayAlert("Sucesso", "Itens sincronizados com o banco de dados!", "OK");

        // Se quiser, limpar a lista em memória
        itensEmMemoria.Clear();
    }

    private void configuracoes_Clicked(object sender, EventArgs e)
    {

    }

    private void AdicionarItem(string nome, int quantidade)
    {
        itensEmMemoria.Add(new EstoqueItem
        {
            Nome = nome,
            Quantidade = quantidade
        });
    }
}
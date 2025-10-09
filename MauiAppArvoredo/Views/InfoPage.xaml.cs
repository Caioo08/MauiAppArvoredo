namespace MauiAppArvoredo.Views;

public partial class InfoPage : ContentPage
{
    public InfoPage()
    {
        InitializeComponent();
        CarregarDatas();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Inicio());
        }
        catch (Exception ex)
        {
            DisplayAlert("Não encontrado", ex.Message, "OK");
        }
    }
    private void CarregarDatas()
    {
        // Pega a data atual e formata no padrão brasileiro
        string dataAtual = DateTime.Now.ToString("dd/MM/yyyy");

        // Define o texto dos labels com a data atual
        lblUltimaSincronizacao.Text = $"Última sincronização: {dataAtual}";
        lblSincronizadoDesde.Text = $"Sincronizado desde: {dataAtual}";
    }
}
namespace MauiAppArvoredo.Views;

public partial class Estoque : ContentPage
{
	public Estoque()
	{
		InitializeComponent();
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

    private void eucalipto_Clicked(object sender, EventArgs e)
    {
        
    }

    private void peroba_Clicked(object sender, EventArgs e)
    {

    }

    private void pau_brasil_Clicked(object sender, EventArgs e)
    {

    }

    private void carvalho_Clicked(object sender, EventArgs e)
    {

    }

    private void jatoba_Clicked(object sender, EventArgs e)
    {

    }

    private void nogueira_Clicked(object sender, EventArgs e)
    {

    }
}
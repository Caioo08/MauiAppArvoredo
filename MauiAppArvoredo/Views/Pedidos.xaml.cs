namespace MauiAppArvoredo.Views;

public partial class Pedidos : ContentPage
{
	public Pedidos()
	{
		InitializeComponent();
	}

    private void voltar_Clicked(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync(new TelaInicial());
		}
		catch(Exception ex)
		{
			DisplayAlert("P�gina n�o encontrada", ex.Message, "OK");
		}
    }
}
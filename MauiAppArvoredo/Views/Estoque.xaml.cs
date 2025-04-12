namespace MauiAppArvoredo.Views;

public partial class Estoque : ContentPage
{
    private bool _isExpanded;
    public Estoque()
	{
		InitializeComponent();
        _isExpanded = false;
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
        if(_isExpanded == false)
        {
            eucalipto.HeightRequest = 250;
            _isExpanded = true;
            eucalipto.Text = "      Eucalipto \n Ripa            QTD";
            peroba.HeightRequest = 0;
        }else if(_isExpanded == true)
        {
            eucalipto.HeightRequest = 60;
            _isExpanded = false;
            eucalipto.Text = "Eucalipto";
            peroba.HeightRequest = 60;
        }
    }

    private void peroba_Clicked(object sender, EventArgs e)
    {
        if (_isExpanded == false)
        {
            peroba.HeightRequest = 200;
            _isExpanded = true;
            peroba.Text = "      Peroba \n Ripa            QTD";
        }
        else if (_isExpanded == true)
        {
            peroba.HeightRequest = 60;
            _isExpanded = false;
            peroba.Text = "Peroba";
        }
    }

    private void pau_brasil_Clicked(object sender, EventArgs e)
    {
        if (_isExpanded == false)
        {
            pau_brasil.HeightRequest = 200;
            _isExpanded = true;
            pau_brasil.Text = "      Pau Brasil \n Ripa            QTD";
        }
        else if (_isExpanded == true)
        {
            pau_brasil.HeightRequest = 60;
            _isExpanded = false;
            pau_brasil.Text = "Pau Brasil";
        }
    }

    private void carvalho_Clicked(object sender, EventArgs e)
    {
        if (_isExpanded == false)
        {
            carvalho.HeightRequest = 200;
            _isExpanded = true;
            carvalho.Text = "      Carvalho \n Ripa            QTD";
        }
        else if (_isExpanded == true)
        {
            carvalho.HeightRequest = 60;
            _isExpanded = false;
            carvalho.Text = "Carvalho";
        }
    }

    private void jatoba_Clicked(object sender, EventArgs e)
    {
        if (_isExpanded == false)
        {
            jatoba.HeightRequest = 200;
            _isExpanded = true;
            jatoba.Text = "      Jatoba \n Ripa            QTD";
        }
        else if (_isExpanded == true)
        {
            jatoba.HeightRequest = 60;
            _isExpanded = false;
            jatoba.Text = "Jatoba";
        }
    }

    private void nogueira_Clicked(object sender, EventArgs e)
    {
        if (_isExpanded == false)
        {
            nogueira.HeightRequest = 200;
            _isExpanded = true;
            nogueira.Text = "      Nogueira \n Ripa            QTD";
        }
        else if (_isExpanded == true)
        {
            nogueira.HeightRequest = 60;
            _isExpanded = false;
            nogueira.Text = "Nogueira";
        }
    }
}
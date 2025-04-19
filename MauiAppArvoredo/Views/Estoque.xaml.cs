namespace MauiAppArvoredo.Views;

public partial class Estoque : ContentPage
{
    private bool _isExpanded;
    private bool frameVisible = false;
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
        frameVisible = !frameVisible;
        expansive_eucalipto.IsVisible = frameVisible;
        if (_isExpanded == false)
        {
            _isExpanded = true;
            expansive_eucalipto.IsVisible = frameVisible;
            peroba.IsVisible = false;
            pau_brasil.IsVisible = false;
            carvalho.IsVisible = false;
            jatoba.IsVisible = false;
            nogueira.IsVisible = false;
        }
        else if(_isExpanded == true)
        {
            _isExpanded = false;
            peroba.IsVisible = true;
            pau_brasil.IsVisible = true;
            carvalho.IsVisible = true;
            jatoba.IsVisible = true;
            nogueira.IsVisible = true;
        }
    }

    private void peroba_Clicked(object sender, EventArgs e)
    {
        frameVisible = !frameVisible;
        expansive_peroba.IsVisible = frameVisible;
        if (_isExpanded == false)
        {
            _isExpanded = true;
            expansive_peroba.IsVisible = frameVisible;
            eucalipto.IsVisible = false;
            pau_brasil.IsVisible = false;
            carvalho.IsVisible = false;
            jatoba.IsVisible = false;
            nogueira.IsVisible = false;
        }
        else if (_isExpanded == true)
        {
            _isExpanded = false;
            eucalipto.IsVisible = true;
            pau_brasil.IsVisible = true;
            carvalho.IsVisible = true;
            jatoba.IsVisible = true;
            nogueira.IsVisible = true;
        }
    }

    private void pau_brasil_Clicked(object sender, EventArgs e)
    {
        frameVisible = !frameVisible;
        expansive_pau_brasil.IsVisible = frameVisible;
        if (_isExpanded == false)
        {
            _isExpanded = true;
            expansive_pau_brasil.IsVisible = frameVisible;
            peroba.IsVisible = false;
            eucalipto.IsVisible = false;
            carvalho.IsVisible = false;
            jatoba.IsVisible = false;
            nogueira.IsVisible = false;
        }
        else if (_isExpanded == true)
        {
            _isExpanded = false;
            peroba.IsVisible = true;
            eucalipto.IsVisible = true;
            carvalho.IsVisible = true;
            jatoba.IsVisible = true;
            nogueira.IsVisible = true;
        }
    }

    private void carvalho_Clicked(object sender, EventArgs e)
    {
        frameVisible = !frameVisible;
        expansive_carvalho.IsVisible = frameVisible;
        if (_isExpanded == false)
        {
            _isExpanded = true;
            expansive_carvalho.IsVisible = frameVisible;
            peroba.IsVisible = false;
            pau_brasil.IsVisible = false;
            eucalipto.IsVisible = false;
            jatoba.IsVisible = false;
            nogueira.IsVisible = false;
        }
        else if (_isExpanded == true)
        {
            _isExpanded = false;
            peroba.IsVisible = true;
            pau_brasil.IsVisible = true;
            eucalipto.IsVisible = true;
            jatoba.IsVisible = true;
            nogueira.IsVisible = true;
        }
    }

    private void jatoba_Clicked(object sender, EventArgs e)
    {
        frameVisible = !frameVisible;
        expansive_jatoba.IsVisible = frameVisible;
        if (_isExpanded == false)
        {
            _isExpanded = true;
            expansive_jatoba.IsVisible = frameVisible;
            peroba.IsVisible = false;
            pau_brasil.IsVisible = false;
            carvalho.IsVisible = false;
            eucalipto.IsVisible = false;
            nogueira.IsVisible = false;
        }
        else if (_isExpanded == true)
        {
            _isExpanded = false;
            peroba.IsVisible = true;
            pau_brasil.IsVisible = true;
            carvalho.IsVisible = true;
            eucalipto.IsVisible = true;
            nogueira.IsVisible = true;
        }
    }

    private void nogueira_Clicked(object sender, EventArgs e)
    {
        frameVisible = !frameVisible;
        expansive_nogueira.IsVisible = frameVisible;
        if (_isExpanded == false)
        {
            _isExpanded = true;
            expansive_nogueira.IsVisible = frameVisible;
            peroba.IsVisible = false;
            pau_brasil.IsVisible = false;
            carvalho.IsVisible = false;
            jatoba.IsVisible = false;
            eucalipto.IsVisible = false;
        }
        else if (_isExpanded == true)
        {
            _isExpanded = false;
            peroba.IsVisible = true;
            pau_brasil.IsVisible = true;
            carvalho.IsVisible = true;
            jatoba.IsVisible = true;
            eucalipto.IsVisible = true;
        }
    }
}
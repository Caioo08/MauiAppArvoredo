namespace MauiAppArvoredo.Views;

public partial class EditarEstoque : ContentPage
{
	public EditarEstoque()
	{
		InitializeComponent();
		CriarTela();
	}

	private void CriarTela()
	{
		Entry entry = new Entry()
		{
			BackgroundColor = Color.FromArgb("#efd4ac"),
			WidthRequest = 330,

		};

		Stackprincipal.Add(entry);
	}
}
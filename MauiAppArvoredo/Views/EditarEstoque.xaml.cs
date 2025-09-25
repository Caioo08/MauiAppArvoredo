using MauiAppArvoredo.Models;

namespace MauiAppArvoredo.Views;

public partial class EditarEstoque : ContentPage
{
    private Madeira madeiraAtual;

    public EditarEstoque(Madeira madeira)
    {
        InitializeComponent();
        madeiraAtual = madeira;
    }

    private void Sair(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Estoque());
        }
        catch (Exception ex)
        {
            DisplayAlert("Página não encontrada", ex.Message, "OK");
        }
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        if (FormatoPicker.SelectedItem == null ||
            TamanhoPicker.SelectedItem == null ||
            string.IsNullOrWhiteSpace(QuantidadeEntry.Text))
        {
            await DisplayAlert("Erro", "Preencha todos os campos!", "OK");
            return;
        }

        string formato = FormatoPicker.SelectedItem.ToString();
        string tamanho = TamanhoPicker.SelectedItem.ToString();
        int quantidade = int.Parse(QuantidadeEntry.Text);

        // Verifica se já existe item desse formato+tamanho para a madeira
        var itensExistentes = await App.Database.GetItensByMadeiraAsync(madeiraAtual.Id);
        var existente = itensExistentes.FirstOrDefault(i => i.Formato == formato && i.Tamanho == tamanho);

        if (existente != null)
        {
            // Atualiza o existente
            existente.Quantidade = quantidade;
            await App.Database.SaveItemMadeiraAsync(existente);
        }
        else
        {
            // Cria novo
            var item = new ItemMadeira
            {
                MadeiraId = madeiraAtual.Id,
                Formato = formato,
                Tamanho = tamanho,
                Quantidade = quantidade
            };

            await App.Database.SaveItemMadeiraAsync(item);
        }

        await DisplayAlert("Sucesso", "Item cadastrado!", "OK");
        await Navigation.PopAsync();
    }
}
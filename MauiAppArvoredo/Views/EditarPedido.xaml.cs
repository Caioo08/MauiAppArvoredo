using MauiAppArvoredo.Models;
using MauiAppArvoredo.Helpers;

namespace MauiAppArvoredo.Views;

public partial class EditarPedido : ContentPage
{
    private List<ItemPedido> itens = new List<ItemPedido>();
    private List<Madeira> madeiras = new List<Madeira>();
    private Pedido pedidoAtual;

    public EditarPedido(Pedido pedido = null)
    {
        InitializeComponent();
        CarregarMadeiras();
        CarregarFormatos();

        if (pedido != null)
        {
            pedidoAtual = pedido;
            EntryCliente.Text = pedido.Cliente;
            CarregarItensDoPedido(pedido.Id);
        }
    }

    private async void CarregarItensDoPedido(int pedidoId)
    {
        itens = (await App.Database.GetItensByPedidoAsync(pedidoId)).ToList();
        ListaItens.ItemsSource = itens;
    }

    private async void CarregarMadeiras()
    {
        madeiras = await App.Database.GetMadeirasAsync();
        PickerMadeira.ItemsSource = madeiras.Select(m => m.Nome).ToList();
    }

    private void CarregarFormatos()
    {
        PickerFormato.ItemsSource = new List<string> { "Tábua", "Viga", "Ripa" };
    }

    private async void PickerMadeira_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (PickerMadeira.SelectedIndex == -1)
        {
            PickerTamanho.ItemsSource = null;
            return;
        }

        var madeiraSelecionada = madeiras[PickerMadeira.SelectedIndex];
        var itensEstoque = await App.Database.GetItensByMadeiraAsync(madeiraSelecionada.Id);

        // Extrai tamanhos disponíveis no estoque
        var tamanhos = itensEstoque
            .Where(i => !string.IsNullOrWhiteSpace(i.Tamanho))
            .Select(i => i.Tamanho.Trim())
            .Distinct()
            .ToList();

        Dispatcher.Dispatch(() =>
        {
            PickerTamanho.ItemsSource = tamanhos;
            PickerTamanho.SelectedIndex = tamanhos.Count > 0 ? 0 : -1;
        });
    }

    private void BtnAdicionarItem_Clicked(object sender, EventArgs e)
    {
        if (PickerMadeira.SelectedIndex == -1 || PickerFormato.SelectedIndex == -1 || PickerTamanho.SelectedIndex == -1)
        {
            DisplayAlert("Erro", "Preencha todos os campos", "OK");
            return;
        }

        if (!int.TryParse(EntryQuantidade.Text, out int qtd) || qtd <= 0)
        {
            DisplayAlert("Erro", "Informe uma quantidade válida", "OK");
            return;
        }

        var itemNome = $"{PickerMadeira.SelectedItem} - {PickerFormato.SelectedItem} - {PickerTamanho.SelectedItem}";

        itens.Add(new ItemPedido
        {
            Nome = itemNome,
            Quantidade = qtd
        });

        ListaItens.ItemsSource = null;
        ListaItens.ItemsSource = itens;

        // Limpa campos
        EntryQuantidade.Text = "";
        PickerMadeira.SelectedIndex = -1;
        PickerFormato.SelectedIndex = -1;
        PickerTamanho.ItemsSource = null;
    }

    private async void BtnSalvar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EntryCliente.Text))
        {
            await DisplayAlert("Erro", "Informe o nome do cliente", "OK");
            return;
        }

        Pedido pedido = pedidoAtual ?? new Pedido { Status = "Pendente" };
        pedido.Cliente = EntryCliente.Text;

        if (pedidoAtual == null)
            await App.Database.SavePedidoAsync(pedido);
        else
            await App.Database.UpdatePedidoAsync(pedido);

        foreach (var item in itens)
        {
            item.PedidoId = pedido.Id;
            await App.Database.SaveItemPedidoAsync(item);
        }

        await Navigation.PopAsync();
    }
    private void BtnSair_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Pedidos());
        }
        catch (Exception ex)
        {
            DisplayAlert("Não encontrado", ex.Message, "OK");
        }
    }
}

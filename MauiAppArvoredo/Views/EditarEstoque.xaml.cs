using MauiAppArvoredo.Models;
using MauiAppArvoredo.Services;
using System.Linq;

namespace MauiAppArvoredo.Views;

public partial class EditarEstoque : ContentPage
{
    private Produto _produto;
    private readonly ApiClient _api;

    public EditarEstoque(Produto produto, ApiClient apiClient)
    {
        InitializeComponent();

        _produto = produto ?? new Produto();
        _api = apiClient;

        // Preenche campos (se vierem nulos, colocamos valores seguros)
        entryDescricao.Text = _produto.Nome ?? "";
        entryQuantidade.Text = _produto.Quantidade.ToString();
        entryQuantidadeMin.Text = _produto.QuantidadeMin.ToString();
        entryUnidade.Text = _produto.Unidade ?? "";
        entryValor.Text = _produto.Valor.ToString("F2");
    }

    private async void BtnSalvar_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Validações simples
            _produto.Nome = entryDescricao.Text?.Trim() ?? "";
            _produto.Quantidade = int.TryParse(entryQuantidade.Text, out var q) ? q : 0;
            _produto.QuantidadeMin = int.TryParse(entryQuantidadeMin.Text, out var qm) ? qm : 0;
            _produto.Unidade = entryUnidade.Text?.Trim() ?? "";
            _produto.Valor = double.TryParse(entryValor.Text, out var v) ? v : 0.0;

            (bool ok, string body) result;
            if (_produto.Id == 0)
            {
                result = await _api.AddProdutoAsync(_produto);
            }
            else
            {
                result = await _api.UpdateProdutoAsync(_produto.Id, _produto);
            }

            if (!result.ok)
            {
                // mostra mensagem de erro retornada pela API
                await DisplayAlert("Erro ao salvar", result.body, "OK");
                return;
            }

            await DisplayAlert("Sucesso", "Produto salvo com sucesso!", "OK");

            // força atualização quando voltar: busca a instância Estoque na pilha e chama o método público
            var estoquePage = Navigation.NavigationStack.OfType<Estoque>().FirstOrDefault();
            if (estoquePage != null)
            {
                await estoquePage.CarregarProdutosAsync();
            }

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Exceção", ex.Message + "\n" + ex.ToString(), "OK");
        }
    }

    private async void Voltar_Clicked(object sender, EventArgs e)
    {
        // sempre pop, não push
        await Navigation.PopAsync();
    }
}

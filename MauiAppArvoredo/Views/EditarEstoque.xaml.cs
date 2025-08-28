using MauiAppArvoredo.Models;

namespace MauiAppArvoredo.Views;

public partial class EditarEstoque : ContentPage
{
    string[] madeiras = Estoque.madeiras;
    string[] tipos = Estoque.tipos;
    string[] tamanhos_viga = Estoque.tamanhos_viga;
    string[] tamanhos_ripa = Estoque.tamanhos_ripa;
    string[] tamanhos_tabua = Estoque.tamanhos_tabua;

    // Declarar os pickers como campos da classe para acessá-los nos eventos
    private Picker pickerMadeiras;
    private Picker pickerTipos;
    private Picker pickerTamanhos;
    private Entry entry;
    public Button buttonSalvar;
    private Button buttonCancelar;

    // Propriedade para armazenar a madeira selecionada
    private string madeiraSelecionada;

    // Construtor original (manter para compatibilidade)
    public EditarEstoque()
    {
        InitializeComponent();
        CriarTela();
    }

    // Construtor que recebe o texto da madeira selecionada
    public EditarEstoque(string textoMadeira)
    {
        InitializeComponent();
        madeiraSelecionada = textoMadeira;
        CriarTela();
    }

    private void CriarTela()
    {
        // Criar os controles
        pickerMadeiras = new Picker
        {
            Title = madeiraSelecionada ?? "Selecione a madeira",
            TitleColor= Colors.Black,
            ItemsSource = madeiras,
            HorizontalOptions = LayoutOptions.Fill,
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.Black,
            FontSize = 16,
            Margin = new Thickness(0, 10),
            WidthRequest=330,
            InputTransparent = true
        };

        pickerTipos = new Picker()
        {
            Title = "Selecione o tipo",
            TitleColor = Colors.Black,
            ItemsSource = madeiras,
            HorizontalOptions = LayoutOptions.Fill,
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.Black,
            FontSize = 16,
            Margin = new Thickness(0, 10),
            WidthRequest = 330
        };

        pickerTamanhos = new Picker()
        {
            Title = "Selecione o tamanho",
            TitleColor = Colors.Black,
            ItemsSource = madeiras,
            HorizontalOptions = LayoutOptions.Fill,
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.Black,
            FontSize = 16,
            Margin = new Thickness(0, 10),
            WidthRequest = 330,
            IsEnabled = false // Desabilitado até selecionar um tipo
        };

        entry = new Entry()
        {
            Placeholder = "Digite a quantidade",
            Keyboard = Keyboard.Numeric,
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.Black,
            PlaceholderColor = Colors.Black,
            FontSize = 16,
            Margin = new Thickness(0, 10),
            HorizontalOptions = LayoutOptions.Fill,
            MaxLength = 5,
            ReturnType = ReturnType.Done,
            CursorPosition = 0,
            WidthRequest = 330
        };

        buttonSalvar = new Button()
        {
            Text = "Salvar",
            BackgroundColor = Color.FromArgb("#6BD14F"),
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            Padding = new Thickness(10, 5),
            WidthRequest = 150,
            Margin = new Thickness(0, 5, 0, 5)
        };

        buttonCancelar = new Button()
        {
            Text = "Cancelar",
            BackgroundColor = Color.FromArgb("#FF2222"),
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            Padding = new Thickness(10, 5),
            WidthRequest = 150,
            Margin = new Thickness(0, 5, 0, 5)
        };

        // Evento do botão Cancelar
        buttonCancelar.Clicked += (s, e) =>
        {
            try
            {
                Navigation.PopAsync(); // Voltar para a página anterior
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro", ex.Message, "OK");
            }
        };

        // NOVO: Evento do botão Salvar
        buttonSalvar.Clicked += async (s, e) =>
        {
            await SalvarDados();
        };

        // Configurar os dados dos pickers
        pickerMadeiras.ItemsSource = madeiras;

        // Se uma madeira específica foi passada, selecionar ela automaticamente
        if (!string.IsNullOrEmpty(madeiraSelecionada))
        {
            int indice = Array.IndexOf(madeiras, madeiraSelecionada);
            if (indice >= 0)
            {
                pickerMadeiras.SelectedIndex = indice;
            }
        }

        pickerTipos.ItemsSource = tipos;

        // Adicionar o evento ANTES de adicionar à tela
        pickerTipos.SelectedIndexChanged += OnTipoSelecionado;

        // Adicionar à tela
        Stackprincipal.Add(pickerMadeiras);
        Stackprincipal.Add(pickerTipos);
        Stackprincipal.Add(pickerTamanhos);
        Stackprincipal.Add(entry);
        Stackprincipal.Add(buttonCancelar);
        Stackprincipal.Add(buttonSalvar);
    }

    // NOVO MÉTODO: Validar e salvar os dados
    private async Task SalvarDados()
    {
        try
        {
            // Validar se todos os campos estão preenchidos
            if (pickerMadeiras.SelectedIndex == -1)
            {
                await DisplayAlert("Erro", "Por favor, selecione uma madeira.", "OK");
                return;
            }

            if (pickerTipos.SelectedIndex == -1)
            {
                await DisplayAlert("Erro", "Por favor, selecione um tipo.", "OK");
                return;
            }

            if (pickerTamanhos.SelectedIndex == -1)
            {
                await DisplayAlert("Erro", "Por favor, selecione um tamanho.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(entry.Text))
            {
                await DisplayAlert("Erro", "Por favor, digite uma quantidade.", "OK");
                return;
            }

            // Validar se a quantidade é um número válido
            if (!int.TryParse(entry.Text, out int quantidade) || quantidade < 0)
            {
                await DisplayAlert("Erro", "Por favor, digite uma quantidade válida (número inteiro positivo).", "OK");
                return;
            }

            // Obter os valores selecionados
            string madeiraSelecionada = (string)pickerMadeiras.SelectedItem;
            string tipoSelecionado = (string)pickerTipos.SelectedItem;
            string tamanhoSelecionado = (string)pickerTamanhos.SelectedItem;

            // Salvar os dados usando a classe DadosEstoque
            DadosEstoque.SalvarQuantidade(madeiraSelecionada, tipoSelecionado, tamanhoSelecionado, quantidade);

            // Mostrar confirmação
            await DisplayAlert("Sucesso", $"Dados salvos com sucesso!\n\n" +
                              $"Madeira: {madeiraSelecionada}\n" +
                              $"Tipo: {tipoSelecionado}\n" +
                              $"Tamanho: {tamanhoSelecionado}\n" +
                              $"Quantidade: {quantidade}", "OK");

            // Voltar para a página anterior
            await Navigation.PushAsync(new Estoque());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao salvar os dados: {ex.Message}", "OK");
        }
    }

    // Evento que é chamado quando o usuário seleciona um tipo
    private void OnTipoSelecionado(object sender, EventArgs e)
    {
        var picker = (Picker)sender;

        if (picker.SelectedIndex != -1)
        {
            string opcaoSelecionada = (string)picker.SelectedItem;

            // Configurar o picker de tamanhos baseado no tipo selecionado
            if (opcaoSelecionada == "Viga")
            {
                pickerTamanhos.ItemsSource = tamanhos_viga;
                pickerTamanhos.IsEnabled = true;
                pickerTamanhos.Title = "Selecione o tamanho da viga";
            }
            else if (opcaoSelecionada == "Ripa")
            {
                pickerTamanhos.ItemsSource = tamanhos_ripa;
                pickerTamanhos.IsEnabled = true;
                pickerTamanhos.Title = "Selecione o tamanho da ripa";
            }
            else if (opcaoSelecionada == "Tábua")
            {
                pickerTamanhos.ItemsSource = tamanhos_tabua;
                pickerTamanhos.IsEnabled = true;
                pickerTamanhos.Title = "Selecione o tamanho da tábua";
            }

            // Limpar seleção anterior do picker de tamanhos
            pickerTamanhos.SelectedIndex = -1;
        }
        else
        {
            // Se nenhum tipo está selecionado, desabilitar picker de tamanhos
            pickerTamanhos.ItemsSource = null;
            pickerTamanhos.IsEnabled = false;
            pickerTamanhos.Title = "Primeiro selecione o tipo";
        }
    }

    

}
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

    public EditarEstoque()
    {
        InitializeComponent();
        CriarTela();
    }

    private void CriarTela()
    {
        // Criar os controles
        pickerMadeiras = new Picker()
        {
            Title = ,
            WidthRequest = 330,
            BackgroundColor = Color.FromArgb("#efd4ac")
        };

        pickerTipos = new Picker()
        {
            Title = "Selecione o tipo",
            WidthRequest = 330,
            BackgroundColor = Color.FromArgb("#efd4ac")
        };

        pickerTamanhos = new Picker()
        {
            Title = "Selecione o tamanho",
            WidthRequest = 330,
            BackgroundColor = Color.FromArgb("#efd4ac"),
            IsEnabled = false // Desabilitado até selecionar um tipo
        };

        entry = new Entry()
        {
            Placeholder = "Digite a quantidade",
            Keyboard = Keyboard.Numeric,
            PlaceholderColor = Color.FromArgb("000"),
            BackgroundColor = Color.FromArgb("#efd4ac"),
            WidthRequest = 330,
        };

        buttonSalvar = new Button()
        {
            Text = "Salvar",
            BackgroundColor = Color.FromArgb("#6BD14F"), // Cor vermelha para indicar remoção
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            Padding = new Thickness(10, 5),
            WidthRequest = 150,
            Margin = new Thickness(0, 5, 0, 5)
        };

        buttonCancelar = new Button()
        {
            Text = "Cancelar",
            BackgroundColor = Color.FromArgb("#FF2222"), // Cor vermelha para indicar remoção
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            Padding = new Thickness(10, 5),
            WidthRequest = 150,
            Margin = new Thickness(0, 5, 0, 5)
        };

        buttonCancelar.Clicked += (s, e) =>
        {
            try
            {
                Navigation.PushAsync(new Estoque());
            }
            catch (Exception ex)
            {
                DisplayAlert("Não encontrado", ex.Message, "OK");
            }
        };

        // Configurar os dados dos pickers
        pickerMadeiras.ItemsSource = madeiras;
        pickerTipos.ItemsSource = tipos;

        // IMPORTANTE: Adicionar o evento ANTES de adicionar à tela
        pickerTipos.SelectedIndexChanged += OnTipoSelecionado;

        // Adicionar à tela
        Stackprincipal.Add(pickerMadeiras);
        Stackprincipal.Add(pickerTipos);
        Stackprincipal.Add(pickerTamanhos);
        Stackprincipal.Add(entry);
        Stackprincipal.Add(buttonCancelar);
        Stackprincipal.Add(buttonSalvar);
    }

    // Evento que é chamado quando o usuário seleciona um tipo
    private void OnTipoSelecionado(object sender, EventArgs e)
    {
        var picker = (Picker)sender;

        if (picker.SelectedIndex != -1)
        {
            string opcaoSelecionada = (string)picker.SelectedItem;

            // Aqui sim funciona o if, porque o usuário já selecionou algo
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
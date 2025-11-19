using MauiAppArvoredo.Models;

namespace MauiAppArvoredo.Views
{
    public partial class DetalhesVenda : ContentPage
    {
        public DetalhesVenda(Venda venda)
        {
            InitializeComponent();
            BindingContext = venda;
        }

        private async void OnVoltarClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}

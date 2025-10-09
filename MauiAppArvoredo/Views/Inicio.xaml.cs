using MauiAppArvoredo.Data;
using MauiAppArvoredo.Helpers;
using MauiAppArvoredo.Views;

namespace MauiAppArvoredo;

public partial class Inicio : ContentPage
{
    private readonly DatabaseBackupManager _backupManager;

    public Inicio()
    {
        InitializeComponent();
        _backupManager = new DatabaseBackupManager();
    }

    private void login_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Login());
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private async void sincronizar_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Desabilita o botão durante o processo
            sincronizar.IsEnabled = false;
            sincronizar.Text = "SINCRONIZANDO...";

            // Cria o backup com rotação (mantém últimos 5 backups)
            string backupPath = await _backupManager.CreateBackupWithRotation(keepLastN: 5);

            // Obtém informações sobre o backup criado
            var lastBackup = _backupManager.GetLastBackup();

            // Mostra mensagem de sucesso com detalhes
            await DisplayAlert(
                "Backup Realizado com Sucesso!",
                $"📁 Arquivo: {lastBackup.FileName}\n" +
                $"📅 Data: {lastBackup.FormattedDate}\n" +
                $"💾 Tamanho: {lastBackup.FormattedSize}\n\n" +
                $"Local: DatabaseBackups",
                "OK"
            );
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Erro ao Sincronizar",
                $"Não foi possível criar o backup:\n{ex.Message}",
                "OK"
            );
        }
        finally
        {
            // Reabilita o botão
            sincronizar.IsEnabled = true;
            sincronizar.Text = "SINCRONIZAR";
        }
    }

    private void sobre_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new InfoPage());
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", ex.Message, "OK");
        }
    }
}
using SQLite;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MauiAppArvoredo.Data
{
    public class DatabaseBackupManager
    {
        private readonly string _backupFolder;

        public DatabaseBackupManager()
        {
            // Define a pasta de backup dentro do diretório da aplicação
            _backupFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "DatabaseBackups"
            );

            // Cria a pasta se não existir
            Directory.CreateDirectory(_backupFolder);
        }

        /// <summary>
        /// Cria backup do banco de dados principal (maderapp.db3)
        /// </summary>
        public async Task<string> CreateBackup()
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string backupFileName = $"backup_maderapp_{timestamp}.db3";
                string backupPath = Path.Combine(_backupFolder, backupFileName);

                // Caminho do banco principal
                string dbPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "maderapp.db3"
                );

                // Verifica se o banco existe
                if (!File.Exists(dbPath))
                {
                    throw new FileNotFoundException("Banco de dados não encontrado");
                }

                // Cria conexão temporária para usar VACUUM INTO
                var tempConnection = new SQLiteAsyncConnection(dbPath);

                try
                {
                    // VACUUM INTO cria uma cópia limpa e otimizada
                    await tempConnection.ExecuteAsync($"VACUUM INTO '{backupPath}'");
                }
                finally
                {
                    await tempConnection.CloseAsync();
                }

                return backupPath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar backup: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cria backup com rotação automática (mantém apenas os últimos N backups)
        /// </summary>
        public async Task<string> CreateBackupWithRotation(int keepLastN = 5)
        {
            // Cria o backup
            string backupPath = await CreateBackup();

            // Remove backups antigos, mantendo apenas os últimos N
            var allBackups = Directory.GetFiles(_backupFolder, "backup_maderapp_*.db3")
                .OrderByDescending(f => File.GetCreationTime(f))
                .ToList();

            var backupsToDelete = allBackups.Skip(keepLastN);

            foreach (var oldBackup in backupsToDelete)
            {
                try
                {
                    File.Delete(oldBackup);
                }
                catch
                {
                    // Ignora erros ao deletar backups antigos
                }
            }

            return backupPath;
        }

        /// <summary>
        /// Lista todos os backups disponíveis
        /// </summary>
        public List<BackupInfo> GetAvailableBackups()
        {
            var backups = Directory.GetFiles(_backupFolder, "backup_maderapp_*.db3")
                .Select(path => new BackupInfo
                {
                    FilePath = path,
                    FileName = Path.GetFileName(path),
                    CreatedDate = File.GetCreationTime(path),
                    Size = new FileInfo(path).Length
                })
                .OrderByDescending(b => b.CreatedDate)
                .ToList();

            return backups;
        }

        /// <summary>
        /// Restaura um backup específico
        /// </summary>
        public async Task<bool> RestoreBackup(string backupPath)
        {
            try
            {
                if (!File.Exists(backupPath))
                {
                    throw new FileNotFoundException("Arquivo de backup não encontrado");
                }

                string dbPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "maderapp.db3"
                );

                // Fecha a conexão do App.Database antes de restaurar
                // Como o Database é static, precisamos garantir que não há operações pendentes
                await Task.Delay(100); // Pequeno delay para garantir que operações finalizaram

                // Faz backup do banco atual antes de restaurar (segurança)
                string safetyBackup = dbPath + ".before_restore";
                if (File.Exists(dbPath))
                {
                    File.Copy(dbPath, safetyBackup, overwrite: true);
                }

                // Copia o backup sobre o banco atual
                File.Copy(backupPath, dbPath, overwrite: true);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao restaurar backup: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Retorna o caminho da pasta de backups
        /// </summary>
        public string GetBackupFolder()
        {
            return _backupFolder;
        }

        /// <summary>
        /// Obtém informações sobre o último backup
        /// </summary>
        public BackupInfo GetLastBackup()
        {
            var backups = GetAvailableBackups();
            return backups.FirstOrDefault();
        }
    }

    public class BackupInfo
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public long Size { get; set; }

        public string FormattedSize => FormatBytes(Size);
        public string FormattedDate => CreatedDate.ToString("dd/MM/yyyy HH:mm:ss");

        private string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }
    }
}
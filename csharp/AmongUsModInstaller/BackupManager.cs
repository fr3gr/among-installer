using System;
using System.IO;
using System.IO.Compression;

namespace AmongUsModInstaller
{
    public static class BackupManager
    {
        private static readonly string BackupFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "AmongUsModInstaller",
            "Backups"
        );

        /// <summary>
        /// Tworzy backup folderu gry przed instalacją modów
        /// </summary>
        /// <param name="gamePath">Ścieżka do folderu gry</param>
        /// <returns>Ścieżka do pliku backup lub null jeśli błąd</returns>
        public static string? CreateBackup(string gamePath)
        {
            try
            {
                // Utwórz folder na backupy jeśli nie istnieje
                Directory.CreateDirectory(BackupFolder);

                // Nazwa pliku backup z timestampem
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string backupFileName = $"AmongUs_Backup_{timestamp}.zip";
                string backupPath = Path.Combine(BackupFolder, backupFileName);

                // Lista plików/folderów do zbackupowania
                string[] itemsToBackup = new[]
                {
                    "BepInEx",
                    "dotnet",
                    "winhttp.dll",
                    "doorstop_config.ini",
                    ".doorstop_version",
                    "steam_appid.txt",
                    "changelog.txt",
                    "EpicGamesStarter.exe"
                };

                // Sprawdź czy są jakieś pliki do backupu
                bool hasFilesToBackup = false;
                foreach (var item in itemsToBackup)
                {
                    string fullPath = Path.Combine(gamePath, item);
                    if (File.Exists(fullPath) || Directory.Exists(fullPath))
                    {
                        hasFilesToBackup = true;
                        break;
                    }
                }

                if (!hasFilesToBackup)
                {
                    // Brak modów do zbackupowania
                    return null;
                }

                // Utwórz archiwum ZIP
                using (ZipArchive archive = ZipFile.Open(backupPath, ZipArchiveMode.Create))
                {
                    foreach (var item in itemsToBackup)
                    {
                        string fullPath = Path.Combine(gamePath, item);

                        if (File.Exists(fullPath))
                        {
                            archive.CreateEntryFromFile(fullPath, item);
                        }
                        else if (Directory.Exists(fullPath))
                        {
                            AddDirectoryToArchive(archive, fullPath, item);
                        }
                    }
                }

                return backupPath;
            }
            catch (Exception ex)
            {
                Logger.Log($"Błąd podczas tworzenia backupu: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Przywraca grę z backupu
        /// </summary>
        /// <param name="backupPath">Ścieżka do pliku backup</param>
        /// <param name="gamePath">Ścieżka do folderu gry</param>
        /// <returns>True jeśli sukces</returns>
        public static bool RestoreBackup(string backupPath, string gamePath)
        {
            try
            {
                if (!File.Exists(backupPath))
                {
                    Logger.Log($"Plik backup nie istnieje: {backupPath}");
                    return false;
                }

                // Najpierw usuń istniejące mody
                UninstallMods(gamePath);

                // Wypakuj backup
                ZipFile.ExtractToDirectory(backupPath, gamePath, overwriteFiles: true);

                Logger.Log($"Pomyślnie przywrócono backup: {backupPath}");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log($"Błąd podczas przywracania backupu: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Usuwa zainstalowane mody z gry
        /// </summary>
        /// <param name="gamePath">Ścieżka do folderu gry</param>
        /// <returns>True jeśli sukces</returns>
        public static bool UninstallMods(string gamePath)
        {
            try
            {
                string[] itemsToDelete = new[]
                {
                    "BepInEx",
                    "dotnet",
                    "winhttp.dll",
                    "doorstop_config.ini",
                    ".doorstop_version",
                    "changelog.txt",
                    "EpicGamesStarter.exe"
                };

                bool anyDeleted = false;

                foreach (var item in itemsToDelete)
                {
                    string fullPath = Path.Combine(gamePath, item);

                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                        Logger.Log($"Usunięto plik: {item}");
                        anyDeleted = true;
                    }
                    else if (Directory.Exists(fullPath))
                    {
                        Directory.Delete(fullPath, recursive: true);
                        Logger.Log($"Usunięto folder: {item}");
                        anyDeleted = true;
                    }
                }

                if (anyDeleted)
                {
                    Logger.Log("Mody zostały odinstalowane");
                }
                else
                {
                    Logger.Log("Nie znaleziono modów do odinstalowania");
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Log($"Błąd podczas odinstalowywania modów: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Pobiera listę dostępnych backupów
        /// </summary>
        /// <returns>Tablica ścieżek do plików backup</returns>
        public static string[] GetAvailableBackups()
        {
            try
            {
                if (!Directory.Exists(BackupFolder))
                {
                    return Array.Empty<string>();
                }

                return Directory.GetFiles(BackupFolder, "AmongUs_Backup_*.zip");
            }
            catch
            {
                return Array.Empty<string>();
            }
        }

        /// <summary>
        /// Pomocnicza funkcja do dodawania folderów do archiwum ZIP
        /// </summary>
        private static void AddDirectoryToArchive(ZipArchive archive, string sourcePath, string entryName)
        {
            string[] files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string relativePath = Path.GetRelativePath(sourcePath, file);
                string entryPath = Path.Combine(entryName, relativePath).Replace('\\', '/');
                archive.CreateEntryFromFile(file, entryPath);
            }
        }

        /// <summary>
        /// Sprawdza czy gra ma zainstalowane mody
        /// </summary>
        /// <param name="gamePath">Ścieżka do folderu gry</param>
        /// <returns>True jeśli mody są zainstalowane</returns>
        public static bool HasModsInstalled(string gamePath)
        {
            string bepInExPath = Path.Combine(gamePath, "BepInEx");
            return Directory.Exists(bepInExPath);
        }
    }
}

using System.IO;
using System.IO.Compression;
using System.Windows;
using Microsoft.Win32;
using System.Diagnostics;
using System.Linq;

namespace AmongUsModInstaller;

public partial class MainWindow : Window
{
    private string? gamePath;
    private string? gameVersion;
    private List<(string path, string version)> foundGames = new();

    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        // Wyczyść stare logi
        Logger.CleanOldLogs();
        Logger.Log("=== Aplikacja uruchomiona ===");
        
        // Sprawdź aktualizacje
        CheckForUpdates();
        
        await Task.Run(() => FindGame());
    }
    
    private async void CheckForUpdates()
    {
        try
        {
            var result = await Updater.CheckForUpdates();
            if (result.HasValue && result.Value.hasUpdate)
            {
                var answer = MessageBox.Show(
                    $"Dostępna jest nowa wersja: {result.Value.latestVersion}\nObecna wersja: {Updater.GetCurrentVersion()}\n\nCzy chcesz zaktualizować teraz?",
                    "Aktualizacja dostępna",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information);
                
                if (answer == MessageBoxResult.Yes)
                {
                    var success = await Updater.DownloadAndInstallUpdate(
                        result.Value.downloadUrl,
                        status => Dispatcher.Invoke(() => StatusText.Text = status));
                    
                    if (!success)
                    {
                        MessageBox.Show("Nie udało się zainstalować aktualizacji.", "Błąd", 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        catch { }
    }

    private void FindGame()
    {
        Dispatcher.Invoke(() => StatusText.Text = "Szukanie gry...");

        foundGames.Clear();

        // Sprawdź wszystkie ścieżki Steam
        FindAllSteamInstallations();
        
        // Sprawdź typowe lokalizacje Epic/MSStore
        string[] commonPaths = [
            @"C:\Program Files\Epic Games\AmongUs",
            @"D:\Epic Games\AmongUs",
            @"E:\Epic Games\AmongUs",
            @"C:\Program Files\WindowsApps"
        ];

        foreach (var path in commonPaths)
        {
            if (VerifyGameFolder(path))
            {
                var version = DetectGameVersion(path);
                if (!foundGames.Any(g => g.path == path))
                {
                    foundGames.Add((path, version));
                }
            }
        }

        // Jeśli znaleziono wiele instalacji, pozwól wybrać
        if (foundGames.Count > 1)
        {
            Dispatcher.Invoke(() => ShowGameSelectionDialog());
        }
        else if (foundGames.Count == 1)
        {
            gamePath = foundGames[0].path;
            gameVersion = foundGames[0].version;
            Dispatcher.Invoke(() => UpdateUIWithFoundGame());
        }
        else
        {
            Dispatcher.Invoke(() =>
            {
                StatusText.Text = "✗ Nie znaleziono gry";
                StatusText.Foreground = System.Windows.Media.Brushes.Red;
                PathText.Text = "Użyj przycisku poniżej aby wybrać folder ręcznie";
            });
        }
    }

    private void FindAllSteamInstallations()
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Valve\Steam") 
                         ?? Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Valve\Steam")
                         ?? Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam");
            
            if (key != null)
            {
                var installPath = key.GetValue("InstallPath") as string;
                if (installPath != null)
                {
                    // Sprawdź główną lokalizację Steam
                    var amongUsPath = Path.Combine(installPath, "steamapps", "common", "Among Us");
                    if (VerifyGameFolder(amongUsPath))
                    {
                        foundGames.Add((amongUsPath, "steam"));
                    }
                    
                    // Sprawdź dodatkowe biblioteki Steam
                    var libraryFolders = Path.Combine(installPath, "steamapps", "libraryfolders.vdf");
                    if (File.Exists(libraryFolders))
                    {
                        var lines = File.ReadAllLines(libraryFolders);
                        foreach (var line in lines)
                        {
                            if (line.Contains("\"path\""))
                            {
                                var pathMatch = System.Text.RegularExpressions.Regex.Match(line, "\"path\"\\s+\"(.+?)\"");
                                if (pathMatch.Success)
                                {
                                    var libraryPath = pathMatch.Groups[1].Value.Replace("\\\\", "\\");
                                    var libraryAmongUs = Path.Combine(libraryPath, "steamapps", "common", "Among Us");
                                    if (VerifyGameFolder(libraryAmongUs) && !foundGames.Any(g => g.path == libraryAmongUs))
                                    {
                                        foundGames.Add((libraryAmongUs, "steam"));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
    }

    private void ShowGameSelectionDialog()
    {
        var message = "Znaleziono wiele instalacji Among Us:\n\n";
        for (int i = 0; i < foundGames.Count; i++)
        {
            var (path, version) = foundGames[i];
            var versionName = version switch
            {
                "steam" => "Steam / Itch.io",
                "epic" => "Epic Games",
                "msstore" => "Microsoft Store",
                _ => version
            };
            message += $"{i + 1}. {versionName}\n   {path}\n\n";
        }
        message += "Wybierz numer instalacji (1-" + foundGames.Count + "):";

        var input = Microsoft.VisualBasic.Interaction.InputBox(message, "Wybierz instalację", "1");
        
        if (int.TryParse(input, out int choice) && choice >= 1 && choice <= foundGames.Count)
        {
            gamePath = foundGames[choice - 1].path;
            gameVersion = foundGames[choice - 1].version;
            UpdateUIWithFoundGame();
        }
        else
        {
            // Jeśli anulowano lub zły wybór, użyj pierwszej
            gamePath = foundGames[0].path;
            gameVersion = foundGames[0].version;
            UpdateUIWithFoundGame();
        }
    }

    private void UpdateUIWithFoundGame()
    {
        StatusText.Text = foundGames.Count > 1 
            ? $"✓ Znaleziono {foundGames.Count} instalacje - wybrano:" 
            : "✓ Gra znaleziona!";
        StatusText.Foreground = System.Windows.Media.Brushes.Green;
        VersionText.Text = gameVersion switch
        {
            "steam" => "Wersja: Steam / Itch.io",
            "epic" => "Wersja: Epic Games",
            "msstore" => "Wersja: Microsoft Store",
            _ => $"Wersja: {gameVersion}"
        };
        PathText.Text = gamePath;
        AutoInstallBtn.IsEnabled = true;
        ManualInstallBtn.IsEnabled = true;
        
        // Włącz przyciski backup/uninstall jeśli są mody
        if (!string.IsNullOrEmpty(gamePath))
        {
            UninstallBtn.IsEnabled = BackupManager.HasModsInstalled(gamePath);
            RestoreBtn.IsEnabled = BackupManager.GetAvailableBackups().Length > 0;
        }
    }

    private bool VerifyGameFolder(string path)
    {
        return File.Exists(Path.Combine(path, "Among Us.exe")) || 
               File.Exists(Path.Combine(path, "GameAssembly.dll"));
    }

    private string DetectGameVersion(string path)
    {
        var pathLower = path.ToLower();
        
        if (pathLower.Contains("steam")) return "steam";
        if (pathLower.Contains("epic")) return "epic";
        if (pathLower.Contains("windowsapps") || pathLower.Contains("microsoft")) return "msstore";
        
        if (File.Exists(Path.Combine(path, "steam_api.dll")) || 
            File.Exists(Path.Combine(path, "steam_api64.dll")))
            return "steam";
        
        if (File.Exists(Path.Combine(path, ".egstore")))
            return "epic";
        
        return "steam";
    }

    private async void AutoInstallBtn_Click(object sender, RoutedEventArgs e)
    {
        var currentDir = AppDomain.CurrentDomain.BaseDirectory;
        var modFolders = Directory.GetDirectories(currentDir)
            .Where(d => !Path.GetFileName(d).StartsWith(".") && 
                       !Path.GetFileName(d).StartsWith("_"))
            .ToList();

        if (modFolders.Count == 0)
        {
            MessageBox.Show("Nie znaleziono modów w folderze aplikacji!", "Błąd", 
                          MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // Auto-wybór based on version
        var selectedMod = modFolders.FirstOrDefault(m =>
        {
            var name = Path.GetFileName(m).ToLower();
            return gameVersion switch
            {
                "steam" => name.Contains("steam") || name.Contains("itch") || name.Contains("x86"),
                "epic" or "msstore" => name.Contains("epic") || name.Contains("msstore") || name.Contains("x64"),
                _ => false
            };
        }) ?? modFolders[0];

        MessageBox.Show($"Instaluję mod: {Path.GetFileName(selectedMod)}", "Rozpoczęcie instalacji", 
                       MessageBoxButton.OK, MessageBoxImage.Information);

        AutoInstallBtn.IsEnabled = false;
        ManualInstallBtn.IsEnabled = false;

        await Task.Run(() => InstallMod(selectedMod));
    }

    private void ManualInstallBtn_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "ZIP files (*.zip)|*.zip|All files (*.*)|*.*",
            Title = "Wybierz plik ZIP z modem"
        };

        if (dialog.ShowDialog() == true)
        {
            AutoInstallBtn.IsEnabled = false;
            ManualInstallBtn.IsEnabled = false;
            Task.Run(() => InstallModFromZip(dialog.FileName));
        }
    }

    private void SelectFolderBtn_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFolderDialog
        {
            Title = "Wybierz folder Among Us"
        };

        if (dialog.ShowDialog() == true)
        {
            var path = dialog.FolderName;
            if (VerifyGameFolder(path))
            {
                gamePath = path;
                gameVersion = DetectGameVersion(path);
                
                StatusText.Text = "✓ Gra znaleziona!";
                StatusText.Foreground = System.Windows.Media.Brushes.Green;
                VersionText.Text = gameVersion switch
                {
                    "steam" => "Wersja: Steam / Itch.io",
                    "epic" => "Wersja: Epic Games",
                    "msstore" => "Wersja: Microsoft Store",
                    _ => $"Wersja: {gameVersion}"
                };
                PathText.Text = gamePath;
                AutoInstallBtn.IsEnabled = true;
                ManualInstallBtn.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Wybrany folder nie zawiera gry Among Us!", "Błąd", 
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void InstallMod(string modFolder)
    {
        try
        {
            Logger.Log($"Rozpoczęcie instalacji moda z: {modFolder}");
            
            // Utwórz backup przed instalacją
            UpdateProgress("Tworzenie backupu...", 10);
            string? backupPath = BackupManager.CreateBackup(gamePath!);
            if (backupPath != null)
            {
                Logger.Log($"Utworzono backup: {backupPath}");
            }
            else
            {
                Logger.Log("Brak plików do backupu (czysta instalacja)");
            }
            
            UpdateProgress("Kopiowanie plików...", 20);
            
            var files = Directory.GetFiles(modFolder, "*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var relativePath = Path.GetRelativePath(modFolder, file);
                var destFile = Path.Combine(gamePath!, relativePath);
                
                Directory.CreateDirectory(Path.GetDirectoryName(destFile)!);
                File.Copy(file, destFile, true);
                
                UpdateProgress($"Kopiowanie: {relativePath}", 20 + (i * 70 / files.Length));
            }
            
            UpdateProgress("Instalacja zakończona!", 100);
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show("Mod został pomyślnie zainstalowany!", "Sukces", 
                              MessageBoxButton.OK, MessageBoxImage.Information);
                AutoInstallBtn.IsEnabled = true;
                ManualInstallBtn.IsEnabled = true;
                ProgressBar.Value = 0;
                ProgressText.Text = "";
            });
        }
        catch (Exception ex)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show($"Błąd: {ex.Message}", "Błąd instalacji", 
                              MessageBoxButton.OK, MessageBoxImage.Error);
                AutoInstallBtn.IsEnabled = true;
                ManualInstallBtn.IsEnabled = true;
            });
        }
    }

    private void InstallModFromZip(string zipPath)
    {
        try
        {
            UpdateProgress("Rozpakowywanie...", 20);
            
            using var archive = ZipFile.OpenRead(zipPath);
            var entries = archive.Entries.ToList();
            
            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                if (string.IsNullOrEmpty(entry.Name)) continue;
                
                var destPath = Path.Combine(gamePath!, entry.FullName);
                Directory.CreateDirectory(Path.GetDirectoryName(destPath)!);
                entry.ExtractToFile(destPath, true);
                
                UpdateProgress($"Rozpakowywanie: {entry.Name}", 20 + (i * 70 / entries.Count));
            }
            
            UpdateProgress("Instalacja zakończona!", 100);
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show("Mod został pomyślnie zainstalowany!", "Sukces", 
                              MessageBoxButton.OK, MessageBoxImage.Information);
                AutoInstallBtn.IsEnabled = true;
                ManualInstallBtn.IsEnabled = true;
                ProgressBar.Value = 0;
                ProgressText.Text = "";
            });
        }
        catch (Exception ex)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show($"Błąd: {ex.Message}", "Błąd instalacji", 
                              MessageBoxButton.OK, MessageBoxImage.Error);
                AutoInstallBtn.IsEnabled = true;
                ManualInstallBtn.IsEnabled = true;
            });
        }
    }

    private void UpdateProgress(string message, int percent)
    {
        Dispatcher.Invoke(() =>
        {
            ProgressText.Text = message;
            ProgressBar.Value = percent;
        });
    }

    private async void UninstallBtn_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(gamePath))
        {
            MessageBox.Show("Nie wykryto ścieżki gry!", "Błąd", 
                          MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (!BackupManager.HasModsInstalled(gamePath))
        {
            MessageBox.Show("Brak zainstalowanych modów do usunięcia.", "Informacja", 
                          MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var result = MessageBox.Show(
            "Czy na pewno chcesz odinstalować mody?\n\nUtworzony zostanie backup przed usunięciem.",
            "Potwierdzenie",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes) return;

        UninstallBtn.IsEnabled = false;
        Logger.Log("Rozpoczęcie odinstalowywania modów");

        await Task.Run(() =>
        {
            UpdateProgress("Tworzenie backupu...", 30);
            BackupManager.CreateBackup(gamePath);

            UpdateProgress("Usuwanie modów...", 60);
            bool success = BackupManager.UninstallMods(gamePath);

            Dispatcher.Invoke(() =>
            {
                if (success)
                {
                    MessageBox.Show("Mody zostały pomyślnie odinstalowane!", "Sukces", 
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    Logger.Log("Mody odinstalowane pomyślnie");
                }
                else
                {
                    MessageBox.Show("Wystąpił problem podczas odinstalowywania.", "Błąd", 
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }

                UninstallBtn.IsEnabled = true;
                ProgressBar.Value = 0;
                ProgressText.Text = "";
            });
        });
    }

    private async void RestoreBtn_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(gamePath))
        {
            MessageBox.Show("Nie wykryto ścieżki gry!", "Błąd", 
                          MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        string[] backups = BackupManager.GetAvailableBackups();
        if (backups.Length == 0)
        {
            MessageBox.Show("Brak dostępnych backupów.", "Informacja", 
                          MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        // Pokazanie listy backupów do wyboru
        string backupList = "Dostępne backupy:\n\n";
        for (int i = 0; i < backups.Length; i++)
        {
            string fileName = Path.GetFileName(backups[i]);
            backupList += $"{i + 1}. {fileName}\n";
        }
        backupList += "\nWpisz numer backupu do przywrócenia:";

        string? input = Microsoft.VisualBasic.Interaction.InputBox(
            backupList,
            "Wybierz backup",
            "1");

        if (string.IsNullOrEmpty(input)) return;

        if (!int.TryParse(input, out int choice) || choice < 1 || choice > backups.Length)
        {
            MessageBox.Show("Nieprawidłowy wybór!", "Błąd", 
                          MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        string selectedBackup = backups[choice - 1];
        var result = MessageBox.Show(
            $"Przywrócić backup:\n{Path.GetFileName(selectedBackup)}?",
            "Potwierdzenie",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes) return;

        RestoreBtn.IsEnabled = false;
        Logger.Log($"Rozpoczęcie przywracania backupu: {selectedBackup}");

        await Task.Run(() =>
        {
            UpdateProgress("Przywracanie backupu...", 50);
            bool success = BackupManager.RestoreBackup(selectedBackup, gamePath);

            Dispatcher.Invoke(() =>
            {
                if (success)
                {
                    MessageBox.Show("Backup został pomyślnie przywrócony!", "Sukces", 
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    Logger.Log("Backup przywrócony pomyślnie");
                }
                else
                {
                    MessageBox.Show("Wystąpił problem podczas przywracania.", "Błąd", 
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }

                RestoreBtn.IsEnabled = true;
                ProgressBar.Value = 0;
                ProgressText.Text = "";
            });
        });
    }
}
using System;
using System.IO;

namespace AmongUsModInstaller
{
    public static class Logger
    {
        private static readonly string LogFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "AmongUsModInstaller",
            "installer.log"
        );

        private static readonly object lockObject = new object();

        /// <summary>
        /// Zapisuje wiadomość do pliku log
        /// </summary>
        /// <param name="message">Wiadomość do zapisania</param>
        public static void Log(string message)
        {
            try
            {
                lock (lockObject)
                {
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string logMessage = $"[{timestamp}] {message}";

                    // Utwórz folder jeśli nie istnieje
                    string? logDirectory = Path.GetDirectoryName(LogFilePath);
                    if (logDirectory != null && !Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                    }

                    // Dodaj wpis do pliku log
                    File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
                }
            }
            catch
            {
                // Ignoruj błędy logowania
            }
        }

        /// <summary>
        /// Zapisuje błąd do pliku log
        /// </summary>
        /// <param name="message">Wiadomość błędu</param>
        /// <param name="ex">Wyjątek</param>
        public static void LogError(string message, Exception ex)
        {
            Log($"ERROR: {message} - {ex.Message}");
            Log($"StackTrace: {ex.StackTrace}");
        }

        /// <summary>
        /// Czyści stary log jeśli przekracza 1MB
        /// </summary>
        public static void CleanOldLogs()
        {
            try
            {
                if (File.Exists(LogFilePath))
                {
                    FileInfo fileInfo = new FileInfo(LogFilePath);
                    if (fileInfo.Length > 1024 * 1024) // 1MB
                    {
                        // Zachowaj ostatnie 100 linii
                        string[] lines = File.ReadAllLines(LogFilePath);
                        int keep = Math.Min(100, lines.Length);
                        string[] lastLines = new string[keep];
                        Array.Copy(lines, lines.Length - keep, lastLines, 0, keep);
                        File.WriteAllLines(LogFilePath, lastLines);
                        Log("Log file cleaned");
                    }
                }
            }
            catch
            {
                // Ignoruj błędy czyszczenia
            }
        }

        /// <summary>
        /// Pobiera ścieżkę do pliku log
        /// </summary>
        /// <returns>Ścieżka do installer.log</returns>
        public static string GetLogFilePath()
        {
            return LogFilePath;
        }
    }
}

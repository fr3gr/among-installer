# Among Us Mod Installer# Among Us Mod Installer# Among Us Mod Installer



Prosty i szybki installer modów do Among Us z automatycznym wykrywaniem gry.



## 🎯 FunkcjeAutomatyczny instalator modów do gry Among Us z obsługą wszystkich wersji gry.Prosty i szybki installer modów do Among Us, który automatycznie lokalizuje folder gry na Twoim komputerze i instaluje wybrany mod.



- **✨ Automatyczna instalacja** - jeden klik i gotowe!

- **🔍 Smart wykrywanie gry** - znajduje Among Us na Steam, Epic Games i Microsoft Store (nawet w niestandardowych lokalizacjach)

- **📦 Obsługa wszystkich wersji** - automatycznie wybiera odpowiedni mod dla Steam/Itch (x86) lub Epic/MSStore (x64)## Funkcje## 🎯 Funkcje

- **🔄 Auto-update** - sprawdza i instaluje nowe wersje aplikacji przy starcie

- **💾 System backupów** - automatyczne backupy przed każdą instalacją

- **🔙 Restore i Uninstall** - możliwość przywrócenia backupu lub odinstalowania modów

- **🛡️ Safety checks** - sprawdza czy gra jest zamknięta, czy jest miejsce na dysku i uprawnienia zapisu- 🔍 **Automatyczne wykrywanie gry** - znajduje Among Us na Steam, Epic Games i Microsoft Store- **🔄 Automatyczne aktualizacje** - sprawdza i instaluje nowe wersje przy starcie

- **📝 Logging** - szczegółowe logi do diagnostyki (w %LocalAppData%\AmongUsModInstaller\)

- 📦 **Obsługa wielu wersji** - automatycznie wybiera odpowiedni mod dla Steam/Itch (x86) lub Epic/MSStore (x64)- **✨ W pełni automatyczna instalacja** - wykrywa wersję gry i instaluje odpowiedni mod bez pytań!

## 📥 Pobieranie

- 🚀 **Szybka instalacja** - jeden klik i mod jest zainstalowany- **Automatyczne wykrywanie gry** - przeszukuje typowe lokalizacje (Steam, Epic Games, Microsoft Store)

Pobierz najnowszą wersję z [GitHub Releases](https://github.com/fr3gr/among-installer/releases/latest)

- 📥 **Instalacja ręczna** - możliwość instalacji własnych modów z pliku ZIP- **Wykrywanie wersji gry** - automatycznie rozpoznaje Steam/Itch (x86) vs Epic/MSStore (x64)

1. Kliknij **Releases** w prawym menu

2. Pobierz `AmongUsModInstaller-vX.X.X.zip`- 🔄 **Auto-update** - automatyczne sprawdzanie i instalowanie aktualizacji- **Obsługa Steam Registry** - sprawdza rejestr Windows aby znaleźć instalację Steam

3. Rozpakuj gdziekolwiek

4. Uruchom `AmongUsModInstaller.exe` - nie wymaga instalacji!- **Interfejs graficzny** - prosty i intuicyjny GUI z paskiem postępu



## 🚀 Jak używać## Jak używać- **Instalacja z folderu lub ZIP** - obsługuje zarówno rozpakowane mody jak i archiwa



### Szybki start (zalecane)- **Smart auto-wybór** - automatycznie znajduje i instaluje właściwy mod dla Twojej wersji



1. Uruchom `AmongUsModInstaller.exe`1. Pobierz najnowszą wersję z [Releases](https://github.com/fr3gr/among-installer/releases)- **Ręczny wybór folderu** - jeśli automatyczne wykrywanie zawiedzie, możesz wybrać folder ręcznie

2. Aplikacja automatycznie znajdzie Among Us

3. Kliknij **Automatyczna Instalacja**2. Rozpakuj ZIP do dowolnego folderu- **Pasek postępu** - śledzenie postępu pobierania i instalacji

4. Gotowe! ✨

3. Uruchom `AmongUsModInstaller.exe`- **Portable** - nie wymaga instalacji, pojedynczy plik `.exe`

### Ręczna instalacja

4. Kliknij **Auto-Install Mod**

Jeśli chcesz zainstalować własny mod:

## 📋 Wymagania

1. Kliknij **Instaluj Własny Mod (ZIP)**

2. Wybierz plik ZIP z modem### Instalacja ręczna

3. Instalator zainstaluje mod w wykrytym folderze gry

- Windows (program używa rejestru Windows i ścieżek specyficznych dla Windows)

### Inne opcje

Jeśli chcesz zainstalować własny mod:- Python 3.7+

- **Wybierz folder gry ręcznie** - jeśli automatyczne wykrywanie nie zadziałało

- **Odinstaluj mody** - usuwa wszystkie zainstalowane mody (z backupem)1. Kliknij **Manual Install (ZIP)**- Biblioteka `requests`

- **Przywróć backup** - przywraca ostatni backup sprzed instalacji

2. Wybierz plik ZIP z modem

## 📋 Wymagania

3. Instalator automatycznie rozpozna wersję gry i zainstaluje mod## � Pobieranie

- **Windows 10/11** (64-bit)

- **.NET 9.0 Runtime** - aplikacja automatycznie poprosi o pobranie jeśli nie jest zainstalowany

- **Among Us** zainstalowany na Steam, Epic Games lub Microsoft Store

## Wymagania### Dla użytkowników (zalecane):

## 🔍 Wspierane lokalizacje gry

Pobierz najnowszą wersję z [GitHub Releases](https://github.com/TwojaNazwa/among-installer/releases/latest)

Aplikacja automatycznie sprawdza:

- Windows 10/11 64-bit

### Steam

- Rejestr Windows Steam (wszystkie biblioteki gier)- .NET 9.0 Runtime (instalator automatycznie poprosi o pobranie jeśli nie jest zainstalowany)1. Przejdź do zakładki **Releases**

- `C:\Program Files (x86)\Steam\steamapps\common\Among Us`

- Dyski D-Z: `\Steam\steamapps\common\Among Us`- Among Us zainstalowany na Steam, Epic Games lub Microsoft Store2. Pobierz plik `AmongUsModInstaller-vX.X.X.exe`

- Niestandardowe lokalizacje z `libraryfolders.vdf`

3. Uruchom - gotowe! Nie wymaga instalacji.

### Epic Games Store

- `C:\Program Files\Epic Games\AmongUs`## Zawartość paczki

- Dyski D-Z: `\Epic Games\AmongUs`

### Dla deweloperów:

### Microsoft Store

- `C:\Program Files\WindowsApps\InnerSloth.AmongUs*````



## 🛡️ BezpieczeństwoAmongUsModInstaller.exe  - Główny program1. Sklonuj lub pobierz to repozytorium



Aplikacja sprawdza przed każdą instalacją:mods_steam/              - Mody dla wersji Steam/Itch (x86)2. Zainstaluj wymagane pakiety:



1. ✅ **Czy gra jest zamknięta** - chroni przed uszkodzeniem plikówmods_epic/               - Mody dla wersji Epic/MSStore (x64)

2. ✅ **Wolne miejsce na dysku** - sprawdza czy jest ~200MB

3. ✅ **Uprawnienia zapisu** - testuje czy może zapisać plikiversion.txt              - Wersja aplikacji```powershell



## 💾 System backupów```pip install -r requirements.txt



- Automatyczny backup przed każdą instalacją```

- Backupy w `%LocalAppData%\AmongUsModInstaller\Backups\`

- Funkcja **Przywróć backup** przywraca ostatni backup## Wspierane wersje Among Us

- Opcja **Odinstaluj mody** usuwa mody i tworzy backup

3. Uruchom ze źródeł:

## 🛠️ Rozwiązywanie problemów

- **Steam / Itch.io** - wersja 32-bit (x86)```powershell

### Gra nie została znaleziona

- Użyj **Wybierz folder gry ręcznie**- **Epic Games Store** - wersja 64-bit (x64)python installer.py

- Upewnij się, że Among Us jest zainstalowany

- Sprawdź czy masz uprawnienia do odczytu folderu- **Microsoft Store** - wersja 64-bit (x64)```



### "Among Us jest uruchomiony"

- Zamknij grę przed instalacją modów

- Sprawdź Task Manager czy proces się nie zawiesił## Budowanie ze źródeł## 💻 Użycie



### "Brak uprawnień"

- Uruchom instalator jako administrator (PPM → Uruchom jako administrator)

```bash### 🚀 Szybki Start (Najprostszy sposób!)

### "Za mało miejsca"

- Zwolnij ~200MB na dysku z grącd csharp/AmongUsModInstaller



### Logidotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true1. **Wrzuć mody** do tego samego folderu co `installer.py`:

- Szczegółowe logi w: `%LocalAppData%\AmongUsModInstaller\installer.log`

- Przydatne do diagnostyki problemów```   ```



## 📦 Zawartość paczki   among-installer\



```## Licencja   ├── installer.py

AmongUsModInstaller-vX.X.X.zip

├── AmongUsModInstaller.exe  - Główna aplikacja   ├── TouMira-v1.3.1-x64-epic-msstore\    (dla Epic/MSStore)

├── version.txt              - Wersja aplikacji (dla auto-update)

├── mods_steam/              - Mody dla Steam/Itch (x86)MIT License - Zobacz [LICENSE](LICENSE) dla szczegółów.   └── TouMira-v1.3.1-x86-steam-itch.zip   (dla Steam/Itch)

│   ├── BepInEx/

│   ├── dotnet/   ```

│   └── winhttp.dll

└── mods_epic/               - Mody dla Epic/MSStore (x64)## Credits

    ├── BepInEx/

    ├── dotnet/2. **Uruchom program**:

    └── winhttp.dll

```- Mody: TouMira   ```powershell



## 🔄 Aktualizacje- Installer: fr3gr   python installer.py



Aplikacja automatycznie:   ```

1. Sprawdza dostępność nowych wersji przy starcie

2. Pokazuje dialog z informacją o aktualizacji3. **Kliknij "🎯 Auto-wybór moda"** - i to wszystko!

3. Po potwierdzeniu pobiera i instaluje nową wersję   - Program sam wykryje wersję gry

4. Automatycznie restartuje się   - Sam znajdzie odpowiedni mod

   - Sam go zainstaluje

**Aktualizacje są opcjonalne** - możesz pominąć i używać aktualnej wersji.   - **Zero pytań, zero klikania!** ✨



## 🔧 Dla deweloperów### Krok po kroku (dla ciekawskich)



### Budowanie ze źródeł1. **Uruchom program** - automatycznie rozpocznie szukanie folderu Among Us

2. **Poczekaj na wykrycie gry** - program przeszuka typowe lokalizacje:

```bash   - Steam (różne dyski i foldery) - wykryje jako **Steam / Itch.io** (x86)

# Wymagania: .NET 9.0 SDK   - Epic Games Store - wykryje jako **Epic Games** (x64)

cd csharp/AmongUsModInstaller   - Microsoft Store - wykryje jako **Microsoft Store** (x64)

dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true3. **Wybierz metodę instalacji**:

```   - **🎯 Auto-wybór moda (ZALECANE)** - automatycznie znajdzie i zainstaluje odpowiedni mod

   - **Instaluj mod (folder lub ZIP)** - wybierz ręcznie folder lub ZIP

Wynik w: `bin/Release/net9.0-windows/win-x64/publish/AmongUsModInstaller.exe`   - **Instaluj mod z URL** - wprowadź link do pliku ZIP z modem

4. **Śledź postęp** - obserwuj pasek postępu instalacji

### Struktura projektu5. **Gotowe!** - mod zostanie zainstalowany w folderze gry



```### Jeśli gra nie została znaleziona

among-installer/

├── csharp/AmongUsModInstaller/Jeśli program nie znajdzie automatycznie folderu gry:

│   ├── MainWindow.xaml      - GUI (WPF)

│   ├── MainWindow.xaml.cs   - Logika główna1. Kliknij przycisk **"Wybierz folder ręcznie"**

│   ├── BackupManager.cs     - System backupów2. Wskaż folder z grą Among Us (ten, który zawiera plik `Among Us.exe`)

│   ├── Logger.cs            - Logging3. Następnie możesz instalować mody normalnie

│   ├── Updater.cs           - Auto-update

│   └── version.txt          - Wersja## 📦 Jak przygotować mod do instalacji

├── mods_steam/              - Mody (gitignored)

├── mods_epic/               - Mody (gitignored)Mod musi być spakowany jako plik ZIP i zawierać pliki w odpowiedniej strukturze. Na przykład:

└── README.md

``````

mod.zip

## 📝 Licencja├── BepInEx/

│   ├── core/

MIT License - zobacz [LICENSE](LICENSE) dla szczegółów.│   └── plugins/

└── Among Us_Data/

## ⚠️ Disclaimer    └── il2cpp_data/

```

Ten program jest niezależnym narzędziem i nie jest oficjalnie wspierany przez InnerSloth LLC. Używaj na własną odpowiedzialność.

Program rozpakuje zawartość ZIP bezpośrednio do folderu gry.

## 🤝 Wsparcie

## 🔍 Obsługiwane lokalizacje gry

Problemy? [GitHub Issues](https://github.com/fr3gr/among-installer/issues)

Program automatycznie sprawdza następujące lokalizacje:

---

### Steam

**Made with ❤️ for Among Us community**- `C:\Program Files (x86)\Steam\steamapps\common\Among Us`

- `D:\Steam\steamapps\common\Among Us`
- `E:\Steam\steamapps\common\Among Us`
- `X:\SteamLibrary\steamapps\common\Among Us`
- Ścieżka ze Steam Registry

### Epic Games
- `C:\Program Files\Epic Games\AmongUs`

### Microsoft Store
- `C:\Program Files\WindowsApps\InnerSloth.AmongUs`

## 🔄 Automatyczne aktualizacje

Aplikacja automatycznie sprawdza dostępność nowych wersji przy każdym uruchomieniu:

1. **Sprawdzenie** - łączy się z GitHub API
2. **Porównanie** - sprawdza czy dostępna jest nowsza wersja
3. **Powiadomienie** - pokazuje dialog z informacją o aktualizacji
4. **Instalacja** - po potwierdzeniu pobiera i instaluje nową wersję
5. **Restart** - automatycznie restartuje aplikację z nową wersją

**Aktualizacje są opcjonalne** - możesz kontynuować używanie aktualnej wersji.

## ⚠️ Uwagi

- **Backup**: Zalecane jest zrobienie kopii zapasowej plików gry przed instalacją modów
- **Antywirusy**: Niektóre antywirusy mogą blokować pobieranie/instalację - dodaj program do wyjątków jeśli jest taka potrzeba
- **Uprawnienia**: Program może wymagać uprawnień administratora jeśli Among Us jest zainstalowany w folderze wymagającym podwyższonych uprawnień (np. Program Files)
- **Kompatybilność modów**: Upewnij się, że mod jest kompatybilny z Twoją wersją gry
- **Połączenie internetowe**: Wymagane tylko do sprawdzania aktualizacji (opcjonalne)

## 🛠️ Rozwiązywanie problemów

### "Nie znaleziono gry"
- Użyj opcji "Wybierz folder ręcznie"
- Upewnij się, że Among Us jest zainstalowany
- Sprawdź czy masz uprawnienia do odczytu folderu gry

### "Błąd podczas instalacji"
- Sprawdź połączenie internetowe (jeśli instalujesz z URL)
- Upewnij się, że plik ZIP nie jest uszkodzony
- Sprawdź czy masz uprawnienia do zapisu w folderze gry
- Zamknij grę przed instalacją moda

### "Mod nie działa po instalacji"
- Sprawdź czy mod jest kompatybilny z Twoją wersją Among Us
- Upewnij się, że wszystkie pliki zostały rozpakowane
- Sprawdź dokumentację konkretnego moda

## 📝 Licencja

Ten projekt jest dostępny jako open source. Możesz go swobodnie modyfikować i dystrybuować.

## ⚡ Popularne mody

Kilka popularnych modów do Among Us:
- **Town of Impostors** - dodatkowe role
- **Sheriff Mod** - nowa rola Sheriff
- **Jester Mod** - rola Jester
- **Better Crewlink** - komunikacja głosowa

**Uwaga**: Ten installer nie zawiera żadnych modów - musisz pobrać je samodzielnie z zaufanych źródeł.

## 🤝 Wsparcie

Jeśli napotkasz problemy:
1. Sprawdź sekcję "Rozwiązywanie problemów" powyżej
2. Upewnij się, że spełniasz wszystkie wymagania
3. Sprawdź czy masz najnowszą wersję programu (aplikacja sprawdzi to automatycznie)
4. Zgłoś problem w [GitHub Issues](https://github.com/TwojaNazwa/among-installer/issues)

## 🛠️ Dla deweloperów

Chcesz zbudować aplikację samodzielnie lub pomóc w rozwoju?

- **Budowanie**: Zobacz [BUILD.md](BUILD.md) dla instrukcji
- **Contribute**: Pull requesty są mile widziane!
- **Issues**: Zgłaszaj bugi i sugestie

---

**Disclaimer**: Ten program jest niezależnym narzędziem i nie jest oficjalnie wspierany przez InnerSloth. Używaj na własną odpowiedzialność.

# Among Us Mod Installer# Among Us Mod Installer



Automatyczny instalator modów do gry Among Us z obsługą wszystkich wersji gry.Prosty i szybki installer modów do Among Us, który automatycznie lokalizuje folder gry na Twoim komputerze i instaluje wybrany mod.



## Funkcje## 🎯 Funkcje



- 🔍 **Automatyczne wykrywanie gry** - znajduje Among Us na Steam, Epic Games i Microsoft Store- **🔄 Automatyczne aktualizacje** - sprawdza i instaluje nowe wersje przy starcie

- 📦 **Obsługa wielu wersji** - automatycznie wybiera odpowiedni mod dla Steam/Itch (x86) lub Epic/MSStore (x64)- **✨ W pełni automatyczna instalacja** - wykrywa wersję gry i instaluje odpowiedni mod bez pytań!

- 🚀 **Szybka instalacja** - jeden klik i mod jest zainstalowany- **Automatyczne wykrywanie gry** - przeszukuje typowe lokalizacje (Steam, Epic Games, Microsoft Store)

- 📥 **Instalacja ręczna** - możliwość instalacji własnych modów z pliku ZIP- **Wykrywanie wersji gry** - automatycznie rozpoznaje Steam/Itch (x86) vs Epic/MSStore (x64)

- 🔄 **Auto-update** - automatyczne sprawdzanie i instalowanie aktualizacji- **Obsługa Steam Registry** - sprawdza rejestr Windows aby znaleźć instalację Steam

- **Interfejs graficzny** - prosty i intuicyjny GUI z paskiem postępu

## Jak używać- **Instalacja z folderu lub ZIP** - obsługuje zarówno rozpakowane mody jak i archiwa

- **Smart auto-wybór** - automatycznie znajduje i instaluje właściwy mod dla Twojej wersji

1. Pobierz najnowszą wersję z [Releases](https://github.com/fr3gr/among-installer/releases)- **Ręczny wybór folderu** - jeśli automatyczne wykrywanie zawiedzie, możesz wybrać folder ręcznie

2. Rozpakuj ZIP do dowolnego folderu- **Pasek postępu** - śledzenie postępu pobierania i instalacji

3. Uruchom `AmongUsModInstaller.exe`- **Portable** - nie wymaga instalacji, pojedynczy plik `.exe`

4. Kliknij **Auto-Install Mod**

## 📋 Wymagania

### Instalacja ręczna

- Windows (program używa rejestru Windows i ścieżek specyficznych dla Windows)

Jeśli chcesz zainstalować własny mod:- Python 3.7+

1. Kliknij **Manual Install (ZIP)**- Biblioteka `requests`

2. Wybierz plik ZIP z modem

3. Instalator automatycznie rozpozna wersję gry i zainstaluje mod## � Pobieranie



## Wymagania### Dla użytkowników (zalecane):

Pobierz najnowszą wersję z [GitHub Releases](https://github.com/TwojaNazwa/among-installer/releases/latest)

- Windows 10/11 64-bit

- .NET 9.0 Runtime (instalator automatycznie poprosi o pobranie jeśli nie jest zainstalowany)1. Przejdź do zakładki **Releases**

- Among Us zainstalowany na Steam, Epic Games lub Microsoft Store2. Pobierz plik `AmongUsModInstaller-vX.X.X.exe`

3. Uruchom - gotowe! Nie wymaga instalacji.

## Zawartość paczki

### Dla deweloperów:

```

AmongUsModInstaller.exe  - Główny program1. Sklonuj lub pobierz to repozytorium

mods_steam/              - Mody dla wersji Steam/Itch (x86)2. Zainstaluj wymagane pakiety:

mods_epic/               - Mody dla wersji Epic/MSStore (x64)

version.txt              - Wersja aplikacji```powershell

```pip install -r requirements.txt

```

## Wspierane wersje Among Us

3. Uruchom ze źródeł:

- **Steam / Itch.io** - wersja 32-bit (x86)```powershell

- **Epic Games Store** - wersja 64-bit (x64)python installer.py

- **Microsoft Store** - wersja 64-bit (x64)```



## Budowanie ze źródeł## 💻 Użycie



```bash### 🚀 Szybki Start (Najprostszy sposób!)

cd csharp/AmongUsModInstaller

dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true1. **Wrzuć mody** do tego samego folderu co `installer.py`:

```   ```

   among-installer\

## Licencja   ├── installer.py

   ├── TouMira-v1.3.1-x64-epic-msstore\    (dla Epic/MSStore)

MIT License - Zobacz [LICENSE](LICENSE) dla szczegółów.   └── TouMira-v1.3.1-x86-steam-itch.zip   (dla Steam/Itch)

   ```

## Credits

2. **Uruchom program**:

- Mody: TouMira   ```powershell

- Installer: fr3gr   python installer.py

   ```

3. **Kliknij "🎯 Auto-wybór moda"** - i to wszystko!
   - Program sam wykryje wersję gry
   - Sam znajdzie odpowiedni mod
   - Sam go zainstaluje
   - **Zero pytań, zero klikania!** ✨

### Krok po kroku (dla ciekawskich)

1. **Uruchom program** - automatycznie rozpocznie szukanie folderu Among Us
2. **Poczekaj na wykrycie gry** - program przeszuka typowe lokalizacje:
   - Steam (różne dyski i foldery) - wykryje jako **Steam / Itch.io** (x86)
   - Epic Games Store - wykryje jako **Epic Games** (x64)
   - Microsoft Store - wykryje jako **Microsoft Store** (x64)
3. **Wybierz metodę instalacji**:
   - **🎯 Auto-wybór moda (ZALECANE)** - automatycznie znajdzie i zainstaluje odpowiedni mod
   - **Instaluj mod (folder lub ZIP)** - wybierz ręcznie folder lub ZIP
   - **Instaluj mod z URL** - wprowadź link do pliku ZIP z modem
4. **Śledź postęp** - obserwuj pasek postępu instalacji
5. **Gotowe!** - mod zostanie zainstalowany w folderze gry

### Jeśli gra nie została znaleziona

Jeśli program nie znajdzie automatycznie folderu gry:

1. Kliknij przycisk **"Wybierz folder ręcznie"**
2. Wskaż folder z grą Among Us (ten, który zawiera plik `Among Us.exe`)
3. Następnie możesz instalować mody normalnie

## 📦 Jak przygotować mod do instalacji

Mod musi być spakowany jako plik ZIP i zawierać pliki w odpowiedniej strukturze. Na przykład:

```
mod.zip
├── BepInEx/
│   ├── core/
│   └── plugins/
└── Among Us_Data/
    └── il2cpp_data/
```

Program rozpakuje zawartość ZIP bezpośrednio do folderu gry.

## 🔍 Obsługiwane lokalizacje gry

Program automatycznie sprawdza następujące lokalizacje:

### Steam
- `C:\Program Files (x86)\Steam\steamapps\common\Among Us`
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

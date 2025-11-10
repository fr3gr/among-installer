# Among Us Mod Installer

Prosty i szybki installer modów do Among Us, który automatycznie lokalizuje folder gry na Twoim komputerze i instaluje wybrany mod.

## 🎯 Funkcje

- **🔄 Automatyczne aktualizacje** - sprawdza i instaluje nowe wersje przy starcie
- **✨ W pełni automatyczna instalacja** - wykrywa wersję gry i instaluje odpowiedni mod bez pytań!
- **Automatyczne wykrywanie gry** - przeszukuje typowe lokalizacje (Steam, Epic Games, Microsoft Store)
- **Wykrywanie wersji gry** - automatycznie rozpoznaje Steam/Itch (x86) vs Epic/MSStore (x64)
- **Obsługa Steam Registry** - sprawdza rejestr Windows aby znaleźć instalację Steam
- **Interfejs graficzny** - prosty i intuicyjny GUI z paskiem postępu
- **Instalacja z folderu lub ZIP** - obsługuje zarówno rozpakowane mody jak i archiwa
- **Smart auto-wybór** - automatycznie znajduje i instaluje właściwy mod dla Twojej wersji
- **Ręczny wybór folderu** - jeśli automatyczne wykrywanie zawiedzie, możesz wybrać folder ręcznie
- **Pasek postępu** - śledzenie postępu pobierania i instalacji
- **Portable** - nie wymaga instalacji, pojedynczy plik `.exe`

## 📋 Wymagania

- Windows (program używa rejestru Windows i ścieżek specyficznych dla Windows)
- Python 3.7+
- Biblioteka `requests`

## � Pobieranie

### Dla użytkowników (zalecane):
Pobierz najnowszą wersję z [GitHub Releases](https://github.com/TwojaNazwa/among-installer/releases/latest)

1. Przejdź do zakładki **Releases**
2. Pobierz plik `AmongUsModInstaller-vX.X.X.exe`
3. Uruchom - gotowe! Nie wymaga instalacji.

### Dla deweloperów:

1. Sklonuj lub pobierz to repozytorium
2. Zainstaluj wymagane pakiety:

```powershell
pip install -r requirements.txt
```

3. Uruchom ze źródeł:
```powershell
python installer.py
```

## 💻 Użycie

### 🚀 Szybki Start (Najprostszy sposób!)

1. **Wrzuć mody** do tego samego folderu co `installer.py`:
   ```
   among-installer\
   ├── installer.py
   ├── TouMira-v1.3.1-x64-epic-msstore\    (dla Epic/MSStore)
   └── TouMira-v1.3.1-x86-steam-itch.zip   (dla Steam/Itch)
   ```

2. **Uruchom program**:
   ```powershell
   python installer.py
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

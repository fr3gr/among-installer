# 🚀 Szybki Start - Publikacja na GitHub

## Krok 1: Przygotuj repozytorium

1. Utwórz nowe repo na GitHub: https://github.com/new
   - Nazwa: `among-installer` (lub inna)
   - Publiczne
   - Bez README (już masz)

2. Zaktualizuj `GITHUB_REPO` w `installer.py`:
   ```python
   GITHUB_REPO = "TwojaNazwa/among-installer"  # <- ZMIEŃ NA SWOJE
   ```

3. Push do GitHub:
   ```bash
   git init
   git add .
   git commit -m "Initial commit - Among Us Mod Installer v1.0.0"
   git branch -M main
   git remote add origin https://github.com/TwojaNazwa/among-installer.git
   git push -u origin main
   ```

## Krok 2: Zbuduj pierwszy release

### Opcja A: Automatycznie (GitHub Actions)

1. Stwórz tag i push:
   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```

2. GitHub Actions automatycznie zbuduje i opublikuje release!
   - Sprawdź: https://github.com/TwojaNazwa/among-installer/actions

### Opcja B: Ręcznie (lokalnie)

1. Zainstaluj PyInstaller:
   ```bash
   pip install -r requirements-build.txt
   ```

2. Zbuduj aplikację:
   ```bash
   python build_release.py
   ```

3. Utwórz release na GitHub:
   - Idź do: https://github.com/TwojaNazwa/among-installer/releases/new
   - Tag: `v1.0.0`
   - Tytuł: "Among Us Mod Installer v1.0.0"
   - Opis:
     ```
     ## 🎮 Pierwszy release!
     
     Funkcje:
     - Automatyczna detekcja gry
     - Instalacja modów dla Steam/Itch i Epic/MSStore
     - Auto-update system
     - Prosty interfejs
     ```
   - Przeciągnij plik `dist/AmongUsModInstaller-v1.0.0.exe`
   - **Publish release**

## Krok 3: Test auto-update

1. Pobierz release z GitHub
2. Uruchom aplikację
3. Zmień wersję na 1.0.1:
   - Zaktualizuj `version.txt`: `1.0.1`
   - Zaktualizuj `VERSION` w `installer.py`: `"1.0.1"`
4. Zbuduj nowy release (tag `v1.0.1`)
5. Uruchom poprzednią wersję (1.0.0)
6. **Powinna wykryć aktualizację do 1.0.1!** 🎉

## Krok 4: Udostępnij użytkownikom

Dodaj do README na górze:

```markdown
## 📥 Pobierz

**[⬇️ Pobierz najnowszą wersję](https://github.com/TwojaNazwa/among-installer/releases/latest/download/AmongUsModInstaller-v1.0.0.exe)**

Lub zobacz [wszystkie wersje](https://github.com/TwojaNazwa/among-installer/releases)
```

## 📋 Checklist

- [ ] Utworzone repo na GitHub
- [ ] Zaktualizowany `GITHUB_REPO` w kodzie
- [ ] Push kodu do GitHub
- [ ] Utworzony tag `v1.0.0`
- [ ] Opublikowany release z `.exe`
- [ ] Przetestowany auto-update
- [ ] Dodany link do pobrania w README

## 🎯 Następne kroki

### Aktualizacja do nowej wersji:

1. Wprowadź zmiany w kodzie
2. Zaktualizuj wersję:
   ```bash
   echo "1.0.1" > version.txt
   # Zmień VERSION w installer.py na "1.0.1"
   ```
3. Commit i push:
   ```bash
   git add .
   git commit -m "v1.0.1 - Opis zmian"
   git push
   ```
4. Utwórz tag:
   ```bash
   git tag v1.0.1
   git push origin v1.0.1
   ```
5. GitHub Actions zrobi resztę!

### Użytkownicy automatycznie dostaną powiadomienie o aktualizacji! 🚀

## 🐛 Troubleshooting

### GitHub Actions nie działa
- Sprawdź Actions w ustawieniach repo (Settings > Actions)
- Musi być włączone "Allow all actions"

### Release się nie tworzy
- Sprawdź logi w Actions
- Upewnij się że tag zaczyna się od `v`
- Sprawdź czy `version.txt` istnieje

### Auto-update nie działa
- Sprawdź `GITHUB_REPO` w kodzie
- Sprawdź czy release jest "latest" (nie draft/prerelease)
- Sprawdź połączenie internetowe

## 💡 Wskazówki

- **Zawsze testuj lokalnie** przed push
- **Używaj semantic versioning**: `MAJOR.MINOR.PATCH`
- **Opisuj zmiany** w release notes
- **Tag = wersja**: `v1.0.0` dla wersji `1.0.0`

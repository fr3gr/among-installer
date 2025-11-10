# Among Us Mod Installer - Instrukcje budowania i publikacji

## 🏗️ Budowanie aplikacji

### Wymagania
```bash
pip install -r requirements.txt
pip install -r requirements-build.txt
```

### Budowanie
```bash
python build_release.py
```

To utworzy plik `.exe` w folderze `dist/`

## 📦 Publikacja release na GitHub

### 1. Przygotuj wersję
Zaktualizuj numer wersji w `version.txt` i `installer.py`:
```
1.0.1
```

### 2. Zbuduj aplikację
```bash
python build_release.py
```

### 3. Utwórz release na GitHub

1. Idź do https://github.com/TwojaNazwa/among-installer/releases/new
2. Kliknij "Choose a tag" i wpisz `v1.0.1` (z prefiksem v!)
3. Tytuł: "Among Us Mod Installer v1.0.1"
4. Opis zmian:
   ```
   ## Zmiany w v1.0.1
   - Dodano automatyczne aktualizacje
   - Naprawiono błędy
   - Ulepszona detekcja gry
   ```
5. Przeciągnij plik `dist/AmongUsModInstaller-v1.0.1.exe` do sekcji "Attach binaries"
6. Kliknij "Publish release"

### 4. Testuj auto-update

Po opublikowaniu:
1. Uruchom starszą wersję aplikacji
2. Powinna automatycznie wykryć nową wersję
3. Zaproponuje aktualizację
4. Po potwierdzeniu pobierze i zainstaluje nową wersję

## 🔄 System Auto-Update

### Jak działa:
1. **Przy starcie** - aplikacja sprawdza GitHub API
2. **Porównuje wersje** - current vs latest release
3. **Jeśli nowsza dostępna** - pokazuje dialog z informacją
4. **Po potwierdzeniu** - pobiera `.exe` z GitHub Releases
5. **Instaluje** - zamyka aplikację i podmienia plik
6. **Restartuje** - uruchamia nową wersję

### API endpoint:
```
https://api.github.com/repos/TwojaNazwa/among-installer/releases/latest
```

### Struktura response:
```json
{
  "tag_name": "v1.0.1",
  "body": "Opis zmian...",
  "assets": [
    {
      "browser_download_url": "https://github.com/.../AmongUsModInstaller-v1.0.1.exe"
    }
  ]
}
```

## 📝 Checklist przed release:

- [ ] Zaktualizuj `version.txt`
- [ ] Zaktualizuj `VERSION` w `installer.py`
- [ ] Zaktualizuj `GITHUB_REPO` w `installer.py` na swoje repo
- [ ] Przetestuj aplikację lokalnie
- [ ] Zbuduj release: `python build_release.py`
- [ ] Utwórz tag i release na GitHub
- [ ] Wgraj `.exe` jako asset
- [ ] Opublikuj release
- [ ] Przetestuj auto-update z poprzedniej wersji

## 🎯 Pierwszy release (v1.0.0):

Jeśli to pierwszy release:
1. Zmień `GITHUB_REPO` w `installer.py` na swoje repozytorium
2. Zbuduj aplikację
3. Utwórz release na GitHub z tagiem `v1.0.0`
4. Wgraj built `.exe`
5. Opublikuj jako latest release

## ⚙️ Konfiguracja repo GitHub:

W `installer.py` zmień:
```python
GITHUB_REPO = "TwojaNazwa/among-installer"  # <- TUTAJ ZMIEŃ
```

Na przykład:
```python
GITHUB_REPO = "jankowalski/among-us-installer"
```

## 🐛 Rozwiązywanie problemów:

### "Nie można sprawdzić aktualizacji"
- Sprawdź połączenie internetowe
- Sprawdź czy `GITHUB_REPO` jest poprawne
- Sprawdź czy release istnieje na GitHub

### "Aktualizacja się nie pobiera"
- Upewnij się że `.exe` jest załączony do release
- Sprawdź czy nazwa pliku jest poprawna
- Sprawdź uprawnienia do zapisu

### "Aplikacja się nie restartuje"
- Zamknij antywirusy tymczasowo
- Uruchom jako administrator
- Sprawdź logi w konsoli

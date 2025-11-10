"""
Among Us Mod Installer
Automatycznie lokalizuje folder gry i instaluje mody
"""

import os
import sys
import winreg
import shutil
import requests
import zipfile
from pathlib import Path
from typing import Optional
import tkinter as tk
from tkinter import ttk, messagebox, filedialog
import threading
import json
import subprocess

# Wersja aplikacji
VERSION = "1.0.0"
GITHUB_REPO = "fr3gr/among-installer"  # Zmień na swoje repo
UPDATE_CHECK_URL = f"https://api.github.com/repos/{GITHUB_REPO}/releases/latest"


class AmongUsInstaller:
    """Klasa do lokalizacji i instalacji modów Among Us"""
    
    def __init__(self):
        self.game_path: Optional[Path] = None
        self.game_version: Optional[str] = None  # "steam", "epic", "msstore", "itch"
        self.common_paths = [
            # Steam
            r"C:\Program Files (x86)\Steam\steamapps\common\Among Us",
            r"D:\Steam\steamapps\common\Among Us",
            r"E:\Steam\steamapps\common\Among Us",
            # Epic Games
            r"C:\Program Files\Epic Games\AmongUs",
            # Microsoft Store
            r"C:\Program Files\WindowsApps\InnerSloth.AmongUs",
        ]
    
    def find_game_folder(self) -> Optional[Path]:
        """Szuka folderu Among Us na komputerze"""
        
        # Sprawdź ścieżkę Steam z rejestru
        steam_path = self._get_steam_path()
        if steam_path:
            among_us_path = steam_path / "steamapps" / "common" / "Among Us"
            if among_us_path.exists() and self._verify_game_folder(among_us_path):
                return among_us_path
        
        # Sprawdź typowe lokalizacje
        for path_str in self.common_paths:
            path = Path(path_str)
            if path.exists() and self._verify_game_folder(path):
                return path
        
        # Przeszukaj wszystkie dyski
        for drive in self._get_available_drives():
            paths_to_check = [
                drive / "Steam" / "steamapps" / "common" / "Among Us",
                drive / "SteamLibrary" / "steamapps" / "common" / "Among Us",
                drive / "Games" / "Steam" / "steamapps" / "common" / "Among Us",
                drive / "Program Files (x86)" / "Steam" / "steamapps" / "common" / "Among Us",
            ]
            
            for path in paths_to_check:
                if path.exists() and self._verify_game_folder(path):
                    return path
        
        return None
    
    def _get_steam_path(self) -> Optional[Path]:
        """Pobiera ścieżkę instalacji Steam z rejestru Windows"""
        try:
            key = winreg.OpenKey(winreg.HKEY_LOCAL_MACHINE, 
                                r"SOFTWARE\WOW6432Node\Valve\Steam")
            install_path, _ = winreg.QueryValueEx(key, "InstallPath")
            winreg.CloseKey(key)
            return Path(install_path)
        except:
            try:
                key = winreg.OpenKey(winreg.HKEY_LOCAL_MACHINE, 
                                    r"SOFTWARE\Valve\Steam")
                install_path, _ = winreg.QueryValueEx(key, "InstallPath")
                winreg.CloseKey(key)
                return Path(install_path)
            except:
                return None
    
    def _get_available_drives(self) -> list:
        """Zwraca listę dostępnych dysków"""
        drives = []
        for letter in 'CDEFGHIJ':
            drive = Path(f"{letter}:/")
            if drive.exists():
                drives.append(drive)
        return drives
    
    def _verify_game_folder(self, path: Path) -> bool:
        """Weryfikuje czy folder zawiera Among Us"""
        game_exe = path / "Among Us.exe"
        game_data = path / "GameAssembly.dll"
        return game_exe.exists() or game_data.exists()
    
    def detect_game_version(self, path: Path) -> str:
        """Wykrywa wersję gry (Steam/Itch lub Epic/MSStore)"""
        path_str = str(path).lower()
        
        # Sprawdź ścieżkę
        if "steam" in path_str:
            return "steam"
        elif "epic" in path_str:
            return "epic"
        elif "windowsapps" in path_str or "microsoft" in path_str:
            return "msstore"
        
        # Sprawdź pliki specyficzne dla platformy
        # Steam/Itch mają folder steam_api
        if (path / "steam_api.dll").exists() or (path / "steam_api64.dll").exists():
            return "steam"
        
        # Epic/MSStore mają inne pliki
        # Sprawdź czy istnieje manifest Epic Games
        if (path / ".egstore").exists():
            return "epic"
        
        # Microsoft Store apps mają specyficzną strukturę
        if "windowsapps" in path_str.lower():
            return "msstore"
        
        # Domyślnie zakładamy Steam/Itch (starsze instalacje)
        return "steam"
    
    def install_mod(self, mod_url: str, progress_callback=None) -> bool:
        """Instaluje mod z podanego URL"""
        if not self.game_path:
            return False
        
        try:
            if progress_callback:
                progress_callback("Pobieranie moda...", 10)
            
            # Pobierz mod
            response = requests.get(mod_url, stream=True)
            response.raise_for_status()
            
            mod_file = Path("temp_mod.zip")
            total_size = int(response.headers.get('content-length', 0))
            downloaded = 0
            
            with open(mod_file, 'wb') as f:
                for chunk in response.iter_content(chunk_size=8192):
                    if chunk:
                        f.write(chunk)
                        downloaded += len(chunk)
                        if progress_callback and total_size:
                            percent = 10 + int((downloaded / total_size) * 40)
                            progress_callback("Pobieranie moda...", percent)
            
            if progress_callback:
                progress_callback("Rozpakowywanie...", 60)
            
            # Rozpakuj mod
            with zipfile.ZipFile(mod_file, 'r') as zip_ref:
                zip_ref.extractall(self.game_path)
            
            if progress_callback:
                progress_callback("Czyszczenie...", 90)
            
            # Usuń tymczasowy plik
            mod_file.unlink()
            
            if progress_callback:
                progress_callback("Instalacja zakończona!", 100)
            
            return True
            
        except Exception as e:
            print(f"Błąd instalacji: {e}")
            return False
    
    def install_mod_from_file(self, mod_file_path: Path, progress_callback=None) -> bool:
        """Instaluje mod z lokalnego pliku ZIP"""
        if not self.game_path:
            return False
        
        try:
            if progress_callback:
                progress_callback("Rozpakowywanie moda...", 30)
            
            # Rozpakuj mod
            with zipfile.ZipFile(mod_file_path, 'r') as zip_ref:
                members = zip_ref.namelist()
                for i, member in enumerate(members):
                    zip_ref.extract(member, self.game_path)
                    if progress_callback:
                        percent = 30 + int((i / len(members)) * 60)
                        progress_callback(f"Rozpakowywanie: {member}", percent)
            
            if progress_callback:
                progress_callback("Instalacja zakończona!", 100)
            
            return True
            
        except Exception as e:
            print(f"Błąd instalacji: {e}")
            return False
    
    def install_mod_from_folder(self, mod_folder_path: Path, progress_callback=None) -> bool:
        """Instaluje mod z rozpakowanego folderu - kopiuje zawartość folderu do gry"""
        if not self.game_path:
            return False
        
        try:
            if progress_callback:
                progress_callback("Przygotowanie instalacji...", 10)
            
            # Sprawdź czy to folder z modem (zawiera np. BepInEx, dotnet itp.)
            if not mod_folder_path.is_dir():
                return False
            
            # Zbierz wszystkie pliki i foldery do skopiowania
            items_to_copy = list(mod_folder_path.iterdir())
            total_items = len(items_to_copy)
            
            if total_items == 0:
                return False
            
            if progress_callback:
                progress_callback("Kopiowanie plików moda...", 20)
            
            # Kopiuj każdy element z folderu moda do folderu gry
            for i, item in enumerate(items_to_copy):
                dest = self.game_path / item.name
                
                if item.is_file():
                    if progress_callback:
                        progress_callback(f"Kopiowanie: {item.name}", 20 + int((i / total_items) * 70))
                    shutil.copy2(item, dest)
                elif item.is_dir():
                    if progress_callback:
                        progress_callback(f"Kopiowanie folderu: {item.name}", 20 + int((i / total_items) * 70))
                    # Jeśli folder już istnieje, skopiuj zawartość do niego
                    if dest.exists():
                        shutil.copytree(item, dest, dirs_exist_ok=True)
                    else:
                        shutil.copytree(item, dest)
            
            if progress_callback:
                progress_callback("Instalacja zakończona!", 100)
            
            return True
            
        except Exception as e:
            print(f"Błąd instalacji: {e}")
            return False


class Updater:
    """Klasa do sprawdzania i pobierania aktualizacji"""
    
    @staticmethod
    def check_for_updates():
        """Sprawdza czy jest dostępna nowa wersja"""
        try:
            response = requests.get(UPDATE_CHECK_URL, timeout=5)
            response.raise_for_status()
            latest_release = response.json()
            
            latest_version = latest_release['tag_name'].lstrip('v')
            current_version = VERSION
            
            if Updater._compare_versions(latest_version, current_version) > 0:
                return {
                    'available': True,
                    'version': latest_version,
                    'download_url': latest_release['assets'][0]['browser_download_url'] if latest_release.get('assets') else None,
                    'body': latest_release.get('body', 'Brak opisu aktualizacji')
                }
            
            return {'available': False}
            
        except Exception as e:
            print(f"Błąd sprawdzania aktualizacji: {e}")
            return {'available': False, 'error': str(e)}
    
    @staticmethod
    def _compare_versions(v1: str, v2: str) -> int:
        """Porównuje dwie wersje. Zwraca: 1 jeśli v1 > v2, -1 jeśli v1 < v2, 0 jeśli równe"""
        parts1 = [int(x) for x in v1.split('.')]
        parts2 = [int(x) for x in v2.split('.')]
        
        for i in range(max(len(parts1), len(parts2))):
            p1 = parts1[i] if i < len(parts1) else 0
            p2 = parts2[i] if i < len(parts2) else 0
            
            if p1 > p2:
                return 1
            elif p1 < p2:
                return -1
        
        return 0
    
    @staticmethod
    def download_update(download_url: str, progress_callback=None):
        """Pobiera aktualizację"""
        try:
            if progress_callback:
                progress_callback("Pobieranie aktualizacji...", 10)
            
            response = requests.get(download_url, stream=True)
            response.raise_for_status()
            
            update_file = Path("among_installer_update.exe")
            total_size = int(response.headers.get('content-length', 0))
            downloaded = 0
            
            with open(update_file, 'wb') as f:
                for chunk in response.iter_content(chunk_size=8192):
                    if chunk:
                        f.write(chunk)
                        downloaded += len(chunk)
                        if progress_callback and total_size:
                            percent = 10 + int((downloaded / total_size) * 80)
                            progress_callback("Pobieranie aktualizacji...", percent)
            
            if progress_callback:
                progress_callback("Aktualizacja pobrana!", 100)
            
            return str(update_file)
            
        except Exception as e:
            print(f"Błąd pobierania aktualizacji: {e}")
            return None
    
    @staticmethod
    def apply_update(update_file: str):
        """Stosuje aktualizację i restartuje aplikację"""
        try:
            # Utwórz skrypt batch do aktualizacji
            batch_script = """
@echo off
timeout /t 2 /nobreak > NUL
move /Y "{update_file}" "{current_file}"
start "" "{current_file}"
del "%~f0"
""".format(
                update_file=update_file,
                current_file=sys.executable if getattr(sys, 'frozen', False) else __file__
            )
            
            batch_file = Path("update_installer.bat")
            batch_file.write_text(batch_script)
            
            # Uruchom skrypt batch i zamknij aplikację
            subprocess.Popen(['cmd', '/c', str(batch_file)], 
                           creationflags=subprocess.CREATE_NO_WINDOW)
            
            return True
            
        except Exception as e:
            print(f"Błąd stosowania aktualizacji: {e}")
            return False


class InstallerGUI:
    """Interfejs graficzny dla installera"""
    
    def __init__(self):
        self.installer = AmongUsInstaller()
        self.window = tk.Tk()
        self.window.title(f"Among Us Mod Installer v{VERSION}")
        self.window.geometry("550x450")
        self.window.resizable(False, False)
        
        self.setup_ui()
        
        # Sprawdź aktualizacje przy starcie
        self.window.after(1000, self.check_updates)
    
    def setup_ui(self):
        """Tworzy interfejs użytkownika"""
        
        # Header
        header = tk.Label(self.window, text="Among Us Mod Installer", 
                         font=("Arial", 16, "bold"))
        header.pack(pady=20)
        
        # Status frame
        status_frame = tk.LabelFrame(self.window, text="Status", padx=10, pady=10)
        status_frame.pack(padx=20, pady=10, fill="x")
        
        self.status_label = tk.Label(status_frame, text="Szukanie gry...", 
                                     font=("Arial", 10))
        self.status_label.pack()
        
        self.version_label = tk.Label(status_frame, text="", 
                                      font=("Arial", 9, "bold"), fg="blue")
        self.version_label.pack()
        
        self.path_label = tk.Label(status_frame, text="", 
                                   font=("Arial", 9), fg="gray")
        self.path_label.pack()
        
        # Buttons frame
        button_frame = tk.Frame(self.window)
        button_frame.pack(pady=15)
        
        self.auto_install_btn = tk.Button(button_frame, text="🎯 Auto-wybór moda (zalecane)", 
                                         command=self.auto_select_mod,
                                         width=30, height=2, state="disabled",
                                         bg="#4CAF50", fg="white", font=("Arial", 10, "bold"))
        self.auto_install_btn.pack(pady=5)
        
        self.install_file_btn = tk.Button(button_frame, text="Instaluj mod (folder lub ZIP)", 
                                         command=self.install_from_file,
                                         width=30, height=2, state="disabled")
        self.install_file_btn.pack(pady=5)
        
        self.manual_btn = tk.Button(button_frame, text="Wybierz folder gry ręcznie", 
                                   command=self.select_folder_manually,
                                   width=30, height=2)
        self.manual_btn.pack(pady=5)
        
        # Progress frame
        progress_frame = tk.LabelFrame(self.window, text="Postęp", padx=10, pady=10)
        progress_frame.pack(padx=20, pady=10, fill="x")
        
        self.progress_label = tk.Label(progress_frame, text="")
        self.progress_label.pack()
        
        self.progress_bar = ttk.Progressbar(progress_frame, mode='determinate', 
                                           length=400)
        self.progress_bar.pack(pady=5)
        
        # Szukaj gry przy starcie
        self.window.after(500, self.find_game)
    
    def check_updates(self):
        """Sprawdza dostępność aktualizacji"""
        def check():
            update_info = Updater.check_for_updates()
            self.window.after(0, lambda: self.on_update_check_complete(update_info))
        
        threading.Thread(target=check, daemon=True).start()
    
    def on_update_check_complete(self, update_info):
        """Callback po sprawdzeniu aktualizacji"""
        if update_info.get('available'):
            response = messagebox.askyesno(
                "Dostępna aktualizacja",
                f"Dostępna jest nowa wersja: {update_info['version']}\n"
                f"Aktualna wersja: {VERSION}\n\n"
                f"Zmiany:\n{update_info['body'][:200]}...\n\n"
                f"Czy chcesz pobrać i zainstalować aktualizację?",
                icon='info'
            )
            
            if response and update_info.get('download_url'):
                self.download_and_install_update(update_info['download_url'])
    
    def download_and_install_update(self, download_url: str):
        """Pobiera i instaluje aktualizację"""
        # Wyłącz wszystkie przyciski
        self.auto_install_btn.config(state="disabled")
        self.install_file_btn.config(state="disabled")
        self.manual_btn.config(state="disabled")
        
        def download_update():
            update_file = Updater.download_update(download_url, self.update_progress)
            
            if update_file:
                self.window.after(0, lambda: self.apply_update(update_file))
            else:
                self.window.after(0, lambda: messagebox.showerror(
                    "Błąd", 
                    "Nie udało się pobrać aktualizacji."
                ))
                self.window.after(0, self.enable_buttons)
        
        threading.Thread(target=download_update, daemon=True).start()
    
    def apply_update(self, update_file: str):
        """Stosuje pobraną aktualizację"""
        response = messagebox.askyesno(
            "Instalacja aktualizacji",
            "Aktualizacja została pobrana.\n\n"
            "Aplikacja zostanie zamknięta i uruchomiona ponownie.\n"
            "Czy kontynuować?",
            icon='question'
        )
        
        if response:
            if Updater.apply_update(update_file):
                self.window.quit()
            else:
                messagebox.showerror("Błąd", "Nie udało się zainstalować aktualizacji.")
                self.enable_buttons()
        else:
            # Usuń pobrany plik jeśli użytkownik anulował
            try:
                Path(update_file).unlink()
            except:
                pass
            self.enable_buttons()
    
    def enable_buttons(self):
        """Włącza przyciski po anulowaniu aktualizacji"""
        if self.installer.game_path:
            self.auto_install_btn.config(state="normal")
            self.install_file_btn.config(state="normal")
        self.manual_btn.config(state="normal")
    
    def find_game(self):
        """Szuka folderu gry"""
        self.status_label.config(text="Szukanie gry...")
        self.window.update()
        
        def search():
            path = self.installer.find_game_folder()
            self.window.after(0, lambda: self.on_game_found(path))
        
        threading.Thread(target=search, daemon=True).start()
    
    def on_game_found(self, path: Optional[Path]):
        """Callback po znalezieniu gry"""
        if path:
            self.installer.game_path = path
            self.installer.game_version = self.installer.detect_game_version(path)
            
            version_text = {
                "steam": "Wersja: Steam / Itch.io",
                "epic": "Wersja: Epic Games",
                "msstore": "Wersja: Microsoft Store"
            }.get(self.installer.game_version, f"Wersja: {self.installer.game_version}")
            
            self.status_label.config(text="✓ Gra znaleziona!", fg="green")
            self.version_label.config(text=version_text)
            self.path_label.config(text=str(path))
            self.auto_install_btn.config(state="normal")
            self.install_file_btn.config(state="normal")
        else:
            self.status_label.config(text="✗ Nie znaleziono gry", fg="red")
            self.version_label.config(text="")
            self.path_label.config(text="Użyj przycisku poniżej aby wybrać folder ręcznie")
    
    def select_folder_manually(self):
        """Pozwala użytkownikowi wybrać folder ręcznie"""
        # Jeśli już znaleziono grę, zacznij od tego samego folderu Among Us
        initial_dir = None
        if self.installer.game_path and self.installer.game_path.exists():
            initial_dir = str(self.installer.game_path)
        
        folder = filedialog.askdirectory(
            title="Wybierz folder Among Us",
            initialdir=initial_dir
        )
        if folder:
            path = Path(folder)
            if self.installer._verify_game_folder(path):
                self.installer.game_path = path
                self.installer.game_version = self.installer.detect_game_version(path)
                
                version_text = {
                    "steam": "Wersja: Steam / Itch.io",
                    "epic": "Wersja: Epic Games",
                    "msstore": "Wersja: Microsoft Store"
                }.get(self.installer.game_version, f"Wersja: {self.installer.game_version}")
                
                self.status_label.config(text="✓ Gra znaleziona!", fg="green")
                self.version_label.config(text=version_text)
                self.path_label.config(text=str(path))
                self.auto_install_btn.config(state="normal")
                self.install_file_btn.config(state="normal")
            else:
                messagebox.showerror("Błąd", "Wybrany folder nie zawiera gry Among Us!")
    
    def auto_select_mod(self):
        """Automatycznie wybiera właściwy mod bazując na wersji gry"""
        # Najpierw sprawdź w bieżącym folderze (gdzie jest installer.py)
        current_dir = Path(__file__).parent
        folder_path = current_dir
        
        # Jeśli nic nie znajdzie w bieżącym folderze, zapytaj użytkownika
        zip_files_current = list(current_dir.glob("*.zip"))
        mod_folders_current = [d for d in current_dir.iterdir() 
                              if d.is_dir() and not d.name.startswith('.') and not d.name.startswith('_')
                              and d.name not in ['.venv', '__pycache__', '.git', 'BepInEx', 'dotnet']]
        
        if not zip_files_current and not mod_folders_current:
            # Zapytaj o folder jeśli nie ma modów w bieżącym katalogu
            folder = filedialog.askdirectory(title="Wybierz folder z modami (foldery lub pliki ZIP)")
            if not folder:
                return
            folder_path = Path(folder)
        
        # Szukaj zarówno folderów z modami jak i plików ZIP
        zip_files = list(folder_path.glob("*.zip"))
        mod_folders = [d for d in folder_path.iterdir() 
                      if d.is_dir() and not d.name.startswith('.') and not d.name.startswith('_')
                      and d.name not in ['.venv', '__pycache__', '.git', 'BepInEx', 'dotnet']]
        
        # Połącz obie listy
        all_mods = zip_files + mod_folders
        
        if not all_mods:
            messagebox.showerror("Błąd", "Nie znaleziono modów (folderów ani plików ZIP) w wybranym folderze!")
            return
        
        # Próbuj dopasować mod do wersji gry
        detected_version = self.installer.game_version or "steam"
        selected_mod = None
        
        # Szukaj odpowiedniego moda (folder lub ZIP)
        for mod in all_mods:
            mod_name_lower = mod.name.lower()
            
            if detected_version in ["steam", "steam_itch"]:
                # Szukaj moda z "steam" lub "itch" w nazwie, ale bez "epic" i "msstore"
                if ("steam" in mod_name_lower or "itch" in mod_name_lower) and \
                   "epic" not in mod_name_lower and "msstore" not in mod_name_lower:
                    selected_mod = mod
                    break
            else:  # epic lub msstore
                # Szukaj moda z "epic" lub "msstore" w nazwie
                if "epic" in mod_name_lower or "msstore" in mod_name_lower or "ms" in mod_name_lower:
                    selected_mod = mod
                    break
        
        # Jeśli nie znaleziono dopasowania automatycznego
        if not selected_mod:
            if len(all_mods) == 1:
                # Tylko jeden mod - użyj go
                selected_mod = all_mods[0]
            elif len(all_mods) == 0:
                # Brak modów
                messagebox.showerror("Błąd", "Nie znaleziono modów do zainstalowania!")
                return
            else:
                # Więcej niż jeden - pokaż ostrzeżenie i automatycznie wybierz pierwszy pasujący
                version_name = {
                    "steam": "Steam / Itch.io",
                    "epic": "Epic Games",
                    "msstore": "Microsoft Store"
                }.get(detected_version, detected_version)
                
                # Sprawdź czy są jakieś częściowo pasujące
                partial_matches = []
                for mod in all_mods:
                    mod_name_lower = mod.name.lower()
                    if detected_version in ["steam", "steam_itch"]:
                        if "steam" in mod_name_lower or "itch" in mod_name_lower or "x86" in mod_name_lower:
                            partial_matches.append(mod)
                    else:
                        if "epic" in mod_name_lower or "msstore" in mod_name_lower or "x64" in mod_name_lower:
                            partial_matches.append(mod)
                
                if partial_matches:
                    selected_mod = partial_matches[0]
                    messagebox.showinfo(
                        "Auto-wybór",
                        f"Wykryto wersję: {version_name}\n\n"
                        f"Automatycznie wybrano: {selected_mod.name}\n\n"
                        f"Kliknij OK aby kontynuować."
                    )
                else:
                    # Nie można automatycznie wybrać - pokaż listę
                    choice_dialog = tk.Toplevel(self.window)
                    choice_dialog.title("Wybierz mod")
                    choice_dialog.geometry("450x350")
                    
                    tk.Label(choice_dialog, 
                            text=f"Wykryto wersję: {version_name}\n\nWybierz odpowiedni mod (folder lub ZIP):",
                            font=("Arial", 10)).pack(pady=10)
                    
                    listbox = tk.Listbox(choice_dialog, width=55, height=12)
                    listbox.pack(padx=10, pady=10)
                    
                    for mod in all_mods:
                        mod_type = "[FOLDER]" if mod.is_dir() else "[ZIP]"
                        listbox.insert(tk.END, f"{mod_type} {mod.name}")
                    
                    selected = [None]
                    
                    def on_select():
                        if listbox.curselection():
                            idx = listbox.curselection()[0]
                            selected[0] = all_mods[idx]
                            choice_dialog.destroy()
                    
                    tk.Button(choice_dialog, text="Wybierz", command=on_select).pack(pady=10)
                    
                    choice_dialog.wait_window()
                    selected_mod = selected[0]
        
        if selected_mod:
            # Automatycznie zainstaluj bez dodatkowego potwierdzenia
            version_name = {
                "steam": "Steam / Itch.io",
                "epic": "Epic Games / Microsoft Store", 
                "msstore": "Epic Games / Microsoft Store"
            }.get(detected_version, detected_version)
            
            mod_type = "folder" if selected_mod.is_dir() else "plik ZIP"
            
            # Pokaż info co będzie instalowane
            messagebox.showinfo(
                "Rozpoczęcie instalacji",
                f"Wersja gry: {version_name}\n"
                f"Instaluję mod ({mod_type}): {selected_mod.name}\n\n"
                f"Proszę czekać..."
            )
            
            if True:  # Zamieniam if response na if True aby zawsze instalować
                self.auto_install_btn.config(state="disabled")
                self.install_file_btn.config(state="disabled")
                
                def install():
                    # Użyj odpowiedniej funkcji w zależności od typu
                    if selected_mod.is_dir():
                        success = self.installer.install_mod_from_folder(
                            selected_mod, 
                            self.update_progress
                        )
                    else:
                        success = self.installer.install_mod_from_file(
                            selected_mod, 
                            self.update_progress
                        )
                    self.window.after(0, lambda: self.on_install_complete(success))
                
                threading.Thread(target=install, daemon=True).start()
    
    def install_from_file(self):
        """Instaluje mod z pliku lub folderu"""
        # Pokaż informację o wersji
        if self.installer.game_version:
            version_info = {
                "steam": "Steam / Itch.io",
                "epic": "Epic Games / Microsoft Store",
                "msstore": "Epic Games / Microsoft Store"
            }.get(self.installer.game_version, self.installer.game_version)
            
            message = f"Wykryto wersję: {version_info}\n\nWybierz folder z modem lub plik ZIP."
            messagebox.showinfo("Informacja o wersji", message)
        
        # Zapytaj użytkownika czy chce wybrać folder czy plik
        choice = messagebox.askquestion(
            "Wybór typu moda",
            "Czy mod jest w postaci rozpakowanego folderu?\n\n"
            "TAK = wybierz folder z modem\n"
            "NIE = wybierz plik ZIP",
            icon='question'
        )
        
        # Ustaw folder startowy - folder z modem lub folder gry
        initial_dir = None
        if self.installer.game_path and self.installer.game_path.exists():
            initial_dir = str(self.installer.game_path)
        
        if choice == 'yes':
            # Wybór folderu
            folder_path = filedialog.askdirectory(
                title="Wybierz folder z modem (odpowiedni dla Twojej wersji gry)",
                initialdir=initial_dir
            )
            
            if not folder_path:
                return
            
            mod_path = Path(folder_path)
            is_folder = True
        else:
            # Wybór pliku ZIP
            file_path = filedialog.askopenfilename(
                title="Wybierz plik moda (odpowiedni dla Twojej wersji gry)",
                filetypes=[("Pliki ZIP", "*.zip"), ("Wszystkie pliki", "*.*")],
                initialdir=initial_dir
            )
            
            if not file_path:
                return
            
            mod_path = Path(file_path)
            is_folder = False
        
        # Sprawdź nazwę dla dodatkowej walidacji
        mod_name = mod_path.name.lower()
        detected_version = self.installer.game_version or "unknown"
        
        warning_needed = False
        if detected_version in ["steam", "steam_itch"] and ("epic" in mod_name or "msstore" in mod_name):
            warning_needed = True
            wrong_version = "Epic/MSStore"
        elif detected_version in ["epic", "msstore"] and "steam" in mod_name and "epic" not in mod_name:
            warning_needed = True
            wrong_version = "Steam/Itch"
        
        if warning_needed:
            response = messagebox.askyesno(
                "Ostrzeżenie",
                f"Nazwa sugeruje wersję {wrong_version}, ale wykryto inną wersję gry.\n\n"
                f"Czy na pewno chcesz kontynuować instalację?",
                icon='warning'
            )
            if not response:
                return
        
        self.auto_install_btn.config(state="disabled")
        self.install_file_btn.config(state="disabled")
        
        def install():
            if is_folder:
                success = self.installer.install_mod_from_folder(
                    mod_path, 
                    self.update_progress
                )
            else:
                success = self.installer.install_mod_from_file(
                    mod_path, 
                    self.update_progress
                )
            self.window.after(0, lambda: self.on_install_complete(success))
        
        threading.Thread(target=install, daemon=True).start()
    
    def update_progress(self, message: str, percent: int):
        """Aktualizuje pasek postępu"""
        self.window.after(0, lambda: self._update_progress_ui(message, percent))
    
    def _update_progress_ui(self, message: str, percent: int):
        """Aktualizuje UI paska postępu"""
        self.progress_label.config(text=message)
        self.progress_bar['value'] = percent
        self.window.update()
    
    def on_install_complete(self, success: bool):
        """Callback po zakończeniu instalacji"""
        self.auto_install_btn.config(state="normal")
        self.install_file_btn.config(state="normal")
        
        if success:
            messagebox.showinfo("Sukces", "Mod został pomyślnie zainstalowany!")
        else:
            messagebox.showerror("Błąd", "Wystąpił błąd podczas instalacji moda.")
        
        self.progress_bar['value'] = 0
        self.progress_label.config(text="")
    
    def run(self):
        """Uruchamia aplikację"""
        self.window.mainloop()


def main():
    """Główna funkcja programu"""
    app = InstallerGUI()
    app.run()


if __name__ == "__main__":
    main()

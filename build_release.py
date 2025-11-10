# Skrypt do budowania wersji release aplikacji
# Wymaga: pip install pyinstaller

import os
import shutil
from pathlib import Path

# Wersja z pliku version.txt
version_file = Path("version.txt")
VERSION = version_file.read_text().strip() if version_file.exists() else "1.0.0"

print(f"Budowanie Among Us Mod Installer v{VERSION}...")

# Usuń stare buildy
if Path("dist").exists():
    shutil.rmtree("dist")
if Path("build").exists():
    shutil.rmtree("build")

# Zbuduj aplikację
import sys
import subprocess
result = subprocess.run([
    sys.executable, "-m", "PyInstaller",
    "--onefile",
    "--windowed", 
    "--name", f"AmongUsModInstaller-v{VERSION}",
    "--add-data", "version.txt;.",
    "installer.py"
])
if result.returncode != 0:
    print("❌ Błąd podczas budowania!")
    sys.exit(1)

print(f"\n✅ Build zakończony!")
print(f"📦 Plik: dist/AmongUsModInstaller-v{VERSION}.exe")
print(f"\n📋 Następne kroki:")
print(f"1. Utwórz nowy release na GitHub z tagiem v{VERSION}")
print(f"2. Wgraj plik AmongUsModInstaller-v{VERSION}.exe jako asset")
print(f"3. Opublikuj release")

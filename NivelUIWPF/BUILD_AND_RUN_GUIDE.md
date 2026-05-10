# 🚀 GHID DE COMPILARE ȘI EXECUTARE

## OPȚIUNEA 1: Utilizând Visual Studio 2022+

### Pași:
1. Deschideți soluția în Visual Studio
   - File → Open → Selectați fișierul .sln

2. Așteptați restaurarea pachetelor NuGet automat
   - Visual Studio va restaura dependențele

3. Setați NivelUIWPF ca startup project
   - Click dreapta pe proiect → Set as Startup Project

4. Compilați soluția
   - Build → Build Solution (Ctrl + Shift + B)

5. Rulați aplicația
   - Press F5 sau Debug → Start Debugging
   - Sau: Debug → Start Without Debugging (Ctrl + F5)

### Troubleshooting Visual Studio:
- Dacă NuGet nu se restaurează: 
  Tools → NuGet Package Manager → Package Manager Console
  Executați: Update-Package -Reinstall
  
- Pentru erori de build, verificați:
  - Versiunea .NET SDK (trebuie 8.0+)
  - Visual Studio este up-to-date

---

## OPȚIUNEA 2: Utilizând Command Line (.NET CLI)

### Pași:

1. **Navigați în directorul soluției**
   \\\powershell
   cd C:\Users\crist\Downloads\Gestionarea-si-prelucrarea-lemnului-main (1)\Gestionarea-si-prelucrarea-lemnului-main
   \\\

2. **Restaurați pachetele NuGet**
   \\\powershell
   dotnet restore
   \\\

3. **Compilați soluția**
   \\\powershell
   dotnet build
   \\\

4. **Rulați aplicația**
   \\\powershell
   cd NivelUIWPF
   dotnet run
   \\\

### Command-uri Utile:

**Compilare Release (Optimizat):**
\\\powershell
dotnet build -c Release
\\\

**Compilare Specific Project:**
\\\powershell
dotnet build NivelUIWPF/NivelUIWPF.csproj
\\\

**Run cu Debugging:**
\\\powershell
dotnet run --project NivelUIWPF/NivelUIWPF.csproj
\\\

**Curățare Build Artifacts:**
\\\powershell
dotnet clean
\\\

---

## OPȚIUNEA 3: Direct Executabil (După Build)

1. Navigați la folderul build:
   \\\
   NivelUIWPF\bin\Debug\net8.0-windows
   \\\

2. Executați fișierul:
   \\\
   NivelUIWPF.exe
   \\\

---

## VERIFICĂRI PRE-BUILD

Asigurați-vă că aveti:

✓ **.NET 8.0 SDK instalat**
   Verificare: \dotnet --version\
   
✓ **Visual Studio 2022 sau mai nou (dacă folosiți IDE)**
   Verificare: Help → About Microsoft Visual Studio
   
✓ **PowerShell 5.0+ (pentru CLI)**
   Verificare: \$PSVersionTable.PSVersion\

✓ **Acces la internet pentru NuGet** (prima dată)

---

## STRUCTURA DIRECTOR BUILD

După compilare, veți avea:

\\\
NivelUIWPF/
├── bin/
│   ├── Debug/
│   │   └── net8.0-windows/
│   │       ├── NivelUIWPF.exe        ← Executabil
│   │       ├── NivelUIWPF.dll
│   │       └── ... (alte DLL-uri)
│   └── Release/
│       └── net8.0-windows/
│           └── ... (optimizat)
└── obj/
    └── ... (intermediate files)
\\\

---

## RULARE APLICAȚIE

### La Startup:
1. Se deschide fereastra principală (MainWindow)
2. Se navighează automat la WelcomePage
3. Poți naviga prin butoane în header

### Pagini Disponibile:
- 📄 Bienvenit (Welcome)
- 📦 Lemn Brut
- 🔨 Produs Lemn
- 👥 Clienți
- ⚙️ Procesare

### Datele de Test:
Aplicația vine cu date populate pentru fiecare entitate

---

## TROUBLESHOOTING

### Eroare: \"Proiect SDK nu găsit\"
**Soluție**: Instalați .NET 8.0 SDK
- Descărcați de la: https://dotnet.microsoft.com/download

### Eroare: \"NuGet Restore Failed\"
**Soluție**: Actualizați NuGet sau restaurați manual
\\\powershell
dotnet package restore
\\\

### Eroare: \"Project reference error\"
**Soluție**: Verificați că toate proiectele au referințe corecte
\\\powershell
dotnet build --verbose
\\\

### Aplicația nu pornește
**Soluție**: Verificați Event Viewer pentru erori specifice
Windows + R → eventvwr.msc

### Interfață UI nu se încarcă
**Soluție**: Curățați și rebuildați soluția
\\\powershell
dotnet clean
dotnet build
\\\

---

## PERFORMANȚĂ ȘI OPTIMIZARE

### Release Build (Recomandat pentru Producție):
\\\powershell
dotnet publish -c Release -o ./publish
\\\

### Dezactivare DEBUG Info (Reducere Mărime):
\\\xml
<!-- În .csproj -->
<PropertyGroup>
  <DebugType>none</DebugType>
</PropertyGroup>
\\\

---

## STATISTICI BUILD

- **Dimensiune Debug**: ~50-80 MB
- **Dimensiune Release**: ~30-50 MB
- **Timp Build (Clean)**: 30-60 secunde
- **Memorie Necesară RAM**: 2 GB minim

---

## SUPORT ȘI DOCUMENTAȚIE

📖 README.md - Documentație generală
📋 IMPLEMENTATION_REPORT.md - Raport detaliat
💻 Cod sursă - Comentat și ușor de urmărit

---

**Versiune Guide**: 1.0  
**Ultima Actualizare**: 2026

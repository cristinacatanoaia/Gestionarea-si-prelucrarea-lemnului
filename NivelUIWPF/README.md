# 🌳 Sistem de Gestionare și Prelucrarea Lemnului - Aplicație WPF

## Despre Aplicație

Aceasta este o aplicație modernă de gestionare și prelucrare a lemnului, dezvoltată în **C# 12** utilizând **.NET 8.0** și **Windows Presentation Foundation (WPF)**. Aplicația oferă o interfață intuitivă și stilizată pentru gestiunea completă a datelor legate de lemn brut, produse finite, clienți și procese de prelucrare.

## 🎯 Funcționalități

### 1. **Gestionare Lemn Brut** 📦
- Vizualizare și gestionare lemn brut din depozite
- Clasificare după tip: Molid, Brad, Fag, Stejar, Pin
- Măsurare în metri cubi (m³)
- Adăugare, ștergere și actualizare înregistrări

### 2. **Produse Finite din Lemn** 🔨
- Evidența produselor prelucrate
- Tip produs și cantitate
- Caracteristici: Uscat, Tratat, Lustruit, Ignifugat, Certificat
- Gestiune completă a inventarului

### 3. **Bază de Date Clienți** 👥
- Gestiune informații clienți
- Date de contact: Telefon, Email
- Ușor acces și căutare
- Administrare completă

### 4. **Evidența Procesării** ⚙️
- Urmărire procese de prelucrare
- Date de procesare și cantități
- Materiale inițiale și produse rezultate
- Rapoarte detaliate

## 🏗️ Arhitectura Aplicației

Aplicația utilizează **arhitectura stratificată N-Tier** pentru o separare clară a responsabilităților:

`
NivelUIWPF (Nivel UI)
    ↓
GestionareLemn (Logică Aplicație)
    ↓
NivelStocareDate (Stocare Date)
    ↓
LibrarieModele (Entități și Structuri)
`

### Componente:

1. **NivelUIWPF** - Interfață grafică WPF
   - MainWindow.xaml - Fereastră principală
   - Pages/ - Pagini pentru fiecare modul
   - ViewModels/ - Logică de prezentare
   - Stiluri moderne și responsive

2. **GestionareLemn** - Logică aplicației
   - Procesare și validare date
   - Reguli de business
   - Factory pattern pentru stocare

3. **NivelStocareDate** - Accesul la date
   - Persistență informații
   - Acces baze de date
   - Interfețe de stocare

4. **LibrarieModele** - Modelele aplicației
   - Clasa LemnBrut cu enum TipLemnEnum
   - Clasa ProdusLemn cu CaracteristiciProdus
   - Clasa Client
   - Clasa Procesare

## 🛠️ Tehnologii Utilizate

- **Framework**: .NET 8.0
- **UI Framework**: WPF (Windows Presentation Foundation)
- **Limbaj**: C# 12
- **Pattern-uri**: MVVM, Factory, Observer
- **Controluri**: DataGrid, Page Navigation, Custom Styles

## 📋 Entități Principale

### LemnBrut
`csharp
public class LemnBrut
{
    public int Id { get; set; }
    public TipLemnEnum TipLemn { get; set; }
    public double CantitateMc { get; set; }
}

public enum TipLemnEnum { Molid, Brad, Fag, Stejar, Pin }
`

### ProdusLemn
`csharp
public class ProdusLemn
{
    public int Id { get; set; }
    public string TipProdus { get; set; }
    public double Cantitate { get; set; }
    public CaracteristiciProdus Caracteristici { get; set; }
}

[Flags]
public enum CaracteristiciProdus
{
    Niciuna = 0, Uscat = 1, Tratat = 2, Lustruit = 4, 
    Ignifugat = 8, Certificat = 16
}
`

### Client
`csharp
public class Client
{
    public int Id { get; set; }
    public string Nume { get; set; }
    public string Telefon { get; set; }
    public string Email { get; set; }
}
`

### Procesare
`csharp
public class Procesare
{
    public int Id { get; set; }
    public LemnBrut LemnInitial { get; set; }
    public double CantitateProcessata { get; set; }
    public List<ProdusLemn> ProduseRezultate { get; set; }
    public DateTime Data { get; set; }
}
`

## 🚀 Cum se Folosește

### Paginile Disponibile

1. **Bienvenit** (Welcome Page)
   - Prezentare completă a aplicației
   - Descriere funcționalități
   - Instrucțiuni de utilizare
   - Informații despre arhitectură

2. **Lemn Brut**
   - Adăugare noi intrări de lemn
   - Afișare lista lemn disponibil
   - Ștergere înregistrări
   - Vizualizare detalii

3. **Produs Lemn**
   - Gestionare produse finite
   - Specificare caracteristici produse
   - Urmărire stoc produse
   - Actualizare informații

4. **Clienți**
   - Adăugare/editare date clienți
   - Gestiune contacte
   - Căutare și filtru
   - Export informații

5. **Procesare**
   - Înregistrare procese de prelucrare
   - Legare materiale inițiale de produse finale
   - Urmărire date de procesare
   - Rapoarte prelucrări

## 🎨 Design și Stilizare

Aplicația utilizează o **paletă de culori profesională și moderna**:

- **Culoare Primară**: #2C3E50 (Gri-Albastru Închis)
- **Culoare Secundară**: #3498DB (Albastru Modern)
- **Culoare Accent**: #E74C3C (Roșu Cald)
- **Fundal**: #ECF0F1 (Gri Ușor)
- **Text**: #2C3E50 (Contrast optim)

### Elemente Stilizate:
- Butoane cu efecte hover și pressed
- TextBox-uri cu border animate
- Card-uri pentru secțiuni
- DataGrid-uri responsive
- Tranziții smooth
- Icone Unicode pentru vizualitate

## 📦 Instalare și Executare

### Cerințe Preliminare:
- .NET 8.0 SDK
- Visual Studio 2022 sau mai nou (optional)
- Windows 7 sau mai nou

### Pași:

1. **Clonare repository**
   `ash
   git clone <repository-url>
   cd <project-path>
   `

2. **Restaurare pachete NuGet**
   `ash
   dotnet restore
   `

3. **Compilare**
   `ash
   dotnet build
   `

4. **Executare**
   `ash
   cd NivelUIWPF
   dotnet run
   `

## 💾 Date de Test

Aplicația vine cu date de test populate:

- **Lemn Brut**: 5 intrări cu diferite tipuri
- **Produse Lemn**: 4 produse cu diverse caracteristici
- **Clienți**: 5 clienți cu date complete
- **Procesări**: 3 exemple de procese

## 🔧 Extensibilități Viitoare

- Integrare bază de date (SQL Server, MySQL)
- Export rapoarte (PDF, Excel)
- Grafice și statistici
- Sistem de autentificare și role
- API REST pentru acces extern
- Notificări și alerte
- Sincronizare cloud

## 📝 Licență

Aceasta este o aplicație educațională. Pentru utilizare comercială, consultați proprietarul.

## ✨ Autori și Contribuitori

Dezvoltat ca parte a cursului de Programare - PIU 2026

---

**Versiune**: 1.0.0  
**Data Creării**: 2026  
**Status**: Activ și în Dezvoltare

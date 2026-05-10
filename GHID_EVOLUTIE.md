# Ghid de Evoluție - Gestionarea Lemnului

## 🎯 Cum să Adaugi Noi Funcționalități

### Exemplu: Adăugare Funcție Nouă - "Raport Vânzări"

#### Pasul 1: Adauga metoda în Program.cs

```csharp
static void RaportVanzari()
{
    Console.WriteLine("\n=== RAPORT VANZARI ===");
    if (vanzari.Count == 0)
    {
        Console.WriteLine("Nu exista vanzari.");
        return;
    }

    // Calculeaza total cantitate pe produs
    var raport = vanzari
        .GroupBy(v => v.Produs.TipProdus)
        .Select(g => new { 
            Produs = g.Key, 
            CantitateTotal = g.Sum(v => v.Cantitate),
            NumarVanzari = g.Count()
        });

    foreach (var item in raport)
        Console.WriteLine($"  {item.Produs}: {item.CantitateTotal} mc ({item.NumarVanzari} tranzactii)");
}
```

#### Pasul 2: Adauga în switch din Main

```csharp
case "15": RaportVanzari(); break;
```

#### Pasul 3: Adauga în Meniu

```csharp
Console.WriteLine(" 15. Raport vanzari");
```

---

## 📊 Structura Actuală Program.cs

```
Program.cs
├── Variables Statice (liniile 9-13)
│   ├── stocareClienti
│   ├── stocareLemn
│   ├── produse
│   ├── procesari
│   └── vanzari
│
├── Main() - Inițializare + Loop Meniu (liniile 15-41)
│
├── AfiseazaMeniu() (liniile 43-67)
│
├── === CLIENTI === (liniile 69-115)
│   ├── AdaugaClient()
│   ├── AfiseazaClienti()
│   ├── CautaClientDupaNume()
│   └── CitireClient() [Helper]
│
├── === LEMN BRUT === (liniile 117-163)
│   ├── AdaugaLemnBrut()
│   ├── AfiseazaStocLemn()
│   ├── CautaLemnDupaTip()
│   └── CitireLemnBrut() [Helper]
│
├── === PROCESARE === (liniile 165-239)
│   ├── AdaugaProcesare()
│   └── AfiseazaProcesari()
│
├── === PRODUSE === (liniile 241-264)
│   ├── AfiseazaProduse()
│   └── CautaProdusDupaTip()
│
├── === VANZARI === (liniile 266-340)
│   ├── AdaugaVanzare()
│   ├── AfiseazaVanzari()
│   ├── CautaVanzariDupaClient()
│   └── CautaVanzariDupaProdus()
│
└── === INIT === (liniile 342-356)
    └── AdaugaDateInitiale()
```

---

## 🔄 Flux de Date

### Adăugare Client:
```
Main() → switch "1" → AdaugaClient()
    ↓
CitireClient() ← Console.ReadLine()
    ↓
stocareClienti.AddClient() → AdministrareClientiMemorie/Text
    ↓
Message de confirmare
```

### Vânzare:
```
Main() → switch "11" → AdaugaVanzare()
    ↓
Validare clienți/produse
    ↓
CitireDate (ID client, ID produs, cantitate)
    ↓
Actualizeaza produse.Cantitate -= cantitate
    ↓
vanzari.Add(new Vanzare {...})
```

---

## 💡 Best Practices în Cod

### 1. **Validare Input**
```csharp
int idClient;
while (!int.TryParse(Console.ReadLine(), out idClient))
    Console.Write("ID invalid! Introdu un numar: ");
```

### 2. **Verificare Stare Inițială**
```csharp
if (produse.Count == 0)
{
    Console.WriteLine("Nu exista produse in stoc!");
    return;
}
```

### 3. **Căutare cu LINQ**
```csharp
var gasiti = clienti.Where(c => c.Nume.ToLower().Contains(cautare)).ToList();
```

### 4. **FirstOrDefault vs GetById**
```csharp
// LINQ
var client = stocareClienti.GetClienti().FirstOrDefault(c => c.Id == idClient);

// SAU
var client = stocareClienti.GetClient(idClient); // Dacă interfața oferă metoda
```

---

## 🧪 Testare Manuală

### Test Scenarioriu Complet:
1. Rulează aplicația
2. Adauga 1-2 clienți (opțiunea 1)
3. Adauga lemn brut (opțiunea 4)
4. Proceseaza lemn (opțiunea 7) → creează produse
5. Adauga vânzare (opțiunea 11)
6. Verifică rapoarte (opțiunile 2, 5, 8, 9, 12)

---

## 🚀 Ture Viitoare

### Easy:
- [ ] Adaugă sorți pentru afișări (ex: clienți alfabetic)
- [ ] Calculează valoare vânzări (necesită Preț/Produs)
- [ ] Afișare pe pagini (meniu să nu fie prea lung)

### Medium:
- [ ] Persistență vânzări în fișier (similar cu clienti)
- [ ] Editare date client existent
- [ ] Ștergere/Anulare vânzare
- [ ] Rapoarte statistice (total MC procesat, etc.)

### Hard:
- [ ] Interfață GUI (WPF sau MAUI)
- [ ] Bază de date (SQLite, SQL Server)
- [ ] Export rapoarte (PDF, Excel)
- [ ] Autentificare utilizatori

---

## ❓ FAQ

**Q: De ce nu sunt metodele de citire în clase?**
A: Program.cs controlează toată interfața utilizator. Clasele model sunt pure (doar date).

**Q: Pot să decompin Program.cs?**
A: Da! Când ajunge la 500+ linii, poți creează clase statice helper în folder Helpers/Utils.

**Q: Cum salvez și procesari și vânzari?**
A: Adaugă interfețe în NivelStocareDate și clase care le implementează, ca pentru Client/LemnBrut.

---

**Ultima actualizare**: Nov 2024

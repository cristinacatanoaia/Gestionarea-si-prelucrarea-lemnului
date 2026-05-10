using System;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Linq;
using LibrarieModele;
using NivelStocareDate;
using GestionareLemn;

class Program
{
	private static IStocareClienti stocareClienti;
	private static IStocareLemnBrut stocareLemn;
	private static List<ProdusLemn> produse = new List<ProdusLemn>();
	private static List<Procesare> procesari = new List<Procesare>();
	private static List<Vanzare> vanzari = new List<Vanzare>();
=======
using System.ComponentModel;
using System.Security.Claims;
using LibrarieModele;
class Program
{
	static Gestiune gestiune = new Gestiune();
>>>>>>> badc449b322548801e74cb4072af652e0ee34f6a

	static void Main(string[] args)
	{
		Console.OutputEncoding = System.Text.Encoding.UTF8;
<<<<<<< HEAD
		stocareClienti = StocareFactory.GetStocareClienti();
		stocareLemn = StocareFactory.GetStocareLemnBrut();
		AdaugaDateInitiale();
=======
		gestiune.AdaugaDateInitiale();
>>>>>>> badc449b322548801e74cb4072af652e0ee34f6a

		bool continua = true;
		while (continua)
		{
			AfiseazaMeniu();
			string optiune = Console.ReadLine();

			switch (optiune)
			{
<<<<<<< HEAD
				case "1": AdaugaClient(); break;
				case "2": AfiseazaClienti(); break;
				case "3": CautaClientDupaNume(); break;
				case "4": AdaugaLemnBrut(); break;
				case "5": AfiseazaStocLemn(); break;
				case "6": CautaLemnDupaTip(); break;
				case "7": AdaugaProcesare(); break;
				case "8": AfiseazaProcesari(); break;
				case "9": AfiseazaProduse(); break;
				case "10": CautaProdusDupaTip(); break;
				case "11": AdaugaVanzare(); break;
				case "12": AfiseazaVanzari(); break;
				case "13": CautaVanzariDupaClient(); break;
				case "14": CautaVanzariDupaProdus(); break;
				case "0": continua = false; break;
				default: Console.WriteLine("Optiune invalida! Incearca din nou."); break;
			}
=======
			
				case "1": gestiune.AdaugaClient(); break;
				case "2": gestiune.AfiseazaClienti(); break;
				case "3": gestiune.CautaClientDupaNume(); break;
				case "4": gestiune.AdaugaLemnBrut(); break;
				case "5": gestiune.AfiseazaStocLemn(); break;
				case "6": gestiune.CautaLemnDupaTip(); break;
				case "7": gestiune.AdaugaProcesare(); break;
				case "8": gestiune.AfiseazaProcesari(); break;
				case "9": gestiune.AfiseazaProduse(); break;
				case "10": gestiune.CautaProdusDupaTip(); break;
				case "11": gestiune.AdaugaVanzare(); break;
				case "12": gestiune.AfiseazaVanzari(); break;
				case "13": gestiune.CautaVanzariDupaClient(); break;
				case "14": gestiune.CautaVanzariDupaProdus(); break;

				case "0":
					continua = false;
					break;
				default:
					Console.WriteLine("Optiune invalida! Incearca din nou.");
					break;
			}

	
>>>>>>> badc449b322548801e74cb4072af652e0ee34f6a
		}
	}

	static void AfiseazaMeniu()
	{
		Console.Clear();
<<<<<<< HEAD
		Console.WriteLine("  GESTIONAREA PRELUCRARII LEMNULUI     ");
=======
		
		Console.WriteLine("  GESTIONAREA PRELUCRARII LEMNULUI     ");

>>>>>>> badc449b322548801e74cb4072af652e0ee34f6a
		Console.WriteLine("  --- CLIENTI --- ");
		Console.WriteLine(" 1.  Adauga client ");
		Console.WriteLine(" 2.  Afiseaza toti clientii");
		Console.WriteLine(" 3.  Cauta client dupa nume");
		Console.WriteLine(" --- LEMN BRUT --- ");
		Console.WriteLine(" 4.  Adauga lemn brut in stoc  ");
		Console.WriteLine(" 5.  Afiseaza stoc lemn brut ");
		Console.WriteLine(" 6.  Cauta lemn dupa tip ");
		Console.WriteLine(" --- PROCESARE ---  ");
		Console.WriteLine(" 7.  Adauga procesare ");
		Console.WriteLine(" 8.  Afiseaza istoric procesari");
		Console.WriteLine(" --- PRODUSE PRELUCRATE --- ");
		Console.WriteLine(" 9.  Afiseaza produse in stoc ");
		Console.WriteLine(" 10. Cauta produs dupa tip ");
		Console.WriteLine(" --- VANZARI --- ");
		Console.WriteLine(" 11. Adauga vanzare");
		Console.WriteLine(" 12. Afiseaza toate vanzarile ");
		Console.WriteLine(" 13. Cauta vanzari dupa client");
		Console.WriteLine(" 14. Cauta vanzari dupa produs");
		Console.WriteLine(" 0.  Iesire ");
		Console.Write("Alege optiunea: ");
	}
<<<<<<< HEAD

	// ========== CLIENTI ==========
	static void AdaugaClient()
	{
		Console.WriteLine("\n=== ADAUGA CLIENT ===");
		Client c = CitireClient(stocareClienti.GetClienti().Count + 1);
		stocareClienti.AddClient(c);
		Console.WriteLine($"Clientul '{c.Nume}' a fost adaugat (ID: {c.Id})");
	}

	static void AfiseazaClienti()
	{
		Console.WriteLine("\n=== LISTA CLIENTI ===");
		var clienti = stocareClienti.GetClienti();
		if (clienti.Count == 0)
		{
			Console.WriteLine("Nu exista clienti inregistrati.");
		}
		else
		{
			foreach (var c in clienti)
				Console.WriteLine($"  ID: {c.Id} | Nume: {c.Nume} | Telefon: {c.Telefon} | Email: {c.Email}");
			Console.WriteLine($"Total: {clienti.Count} clienti");
		}
		
		Console.ReadKey();
	}

	static void CautaClientDupaNume()
	{
		Console.WriteLine("\n=== CAUTA CLIENT DUPA NUME ===");
		Console.Write("Introdu numele: ");
		string cautare = Console.ReadLine().ToLower();

		var gasiti = stocareClienti.GetClienti().Where(c => c.Nume.ToLower().Contains(cautare)).ToList();
		if (gasiti.Count == 0)
		{
			Console.WriteLine("Niciun client gasit.");
		}
		else
		{
			foreach (var c in gasiti)
				Console.WriteLine($"  ID: {c.Id} | Nume: {c.Nume} | Telefon: {c.Telefon} | Email: {c.Email}");
		}
		
		Console.ReadKey();
	}

	static Client CitireClient(int id)
	{
		var c = new Client { Id = id };
		Console.Write("Nume firma/client: ");
		c.Nume = Console.ReadLine();
		Console.Write("Telefon: ");
		c.Telefon = Console.ReadLine();
		Console.Write("Email: ");
		c.Email = Console.ReadLine();
		return c;
	}

	// ========== LEMN BRUT ==========
	static void AdaugaLemnBrut()
	{
		Console.WriteLine("\n=== ADAUGA LEMN BRUT ===");
		var l = CitireLemnBrut(stocareLemn.GetStocLemn().Count + 1);
		stocareLemn.AddLemnBrut(l);
		Console.WriteLine($"Lemnul '{l.TipLemn}' a fost adaugat in stoc");
	}

	static void AfiseazaStocLemn()
	{
		Console.WriteLine("\n=== STOC LEMN BRUT ===");
		var stoc = stocareLemn.GetStocLemn();
		if (stoc.Count == 0)
		{
			Console.WriteLine("Stocul de lemn brut este gol.");
		}
		else
		{
			foreach (var l in stoc)
				Console.WriteLine($"  ID: {l.Id} | Tip: {l.TipLemn} | Cantitate: {l.CantitateMc} mc");
		}
		
		Console.ReadKey();
	}

	static void CautaLemnDupaTip()
	{
		Console.WriteLine("\n=== CAUTA LEMN BRUT DUPA TIP ===");
		Console.Write("Introdu tipul de lemn: ");
		string cautare = Console.ReadLine().ToLower();

		var gasiti = stocareLemn.GetStocLemn().Where(l => l.TipLemn.ToString().ToLower().Contains(cautare)).ToList();
		if (gasiti.Count == 0)
		{
			Console.WriteLine("Niciun lemn gasit.");
		}
		else
		{
			foreach (var l in gasiti)
				Console.WriteLine($"  ID: {l.Id} | Tip: {l.TipLemn} | Cantitate: {l.CantitateMc} mc");
		}
		
		Console.ReadKey();
	}

	static LemnBrut CitireLemnBrut(int id)
	{
		var l = new LemnBrut { Id = id };
		Console.WriteLine("Tip lemn:");
		Console.WriteLine("  1. Molid\n  2. Brad\n  3. Fag\n  4. Stejar\n  5. Pin");
		Console.Write("Alege optiune: ");

		int opt;
		while (!int.TryParse(Console.ReadLine(), out opt) || opt < 1 || opt > 5)
			Console.Write("Invalida! Alege intre 1-5: ");

		l.TipLemn = (TipLemnEnum)(opt - 1);

		Console.Write("Cantitate (metri cubi): ");
		double cantitate;
		while (!double.TryParse(Console.ReadLine(), out cantitate) || cantitate <= 0)
			Console.Write("Valoare invalida: ");
		l.CantitateMc = cantitate;

		return l;
	}

	// ========== PROCESARE ==========
	static void AdaugaProcesare()
	{
		Console.WriteLine("\n=== ADAUGA PROCESARE LEMN ===");

		var stoc = stocareLemn.GetStocLemn();
		if (stoc.Count == 0)
		{
			Console.WriteLine("Nu exista lemn brut in stoc");
			return;
		}

		Console.WriteLine("Lemn brut disponibil:");
		foreach (var l in stoc)
			Console.WriteLine($"  ID: {l.Id} | Tip: {l.TipLemn} | Cantitate: {l.CantitateMc} mc");

		Console.Write("ID lemn brut de procesat: ");
		int idLemn;
		while (!int.TryParse(Console.ReadLine(), out idLemn))
			Console.Write("ID invalid. Introdu un numar: ");

		var lemnAles = stocareLemn.GetLemnBrut(idLemn);
		if (lemnAles == null)
		{
			Console.WriteLine("Lemn brut cu acest ID nu a fost gasit");
			return;
		}

		Console.Write($"Cantitate de procesat (max {lemnAles.CantitateMc} mc): ");
		double cantProc;
		while (!double.TryParse(Console.ReadLine(), out cantProc) || cantProc <= 0 || cantProc > lemnAles.CantitateMc)
			Console.Write($"Invalida! Introdu intre 0 si {lemnAles.CantitateMc}: ");

		lemnAles.CantitateMc -= cantProc;
		stocareLemn.UpdateLemnBrut(lemnAles);

		var procesare = new Procesare
		{
			Id = procesari.Count + 1,
			LemnInitial = lemnAles,
			CantitateProcessata = cantProc,
			Data = DateTime.Now
		};

		Console.WriteLine("Adauga produsele rezultate.Scrie 'stop' la tipul produsului pentru a termina");

		while (true)
		{
			Console.Write("Tip produs rezultat: ");
			string tip = Console.ReadLine();
			if (tip.ToLower() == "stop") break;

			Console.Write($"Cantitate {tip} rezultata (mc): ");
			double cantRez;
			while (!double.TryParse(Console.ReadLine(), out cantRez) || cantRez <= 0)
				Console.Write("Valoare invalida! Introdu un numar pozitiv: ");

			var produsExistent = produse.FirstOrDefault(p => p.TipProdus.ToLower() == tip.ToLower());

			if (produsExistent != null)
			{
				produsExistent.Cantitate += cantRez;
				procesare.ProduseRezultate.Add(produsExistent);
				Console.WriteLine($"Adaugat {cantRez} mc la stocul existent de '{tip}'.");
			}
			else
			{
				var produsNou = new ProdusLemn
				{
					Id = produse.Count + 1,
					TipProdus = tip,
					Cantitate = cantRez
				};
				produse.Add(produsNou);
				procesare.ProduseRezultate.Add(produsNou);
				Console.WriteLine($"Produs nou '{tip}' creat si adaugat in stoc.");
			}
		}

		procesari.Add(procesare);
		Console.WriteLine("Procesarea a fost inregistrata cu succes!");
	}

	static void AfiseazaProcesari()
	{
		Console.WriteLine("\n=== ISTORIC PROCESARI ===");
		if (procesari.Count == 0)
		{
			Console.WriteLine("Nu exista procesari inregistrate.");
		}
		else
		{
			foreach (var p in procesari)
			{
				Console.WriteLine($"  ID: {p.Id} | Data: {p.Data:dd/MM/yyyy} | Lemn folosit: {p.LemnInitial.TipLemn} - {p.CantitateProcessata} mc");
				Console.WriteLine("  Produse rezultate:");
				foreach (var prod in p.ProduseRezultate)
					Console.WriteLine($"    -> {prod.TipProdus}: {prod.Cantitate} mc");
			}
			Console.WriteLine($"Total: {procesari.Count} procesari");
		}
		
		Console.ReadKey();
	}

	// ========== PRODUSE ==========
	static void AfiseazaProduse()
	{
		Console.WriteLine("\n=== PRODUSE PRELUCRATE IN STOC ===");
		if (produse.Count == 0)
		{
			Console.WriteLine("Nu exista produse prelucrate in stoc.");
		}
		else
		{
			foreach (var p in produse)
				Console.WriteLine($"  ID: {p.Id} | Tip: {p.TipProdus} | Cantitate: {p.Cantitate} mc | Caracteristici: {p.Caracteristici}");
		}
		
		Console.ReadKey();
	}

	static void CautaProdusDupaTip()
	{
		Console.WriteLine("\n=== CAUTA PRODUS DUPA TIP ===");
		Console.Write("Introdu tipul produsului: ");
		string cautare = Console.ReadLine().ToLower();

		var gasiti = produse.Where(p => p.TipProdus.ToLower().Contains(cautare)).ToList();
		if (gasiti.Count == 0)
		{
			Console.WriteLine("Niciun produs gasit.");
		}
		else
		{
			foreach (var p in gasiti)
				Console.WriteLine($"  ID: {p.Id} | Tip: {p.TipProdus} | Cantitate: {p.Cantitate} mc");
		}
		
		Console.ReadKey();
	}

	// ========== VANZARI ==========
	static void AdaugaVanzare()
	{
		Console.WriteLine("\n=== ADAUGA VANZARE ===");

		var clienti = stocareClienti.GetClienti();
		if (clienti.Count == 0)
		{
			Console.WriteLine("Nu exista clienti! Adauga intai un client.");
			return;
		}
		if (produse.Count == 0)
		{
			Console.WriteLine("Nu exista produse in stoc! Proceseaza intai lemn brut.");
			return;
		}

		Console.WriteLine("Clienti disponibili:");
		foreach (var c in clienti)
			Console.WriteLine($"  ID: {c.Id} | Nume: {c.Nume}");

		Console.Write("ID client: ");
		int idClient;
		while (!int.TryParse(Console.ReadLine(), out idClient))
			Console.Write("ID invalid! Introdu un numar: ");

		var clientAles = stocareClienti.GetClient(idClient);
		if (clientAles == null)
		{
			Console.WriteLine("Clientul nu a fost gasit!");
			return;
		}

		Console.WriteLine("Produse disponibile:");
		foreach (var p in produse)
			Console.WriteLine($"  ID: {p.Id} | Tip: {p.TipProdus} | Cantitate: {p.Cantitate} mc");

		Console.Write("ID produs: ");
		int idProdus;
		while (!int.TryParse(Console.ReadLine(), out idProdus))
			Console.Write("ID invalid! Introdu un numar: ");

		var produsAles = produse.FirstOrDefault(p => p.Id == idProdus);
		if (produsAles == null)
		{
			Console.WriteLine("Produsul nu a fost gasit!");
			return;
		}
		if (produsAles.Cantitate == 0)
		{
			Console.WriteLine("Stocul acestui produs este 0! Alege alt produs.");
			return;
		}

		Console.Write($"Cantitate de vandut (max {produsAles.Cantitate} mc): ");
		double cantitate;
		while (!double.TryParse(Console.ReadLine(), out cantitate) || cantitate <= 0 || cantitate > produsAles.Cantitate)
			Console.Write($"Invalida! Introdu intre 0 si {produsAles.Cantitate}: ");

		var v = new Vanzare
		{
			Id = vanzari.Count + 1,
			Client = clientAles,
			Produs = produsAles,
			Cantitate = cantitate,
			Data = DateTime.Now
		};

		produsAles.Cantitate -= cantitate;
		vanzari.Add(v);
		Console.WriteLine("Vanzare inregistrata cu succes!");
	}

	static void AfiseazaVanzari()
	{
		Console.WriteLine("\n=== TOATE VANZARILE ===");
		if (vanzari.Count == 0)
		{
			Console.WriteLine("Nu exista vanzari inregistrate.");
		}
		else
		{
			foreach (var v in vanzari)
				Console.WriteLine($"  ID: {v.Id} | Data: {v.Data:dd/MM/yyyy} | Client: {v.Client.Nume} | Produs: {v.Produs.TipProdus} | Cantitate: {v.Cantitate} mc");
			Console.WriteLine($"Total: {vanzari.Count} vanzari");
		}
		
		Console.ReadKey();
	}

	static void CautaVanzariDupaClient()
	{
		Console.WriteLine("\n=== CAUTA VANZARI DUPA CLIENT ===");
		Console.Write("Introdu numele clientului: ");
		string cautare = Console.ReadLine().ToLower();

		var gasiti = vanzari.Where(v => v.Client.Nume.ToLower().Contains(cautare)).ToList();
		if (gasiti.Count == 0)
		{
			Console.WriteLine("Nu au fost gasite vanzari.");
		}
		else
		{
			foreach (var v in gasiti)
				Console.WriteLine($"  ID: {v.Id} | Data: {v.Data:dd/MM/yyyy} | Client: {v.Client.Nume} | Produs: {v.Produs.TipProdus} | Cantitate: {v.Cantitate} mc");
		}
		
		Console.ReadKey();
	}

	static void CautaVanzariDupaProdus()
	{
		Console.WriteLine("\n=== CAUTA VANZARI DUPA PRODUS ===");
		Console.Write("Introdu tipul produsului: ");
		string cautare = Console.ReadLine().ToLower();

		var gasiti = vanzari.Where(v => v.Produs.TipProdus.ToLower().Contains(cautare)).ToList();
		if (gasiti.Count == 0)
		{
			Console.WriteLine("Nu au fost gasite vanzari.");
		}
		else
		{
			foreach (var v in gasiti)
				Console.WriteLine($"  ID: {v.Id} | Data: {v.Data:dd/MM/yyyy} | Client: {v.Client.Nume} | Produs: {v.Produs.TipProdus} | Cantitate: {v.Cantitate} mc");
		}
		
		Console.ReadKey();
	}

	static void AdaugaDateInitiale()
	{
		if (stocareClienti.GetClienti().Count == 0)
		{
			stocareClienti.AddClient(new Client { Id = 1, Nume = "Construct SRL", Telefon = "0740123456", Email = "contact@construct.ro" });
			stocareClienti.AddClient(new Client { Id = 2, Nume = "Casa Lemn SRL", Telefon = "0751987654", Email = "office@casalemn.ro" });
		}
		if (stocareLemn.GetStocLemn().Count == 0)
		{
			stocareLemn.AddLemnBrut(new LemnBrut { Id = 1, TipLemn = TipLemnEnum.Molid, CantitateMc = 150.0 });
			stocareLemn.AddLemnBrut(new LemnBrut { Id = 2, TipLemn = TipLemnEnum.Fag, CantitateMc = 80.0 });
		}
	}
=======
>>>>>>> badc449b322548801e74cb4072af652e0ee34f6a
}

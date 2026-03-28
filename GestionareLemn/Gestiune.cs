using System;
using System.Collections.Generic;
using LibrarieModele;
public class Gestiune
{
	
	public List<Client> Clienti { get; set; } = new List<Client>();
	public List<LemnBrut> StocLemn { get; set; } = new List<LemnBrut>();
	public List<ProdusLemn> Produse { get; set; } = new List<ProdusLemn>();
	public List<Procesare> Procesari { get; set; } = new List<Procesare>();
	public List<Vanzare> Vanzari { get; set; } = new List<Vanzare>();

	

	public void AdaugaClient()
	{
		Console.WriteLine("\n=== ADAUGA CLIENT ===");
		Client c = Client.CitesteDeLatastatura(Clienti.Count + 1);
		Clienti.Add(c);
		Console.WriteLine($"Clientul '{c.Nume}' a fost adaugat (ID: {c.Id})");
	}

	public void AfiseazaClienti()
	{
		Console.WriteLine("\n=== LISTA CLIENTI ===");
		if (Clienti.Count == 0)
		{
			Console.WriteLine("Nu exista clienti inregistrati.");
			return;
		}
		foreach (Client c in Clienti)
			c.Afiseaza();
		Console.WriteLine($"Total: {Clienti.Count} clienti");
	}

	public void CautaClientDupaNume()
	{
		Console.WriteLine("\n=== CAUTA CLIENT DUPA NUME ===");
		Console.Write("Introdu numele: ");
		string cautare = Console.ReadLine().ToLower();

		bool gasit = false;
		foreach (Client c in Clienti)
		{
			if (c.Nume.ToLower().Contains(cautare))
			{
				c.Afiseaza();
				gasit = true;
			}
		}
		if (!gasit)
			Console.WriteLine("Niciun client gasit cu acest nume");
	}

	

	public void AdaugaLemnBrut()
	{
		Console.WriteLine("\n=== ADAUGA LEMN BRUT ===");
		LemnBrut l = LemnBrut.CitesteDeLatastatura(StocLemn.Count + 1);
		StocLemn.Add(l);
		Console.WriteLine($"Lemnul '{l.TipLemn}' a fost adaugat in stoc");
	}

	public void AfiseazaStocLemn()
	{
		Console.WriteLine("\n=== STOC LEMN BRUT ===");
		if (StocLemn.Count == 0)
		{
			Console.WriteLine("Stocul de lemn brut este gol.");
			return;
		}
		foreach (LemnBrut l in StocLemn)
			l.Afiseaza();
	}

	public void CautaLemnDupaTip()
	{
		Console.WriteLine("\n=== CAUTA LEMN BRUT DUPA TIP ===");
		Console.Write("Introdu tipul de lemn: ");
		string cautare = Console.ReadLine().ToLower();

		bool gasit = false;
		foreach (LemnBrut l in StocLemn)
		{
			if (l.TipLemn.ToString().ToLower().Contains(cautare))
			{
				l.Afiseaza();
				gasit = true;
			}
		}
		if (!gasit)
			Console.WriteLine("Niciun lemn gasit cu acest tip.");
	}

	

	public void AdaugaProcesare()
	{
		Console.WriteLine("\n=== ADAUGA PROCESARE LEMN ===");

		if (StocLemn.Count == 0)
		{
			Console.WriteLine("Nu exista lemn brut in stoc");
			return;
		}

		
		Console.WriteLine("Lemn brut disponibil:");
		foreach (LemnBrut l in StocLemn)
			l.Afiseaza();

		Console.Write("ID lemn brut de procesat: ");
		int idLemn;
		while (!int.TryParse(Console.ReadLine(), out idLemn))
			Console.Write("ID invalid. Introdu un numar: ");

		LemnBrut lemnAles = null;
		foreach (LemnBrut l in StocLemn)
			if (l.Id == idLemn) { lemnAles = l; break; }

		if (lemnAles == null)
		{
			Console.WriteLine("Lemn brut cu acest ID nu a fost gasit");
			return;
		}

		
		Console.Write($"Cantitate de procesat (max {lemnAles.CantitateMc} mc): ");
		double cantProc;
		while (!double.TryParse(Console.ReadLine(), out cantProc)
			   || cantProc <= 0 || cantProc > lemnAles.CantitateMc)
			Console.Write($"Invalida! Introdu intre 0 si {lemnAles.CantitateMc}: ");

		
		lemnAles.CantitateMc -= cantProc;

		Procesare procesare = new Procesare
		{
			Id = Procesari.Count + 1,
			LemnInitial = lemnAles,
			CantitateProcessata = cantProc,
			Data = DateTime.Now
		};

		
		Console.WriteLine("Adauga produsele rezultate.");
		Console.WriteLine("(Scrie 'stop' la tipul produsului pentru a termina)");

		while (true)
		{
			Console.Write("Tip produs rezultat: ");
			string tip = Console.ReadLine();
			if (tip.ToLower() == "stop") break;

			Console.Write($"Cantitate {tip} rezultata (mc): ");
			double cantRez;
			while (!double.TryParse(Console.ReadLine(), out cantRez) || cantRez <= 0)
				Console.Write("Valoare invalida! Introdu un numar pozitiv: ");

			
			ProdusLemn produsExistent = null;
			foreach (ProdusLemn p in Produse)
				if (p.TipProdus.ToLower() == tip.ToLower())
				{ produsExistent = p; break; }

			if (produsExistent != null)
			{
				produsExistent.Cantitate += cantRez;
				procesare.ProduseRezultate.Add(produsExistent);
				Console.WriteLine($"Adaugat {cantRez} mc la stocul existent de '{tip}'.");
			}
			else
			{
				ProdusLemn produsNou = new ProdusLemn
				{
					Id = Produse.Count + 1,
					TipProdus = tip,
					Cantitate = cantRez
				};
				Produse.Add(produsNou);
				procesare.ProduseRezultate.Add(produsNou);
				Console.WriteLine($"Produs nou '{tip}' creat si adaugat in stoc.");
			}
		}

		Procesari.Add(procesare);
		Console.WriteLine("Procesarea a fost inregistrata cu succes!");
	}

	public void AfiseazaProcesari()
	{
		Console.WriteLine("\n=== ISTORIC PROCESARI ===");
		if (Procesari.Count == 0)
		{
			Console.WriteLine("Nu exista procesari inregistrate.");
			return;
		}
		foreach (Procesare p in Procesari)
			p.Afiseaza();
		Console.WriteLine($"Total: {Procesari.Count} procesari");
	}

	

	public void AfiseazaProduse()
	{
		Console.WriteLine("\n=== PRODUSE PRELUCRATE IN STOC ===");
		if (Produse.Count == 0)
		{
			Console.WriteLine("Nu exista produse prelucrate in stoc.");
			return;
		}
		foreach (ProdusLemn p in Produse)
			p.Afiseaza();
	}

	public void CautaProdusDupaTip()
	{
		Console.WriteLine("\n=== CAUTA PRODUS DUPA TIP ===");
		Console.Write("Introdu tipul produsului: ");
		string cautare = Console.ReadLine().ToLower();

		bool gasit = false;
		foreach (ProdusLemn p in Produse)
		{
			if (p.TipProdus.ToLower().Contains(cautare))
			{
				p.Afiseaza();
				gasit = true;
			}
		}
		if (!gasit)
			Console.WriteLine("Niciun produs gasit cu acest tip.");
	}



	public void AdaugaVanzare()
	{
		Console.WriteLine("\n=== ADAUGA VANZARE ===");

		if (Clienti.Count == 0)
		{
			Console.WriteLine("Nu exista clienti! Adauga intai un client.");
			return;
		}
		if (Produse.Count == 0)
		{
			Console.WriteLine("Nu exista produse in stoc! Proceseaza intai lemn brut.");
			return;
		}

		
		Console.WriteLine("Clienti disponibili:");
		foreach (Client c in Clienti)
			c.Afiseaza();

		Console.Write("ID client: ");
		int idClient;
		while (!int.TryParse(Console.ReadLine(), out idClient))
			Console.Write("ID invalid! Introdu un numar: ");

		Client clientAles = null;
		foreach (Client c in Clienti)
			if (c.Id == idClient) { clientAles = c; break; }

		if (clientAles == null)
		{
			Console.WriteLine("Clientul nu a fost gasit!");
			return;
		}

		
		Console.WriteLine("Produse disponibile:");
		foreach (ProdusLemn p in Produse)
			p.Afiseaza();

		Console.Write("ID produs: ");
		int idProdus;
		while (!int.TryParse(Console.ReadLine(), out idProdus))
			Console.Write("ID invalid! Introdu un numar: ");

		ProdusLemn produsAles = null;
		foreach (ProdusLemn p in Produse)
			if (p.Id == idProdus) { produsAles = p; break; }

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
		while (!double.TryParse(Console.ReadLine(), out cantitate)
			   || cantitate <= 0 || cantitate > produsAles.Cantitate)
			Console.Write($"Invalida! Introdu intre 0 si {produsAles.Cantitate}: ");

		Vanzare v = new Vanzare
		{
			Id = Vanzari.Count + 1,
			Client = clientAles,
			Produs = produsAles,
			Cantitate = cantitate,
			Data = DateTime.Now
		};

		produsAles.Cantitate -= cantitate;
		Vanzari.Add(v);
		Console.WriteLine($"Vanzare inregistrata cu succes!");
	}

	public void AfiseazaVanzari()
	{
		Console.WriteLine("\n=== TOATE VANZARILE ===");
		if (Vanzari.Count == 0)
		{
			Console.WriteLine("Nu exista vanzari inregistrate.");
			return;
		}
		foreach (Vanzare v in Vanzari)
			v.Afiseaza();
		Console.WriteLine($"Total: {Vanzari.Count} vanzari");
	}

	public void CautaVanzariDupaClient()
	{
		Console.WriteLine("\n=== CAUTA VANZARI DUPA CLIENT ===");
		Console.Write("Introdu numele clientului: ");
		string cautare = Console.ReadLine().ToLower();

		bool gasit = false;
		foreach (Vanzare v in Vanzari)
		{
			if (v.Client.Nume.ToLower().Contains(cautare))
			{
				v.Afiseaza();
				gasit = true;
			}
		}
		if (!gasit)
			Console.WriteLine("Nu au fost gasite vanzari pentru acest client.");
	}

	public void CautaVanzariDupaProdus()
	{
		Console.WriteLine("\n=== CAUTA VANZARI DUPA PRODUS ===");
		Console.Write("Introdu tipul produsului: ");
		string cautare = Console.ReadLine().ToLower();

		bool gasit = false;
		foreach (Vanzare v in Vanzari)
		{
			if (v.Produs.TipProdus.ToLower().Contains(cautare))
			{
				v.Afiseaza();
				gasit = true;
			}
		}
		if (!gasit)
			Console.WriteLine("Nu au fost gasite vanzari pentru acest produs.");
	}



	public void AdaugaDateInitiale()
	{
		Clienti.Add(new Client
		{
			Id = 1,
			Nume = "Construct SRL",
			Telefon = "0740123456",
			Email = "contact@construct.ro"
		});
		Clienti.Add(new Client
		{
			Id = 2,
			Nume = "Casa Lemn SRL",
			Telefon = "0751987654",
			Email = "office@casalemn.ro"
		});

		StocLemn.Add(new LemnBrut { Id = 1, TipLemn = TipLemnEnum.Molid, CantitateMc = 150.0 });
		StocLemn.Add(new LemnBrut { Id = 2, TipLemn = TipLemnEnum.Fag, CantitateMc = 80.0 });
	}
}
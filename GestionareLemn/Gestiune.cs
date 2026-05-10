using System;
using System.Collections.Generic;
using LibrarieModele;
using NivelStocareDate;
using GestionareLemn;


public class Gestiune
{
	private IStocareClienti stocareClienti;
	private IStocareLemnBrut stocareLemn;

	public List<ProdusLemn> Produse { get; set; } = new List<ProdusLemn>();
	public List<Procesare> Procesari { get; set; } = new List<Procesare>();
	public List<Vanzare> Vanzari { get; set; } = new List<Vanzare>();

	public Gestiune()
	{
		stocareClienti = StocareFactory.GetStocareClienti();
		stocareLemn = StocareFactory.GetStocareLemnBrut();
	}

	public void AdaugaClient()
	{
		Console.WriteLine("\n=== ADAUGA CLIENT ===");
		Client c = Client.CitesteDeLatastatura(0);
		stocareClienti.AddClient(c);
		Console.WriteLine("Clientul '" + c.Nume + "' a fost adaugat (ID: " + c.Id + ")");
	}

	public void AfiseazaClienti()
	{
		Console.WriteLine("\n=== LISTA CLIENTI ===");
		List<Client> clienti = stocareClienti.GetClienti();
		if (clienti.Count == 0)
		{
			Console.WriteLine("Nu exista clienti inregistrati.");
			return;
		}
		foreach (Client c in clienti)
		{
			c.Afiseaza();
		}
		Console.WriteLine("Total: " + clienti.Count + " clienti");
	}

	public void CautaClientDupaNume()
	{
		Console.WriteLine("\n=== CAUTA CLIENT DUPA NUME ===");
		Console.Write("Introdu numele: ");
		string cautare = Console.ReadLine().ToLower();

		bool gasit = false;
		foreach (Client c in stocareClienti.GetClienti())
		{
			if (c.Nume.ToLower().Contains(cautare))
			{
				c.Afiseaza();
				gasit = true;
			}
		}
		if (!gasit)
			Console.WriteLine("Niciun client gasit cu acest nume.");
	}

	

	public void AdaugaLemnBrut()
	{
		Console.WriteLine("\n=== ADAUGA LEMN BRUT ===");
		LemnBrut l = LemnBrut.CitesteDeLatastatura(0);
		stocareLemn.AddLemnBrut(l);
		Console.WriteLine("Lemnul '" + l.TipLemn + "' a fost adaugat in stoc");
	}

	public void AfiseazaStocLemn()
	{
		Console.WriteLine("\n=== STOC LEMN BRUT ===");
		List<LemnBrut> stoc = stocareLemn.GetStocLemn();
		if (stoc.Count == 0)
		{
			Console.WriteLine("Stocul de lemn brut este gol.");
			return;
		}
		foreach (LemnBrut l in stoc)
		{
			l.Afiseaza();
		}
	}

	public void CautaLemnDupaTip()
	{
		Console.WriteLine("\n=== CAUTA LEMN BRUT DUPA TIP ===");
		Console.Write("Introdu tipul de lemn: ");
		string cautare = Console.ReadLine().ToLower();

		bool gasit = false;
		foreach (LemnBrut l in stocareLemn.GetStocLemn())
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

		List<LemnBrut> stoc = stocareLemn.GetStocLemn();
		if (stoc.Count == 0)
		{
			Console.WriteLine("Nu exista lemn brut in stoc");
			return;
		}

		Console.WriteLine("Lemn brut disponibil:");
		foreach (LemnBrut l in stoc)
			l.Afiseaza();

		Console.Write("ID lemn brut de procesat: ");
		int idLemn;
		while (!int.TryParse(Console.ReadLine(), out idLemn))
			Console.Write("ID invalid. Introdu un numar: ");

		LemnBrut lemnAles = stocareLemn.GetLemnBrut(idLemn);
		if (lemnAles == null)
		{
			Console.WriteLine("Lemn brut cu acest ID nu a fost gasit");
			return;
		}

		Console.Write("Cantitate de procesat (max " + lemnAles.CantitateMc + " mc): ");
		double cantProc;
		while (!double.TryParse(Console.ReadLine(), out cantProc) || cantProc <= 0 || cantProc > lemnAles.CantitateMc)
			Console.Write("Invalida! Introdu intre 0 si " + lemnAles.CantitateMc + ": ");

		lemnAles.CantitateMc -= cantProc;
		stocareLemn.UpdateLemnBrut(lemnAles);

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

			Console.Write("Cantitate " + tip + " rezultata (mc): ");
			double cantRez;
			while (!double.TryParse(Console.ReadLine(), out cantRez) || cantRez <= 0)
				Console.Write("Valoare invalida! Introdu un numar pozitiv: ");

			ProdusLemn produsExistent = null;
			foreach (ProdusLemn p in Produse)
			{
				if (p.TipProdus.ToLower() == tip.ToLower())
				{
					produsExistent = p;
					break;
				}
			}

			if (produsExistent != null)
			{
				produsExistent.Cantitate += cantRez;
				procesare.ProduseRezultate.Add(produsExistent);
				Console.WriteLine("Adaugat " + cantRez + " mc la stocul existent de '" + tip + "'.");
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
				Console.WriteLine("Produs nou '" + tip + "' creat si adaugat in stoc.");
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
		Console.WriteLine("Total: " + Procesari.Count + " procesari");
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

		List<Client> clienti = stocareClienti.GetClienti();
		if (clienti.Count == 0)
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
		foreach (Client c in clienti)
			c.Afiseaza();

		Console.Write("ID client: ");
		int idClient;
		while (!int.TryParse(Console.ReadLine(), out idClient))
			Console.Write("ID invalid! Introdu un numar: ");

		Client clientAles = stocareClienti.GetClient(idClient);
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
		{
			if (p.Id == idProdus)
			{
				produsAles = p;
				break;
			}
		}

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

		Console.Write("Cantitate de vandut (max " + produsAles.Cantitate + " mc): ");
		double cantitate;
		while (!double.TryParse(Console.ReadLine(), out cantitate) || cantitate <= 0 || cantitate > produsAles.Cantitate)
			Console.Write("Invalida! Introdu intre 0 si " + produsAles.Cantitate + ": ");

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
		Console.WriteLine("Vanzare inregistrata cu succes!");
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
		Console.WriteLine("Total: " + Vanzari.Count + " vanzari");
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
		if (stocareClienti.GetClienti().Count == 0)
		{
			stocareClienti.AddClient(new Client { Nume = "Construct SRL", Telefon = "0740123456", Email = "contact@construct.ro" });
			stocareClienti.AddClient(new Client { Nume = "Casa Lemn SRL", Telefon = "0751987654", Email = "office@casalemn.ro" });
		}
		if (stocareLemn.GetStocLemn().Count == 0)
		{
			stocareLemn.AddLemnBrut(new LemnBrut { TipLemn = TipLemnEnum.Molid, CantitateMc = 150.0 });
			stocareLemn.AddLemnBrut(new LemnBrut { TipLemn = TipLemnEnum.Fag, CantitateMc = 80.0 });
		}
	}
}
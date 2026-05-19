using Gestionarea_lemnului_copie;
using LibrarieModele;
using NivelStocareDate;

namespace Gestionarea_lemnului_copie
{
	class Program
	{
		// Stocare in memorie pentru procesari, produse si vanzari
		private static List<Procesare> procesari = new List<Procesare>();
		private static List<ProdusLemn> produse = new List<ProdusLemn>();
		private static List<Vanzare> vanzari = new List<Vanzare>();

		public static void Main()
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			IStocareClienti stocareClienti = StocareFactory.GetStocareClienti();
			IStocareLemnBrut stocareLemn = StocareFactory.GetStocareLemnBrut();

			AdaugaDateInitiale(stocareClienti, stocareLemn);

			string optiune;

			do
			{
				AfiseazaMeniu();
				optiune = Console.ReadLine() ?? string.Empty;

				switch (optiune)
				{
					case "1":
						AdaugaClient(stocareClienti);
						break;
					case "2":
						AfiseazaClienti(stocareClienti);
						break;
					case "3":
						CautaClientDupaNume(stocareClienti);
						break;
					case "4":
						AdaugaLemnBrut(stocareLemn);
						break;
					case "5":
						AfiseazaStocLemn(stocareLemn);
						break;
					case "6":
						CautaLemnDupaTip(stocareLemn);
						break;
					case "7":
						AdaugaProcesare(stocareLemn);
						break;
					case "8":
						AfiseazaProcesari();
						break;
					case "9":
						AfiseazaProduse();
						break;
					case "10":
						CautaProdusDupaTip();
						break;
					case "11":
						AdaugaVanzare(stocareClienti);
						break;
					case "12":
						AfiseazaVanzari();
						break;
					case "13":
						CautaVanzariDupaClient();
						break;
					case "14":
						CautaVanzariDupaProdus();
						break;
					case "0":
						Console.WriteLine("Aplicatia va fi inchisa.");
						break;
					default:
						Console.WriteLine("Optiune invalida! Incearca din nou.");
						break;
				}

				if (optiune != "0")
				{
					Console.WriteLine("\nApasa orice tasta pentru a continua...");
					Console.ReadKey();
				}

			} while (optiune != "0");
		}

		// ── MENIU ──────────────────────────────────────────────────────────────

		public static void AfiseazaMeniu()
		{
			Console.Clear();
			Console.WriteLine("  GESTIONAREA PRELUCRARII LEMNULUI");
			Console.WriteLine("  --- CLIENTI ---");
			Console.WriteLine("  1.  Adauga client");
			Console.WriteLine("  2.  Afiseaza toti clientii");
			Console.WriteLine("  3.  Cauta client dupa nume");
			Console.WriteLine("  --- LEMN BRUT ---");
			Console.WriteLine("  4.  Adauga lemn brut in stoc");
			Console.WriteLine("  5.  Afiseaza stoc lemn brut");
			Console.WriteLine("  6.  Cauta lemn dupa tip");
			Console.WriteLine("  --- PROCESARE ---");
			Console.WriteLine("  7.  Adauga procesare");
			Console.WriteLine("  8.  Afiseaza istoric procesari");
			Console.WriteLine("  --- PRODUSE PRELUCRATE ---");
			Console.WriteLine("  9.  Afiseaza produse in stoc");
			Console.WriteLine("  10. Cauta produs dupa tip");
			Console.WriteLine("  --- VANZARI ---");
			Console.WriteLine("  11. Adauga vanzare");
			Console.WriteLine("  12. Afiseaza toate vanzarile");
			Console.WriteLine("  13. Cauta vanzari dupa client");
			Console.WriteLine("  14. Cauta vanzari dupa produs");
			Console.WriteLine("  0.  Iesire");
			Console.Write("Alege optiunea: ");
		}

		// ── CLIENTI ────────────────────────────────────────────────────────────

		public static void AdaugaClient(IStocareClienti stocareClienti)
		{
			Console.WriteLine("\n=== ADAUGA CLIENT ===");
			Client client = CitireClientTastatura();
			stocareClienti.AddClient(client);
			Console.WriteLine("Clientul '" + client.Nume + "' a fost adaugat (ID: " + client.Id + ")");
		}

		public static Client CitireClientTastatura()
		{
			Client c = new Client();

			Console.Write("Nume firma/client: ");
			c.Nume = Console.ReadLine();

			Console.Write("Telefon: ");
			c.Telefon = Console.ReadLine();

			Console.Write("Email: ");
			c.Email = Console.ReadLine();

			return c;
		}

		public static void AfiseazaClient(Client client)
		{
			Console.WriteLine(client?.Info());
		}

		public static void AfiseazaClienti(IStocareClienti stocareClienti)
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
				AfiseazaClient(c);
			}

			Console.WriteLine("Total: " + clienti.Count + " clienti");
		}

		public static void CautaClientDupaNume(IStocareClienti stocareClienti)
		{
			Console.WriteLine("\n=== CAUTA CLIENT DUPA NUME ===");
			Console.Write("Introdu numele: ");
			string cautare = Console.ReadLine() ?? string.Empty;

			List<Client> clienti = stocareClienti.GetClienti();
			bool gasit = false;

			foreach (Client c in clienti)
			{
				if (c.Nume.Contains(cautare, StringComparison.OrdinalIgnoreCase))
				{
					AfiseazaClient(c);
					gasit = true;
				}
			}

			if (!gasit)
				Console.WriteLine("Niciun client gasit cu acest nume.");
		}

		// ── LEMN BRUT ──────────────────────────────────────────────────────────

		public static void AdaugaLemnBrut(IStocareLemnBrut stocareLemn)
		{
			Console.WriteLine("\n=== ADAUGA LEMN BRUT ===");
			LemnBrut lemn = CitireLemnBrutTastatura();
			stocareLemn.AddLemnBrut(lemn);
			Console.WriteLine("Lemnul '" + lemn.TipLemn + "' a fost adaugat in stoc.");
		}

		public static LemnBrut CitireLemnBrutTastatura()
		{
			LemnBrut l = new LemnBrut();

			Console.WriteLine("Tip lemn:");
			Console.WriteLine("  1. Molid");
			Console.WriteLine("  2. Brad");
			Console.WriteLine("  3. Fag");
			Console.WriteLine("  4. Stejar");
			Console.WriteLine("  5. Pin");
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

		public static void AfiseazaLemnBrut(LemnBrut lemn)
		{
			Console.WriteLine(lemn?.Info());
		}

		public static void AfiseazaStocLemn(IStocareLemnBrut stocareLemn)
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
				AfiseazaLemnBrut(l);
			}
		}

		public static void CautaLemnDupaTip(IStocareLemnBrut stocareLemn)
		{
			Console.WriteLine("\n=== CAUTA LEMN BRUT DUPA TIP ===");
			Console.Write("Introdu tipul de lemn: ");
			string cautare = Console.ReadLine() ?? string.Empty;

			bool gasit = false;
			foreach (LemnBrut l in stocareLemn.GetStocLemn())
			{
				if (l.TipLemn.ToString().Contains(cautare, StringComparison.OrdinalIgnoreCase))
				{
					AfiseazaLemnBrut(l);
					gasit = true;
				}
			}

			if (!gasit)
				Console.WriteLine("Niciun lemn gasit cu acest tip.");
		}

		// ── PROCESARE ──────────────────────────────────────────────────────────

		public static void AdaugaProcesare(IStocareLemnBrut stocareLemn)
		{
			Console.WriteLine("\n=== ADAUGA PROCESARE LEMN ===");

			List<LemnBrut> stoc = stocareLemn.GetStocLemn();
			if (stoc.Count == 0)
			{
				Console.WriteLine("Nu exista lemn brut in stoc.");
				return;
			}

			Console.WriteLine("Lemn brut disponibil:");
			foreach (LemnBrut l in stoc)
				AfiseazaLemnBrut(l);

			Console.Write("ID lemn brut de procesat: ");
			int.TryParse(Console.ReadLine(), out int idLemn);

			LemnBrut lemnAles = stocareLemn.GetLemnBrut(idLemn);
			if (lemnAles == null)
			{
				Console.WriteLine("Lemn brut cu acest ID nu a fost gasit.");
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
				Id = procesari.Count + 1,
				LemnInitial = lemnAles,
				CantitateProcessata = cantProc,
				Data = DateTime.Now
			};

			Console.WriteLine("Adauga produsele rezultate. (Scrie 'stop' la tip pentru a termina)");

			while (true)
			{
				Console.Write("Tip produs rezultat: ");
				string tip = Console.ReadLine() ?? string.Empty;
				if (tip.ToLower() == "stop") break;

				Console.Write("Cantitate " + tip + " rezultata (mc): ");
				double cantRez;
				while (!double.TryParse(Console.ReadLine(), out cantRez) || cantRez <= 0)
					Console.Write("Valoare invalida! Introdu un numar pozitiv: ");

				ProdusLemn produsExistent = produse.FirstOrDefault(p =>
					p.TipProdus.Equals(tip, StringComparison.OrdinalIgnoreCase));

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
						Id = produse.Count + 1,
						TipProdus = tip,
						Cantitate = cantRez
					};
					produse.Add(produsNou);
					procesare.ProduseRezultate.Add(produsNou);
					Console.WriteLine("Produs nou '" + tip + "' creat si adaugat in stoc.");
				}
			}

			procesari.Add(procesare);
			Console.WriteLine("Procesarea a fost inregistrata cu succes!");
		}

		public static void AfiseazaProcesari()
		{
			Console.WriteLine("\n=== ISTORIC PROCESARI ===");

			if (procesari.Count == 0)
			{
				Console.WriteLine("Nu exista procesari inregistrate.");
				return;
			}

			foreach (Procesare p in procesari)
				Console.WriteLine(p.Info());

			Console.WriteLine("Total: " + procesari.Count + " procesari");
		}

		// ── PRODUSE ────────────────────────────────────────────────────────────

		public static void AfiseazaProduse()
		{
			Console.WriteLine("\n=== PRODUSE PRELUCRATE IN STOC ===");

			if (produse.Count == 0)
			{
				Console.WriteLine("Nu exista produse prelucrate in stoc.");
				return;
			}

			foreach (ProdusLemn p in produse)
				Console.WriteLine(p.Info());
		}

		public static void CautaProdusDupaTip()
		{
			Console.WriteLine("\n=== CAUTA PRODUS DUPA TIP ===");
			Console.Write("Introdu tipul produsului: ");
			string cautare = Console.ReadLine() ?? string.Empty;

			bool gasit = false;
			foreach (ProdusLemn p in produse)
			{
				if (p.TipProdus.Contains(cautare, StringComparison.OrdinalIgnoreCase))
				{
					Console.WriteLine(p.Info());
					gasit = true;
				}
			}

			if (!gasit)
				Console.WriteLine("Niciun produs gasit cu acest tip.");
		}

		// ── VANZARI ────────────────────────────────────────────────────────────

		public static void AdaugaVanzare(IStocareClienti stocareClienti)
		{
			Console.WriteLine("\n=== ADAUGA VANZARE ===");

			List<Client> clienti = stocareClienti.GetClienti();
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
			foreach (Client c in clienti)
				AfiseazaClient(c);

			Console.Write("ID client: ");
			int.TryParse(Console.ReadLine(), out int idClient);

			Client clientAles = stocareClienti.GetClient(idClient);
			if (clientAles == null)
			{
				Console.WriteLine("Clientul nu a fost gasit!");
				return;
			}

			Console.WriteLine("Produse disponibile:");
			foreach (ProdusLemn p in produse)
				Console.WriteLine(p.Info());

			Console.Write("ID produs: ");
			int.TryParse(Console.ReadLine(), out int idProdus);

			ProdusLemn produsAles = produse.FirstOrDefault(p => p.Id == idProdus);
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

		public static void AfiseazaVanzari()
		{
			Console.WriteLine("\n=== TOATE VANZARILE ===");

			if (vanzari.Count == 0)
			{
				Console.WriteLine("Nu exista vanzari inregistrate.");
				return;
			}

			foreach (Vanzare v in vanzari)
				Console.WriteLine(v.Info());

			Console.WriteLine("Total: " + vanzari.Count + " vanzari");
		}

		public static void CautaVanzariDupaClient()
		{
			Console.WriteLine("\n=== CAUTA VANZARI DUPA CLIENT ===");
			Console.Write("Introdu numele clientului: ");
			string cautare = Console.ReadLine() ?? string.Empty;

			bool gasit = false;
			foreach (Vanzare v in vanzari)
			{
				if (v.Client.Nume.Contains(cautare, StringComparison.OrdinalIgnoreCase))
				{
					Console.WriteLine(v.Info());
					gasit = true;
				}
			}

			if (!gasit)
				Console.WriteLine("Nu au fost gasite vanzari pentru acest client.");
		}

		public static void CautaVanzariDupaProdus()
		{
			Console.WriteLine("\n=== CAUTA VANZARI DUPA PRODUS ===");
			Console.Write("Introdu tipul produsului: ");
			string cautare = Console.ReadLine() ?? string.Empty;

			bool gasit = false;
			foreach (Vanzare v in vanzari)
			{
				if (v.Produs.TipProdus.Contains(cautare, StringComparison.OrdinalIgnoreCase))
				{
					Console.WriteLine(v.Info());
					gasit = true;
				}
			}

			if (!gasit)
				Console.WriteLine("Nu au fost gasite vanzari pentru acest produs.");
		}

		// ── DATE INITIALE ──────────────────────────────────────────────────────

		public static void AdaugaDateInitiale(IStocareClienti stocareClienti, IStocareLemnBrut stocareLemn)
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
}
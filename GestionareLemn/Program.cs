using System;
using System.ComponentModel;
using System.Security.Claims;
using LibrarieModele;
class Program
{
	static Gestiune gestiune = new Gestiune();

	static void Main(string[] args)
	{
		Console.OutputEncoding = System.Text.Encoding.UTF8;
		gestiune.AdaugaDateInitiale();

		bool continua = true;
		while (continua)
		{
			AfiseazaMeniu();
			string optiune = Console.ReadLine();

			switch (optiune)
			{
			
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

	
		}
	}

	static void AfiseazaMeniu()
	{
		Console.Clear();
		
		Console.WriteLine("  GESTIONAREA PRELUCRARII LEMNULUI     ");

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
}

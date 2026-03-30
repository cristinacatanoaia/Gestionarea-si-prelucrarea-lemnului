using System;

namespace LibrarieModele
{
	public class Client
	{

		private const char SEPARATOR_PRINCIPAL_FISIER = ';';
		private const int ID = 0;
		private const int NUME = 1;
		private const int TELEFON = 2;
		private const int EMAIL = 3;
		public int Id { get; set; }
		public string Nume { get; set; }
		public string Telefon { get; set; }
		public string Email { get; set; }

		public Client()
		{
			Nume = string.Empty;
			Telefon = string.Empty;
			Email = string.Empty;
		}
		public Client(string linieFisier)
		{
			string[] dateFisier = linieFisier.Split(SEPARATOR_PRINCIPAL_FISIER);
			this.Id = Convert.ToInt32(dateFisier[ID]);
			this.Nume = dateFisier[NUME];
			this.Telefon = dateFisier[TELEFON];
			this.Email = dateFisier[EMAIL];
		}
		public string ConversieLaSirPentruFisier()
		{
			return string.Format("{1}{0}{2}{0}{3}{0}{4}",
				SEPARATOR_PRINCIPAL_FISIER,
				Id.ToString(),
				Nume ?? "NECUNOSCUT",
				Telefon ?? "NECUNOSCUT",
				Email ?? "NECUNOSCUT");
		}

		public static Client CitesteDeLatastatura(int id)
		{
			Client c = new Client();
			c.Id = id;

			Console.Write("Nume firma/client: ");
			c.Nume = Console.ReadLine();

			Console.Write("Telefon: ");
			c.Telefon = Console.ReadLine();

			Console.Write("Email: ");
			c.Email = Console.ReadLine();

			return c;
		}

		public void Afiseaza()
		{
			Console.WriteLine($"  ID: {Id} | Nume: {Nume} | Telefon: {Telefon} | Email: {Email}");
		}
	}
}
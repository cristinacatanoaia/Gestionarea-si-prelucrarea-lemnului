<<<<<<< HEAD
﻿namespace LibrarieModele
=======
﻿using System;

namespace LibrarieModele
>>>>>>> badc449b322548801e74cb4072af652e0ee34f6a
{
	public enum TipLemnEnum
	{
		Molid,
		Brad,
		Fag,
		Stejar,
		Pin
	}
<<<<<<< HEAD

=======
>>>>>>> badc449b322548801e74cb4072af652e0ee34f6a
	public class LemnBrut
	{
		private const char SEPARATOR_PRINCIPAL_FISIER = ';';
		private const int ID = 0;
		private const int TIP_LEMN = 1;
		private const int CANTITATE = 2;

		public int Id { get; set; }
		public TipLemnEnum TipLemn { get; set; }
		public double CantitateMc { get; set; }
<<<<<<< HEAD

		public LemnBrut() { }

=======
		public LemnBrut() { }
>>>>>>> badc449b322548801e74cb4072af652e0ee34f6a
		public LemnBrut(string linieFisier)
		{
			string[] dateFisier = linieFisier.Split(SEPARATOR_PRINCIPAL_FISIER);
			this.Id = Convert.ToInt32(dateFisier[ID]);
			this.TipLemn = (TipLemnEnum)Enum.Parse(typeof(TipLemnEnum), dateFisier[TIP_LEMN]);
			this.CantitateMc = Convert.ToDouble(dateFisier[CANTITATE]);
		}
<<<<<<< HEAD

=======
>>>>>>> badc449b322548801e74cb4072af652e0ee34f6a
		public string ConversieLaSirPentruFisier()
		{
			return string.Format("{1}{0}{2}{0}{3}",
				SEPARATOR_PRINCIPAL_FISIER,
				Id.ToString(),
				TipLemn.ToString(),
				CantitateMc.ToString());
		}
<<<<<<< HEAD
=======
		public static LemnBrut CitesteDeLatastatura(int id)
		{
			LemnBrut l = new LemnBrut();
			l.Id = id;

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

			l.TipLemn = (TipLemnEnum)(opt - 1);  // converteste numarul in enum

			Console.Write("Cantitate (metri cubi): ");
			double cantitate;
			while (!double.TryParse(Console.ReadLine(), out cantitate) || cantitate <= 0)
				Console.Write("Valoare invalida: ");
			l.CantitateMc = cantitate;

			return l;
		}

		public void Afiseaza()
		{
			Console.WriteLine($"  ID: {Id} | Tip: {TipLemn} | Cantitate: {CantitateMc} mc");
		}
>>>>>>> badc449b322548801e74cb4072af652e0ee34f6a
	}
}
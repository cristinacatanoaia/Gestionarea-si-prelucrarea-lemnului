using System;

namespace LibrarieModele
{
	public enum TipLemnEnum
	{
		Molid,
		Brad,
		Fag,
		Stejar,
		Pin
	}
	public class LemnBrut
	{
		public int Id { get; set; }
		public TipLemnEnum TipLemn { get; set; }
		public double CantitateMc { get; set; }

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
	}
}
using System;

namespace LibrarieModele
{
	[Flags]
	public enum CaracteristiciProdus
	{
		Niciuna = 0,
		Uscat = 1,
		Tratat = 2,
		Lustruit = 4,
		Ignifugat = 8,
		Certificat = 16
	}

	public class ProdusLemn
	{
		public int Id { get; set; }
		public string TipProdus { get; set; }
		public double Cantitate { get; set; }
		public CaracteristiciProdus Caracteristici { get; set; }

		public string Info()
		{
			return $"Id:{Id} | Tip:{TipProdus} | Cantitate:{Cantitate} mc | Caracteristici:{Caracteristici}";
		}
	}
}

using System;

namespace LibrarieModele
{
	public class Vanzare
	{
		public int Id { get; set; }
		public Client Client { get; set; }
		public ProdusLemn Produs { get; set; }
		public double Cantitate { get; set; }
		public DateTime Data { get; set; }

		public string Info()
		{
			return $"Id:{Id} | Data:{Data:dd/MM/yyyy} | Client:{Client?.Nume} | Produs:{Produs?.TipProdus} | Cantitate:{Cantitate} mc";
		}
	}
}
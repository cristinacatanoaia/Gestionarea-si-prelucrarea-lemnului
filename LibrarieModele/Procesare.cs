using System;
using System.Collections.Generic;
using System.Text;
using LibrarieModele;

namespace LibrarieModele
{
	public class Procesare
	{
		public int Id { get; set; }
		public LemnBrut LemnInitial { get; set; }
		public double CantitateProcessata { get; set; }
		public List<ProdusLemn> ProduseRezultate { get; set; } = new List<ProdusLemn>();
		public DateTime Data { get; set; }

		public string Info()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append($"Id:{Id} | Data:{Data:dd/MM/yyyy} | Lemn:{LemnInitial?.TipLemn} - {CantitateProcessata} mc");
			sb.Append(" | Produse rezultate:");
			foreach (ProdusLemn p in ProduseRezultate)
				sb.Append($" [{p.TipProdus}: {p.Cantitate} mc]");
			return sb.ToString();
		}
	}
}
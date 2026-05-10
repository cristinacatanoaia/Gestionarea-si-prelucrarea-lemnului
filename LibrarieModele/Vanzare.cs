<<<<<<< HEAD
﻿namespace LibrarieModele
=======
﻿using System;

namespace LibrarieModele
>>>>>>> badc449b322548801e74cb4072af652e0ee34f6a
{
	public class Vanzare
	{
		public int Id { get; set; }
		public Client Client { get; set; }
		public ProdusLemn Produs { get; set; }
		public double Cantitate { get; set; }
		public DateTime Data { get; set; }
<<<<<<< HEAD
=======

		public void Afiseaza()
		{
			Console.WriteLine($"  ID: {Id} | Data: {Data:dd/MM/yyyy} | " +
							  $"Client: {Client.Nume} | Produs: {Produs.TipProdus} | " +
							  $"Cantitate: {Cantitate} mc");
		}
>>>>>>> badc449b322548801e74cb4072af652e0ee34f6a
	}
}
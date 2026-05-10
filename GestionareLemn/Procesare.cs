using System;
using System.Collections.Generic;
using LibrarieModele;
<<<<<<< HEAD

=======
>>>>>>> badc449b322548801e74cb4072af652e0ee34f6a
public class Procesare
{
	public int Id { get; set; }
	public LemnBrut LemnInitial { get; set; }
	public double CantitateProcessata { get; set; }
	public List<ProdusLemn> ProduseRezultate { get; set; } = new List<ProdusLemn>();
	public DateTime Data { get; set; }
<<<<<<< HEAD
=======

	public void Afiseaza()
	{
		Console.WriteLine($"  ID: {Id} | Data: {Data:dd/MM/yyyy} | " +
						  $"Lemn folosit: {LemnInitial.TipLemn} - {CantitateProcessata} mc");
		Console.WriteLine($"  Produse rezultate:");
		foreach (ProdusLemn p in ProduseRezultate)
			Console.WriteLine($"    -> {p.TipProdus}: {p.Cantitate} mc");
	}
>>>>>>> badc449b322548801e74cb4072af652e0ee34f6a
}
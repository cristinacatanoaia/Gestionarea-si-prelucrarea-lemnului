using System;
using System.Collections.Generic;
using LibrarieModele;

public class Procesare
{
	public int Id { get; set; }
	public LemnBrut LemnInitial { get; set; }
	public double CantitateProcessata { get; set; }
	public List<ProdusLemn> ProduseRezultate { get; set; } = new List<ProdusLemn>();
	public DateTime Data { get; set; }
}
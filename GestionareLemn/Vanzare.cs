using System;

public class Vanzare
{
	public int Id { get; set; }
	public Client Client { get; set; }
	public ProdusLemn Produs { get; set; }
	public double Cantitate { get; set; }
	public DateTime Data { get; set; }
}
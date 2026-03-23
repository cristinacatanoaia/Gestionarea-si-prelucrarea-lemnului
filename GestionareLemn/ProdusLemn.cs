using System;

public class ProdusLemn
{
	public int Id { get; set; }
	public string TipProdus { get; set; }
	public double Cantitate { get; set; }

	public static ProdusLemn CitesteDeLatastatura(int id)
	{
		ProdusLemn p = new ProdusLemn();
		p.Id = id;

		Console.Write("Tip produs (ex: cherestea, lambriu, scandura): ");
		p.TipProdus = Console.ReadLine();

		Console.Write("Cantitate disponibila (mc): ");
		double cantitate;
		while (!double.TryParse(Console.ReadLine(), out cantitate) || cantitate <= 0)
			Console.Write("Valoare invalida ");
		p.Cantitate = cantitate;

		return p;
	}

	public void Afiseaza()
	{
		Console.WriteLine($"  ID: {Id} | Tip: {TipProdus} | Cantitate: {Cantitate} mc");
	}
}
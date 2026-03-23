using System;

public class LemnBrut
{
	public int Id { get; set; }
	public string TipLemn { get; set; }
	public double CantitateMc { get; set; }

	public static LemnBrut CitesteDeLatastatura(int id)
	{
		LemnBrut l = new LemnBrut();
		l.Id = id;

		Console.Write("Tip lemn (brad, molid, fag, stejar): ");
		l.TipLemn = Console.ReadLine();

		Console.Write("Cantitate (metri cubi): ");
		double cantitate;
		while (!double.TryParse(Console.ReadLine(), out cantitate) || cantitate <= 0)
			Console.Write("Valoare invalida");
		l.CantitateMc = cantitate;

		return l;
	}

	public void Afiseaza()
	{
		Console.WriteLine($"  ID: {Id} | Tip: {TipLemn} | Cantitate: {CantitateMc} mc");
	}
}
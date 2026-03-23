using System;

public class Client
{
	public int Id { get; set; }
	public string Nume { get; set; }
	public string Telefon { get; set; }
	public string Email { get; set; }

	public static Client CitesteDeLatastatura(int id)
	{
		Client c = new Client();
		c.Id = id;

		Console.Write("Nume firma/client: ");
		c.Nume = Console.ReadLine();

		Console.Write("Telefon: ");
		c.Telefon = Console.ReadLine();

		Console.Write("Email: ");
		c.Email = Console.ReadLine();

		return c;
	}

	public void Afiseaza()
	{
		Console.WriteLine($"  ID: {Id} | Nume: {Nume} | Telefon: {Telefon} | Email: {Email}");
	}
}
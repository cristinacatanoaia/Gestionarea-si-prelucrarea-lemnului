<<<<<<< HEAD
﻿namespace LibrarieModele
=======
﻿using System;

namespace LibrarieModele
>>>>>>> badc449b322548801e74cb4072af652e0ee34f6a
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
<<<<<<< HEAD

=======
>>>>>>> badc449b322548801e74cb4072af652e0ee34f6a
	public class ProdusLemn
	{
		public int Id { get; set; }
		public string TipProdus { get; set; }
		public double Cantitate { get; set; }
		public CaracteristiciProdus Caracteristici { get; set; }
<<<<<<< HEAD
=======
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

			Console.WriteLine("Caracteristici produs (poti alege mai multe, scrie 'stop' pentru a termina):");
			Console.WriteLine("  1. Uscat");
			Console.WriteLine("  2. Tratat");
			Console.WriteLine("  3. Lustruit");
			Console.WriteLine("  4. Ignifugat");
			Console.WriteLine("  5. Certificat");

			p.Caracteristici = CaracteristiciProdus.Niciuna;

			while (true)
			{
				Console.Write("Adauga caracteristica (1-5) sau 'stop': ");
				string input = Console.ReadLine();
				if (input.ToLower() == "stop") break;

				switch (input)
				{
					case "1": p.Caracteristici |= CaracteristiciProdus.Uscat; break;
					case "2": p.Caracteristici |= CaracteristiciProdus.Tratat; break;
					case "3": p.Caracteristici |= CaracteristiciProdus.Lustruit; break;
					case "4": p.Caracteristici |= CaracteristiciProdus.Ignifugat; break;
					case "5": p.Caracteristici |= CaracteristiciProdus.Certificat; break;
					default: Console.WriteLine("Optiune invalida!"); break;
				}
			}

			return p;
		}

		public void Afiseaza()
		{
			Console.WriteLine($"  ID: {Id} | Tip: {TipProdus} | Cantitate: {Cantitate} mc | Caracteristici: {Caracteristici}");
		}
>>>>>>> badc449b322548801e74cb4072af652e0ee34f6a
	}
}
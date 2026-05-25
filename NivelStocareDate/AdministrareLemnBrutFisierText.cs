using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LibrarieModele;

namespace NivelStocareDate
{
	public class AdministrareLemnBrutFisierText : IStocareLemnBrut
	{
		private const int ID_PRIMUL = 1;
		private const int INCREMENT = 1;
		private string numeFisier;

		public AdministrareLemnBrutFisierText(string numeFisier)
		{
			this.numeFisier = numeFisier;
			Stream streamFisier = File.Open(numeFisier, FileMode.OpenOrCreate);
			streamFisier.Close();
		}

		public void AddLemnBrut(LemnBrut lemn)
		{
			lemn.Id = GetNextId();
			using (StreamWriter sw = new StreamWriter(numeFisier, true))
			{
				sw.WriteLine(lemn.ConversieLaSirPentruFisier());
			}
		}

		public List<LemnBrut> GetStocLemn()
		{
			List<LemnBrut> stoc = new List<LemnBrut>();
			using (StreamReader sr = new StreamReader(numeFisier))
			{
				string linie;
				while ((linie = sr.ReadLine()) != null)
				{
					stoc.Add(new LemnBrut(linie));
				}
			}
			return stoc;
		}

		public LemnBrut GetLemnBrut(int id)
		{
			using (StreamReader sr = new StreamReader(numeFisier))
			{
				string linie;
				while ((linie = sr.ReadLine()) != null)
				{
					LemnBrut lemn = new LemnBrut(linie);
					if (lemn.Id == id)
						return lemn;
				}
			}
			return null;
		}

		public LemnBrut GetLemnBrut(TipLemnEnum tip)
		{
			using (StreamReader sr = new StreamReader(numeFisier))
			{
				string linie;
				while ((linie = sr.ReadLine()) != null)
				{
					LemnBrut lemn = new LemnBrut(linie);
					if (lemn.TipLemn == tip)
						return lemn;
				}
			}
			return null;
		}

		public bool UpdateLemnBrut(LemnBrut lemnActualizat)
		{
			List<LemnBrut> stoc = GetStocLemn();
			using (StreamWriter sw = new StreamWriter(numeFisier, false))
			{
				foreach (LemnBrut l in stoc)
				{
					LemnBrut lemnPentruScris = l;
					if (l.Id == lemnActualizat.Id)
					{
						lemnPentruScris = lemnActualizat;
					}
					sw.WriteLine(lemnPentruScris.ConversieLaSirPentruFisier());
				}
			}
			return true;
		}

		public bool DeleteLemnBrut(int id)
		{
			List<LemnBrut> stoc = GetStocLemn();
			bool gasit = false;
			using (StreamWriter sw = new StreamWriter(numeFisier, false))
			{
				foreach (LemnBrut l in stoc)
				{
					if (l.Id == id)
					{
						gasit = true;
						continue;
					}
					sw.WriteLine(l.ConversieLaSirPentruFisier());
				}
			}
			return gasit;
		}

		private int GetNextId()
		{
			List<LemnBrut> stoc = GetStocLemn();
			if (stoc.Count == 0)
				return ID_PRIMUL;
			return stoc[stoc.Count - 1].Id + INCREMENT;
		}
	}
}
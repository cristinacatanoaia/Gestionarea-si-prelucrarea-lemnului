using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarieModele;


namespace NivelStocareDate
{
	public class AdministrareLemnBrutMemorie : IStocareLemnBrut

	{
		private List<LemnBrut> stoc = new List<LemnBrut>();


		public void AddLemnBrut(LemnBrut lemn)
		{
			lemn.Id = GetNextId();
			stoc.Add(lemn);
		}


		public List<LemnBrut> GetStocLemn()
		{
			return stoc;
		}


		public LemnBrut GetLemnBrut(int id)
		{
			return stoc.FirstOrDefault(l => l.Id == id);
		}


		public LemnBrut GetLemnBrut(TipLemnEnum tip)
		{
			return stoc.FirstOrDefault(l => l.TipLemn == tip);
		}

		public bool UpdateLemnBrut(LemnBrut lemnActualizat)
		{
			LemnBrut existent = stoc.FirstOrDefault(l => l.Id == lemnActualizat.Id);
			if (existent == null) return false;
			existent.TipLemn = lemnActualizat.TipLemn;
			existent.CantitateMc = lemnActualizat.CantitateMc;
			return true;
		}

		public bool DeleteLemnBrut(int id)
		{
			LemnBrut existent = stoc.FirstOrDefault(l => l.Id == id);
			if (existent == null) return false;
			return stoc.Remove(existent);
		}

		private int GetNextId()
		{
			if (stoc.Count == 0) return 1;
			return stoc.Last().Id + 1;
		}
	}

}
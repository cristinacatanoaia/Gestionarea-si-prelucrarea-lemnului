using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarieModele;

namespace NivelStocareDate
{
	public interface IStocareLemnBrut
	{
		void AddLemnBrut(LemnBrut lemn);
		List<LemnBrut> GetStocLemn();
		LemnBrut GetLemnBrut(int id);
		LemnBrut GetLemnBrut(TipLemnEnum tip);
		bool UpdateLemnBrut(LemnBrut lemn);
	}
}

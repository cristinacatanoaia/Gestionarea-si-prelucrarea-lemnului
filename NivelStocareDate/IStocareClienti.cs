using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarieModele;

namespace NivelStocareDate
{
	public interface IStocareClienti
	{
		void AddClient(Client client);
		List<Client> GetClienti();
		Client GetClient(int id);
		Client GetClient(string nume);
		bool UpdateClient(Client client);
	}
}

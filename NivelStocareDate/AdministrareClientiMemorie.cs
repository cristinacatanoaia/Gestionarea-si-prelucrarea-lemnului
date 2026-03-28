using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarieModele;

namespace NivelStocareDate
{
	public class AdministrareClientiMemorie
	{
		private List<Client> clienti = new List<Client>();

		
		public void Add(Client client)
		{
			client.Id = GetNextId();
			clienti.Add(client);
		}

		
		public List<Client> GetAll()
		{
			return clienti;
		}

		
		public Client GetById(int id)
		{
			return clienti.FirstOrDefault(c => c.Id == id);
		}

	
		public Client GetByNume(string nume)
		{
			return clienti.FirstOrDefault(c =>c.Nume.Equals(nume, StringComparison.OrdinalIgnoreCase));
		}

		
		public int GetNextId()
		{
			if (clienti.Count == 0) return 1;
			return clienti.Last().Id + 1;
		}
	}
}

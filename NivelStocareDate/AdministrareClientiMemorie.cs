using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarieModele;

namespace NivelStocareDate
{
	public class AdministrareClientiMemorie : IStocareClienti

	{
		private List<Client> clienti = new List<Client>();


		public void AddClient(Client client)
		{
			client.Id = GetNextId();
			clienti.Add(client);
		}


		public List<Client> GetClienti()
		{
			return clienti;
		}


		public Client GetClient(int id)
		{
			return clienti.FirstOrDefault(c => c.Id == id);
		}


		public Client GetClient(string nume)
		{
			return clienti.FirstOrDefault(c => c.Nume.Equals(nume, StringComparison.OrdinalIgnoreCase));
		}

		public bool UpdateClient(Client clientActualizat)
		{
			Client clientExistent = clienti.FirstOrDefault(c => c.Id == clientActualizat.Id);
			if (clientExistent == null) return false;
			clientExistent.Nume = clientActualizat.Nume;
			clientExistent.Telefon = clientActualizat.Telefon;
			clientExistent.Email = clientActualizat.Email;
			return true;
		}

		private int GetNextId()
		{
			if (clienti.Count == 0) return 1;
			return clienti.Last().Id + 1;
		}
	}
}
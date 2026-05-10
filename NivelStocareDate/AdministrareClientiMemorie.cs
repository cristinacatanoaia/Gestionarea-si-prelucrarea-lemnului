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
<<<<<<< HEAD
		static private List<Client> clienti = new List<Client>();
=======
		private List<Client> clienti = new List<Client>();
>>>>>>> badc449b322548801e74cb4072af652e0ee34f6a
		

		public void AddClient(Client client)
		{
<<<<<<< HEAD
			if (client.Id <= 0)
			{
				client.Id = GetNextId();
			}
			clienti.Add(client);
		}


=======
			client.Id = GetNextId();
			clienti.Add(client);
		}

		
>>>>>>> badc449b322548801e74cb4072af652e0ee34f6a
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
			return clienti.FirstOrDefault(c =>c.Nume.Equals(nume, StringComparison.OrdinalIgnoreCase));
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

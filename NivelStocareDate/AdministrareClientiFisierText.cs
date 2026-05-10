using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarieModele;
using System.IO;

namespace NivelStocareDate
{
	public class AdministrareClientiFisierText : IStocareClienti
	{
		private const int ID_PRIMUL_CLIENT = 1;
		private const int INCREMENT = 1;
		private string numeFisier;

		public AdministrareClientiFisierText(string numeFisier)
		{
			this.numeFisier = numeFisier;
			// creeaza fisierul daca nu exista
			Stream streamFisierText = File.Open(numeFisier, FileMode.OpenOrCreate);
			streamFisierText.Close();
		}

		public void AddClient(Client client)
		{
			client.Id = GetNextIdClient();
			using (StreamWriter streamWriterFisierText = new StreamWriter(numeFisier, true))
			{
				streamWriterFisierText.WriteLine(client.ConversieLaSirPentruFisier());
			}
		}

		public List<Client> GetClienti()
		{
			List<Client> clienti = new List<Client>();
			using (StreamReader streamReader = new StreamReader(numeFisier))
			{
				string linieFisier;
				while ((linieFisier = streamReader.ReadLine()) != null)
				{
					clienti.Add(new Client(linieFisier));
				}
			}
			return clienti;
		}

		public Client GetClient(int id)
		{
			using (StreamReader streamReader = new StreamReader(numeFisier))
			{
				string linieFisier;
				while ((linieFisier = streamReader.ReadLine()) != null)
				{
					Client client = new Client(linieFisier);
					if (client.Id == id)
						return client;
				}
			}
			return null;
		}

		public Client GetClient(string nume)
		{
			using (StreamReader streamReader = new StreamReader(numeFisier))
			{
				string linieFisier;
				while ((linieFisier = streamReader.ReadLine()) != null)
				{
					Client client = new Client(linieFisier);
					if (client.Nume.Equals(nume))
						return client;
				}
			}
			return null;
		}

		public bool UpdateClient(Client clientActualizat)
		{
			List<Client> clienti = GetClienti();
			bool actualizareCuSucces = false;
			using (StreamWriter streamWriterFisierText = new StreamWriter(numeFisier, false))
			{
				foreach (Client client in clienti)
				{
					Client clientPentruScrisInFisier = client;
					if (client.Id == clientActualizat.Id)
					{
						clientPentruScrisInFisier = clientActualizat;
					}
					streamWriterFisierText.WriteLine(clientPentruScrisInFisier.ConversieLaSirPentruFisier());
				}
				actualizareCuSucces = true;
			}
			return actualizareCuSucces;
		}

		private int GetNextIdClient()
		{
			List<Client> clienti = GetClienti();
			if (clienti.Count == 0) return ID_PRIMUL_CLIENT;
			return clienti.Last().Id + INCREMENT;
		}
	}
}

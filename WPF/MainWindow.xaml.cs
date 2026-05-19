using System.Collections.Generic;
using System.Windows;
using LibrarieModele;
using NivelStocareDate;
using Gestionarea_lemnului_copie;

namespace WPF
{
	public partial class MainWindow : Window
	{
		private IStocareClienti stocareClienti;

		public MainWindow()
		{
			InitializeComponent();
			stocareClienti = StocareFactory.GetStocareClienti();
		}

		private void btnAdauga_Click(object sender, RoutedEventArgs e)
		{
			string nume = txtNume.Text.Trim();
			string telefon = txtTelefon.Text.Trim();
			string email = txtEmail.Text.Trim();

			// Validari
			if (string.IsNullOrEmpty(nume))
			{
				txtMesaj.Text = "Introduceti numele!";
				txtMesaj.Foreground = System.Windows.Media.Brushes.Red;
				return;
			}

			
			if (string.IsNullOrEmpty(telefon))
			{
				txtMesaj.Text = "Introduceti telefonul!";
				txtMesaj.Foreground = System.Windows.Media.Brushes.Red;
				return;
			}

			if (telefon.Length != 10)
			{
				txtMesaj.Text = "Telefonul trebuie sa aiba 10 cifre!";
				txtMesaj.Foreground = System.Windows.Media.Brushes.Red;
				return;
			}
			foreach (char c in telefon)
			{
				if (!char.IsDigit(c))
				{
					txtMesaj.Text = "Telefonul trebuie sa contina doar cifre!";
					txtMesaj.Foreground = System.Windows.Media.Brushes.Red;
					return;
				}
			}
			if (string.IsNullOrEmpty(email))
			{
				txtMesaj.Text = "Introduceti emailul!";
				txtMesaj.Foreground = System.Windows.Media.Brushes.Red;
				return;
			}
			if (string.IsNullOrEmpty(email))
			{
				txtMesaj.Text = "Introduceti emailul!";
				txtMesaj.Foreground = System.Windows.Media.Brushes.Red;
				return;
			}

			if (!email.Contains("@") || !email.Contains("."))
			{
				txtMesaj.Text = "Emailul nu este valid! (ex: nume@domeniu.ro)";
				txtMesaj.Foreground = System.Windows.Media.Brushes.Red;
				return;
			}
			// Validare duplicat
			if (stocareClienti.GetClient(nume) != null)
			{
				txtMesaj.Text = "Exista deja un client cu acest nume!";
				txtMesaj.Foreground = System.Windows.Media.Brushes.Red;
				return;
			}

			// Adaugare client
			Client clientNou = new Client
			{
				Nume = nume,
				Telefon = telefon,
				Email = email
			};

			stocareClienti.AddClient(clientNou);

			txtMesaj.Text = "Client adaugat cu succes!";
			txtMesaj.Foreground = System.Windows.Media.Brushes.Green;

			

			txtNume.Text = string.Empty;
			txtTelefon.Text = string.Empty;
			txtEmail.Text = string.Empty;
		}

		private void btnAfiseaza_Click(object sender, RoutedEventArgs e)
		{
			List<Client> clienti = stocareClienti.GetClienti();
			dgClienti.ItemsSource = null;
			dgClienti.ItemsSource = clienti;
		}
	}
}
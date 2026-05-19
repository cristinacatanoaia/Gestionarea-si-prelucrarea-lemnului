using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Gestionarea_lemnului_copie;
using LibrarieModele;
using NivelStocareDate;

namespace WPF
{
	public partial class MainWindow : Window
	{
		// Constante validare Client
		
		private const int LUNGIME_TELEFON = 10;

		// Constante validare LemnBrut
		private const double CANTITATE_MIN = 0.1;
		private const double CANTITATE_MAX = 10000.0;

		private IStocareClienti stocareClienti;
		private IStocareLemnBrut stocareLemn;

		public MainWindow()
		{
			InitializeComponent();
			stocareClienti = StocareFactory.GetStocareClienti();
			stocareLemn = StocareFactory.GetStocareLemnBrut();
		}

	

		private void btnMenuClienti_Click(object sender, RoutedEventArgs e)
		{
			panelClienti.Visibility = Visibility.Visible;
			panelLemnBrut.Visibility = Visibility.Collapsed;
		}

		private void btnMenuLemnBrut_Click(object sender, RoutedEventArgs e)
		{
			panelClienti.Visibility = Visibility.Collapsed;
			panelLemnBrut.Visibility = Visibility.Visible;
		}

		// Clienti
		private void btnAdaugaClient_Click(object sender, RoutedEventArgs e)
		{
			
			lblNume.Foreground = Brushes.Black;
			lblTelefon.Foreground = Brushes.Black;
			lblEmail.Foreground = Brushes.Black;
			txtMesajClient.Text = string.Empty;

			string nume = txtNume.Text.Trim();
			string telefon = txtTelefon.Text.Trim();
			string email = txtEmail.Text.Trim();

			// Validare
			if (string.IsNullOrEmpty(nume))
			{
				lblNume.Foreground = Brushes.Red;
				txtMesajClient.Foreground = Brushes.Red;
				txtMesajClient.Text = "Introduceti numele!";
				return;
			}
			
			if (string.IsNullOrEmpty(telefon))
			{
				lblTelefon.Foreground = Brushes.Red;
				txtMesajClient.Foreground = Brushes.Red;
				txtMesajClient.Text = "Introduceti telefonul!";
				return;
			}
			if (telefon.Length != LUNGIME_TELEFON)
			{
				lblTelefon.Foreground = Brushes.Red;
				txtMesajClient.Foreground = Brushes.Red;
				txtMesajClient.Text = $"Telefonul trebuie sa aiba exact {LUNGIME_TELEFON} cifre!";
				return;
			}
			foreach (char c in telefon)
			{
				if (!char.IsDigit(c))
				{
					lblTelefon.Foreground = Brushes.Red;
					txtMesajClient.Foreground = Brushes.Red;
					txtMesajClient.Text = "Telefonul trebuie sa contina doar cifre!";
					return;
				}
			}

			
			if (string.IsNullOrEmpty(email))
			{
				lblEmail.Foreground = Brushes.Red;
				txtMesajClient.Foreground = Brushes.Red;
				txtMesajClient.Text = "Introduceti emailul!";
				return;
			}
			if (!email.Contains("@") || !email.Contains("."))
			{
				lblEmail.Foreground = Brushes.Red;
				txtMesajClient.Foreground = Brushes.Red;
				txtMesajClient.Text = "Emailul nu este valid! (ex: nume@domeniu.ro)";
				return;
			}

			// Validare duplicat
			if (stocareClienti.GetClient(nume) != null)
			{
				lblNume.Foreground = Brushes.Red;
				txtMesajClient.Foreground = Brushes.Red;
				txtMesajClient.Text = "Exista deja un client cu acest nume!";
				return;
			}

			
			Client clientNou = new Client
			{
				Nume = nume,
				Telefon = telefon,
				Email = email
			};
			stocareClienti.AddClient(clientNou);

			txtNume.Text = string.Empty;
			txtTelefon.Text = string.Empty;
			txtEmail.Text = string.Empty;
			txtMesajClient.Foreground = Brushes.Green;
			txtMesajClient.Text = "Client adaugat cu succes!";
		}

		private void btnAfiseazaClienti_Click(object sender, RoutedEventArgs e)
		{
			List<Client> clienti = stocareClienti.GetClienti();
			dgClienti.ItemsSource = null;
			dgClienti.ItemsSource = clienti;
		}

		

		private void btnAdaugaLemn_Click(object sender, RoutedEventArgs e)
		{
			// Resetare etichete la culoarea normala
			lblTipLemn.Foreground = Brushes.Black;
			lblCantitate.Foreground = Brushes.Black;
			txtMesajLemn.Text = string.Empty;

			string cantitateText = txtCantitate.Text.Trim();

			// Validare Cantitate
			if (string.IsNullOrEmpty(cantitateText))
			{
				lblCantitate.Foreground = Brushes.Red;
				txtMesajLemn.Foreground = Brushes.Red;
				txtMesajLemn.Text = "Introduceti cantitatea!";
				return;
			}
			if (!double.TryParse(cantitateText, out double cantitate))
			{
				lblCantitate.Foreground = Brushes.Red;
				txtMesajLemn.Foreground = Brushes.Red;
				txtMesajLemn.Text = "Cantitatea trebuie sa fie un numar! (ex: 12.5)";
				return;
			}
			if (cantitate < CANTITATE_MIN)
			{
				lblCantitate.Foreground = Brushes.Red;
				txtMesajLemn.Foreground = Brushes.Red;
				txtMesajLemn.Text = $"Cantitatea minima este {CANTITATE_MIN} mc!";
				return;
			}
			if (cantitate > CANTITATE_MAX)
			{
				lblCantitate.Foreground = Brushes.Red;
				txtMesajLemn.Foreground = Brushes.Red;
				txtMesajLemn.Text = $"Cantitatea maxima este {CANTITATE_MAX} mc!";
				return;
			}

			// Adaugare lemn brut
			LemnBrut lemnNou = new LemnBrut
			{
				TipLemn = (TipLemnEnum)cmbTipLemn.SelectedIndex,
				CantitateMc = cantitate
			};
			stocareLemn.AddLemnBrut(lemnNou);

			txtCantitate.Text = string.Empty;
			txtMesajLemn.Foreground = Brushes.Green;
			txtMesajLemn.Text = "Lemn brut adaugat cu succes!";
		}

		private void btnAfiseazaLemn_Click(object sender, RoutedEventArgs e)
		{
			List<LemnBrut> stoc = stocareLemn.GetStocLemn();
			dgLemnBrut.ItemsSource = null;
			dgLemnBrut.ItemsSource = stoc;
		}
	}
}
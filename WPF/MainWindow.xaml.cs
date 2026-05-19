using System.Collections.Generic;
using System.Linq;
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
		private readonly List<string> produseDisponibile = new List<string> { "Scandura", "Panou", "Grinda", "Lambriu", "Placaj" };

		private IStocareClienti stocareClienti;
		private IStocareLemnBrut stocareLemn;
		private readonly List<Procesare> procesari = new List<Procesare>();
		private readonly List<Vanzare> vanzari = new List<Vanzare>();

		public MainWindow()
		{
			InitializeComponent();
			stocareClienti = StocareFactory.GetStocareClienti();
			stocareLemn = StocareFactory.GetStocareLemnBrut();
			SetProduseDisponibile();
			RefreshClientiDisponibili();
			RefreshProduseVanzareDisponibile();
		}

		private void SetProduseDisponibile()
		{
			cmbTipProdusProcesare.ItemsSource = produseDisponibile;
			cmbTipProdusProcesare.SelectedIndex = 0;
		}

		private void RefreshProduseVanzareDisponibile()
		{
			List<string> produseDisponibilePentruVanzare = procesari
				.SelectMany(procesare => procesare.ProduseRezultate)
				.Where(prod => prod.Cantitate > 0)
				.Select(prod => prod.TipProdus)
				.Distinct()
				.ToList();

			cmbProdusVanzare.ItemsSource = null;
			cmbProdusVanzare.ItemsSource = produseDisponibilePentruVanzare;
			cmbProdusVanzare.SelectedIndex = produseDisponibilePentruVanzare.Count > 0 ? 0 : -1;
		}

		private void RefreshClientiDisponibili()
		{
			cmbClientVanzare.ItemsSource = null;
			cmbClientVanzare.ItemsSource = stocareClienti.GetClienti().ToList();
		}

		private void AscundeToatePanourile()
		{
			panelClienti.Visibility = Visibility.Collapsed;
			panelLemnBrut.Visibility = Visibility.Collapsed;
			panelProcesare.Visibility = Visibility.Collapsed;
			panelVanzare.Visibility = Visibility.Collapsed;
		}

		private void btnMenuClienti_Click(object sender, RoutedEventArgs e)
		{
			AscundeToatePanourile();
			panelClienti.Visibility = Visibility.Visible;
			RefreshClientiDisponibili();
		}

		private void btnMenuLemnBrut_Click(object sender, RoutedEventArgs e)
		{
			AscundeToatePanourile();
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
			RefreshClientiDisponibili();

			txtNume.Text = string.Empty;
			txtTelefon.Text = string.Empty;
			txtEmail.Text = string.Empty;
			txtMesajClient.Foreground = Brushes.Green;
			txtMesajClient.Text = "Client adaugat cu succes!";
		}

		private void btnAfiseazaClienti_Click(object sender, RoutedEventArgs e)
		{
			dgCautareClienti.Visibility = Visibility.Collapsed;
			txtRezultatCautareClient.Text = string.Empty;
			dgClienti.ItemsSource = stocareClienti.GetClienti().ToList();
			dgClienti.Visibility = Visibility.Visible;
		}
		private void btnCautaClient_Click(object sender, RoutedEventArgs e)
		{
			dgClienti.Visibility = Visibility.Collapsed;
			string nume = txtCautareClient.Text.Trim();

			if (string.IsNullOrEmpty(nume))
			{
				txtRezultatCautareClient.Foreground = Brushes.Red;
				txtRezultatCautareClient.Text = "Introduceti un nume pentru cautare!";
				dgCautareClienti.Visibility = Visibility.Collapsed;
				return;
			}

			List<Client> clientiGasiti = stocareClienti.GetClienti()
				.Where(client => client.Nume.Contains(nume, System.StringComparison.OrdinalIgnoreCase))
				.ToList();

			if (clientiGasiti.Count == 0)
			{
				txtRezultatCautareClient.Foreground = Brushes.Red;
				txtRezultatCautareClient.Text = "Nu a fost gasit niciun client cu acest nume!";
				dgCautareClienti.Visibility = Visibility.Collapsed;
				return;
			}

			txtRezultatCautareClient.Foreground = Brushes.Green;
			txtRezultatCautareClient.Text = $"Au fost gasiti {clientiGasiti.Count} client(i).";
			dgCautareClienti.ItemsSource = clientiGasiti;
			dgCautareClienti.Visibility = Visibility.Visible;
		}


		private void btnAdaugaLemn_Click(object sender, RoutedEventArgs e)
		{
			// Resetare etichete la culoarea normala
			lblTipLemn.Foreground = Brushes.Black;
			lblCantitate.Foreground = Brushes.Black;
			txtMesajLemn.Text = string.Empty;

			// Validare ListBox (selectare tip lemn)
			if (lstTipLemn.SelectedIndex == -1)
			{
				lblTipLemn.Foreground = Brushes.Red;
				txtMesajLemn.Foreground = Brushes.Red;
				txtMesajLemn.Text = "Selectati tipul de lemn din ListBox!";
				return;
			}

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

			// Adaugare lemn brut (folosind valoarea din ListBox)
			LemnBrut lemnNou = new LemnBrut
			{
				TipLemn = (TipLemnEnum)lstTipLemn.SelectedIndex,
				CantitateMc = cantitate
			};
			stocareLemn.AddLemnBrut(lemnNou);

			txtCantitate.Text = string.Empty;
			lstTipLemn.SelectedIndex = -1;
			txtMesajLemn.Foreground = Brushes.Green;
			txtMesajLemn.Text = $"Lemn brut adaugat cu succes! Tip: {lemnNou.TipLemn}, Cantitate: {cantitate} mc";
		}

		private void btnAfiseazaLemn_Click(object sender, RoutedEventArgs e)
		{
			List<LemnBrut> stoc = stocareLemn.GetStocLemn().ToList();
			dgLemnBrut.ItemsSource = null;
			dgLemnBrut.ItemsSource = stoc;
		}


		private void btnMenuProcesare_Click(object sender, RoutedEventArgs e)
		{
			AscundeToatePanourile();
			panelProcesare.Visibility = Visibility.Visible;
		}

		private void btnMenuVanzare_Click(object sender, RoutedEventArgs e)
		{
			AscundeToatePanourile();
			panelVanzare.Visibility = Visibility.Visible;
		}

		//procesare

		private void btnAdaugaProcesare_Click(object sender, RoutedEventArgs e)
		{
			lblTipLemnProcesare.Foreground = Brushes.Black;
			lblCantProcesare.Foreground = Brushes.Black;
			txtMesajProcesare.Text = string.Empty;
			string produsRezultat = cmbTipProdusProcesare.SelectedItem as string ?? string.Empty;

			// Validare tip lemn
			bool tipSelectat = rdoMolid.IsChecked == true || rdoBrad.IsChecked == true ||
							   rdoFag.IsChecked == true || rdoStejar.IsChecked == true ||
							   rdoPin.IsChecked == true;
			if (!tipSelectat)
			{
				lblTipLemnProcesare.Foreground = Brushes.Red;
				txtMesajProcesare.Foreground = Brushes.Red;
				txtMesajProcesare.Text = "Selectati tipul de lemn!";
				return;
			}

			// Validare cantitate
			if (string.IsNullOrEmpty(txtCantProcesare.Text.Trim()))
			{
				lblCantProcesare.Foreground = Brushes.Red;
				txtMesajProcesare.Foreground = Brushes.Red;
				txtMesajProcesare.Text = "Introduceti cantitatea procesata!";
				return;
			}
			if (!double.TryParse(txtCantProcesare.Text.Trim(), out double cantitate))
			{
				lblCantProcesare.Foreground = Brushes.Red;
				txtMesajProcesare.Foreground = Brushes.Red;
				txtMesajProcesare.Text = "Cantitatea trebuie sa fie un numar!";
				return;
			}

			if (string.IsNullOrWhiteSpace(produsRezultat))
			{
				txtMesajProcesare.Foreground = Brushes.Red;
				txtMesajProcesare.Text = "Alegeti produsul rezultat!";
				return;
			}

			// Determinare tip lemn selectat
			TipLemnEnum tipLemn = TipLemnEnum.Molid;
			if (rdoBrad.IsChecked == true) tipLemn = TipLemnEnum.Brad;
			else if (rdoFag.IsChecked == true) tipLemn = TipLemnEnum.Fag;
			else if (rdoStejar.IsChecked == true) tipLemn = TipLemnEnum.Stejar;
			else if (rdoPin.IsChecked == true) tipLemn = TipLemnEnum.Pin;

			List<LemnBrut> lemnDisponibil = stocareLemn.GetStocLemn()
				.Where(lemn => lemn.TipLemn == tipLemn)
				.ToList();
			double cantitateDisponibila = lemnDisponibil.Sum(lemn => lemn.CantitateMc);
			if (cantitate > cantitateDisponibila)
			{
				txtMesajProcesare.Foreground = Brushes.Red;
				txtMesajProcesare.Text = $"Nu exista suficient lemn brut. Disponibil: {cantitateDisponibila} mc.";
				return;
			}

			// Determinare caracteristici bifate
			CaracteristiciProdus caracteristici = CaracteristiciProdus.Niciuna;
			if (chkUscat.IsChecked == true) caracteristici |= CaracteristiciProdus.Uscat;
			if (chkTratat.IsChecked == true) caracteristici |= CaracteristiciProdus.Tratat;
			if (chkLustruit.IsChecked == true) caracteristici |= CaracteristiciProdus.Lustruit;
			if (chkIgnifugat.IsChecked == true) caracteristici |= CaracteristiciProdus.Ignifugat;
			if (chkCertificat.IsChecked == true) caracteristici |= CaracteristiciProdus.Certificat;

			Procesare procesareNoua = new Procesare
			{
				Id = procesari.Count + 1,
				LemnInitial = new LemnBrut { TipLemn = tipLemn, CantitateMc = cantitate },
				CantitateProcessata = cantitate,
				Data = DateTime.Now,
				ProduseRezultate = new List<ProdusLemn>
				{
					new ProdusLemn
					{
						TipProdus = produsRezultat,
						Cantitate = cantitate,
						Caracteristici = caracteristici
					}
				}
			};
			procesari.Add(procesareNoua);
			RefreshProduseVanzareDisponibile();

			double cantitateRamasa = cantitate;
			foreach (LemnBrut lemn in lemnDisponibil)
			{
				if (cantitateRamasa <= 0)
				{
					break;
				}

				double cantitateDeScazut = Math.Min(lemn.CantitateMc, cantitateRamasa);
				lemn.CantitateMc -= cantitateDeScazut;
				cantitateRamasa -= cantitateDeScazut;
				stocareLemn.UpdateLemnBrut(lemn);
			}

			txtMesajProcesare.Foreground = Brushes.Green;
			txtMesajProcesare.Text = $"Procesare adaugata! Lemn: {tipLemn}, Cantitate: {cantitate} mc, Produs: {produsRezultat}, Caracteristici: {caracteristici}";

			ResetareProcesare();
		}

		private void ResetareProcesare()
		{
			txtCantProcesare.Text = string.Empty;
			cmbTipProdusProcesare.SelectedIndex = 0;
			rdoMolid.IsChecked = false;
			rdoBrad.IsChecked = false;
			rdoFag.IsChecked = false;
			rdoStejar.IsChecked = false;
			rdoPin.IsChecked = false;
			chkUscat.IsChecked = false;
			chkTratat.IsChecked = false;
			chkLustruit.IsChecked = false;
			chkIgnifugat.IsChecked = false;
			chkCertificat.IsChecked = false;
		}

		// vanzare

		private void btnAdaugaVanzare_Click(object sender, RoutedEventArgs e)
		{
			lblClientVanzare.Foreground = Brushes.Black;
			lblProdusVanzare.Foreground = Brushes.Black;
			lblCantVanzare.Foreground = Brushes.Black;
			txtMesajVanzare.Text = string.Empty;

			Client client = cmbClientVanzare.SelectedItem as Client;
			string produs = cmbProdusVanzare.SelectedItem as string ?? string.Empty;
			string cantitateText = txtCantVanzare.Text.Trim();
			DateTime dataVanzare = dtpDataVanzare.SelectedDate ?? DateTime.Today;

			// Validare client
			if (client == null)
			{
				lblClientVanzare.Foreground = Brushes.Red;
				txtMesajVanzare.Foreground = Brushes.Red;
				txtMesajVanzare.Text = "Alegeti clientul!";
				return;
			}

			// Validare produs
			if (string.IsNullOrEmpty(produs))
			{
				lblProdusVanzare.Foreground = Brushes.Red;
				txtMesajVanzare.Foreground = Brushes.Red;
				txtMesajVanzare.Text = "Alegeti tipul produsului!";
				return;
			}

			// Validare cantitate
			if (string.IsNullOrEmpty(cantitateText))
			{
				lblCantVanzare.Foreground = Brushes.Red;
				txtMesajVanzare.Foreground = Brushes.Red;
				txtMesajVanzare.Text = "Introduceti cantitatea!";
				return;
			}
			if (!double.TryParse(cantitateText, out double cantitate))
			{
				lblCantVanzare.Foreground = Brushes.Red;
				txtMesajVanzare.Foreground = Brushes.Red;
				txtMesajVanzare.Text = "Cantitatea trebuie sa fie un numar!";
				return;
			}

			List<ProdusLemn> stocProdusDisponibil = procesari
				.SelectMany(procesare => procesare.ProduseRezultate)
				.Where(prod => prod.TipProdus.Equals(produs, System.StringComparison.OrdinalIgnoreCase) && prod.Cantitate > 0)
				.ToList();
			double cantitateDisponibila = stocProdusDisponibil.Sum(prod => prod.Cantitate);
			if (cantitate > cantitateDisponibila)
			{
				lblCantVanzare.Foreground = Brushes.Red;
				txtMesajVanzare.Foreground = Brushes.Red;
				txtMesajVanzare.Text = $"Nu exista suficient produs in stoc. Disponibil: {cantitateDisponibila} mc.";
				return;
			}

			double cantitateRamasa = cantitate;
			foreach (ProdusLemn produsStoc in stocProdusDisponibil)
			{
				if (cantitateRamasa <= 0)
				{
					break;
				}

				double cantitateDeScazut = Math.Min(produsStoc.Cantitate, cantitateRamasa);
				produsStoc.Cantitate -= cantitateDeScazut;
				cantitateRamasa -= cantitateDeScazut;
			}

			Vanzare vanzareNoua = new Vanzare
			{
				Id = vanzari.Count + 1,
				Client = client,
				Produs = new ProdusLemn { TipProdus = produs, Cantitate = cantitate, Caracteristici = CaracteristiciProdus.Niciuna },
				Cantitate = cantitate,
				Data = dataVanzare
			};
			vanzari.Add(vanzareNoua);

			txtMesajVanzare.Foreground = Brushes.Green;
			txtMesajVanzare.Text = $"Vanzare adaugata! Data: {dataVanzare:dd/MM/yyyy}, Client: {client.Nume}, Produs: {produs}, Cantitate: {cantitate} mc";

			RefreshProduseVanzareDisponibile();
			ResetareVanzare();
		}

		private void ResetareVanzare()
		{
			cmbClientVanzare.SelectedItem = null;
			cmbProdusVanzare.SelectedIndex = 0;
			txtCantVanzare.Text = string.Empty;
			dtpDataVanzare.SelectedDate = DateTime.Today;
		}

		private void btnAfiseazaVanzari_Click(object sender, RoutedEventArgs e)
		{
			dgVanzari.ItemsSource = null;
			dgVanzari.ItemsSource = vanzari.ToList();
		}

		private void btnAfiseazaProcesari_Click(object sender, RoutedEventArgs e)
		{
			dgProcesari.ItemsSource = null;
			dgProcesari.ItemsSource = procesari.ToList();
		}

		// ========== MODIFICA CLIENTI ==========
		private void btnMenuModificaClienti_Click(object sender, RoutedEventArgs e)
		{
			AscundeToatePanourile();
			panelModificaClienti.Visibility = Visibility.Visible;
			RefreshClientiModifica();
		}

		private void RefreshClientiModifica()
		{
			cmbClientModifica.ItemsSource = null;
			cmbClientModifica.ItemsSource = stocareClienti.GetClienti().ToList();
			cmbClientModifica.SelectedIndex = -1;
			AnuleazaModificareClient();
		}

		private void cmbClientModifica_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			Client clientSelectat = cmbClientModifica.SelectedItem as Client;
			if (clientSelectat != null)
			{
				txtNumeModifica.Text = clientSelectat.Nume;
				txtTelefonModifica.Text = clientSelectat.Telefon;
				txtEmailModifica.Text = clientSelectat.Email;
				dtpDataActualizare.SelectedDate = DateTime.Today;
				txtMesajModifica.Text = string.Empty;
			}
			else
			{
				AnuleazaModificareClient();
			}
		}

		private void btnSalveazaModificareClient_Click(object sender, RoutedEventArgs e)
		{
			Client clientSelectat = cmbClientModifica.SelectedItem as Client;
			if (clientSelectat == null)
			{
				txtMesajModifica.Foreground = Brushes.Red;
				txtMesajModifica.Text = "Selectati un client!";
				return;
			}

			string nume = txtNumeModifica.Text.Trim();
			string telefon = txtTelefonModifica.Text.Trim();
			string email = txtEmailModifica.Text.Trim();

			// Validare
			if (string.IsNullOrEmpty(nume))
			{
				txtMesajModifica.Foreground = Brushes.Red;
				txtMesajModifica.Text = "Introduceti numele!";
				return;
			}

			if (string.IsNullOrEmpty(telefon))
			{
				txtMesajModifica.Foreground = Brushes.Red;
				txtMesajModifica.Text = "Introduceti telefonul!";
				return;
			}
			if (telefon.Length != LUNGIME_TELEFON)
			{
				txtMesajModifica.Foreground = Brushes.Red;
				txtMesajModifica.Text = $"Telefonul trebuie sa aiba exact {LUNGIME_TELEFON} cifre!";
				return;
			}
			foreach (char c in telefon)
			{
				if (!char.IsDigit(c))
				{
					txtMesajModifica.Foreground = Brushes.Red;
					txtMesajModifica.Text = "Telefonul trebuie sa contina doar cifre!";
					return;
				}
			}

			if (string.IsNullOrEmpty(email))
			{
				txtMesajModifica.Foreground = Brushes.Red;
				txtMesajModifica.Text = "Introduceti emailul!";
				return;
			}
			if (!email.Contains("@") || !email.Contains("."))
			{
				txtMesajModifica.Foreground = Brushes.Red;
				txtMesajModifica.Text = "Emailul nu este valid! (ex: nume@domeniu.ro)";
				return;
			}

			// Actualizare client
			clientSelectat.Nume = nume;
			clientSelectat.Telefon = telefon;
			clientSelectat.Email = email;
			stocareClienti.UpdateClient(clientSelectat);
			RefreshClientiModifica();

			txtMesajModifica.Foreground = Brushes.Green;
			txtMesajModifica.Text = "Client actualizat cu succes!";
		}

		private void btnAnuleazaModificareClient_Click(object sender, RoutedEventArgs e)
		{
			AnuleazaModificareClient();
		}

		private void AnuleazaModificareClient()
		{
			cmbClientModifica.SelectedIndex = -1;
			txtNumeModifica.Text = string.Empty;
			txtTelefonModifica.Text = string.Empty;
			txtEmailModifica.Text = string.Empty;
			dtpDataActualizare.SelectedDate = DateTime.Today;
			txtMesajModifica.Text = string.Empty;
		}

		// ========== INTEGRATIONS: ListBox (LemnBrut) & DatePicker (Vanzare) ==========
		private void btnAdaugaLemn_Click_UpdateListBox(object sender, RoutedEventArgs e)
		{
			// ListBox automatically displays items from XAML ListBoxItems
			// To extract selected value: 
			// var selected = lstTipLemn.SelectedItem as ListBoxItem;
			// string tipLemn = selected?.Content.ToString();
		}
		
	}
}
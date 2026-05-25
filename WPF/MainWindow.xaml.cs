using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Gestionarea_lemnului_copie;
using LibrarieModele;
using NivelStocareDate;

namespace WPF
{
	public partial class MainWindow : Window
	{
		private IStocareClienti stocareClienti;
		private IStocareLemnBrut stocareLemn;
		private readonly List<Procesare> procesari = new List<Procesare>();
		private readonly List<ProdusLemn> produse = new List<ProdusLemn>();
		private readonly List<Vanzare> vanzari = new List<Vanzare>();
		private readonly ClientFormViewModel clientForm;

		public MainWindow()
		{
			InitializeComponent();
			clientForm = new ClientFormViewModel();
			DataContext = clientForm;
			stocareClienti = StocareFactory.GetStocareClienti();
			stocareLemn = StocareFactory.GetStocareLemnBrut();
			SetProduseDisponibile();
			RefreshClientiDisponibili();
			RefreshProduseVanzareDisponibile();
			RefreshLemnGrid();
		}

		private void SetProduseDisponibile()
		{
			string[] tipuri = new[] { "Scandura", "Grinda", "Cherestea", "Pal", "Bricheta" };
			cmbTipProdusProcesare.ItemsSource = tipuri;
			if (cmbTipProdusProcesare.Items.Count > 0)
			{
				cmbTipProdusProcesare.SelectedIndex = 0;
			}
		}

		private void RefreshClientiDisponibili()
		{
			try
			{
				var clienti = stocareClienti.GetClienti();
				cmbClientVanzare.ItemsSource = clienti;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Eroare la refresh clienti: {ex.Message}");
			}
		}

		private void RefreshProduseVanzareDisponibile()
		{
			cmbProdusVanzare.ItemsSource = null;
			cmbProdusVanzare.ItemsSource = produse;
			cmbProdusVanzare.DisplayMemberPath = "TipProdus";
		}

		private void btnMenuClienti_Click(object sender, RoutedEventArgs e)
		{
			panelClienti.Visibility = Visibility.Visible;
			panelLemnBrut.Visibility = Visibility.Collapsed;
			panelProcesare.Visibility = Visibility.Collapsed;
			panelVanzare.Visibility = Visibility.Collapsed;
		}

		private void btnMenuLemnBrut_Click(object sender, RoutedEventArgs e)
		{
			panelClienti.Visibility = Visibility.Collapsed;
			panelLemnBrut.Visibility = Visibility.Visible;
			panelProcesare.Visibility = Visibility.Collapsed;
			panelVanzare.Visibility = Visibility.Collapsed;
		}

		private void btnMenuProcesare_Click(object sender, RoutedEventArgs e)
		{
			panelClienti.Visibility = Visibility.Collapsed;
			panelLemnBrut.Visibility = Visibility.Collapsed;
			panelProcesare.Visibility = Visibility.Visible;
			panelVanzare.Visibility = Visibility.Collapsed;
		}

		private void btnMenuVanzare_Click(object sender, RoutedEventArgs e)
		{
			panelClienti.Visibility = Visibility.Collapsed;
			panelLemnBrut.Visibility = Visibility.Collapsed;
			panelProcesare.Visibility = Visibility.Collapsed;
			panelVanzare.Visibility = Visibility.Visible;
		}

		private void TipLemnProcesare_Checked(object sender, RoutedEventArgs e)
		{
			UpdateCantitateDisponibilaProcesare();
		}

		// Clienti
		private void btnAdaugaClient_Click(object sender, RoutedEventArgs e)
		{
			ResetClientInputs();
			clientForm.MarkAllTouched();
			if (!clientForm.IsValid)
			{
				string eroare = clientForm.GetFirstError();
				txtMesajClient.Text = string.IsNullOrWhiteSpace(eroare)
					? "Corecteaza campurile marcate."
					: eroare;
				txtMesajClient.Foreground = System.Windows.Media.Brushes.Red;
				return;
			}

			string nume = clientForm.Nume.Trim();
			try
			{
				var client = new Client
				{
					Nume = nume,
					Telefon = clientForm.Telefon.Trim(),
					Email = clientForm.Email.Trim()
				};
				stocareClienti.AddClient(client);

				txtMesajClient.Text = $" Client '{nume}' adaugat cu succes (ID: {client.Id})!";
				txtMesajClient.Foreground = System.Windows.Media.Brushes.Green;

				// Golire campuri
				clientForm.Clear();

				RefreshClientiDisponibili();
			}
			catch (Exception ex)
			{
				txtMesajClient.Text = $" Eroare: {ex.Message}";
				txtMesajClient.Foreground = System.Windows.Media.Brushes.Red;
			}
		}

		private void ResetClientInputs()
		{
			txtMesajClient.Foreground = System.Windows.Media.Brushes.Black;
			txtMesajClient.Text = "";
		}

		private void btnAfiseazaClienti_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var clienti = stocareClienti.GetClienti();
				dgClienti.ItemsSource = null;
				dgClienti.ItemsSource = clienti;
				txtMesajClient.Text = $"S-au afișat {clienti.Count} clienți.";
			}
			catch (Exception ex)
			{
				txtMesajClient.Text = $"Eroare la afișare: {ex.Message}";
			}
		}

		private void btnCautaClient_Click(object sender, RoutedEventArgs e)
		{
			string cautare = txtCautaClient.Text.Trim();
			if (string.IsNullOrEmpty(cautare))
			{
				txtMesajClient.Text = "Introduceti un nume pentru cautare.";
				txtMesajClient.Foreground = System.Windows.Media.Brushes.Red;
				return;
			}

			try
			{
				var clienti = stocareClienti.GetClienti();
				var rezultate = clienti
					.Where(c => !string.IsNullOrEmpty(c.Nume) &&
						c.Nume.StartsWith(cautare, StringComparison.OrdinalIgnoreCase))
					.ToList();

				dgClienti.ItemsSource = null;
				dgClienti.ItemsSource = rezultate;

				if (rezultate.Count == 0)
				{
					txtMesajClient.Text = "Nu s-au gasit clienti cu acest nume.";
					txtMesajClient.Foreground = System.Windows.Media.Brushes.Red;
				}
				else
				{
					txtMesajClient.Text = $"Gasiti {rezultate.Count} clienti.";
					txtMesajClient.Foreground = System.Windows.Media.Brushes.Green;
				}
			}
			catch (Exception ex)
			{
				txtMesajClient.Text = $"Eroare la cautare: {ex.Message}";
				txtMesajClient.Foreground = System.Windows.Media.Brushes.Red;
			}
		}

		// Lemn brut
		private void btnAdaugaLemn_Click(object sender, RoutedEventArgs e)
		{
			var selectedItem = lstTipLemn.SelectedItem as ListBoxItem;
			if (selectedItem == null)
			{
				txtMesajLemn.Text = "Selectați un tip de lemn!";
				return;
			}

			string? tipLemn = selectedItem.Content?.ToString();
			string cantitateText = txtCantitate.Text.Trim();

			if (string.IsNullOrEmpty(tipLemn))
			{
				txtMesajLemn.Text = "Tip lemn invalid!";
				return;
			}

			if (!double.TryParse(cantitateText, out double cantitate) || cantitate <= 0)
			{
				txtMesajLemn.Text = "Introduceți o cantitate validă!";
				return;
			}

			try
			{
				if (!Enum.TryParse(typeof(TipLemnEnum), tipLemn, out object? tipEnumObj) || tipEnumObj == null)
				{
					txtMesajLemn.Text = "Tip lemn invalid!";
					return;
				}

				TipLemnEnum tipEnum = (TipLemnEnum)tipEnumObj;
				LemnBrut lemnExistent = stocareLemn.GetLemnBrut(tipEnum);
				if (lemnExistent != null)
				{
					lemnExistent.CantitateMc += cantitate;
					stocareLemn.UpdateLemnBrut(lemnExistent);
				}
				else
				{
					var lemn = new LemnBrut
					{
						TipLemn = tipEnum,
						CantitateMc = cantitate
					};
					stocareLemn.AddLemnBrut(lemn);
				}

				txtMesajLemn.Text = "Lemn adăugat cu succes!";
				txtCantitate.Clear();
				lstTipLemn.SelectedIndex = -1;

				RefreshProduseVanzareDisponibile();
				RefreshLemnGrid();
			}
			catch (Exception ex)
			{
				txtMesajLemn.Text = $"Eroare: {ex.Message}";
			}
		}

		// Procesare
		private void btnAdaugaProcesare_Click(object sender, RoutedEventArgs e)
		{
			TipLemnEnum? tipSelectat = GetSelectedTipLemn();
			if (tipSelectat == null)
			{
				txtMesajProcesare.Text = "Selectati tipul de lemn!";
				return;
			}

			if (!double.TryParse(txtCantProcesare.Text.Trim(), out double cantitate) || cantitate <= 0)
			{
				txtMesajProcesare.Text = "Introduceti o cantitate valida!";
				return;
			}

			string? tipProdus = cmbTipProdusProcesare.SelectedItem as string;
			if (string.IsNullOrWhiteSpace(tipProdus))
			{
				txtMesajProcesare.Text = "Selectati tipul produsului!";
				return;
			}

			LemnBrut lemn = stocareLemn.GetLemnBrut(tipSelectat.Value);
			if (lemn == null)
			{
				txtMesajProcesare.Text = "Nu exista lemn brut in stoc pentru tipul selectat!";
				return;
			}

			if (cantitate > lemn.CantitateMc)
			{
				txtMesajProcesare.Text = "Cantitatea depaseste stocul disponibil!";
				return;
			}

			CaracteristiciProdus caracteristici = GetCaracteristiciSelectate();
			ProdusLemn produsExistent = produse.FirstOrDefault(p =>
				p.TipProdus.Equals(tipProdus, StringComparison.OrdinalIgnoreCase) &&
				p.Caracteristici == caracteristici);

			if (produsExistent == null)
			{
				produsExistent = new ProdusLemn
				{
					Id = produse.Count + 1,
					TipProdus = tipProdus,
					Cantitate = 0,
					Caracteristici = caracteristici
				};
				produse.Add(produsExistent);
			}

			produsExistent.Cantitate += cantitate;

			lemn.CantitateMc -= cantitate;
			stocareLemn.UpdateLemnBrut(lemn);

			Procesare procesare = new Procesare
			{
				Id = procesari.Count + 1,
				LemnInitial = lemn,
				CantitateProcessata = cantitate,
				Data = DateTime.Now
			};
			procesare.ProduseRezultate.Add(produsExistent);
			procesari.Add(procesare);
			RefreshProcesariGrid();

			txtMesajProcesare.Text = "Procesarea a fost inregistrata cu succes!";
			txtCantProcesare.Clear();
			ClearTipLemnSelection();
			if (cmbTipProdusProcesare.Items.Count > 0)
			{
				cmbTipProdusProcesare.SelectedIndex = 0;
			}
			ClearCaracteristici();
			RefreshLemnGrid();
			RefreshProduseVanzareDisponibile();
		}

		// Vanzare
		private void btnAdaugaVanzare_Click(object sender, RoutedEventArgs e)
		{
			Client client = cmbClientVanzare.SelectedItem as Client;
			if (client == null)
			{
				txtMesajVanzare.Text = "Selectati clientul!";
				return;
			}

			ProdusLemn produs = cmbProdusVanzare.SelectedItem as ProdusLemn;
			if (produs == null)
			{
				txtMesajVanzare.Text = "Selectati produsul!";
				return;
			}

			if (!double.TryParse(txtCantVanzare.Text.Trim(), out double cantitate) || cantitate <= 0)
			{
				txtMesajVanzare.Text = "Introduceti o cantitate valida!";
				return;
			}

			if (cantitate > produs.Cantitate)
			{
				txtMesajVanzare.Text = "Cantitatea depaseste stocul disponibil!";
				return;
			}

			DateTime data = dtpDataVanzare.SelectedDate ?? DateTime.Now;

			Vanzare vanzare = new Vanzare
			{
				Id = vanzari.Count + 1,
				Client = client,
				Produs = produs,
				Cantitate = cantitate,
				Data = data
			};

			produs.Cantitate -= cantitate;
			vanzari.Add(vanzare);
			RefreshVanzariGrid();

			txtMesajVanzare.Text = "Vanzare inregistrata cu succes!";
			txtCantVanzare.Clear();
			dtpDataVanzare.SelectedDate = null;
			RefreshProduseVanzareDisponibile();
			UpdateCantitateDisponibilaVanzare();
		}

		private void CmbProdusVanzare_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			UpdateCantitateDisponibilaVanzare();
		}

		private TipLemnEnum? GetSelectedTipLemn()
		{
			if (rdoMolid.IsChecked == true) return TipLemnEnum.Molid;
			if (rdoBrad.IsChecked == true) return TipLemnEnum.Brad;
			if (rdoFag.IsChecked == true) return TipLemnEnum.Fag;
			if (rdoStejar.IsChecked == true) return TipLemnEnum.Stejar;
			if (rdoPin.IsChecked == true) return TipLemnEnum.Pin;
			return null;
		}

		private void UpdateCantitateDisponibilaProcesare()
		{
			TipLemnEnum? tipSelectat = GetSelectedTipLemn();
			if (tipSelectat == null)
			{
				txtCantitateDisponibila.Text = string.Empty;
				return;
			}

			LemnBrut lemn = stocareLemn.GetLemnBrut(tipSelectat.Value);
			if (lemn == null)
			{
				txtCantitateDisponibila.Text = "Stoc disponibil: 0 mc";
				return;
			}

			txtCantitateDisponibila.Text = $"Stoc disponibil: {lemn.CantitateMc} mc";
		}

		private void ClearTipLemnSelection()
		{
			rdoMolid.IsChecked = false;
			rdoBrad.IsChecked = false;
			rdoFag.IsChecked = false;
			rdoStejar.IsChecked = false;
			rdoPin.IsChecked = false;
			txtCantitateDisponibila.Text = string.Empty;
		}

		private CaracteristiciProdus GetCaracteristiciSelectate()
		{
			CaracteristiciProdus caracteristici = CaracteristiciProdus.Niciuna;
			if (chkUscat.IsChecked == true) caracteristici |= CaracteristiciProdus.Uscat;
			if (chkTratat.IsChecked == true) caracteristici |= CaracteristiciProdus.Tratat;
			if (chkLustruit.IsChecked == true) caracteristici |= CaracteristiciProdus.Lustruit;
			if (chkCertificat.IsChecked == true) caracteristici |= CaracteristiciProdus.Certificat;
			return caracteristici;
		}

		private void ClearCaracteristici()
		{
			chkUscat.IsChecked = false;
			chkTratat.IsChecked = false;
			chkLustruit.IsChecked = false;
			chkCertificat.IsChecked = false;
		}

		private void UpdateCantitateDisponibilaVanzare()
		{
			ProdusLemn produs = cmbProdusVanzare.SelectedItem as ProdusLemn;
			if (produs == null)
			{
				txtCantitateDisponibilaVanzare.Text = string.Empty;
				return;
			}

			txtCantitateDisponibilaVanzare.Text = $"Stoc disponibil: {produs.Cantitate} mc";
		}

		private void RefreshProcesariGrid()
		{
			dgProcesari.ItemsSource = null;
			dgProcesari.ItemsSource = procesari;
		}

		private void RefreshVanzariGrid()
		{
			dgVanzari.ItemsSource = null;
			dgVanzari.ItemsSource = vanzari;
		}

		private void RefreshLemnGrid()
		{
			var lemn = stocareLemn.GetStocLemn()
				.Where(item => item.CantitateMc > 0)
				.Select((item, index) => new LemnBrut
				{
					Id = index + 1,
					TipLemn = item.TipLemn,
					CantitateMc = item.CantitateMc
				})
				.ToList();
			dgLemnBrut.ItemsSource = null;
			dgLemnBrut.ItemsSource = lemn;
		}
	}
}

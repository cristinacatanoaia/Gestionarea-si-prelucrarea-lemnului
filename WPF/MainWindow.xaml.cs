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
		private Client? selectedClient;
		private LemnBrut? selectedLemn;
		private ProdusLemn? selectedProdus;
		private Vanzare? selectedVanzare;
		private Procesare? selectedProcesare;

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
			RefreshProduseGrid();
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

		private void btnEditeazaClient_Click(object sender, RoutedEventArgs e)
		{
			ResetClientInputs();
			if (selectedClient == null)
			{
				txtMesajClient.Text = "Selectati un client din lista.";
				txtMesajClient.Foreground = System.Windows.Media.Brushes.Red;
				return;
			}

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

			try
			{
				Client actualizat = new Client
				{
					Id = selectedClient.Id,
					Nume = clientForm.Nume.Trim(),
					Telefon = clientForm.Telefon.Trim(),
					Email = clientForm.Email.Trim()
				};

				bool ok = stocareClienti.UpdateClient(actualizat);
				if (!ok)
				{
					txtMesajClient.Text = "Clientul nu a fost gasit pentru actualizare.";
					txtMesajClient.Foreground = System.Windows.Media.Brushes.Red;
					return;
				}

				txtMesajClient.Text = "Client actualizat cu succes.";
				txtMesajClient.Foreground = System.Windows.Media.Brushes.Green;
				clientForm.Clear();
				selectedClient = null;
				dgClienti.SelectedItem = null;
				RefreshClientiDisponibili();
				btnAfiseazaClienti_Click(sender, e);
			}
			catch (Exception ex)
			{
				txtMesajClient.Text = $"Eroare: {ex.Message}";
				txtMesajClient.Foreground = System.Windows.Media.Brushes.Red;
			}
		}

		private void btnStergeClient_Click(object sender, RoutedEventArgs e)
		{
			ResetClientInputs();
			if (selectedClient == null)
			{
				txtMesajClient.Text = "Selectati un client din lista.";
				txtMesajClient.Foreground = System.Windows.Media.Brushes.Red;
				return;
			}

			MessageBoxResult confirm = MessageBox.Show(
				$"Stergeti clientul '{selectedClient.Nume}'?",
				"Confirmare stergere",
				MessageBoxButton.YesNo,
				MessageBoxImage.Warning);

			if (confirm != MessageBoxResult.Yes)
			{
				return;
			}

			try
			{
				bool ok = stocareClienti.DeleteClient(selectedClient.Id);
				if (!ok)
				{
					txtMesajClient.Text = "Clientul nu a fost gasit pentru stergere.";
					txtMesajClient.Foreground = System.Windows.Media.Brushes.Red;
					return;
				}

				txtMesajClient.Text = "Client sters cu succes.";
				txtMesajClient.Foreground = System.Windows.Media.Brushes.Green;
				clientForm.Clear();
				selectedClient = null;
				dgClienti.SelectedItem = null;
				RefreshClientiDisponibili();
				btnAfiseazaClienti_Click(sender, e);
			}
			catch (Exception ex)
			{
				txtMesajClient.Text = $"Eroare: {ex.Message}";
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

		private void btnEditeazaLemn_Click(object sender, RoutedEventArgs e)
		{
			if (selectedLemn == null)
			{
				txtMesajLemn.Text = "Selectati lemnul din lista pentru editare.";
				return;
			}

			var selectedItem = lstTipLemn.SelectedItem as ListBoxItem;
			if (selectedItem == null)
			{
				txtMesajLemn.Text = "Selectati un tip de lemn!";
				return;
			}

			string? tipLemn = selectedItem.Content?.ToString();
			if (string.IsNullOrEmpty(tipLemn))
			{
				txtMesajLemn.Text = "Tip lemn invalid!";
				return;
			}

			if (!double.TryParse(txtCantitate.Text.Trim(), out double cantitate) || cantitate < 0)
			{
				txtMesajLemn.Text = "Introduceți o cantitate valida!";
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
				LemnBrut lemnActualizat = new LemnBrut
				{
					Id = selectedLemn.Id,
					TipLemn = tipEnum,
					CantitateMc = cantitate
				};

				bool ok = stocareLemn.UpdateLemnBrut(lemnActualizat);
				if (!ok)
				{
					txtMesajLemn.Text = "Lemnul nu a fost gasit pentru actualizare!";
					return;
				}

				txtMesajLemn.Text = "Lemn actualizat cu succes!";
				selectedLemn = null;
				dgLemnBrut.SelectedItem = null;
				txtCantitate.Clear();
				lstTipLemn.SelectedIndex = -1;
				RefreshLemnGrid();
				RefreshProduseVanzareDisponibile();
			}
			catch (Exception ex)
			{
				txtMesajLemn.Text = $"Eroare: {ex.Message}";
			}
		}

		private void btnStergeLemn_Click(object sender, RoutedEventArgs e)
		{
			if (selectedLemn == null)
			{
				txtMesajLemn.Text = "Selectati lemnul din lista pentru stergere.";
				return;
			}

			MessageBoxResult confirm = MessageBox.Show(
				$"Stergeti lemnul '{selectedLemn.TipLemn}'?",
				"Confirmare stergere",
				MessageBoxButton.YesNo,
				MessageBoxImage.Warning);

			if (confirm != MessageBoxResult.Yes)
			{
				return;
			}

			try
			{
				bool ok = stocareLemn.DeleteLemnBrut(selectedLemn.Id);
				if (!ok)
				{
					txtMesajLemn.Text = "Lemnul nu a fost gasit pentru stergere!";
					return;
				}

				txtMesajLemn.Text = "Lemn sters cu succes!";
				selectedLemn = null;
				dgLemnBrut.SelectedItem = null;
				txtCantitate.Clear();
				lstTipLemn.SelectedIndex = -1;
				RefreshLemnGrid();
				RefreshProduseVanzareDisponibile();
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
			RefreshProduseGrid();
		}

		private void btnEditeazaProcesare_Click(object sender, RoutedEventArgs e)
		{
			if (selectedProcesare == null)
			{
				txtMesajProcesare.Text = "Selectati o procesare din lista.";
				return;
			}

			TipLemnEnum? tipSelectat = GetSelectedTipLemn();
			if (tipSelectat == null)
			{
				txtMesajProcesare.Text = "Selectati tipul de lemn!";
				return;
			}

			if (!double.TryParse(txtCantProcesare.Text.Trim(), out double cantitateNoua) || cantitateNoua <= 0)
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

			CaracteristiciProdus caracteristici = GetCaracteristiciSelectate();
			LemnBrut lemnNou = stocareLemn.GetLemnBrut(tipSelectat.Value);
			if (lemnNou == null)
			{
				txtMesajProcesare.Text = "Nu exista lemn brut in stoc pentru tipul selectat!";
				return;
			}

			LemnBrut lemnVechi = selectedProcesare.LemnInitial;
			double cantitateVeche = selectedProcesare.CantitateProcessata;
			double disponibil = lemnNou.CantitateMc + (lemnVechi.TipLemn == lemnNou.TipLemn ? cantitateVeche : 0);
			if (cantitateNoua > disponibil)
			{
				txtMesajProcesare.Text = "Cantitatea depaseste stocul disponibil!";
				return;
			}

			ProdusLemn produsNou = produse.FirstOrDefault(p =>
				p.TipProdus.Equals(tipProdus, StringComparison.OrdinalIgnoreCase) &&
				p.Caracteristici == caracteristici);
			if (produsNou == null)
			{
				produsNou = new ProdusLemn
				{
					Id = produse.Count + 1,
					TipProdus = tipProdus,
					Cantitate = 0,
					Caracteristici = caracteristici
				};
				produse.Add(produsNou);
			}

			ProdusLemn produsVechi = selectedProcesare.ProduseRezultate.FirstOrDefault();
			if (produsVechi != null)
			{
				produsVechi.Cantitate -= cantitateVeche;
			}

			lemnVechi.CantitateMc += cantitateVeche;
			stocareLemn.UpdateLemnBrut(lemnVechi);

			lemnNou.CantitateMc -= cantitateNoua;
			stocareLemn.UpdateLemnBrut(lemnNou);
			produsNou.Cantitate += cantitateNoua;

			selectedProcesare.LemnInitial = lemnNou;
			selectedProcesare.CantitateProcessata = cantitateNoua;
			selectedProcesare.Data = DateTime.Now;
			selectedProcesare.ProduseRezultate.Clear();
			selectedProcesare.ProduseRezultate.Add(produsNou);

			txtMesajProcesare.Text = "Procesare actualizata cu succes!";
			selectedProcesare = null;
			dgProcesari.SelectedItem = null;
			ClearTipLemnSelection();
			txtCantProcesare.Clear();
			ClearCaracteristici();
			if (cmbTipProdusProcesare.Items.Count > 0)
			{
				cmbTipProdusProcesare.SelectedIndex = 0;
			}
			RefreshProcesariGrid();
			RefreshLemnGrid();
			RefreshProduseGrid();
			RefreshProduseVanzareDisponibile();
			UpdateCantitateDisponibilaVanzare();
		}

		private void btnStergeProcesare_Click(object sender, RoutedEventArgs e)
		{
			if (selectedProcesare == null)
			{
				txtMesajProcesare.Text = "Selectati o procesare din lista.";
				return;
			}

			MessageBoxResult confirm = MessageBox.Show(
				"Stergeti procesarea selectata?",
				"Confirmare stergere",
				MessageBoxButton.YesNo,
				MessageBoxImage.Warning);

			if (confirm != MessageBoxResult.Yes)
			{
				return;
			}

			LemnBrut lemn = selectedProcesare.LemnInitial;
			double cantitate = selectedProcesare.CantitateProcessata;
			ProdusLemn produs = selectedProcesare.ProduseRezultate.FirstOrDefault();
			if (produs != null)
			{
				produs.Cantitate -= cantitate;
			}

			lemn.CantitateMc += cantitate;
			stocareLemn.UpdateLemnBrut(lemn);
			procesari.Remove(selectedProcesare);
			selectedProcesare = null;

			txtMesajProcesare.Text = "Procesare stearsa cu succes!";
			selectedProcesare = null;
			dgProcesari.SelectedItem = null;
			ClearTipLemnSelection();
			txtCantProcesare.Clear();
			ClearCaracteristici();
			if (cmbTipProdusProcesare.Items.Count > 0)
			{
				cmbTipProdusProcesare.SelectedIndex = 0;
			}
			RefreshProcesariGrid();
			RefreshLemnGrid();
			RefreshProduseGrid();
			RefreshProduseVanzareDisponibile();
			UpdateCantitateDisponibilaVanzare();
		}

		private void btnEditeazaProdus_Click(object sender, RoutedEventArgs e)
		{
			if (selectedProdus == null)
			{
				txtMesajProdus.Text = "Selectati un produs din lista.";
				txtMesajProdus.Foreground = System.Windows.Media.Brushes.Red;
				return;
			}

			if (!double.TryParse(txtCantitateProdusEdit.Text.Trim(), out double cantitate) || cantitate < 0)
			{
				txtMesajProdus.Text = "Introduceti o cantitate valida.";
				txtMesajProdus.Foreground = System.Windows.Media.Brushes.Red;
				return;
			}

			selectedProdus.Cantitate = cantitate;
			txtMesajProdus.Text = "Produs actualizat cu succes.";
			txtMesajProdus.Foreground = System.Windows.Media.Brushes.Green;
			selectedProdus = null;
			dgProduse.SelectedItem = null;
			txtCantitateProdusEdit.Clear();
			RefreshProduseGrid();
			RefreshProduseVanzareDisponibile();
			UpdateCantitateDisponibilaVanzare();
		}

		private void btnStergeProdus_Click(object sender, RoutedEventArgs e)
		{
			if (selectedProdus == null)
			{
				txtMesajProdus.Text = "Selectati un produs din lista.";
				txtMesajProdus.Foreground = System.Windows.Media.Brushes.Red;
				return;
			}

			bool areVanzari = vanzari.Any(v => v.Produs == selectedProdus);
			if (areVanzari)
			{
				txtMesajProdus.Text = "Produsul are vanzari asociate si nu poate fi sters.";
				txtMesajProdus.Foreground = System.Windows.Media.Brushes.Red;
				return;
			}

			MessageBoxResult confirm = MessageBox.Show(
				$"Stergeti produsul '{selectedProdus.TipProdus}'?",
				"Confirmare stergere",
				MessageBoxButton.YesNo,
				MessageBoxImage.Warning);

			if (confirm != MessageBoxResult.Yes)
			{
				return;
			}

			produse.Remove(selectedProdus);
			selectedProdus = null;
			dgProduse.SelectedItem = null;
			txtCantitateProdusEdit.Clear();
			txtMesajProdus.Text = "Produs sters cu succes.";
			txtMesajProdus.Foreground = System.Windows.Media.Brushes.Green;
			RefreshProduseGrid();
			RefreshProduseVanzareDisponibile();
			UpdateCantitateDisponibilaVanzare();
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
			RefreshClientiDisponibili();
			cmbClientVanzare.SelectedItem = null;
			UpdateCantitateDisponibilaVanzare();
		}

		private void btnEditeazaVanzare_Click(object sender, RoutedEventArgs e)
		{
			if (selectedVanzare == null)
			{
				txtMesajVanzare.Text = "Selectati o vanzare din lista.";
				return;
			}

			Client client = cmbClientVanzare.SelectedItem as Client;
			if (client == null)
			{
				txtMesajVanzare.Text = "Selectati clientul!";
				return;
			}

			ProdusLemn produsNou = cmbProdusVanzare.SelectedItem as ProdusLemn;
			if (produsNou == null)
			{
				txtMesajVanzare.Text = "Selectati produsul!";
				return;
			}

			if (!double.TryParse(txtCantVanzare.Text.Trim(), out double cantitateNoua) || cantitateNoua <= 0)
			{
				txtMesajVanzare.Text = "Introduceti o cantitate valida!";
				return;
			}

			DateTime data = dtpDataVanzare.SelectedDate ?? DateTime.Now;

			selectedVanzare.Produs.Cantitate += selectedVanzare.Cantitate;
			if (cantitateNoua > produsNou.Cantitate)
			{
				selectedVanzare.Produs.Cantitate -= selectedVanzare.Cantitate;
				txtMesajVanzare.Text = "Cantitatea depaseste stocul disponibil!";
				return;
			}

			produsNou.Cantitate -= cantitateNoua;
			selectedVanzare.Client = client;
			selectedVanzare.Produs = produsNou;
			selectedVanzare.Cantitate = cantitateNoua;
			selectedVanzare.Data = data;

			txtMesajVanzare.Text = "Vanzare actualizata cu succes!";
			selectedVanzare = null;
			dgVanzari.SelectedItem = null;
			dtpDataVanzare.SelectedDate = null;
			cmbClientVanzare.SelectedItem = null;
			cmbProdusVanzare.SelectedItem = null;
			txtCantVanzare.Clear();
			RefreshVanzariGrid();
			RefreshProduseVanzareDisponibile();
			UpdateCantitateDisponibilaVanzare();
		}

		private void btnStergeVanzare_Click(object sender, RoutedEventArgs e)
		{
			if (selectedVanzare == null)
			{
				txtMesajVanzare.Text = "Selectati o vanzare din lista.";
				return;
			}

			MessageBoxResult confirm = MessageBox.Show(
				"Stergeti vanzarea selectata?",
				"Confirmare stergere",
				MessageBoxButton.YesNo,
				MessageBoxImage.Warning);

			if (confirm != MessageBoxResult.Yes)
			{
				return;
			}

			selectedVanzare.Produs.Cantitate += selectedVanzare.Cantitate;
			vanzari.Remove(selectedVanzare);
			selectedVanzare = null;
			dgVanzari.SelectedItem = null;
			dtpDataVanzare.SelectedDate = null;
			cmbClientVanzare.SelectedItem = null;
			cmbProdusVanzare.SelectedItem = null;
			txtCantVanzare.Clear();
			txtMesajVanzare.Text = "Vanzare stearsa cu succes!";
			RefreshVanzariGrid();
			RefreshProduseVanzareDisponibile();
			UpdateCantitateDisponibilaVanzare();
		}

		private void CmbProdusVanzare_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			UpdateCantitateDisponibilaVanzare();
		}

		private void dgClienti_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			selectedClient = dgClienti.SelectedItem as Client;
			if (selectedClient == null)
			{
				return;
			}

			clientForm.Nume = selectedClient.Nume;
			clientForm.Telefon = selectedClient.Telefon;
			clientForm.Email = selectedClient.Email;
		}

		private void dgLemnBrut_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			selectedLemn = dgLemnBrut.SelectedItem as LemnBrut;
			if (selectedLemn == null)
			{
				return;
			}

			txtCantitate.Text = selectedLemn.CantitateMc.ToString();
			foreach (ListBoxItem item in lstTipLemn.Items)
			{
				if (item.Content?.ToString() == selectedLemn.TipLemn.ToString())
				{
					lstTipLemn.SelectedItem = item;
					break;
				}
			}
		}

		private void dgProduse_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			selectedProdus = dgProduse.SelectedItem as ProdusLemn;
			if (selectedProdus == null)
			{
				return;
			}

			txtCantitateProdusEdit.Text = selectedProdus.Cantitate.ToString();
		}

		private void dgVanzari_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			selectedVanzare = dgVanzari.SelectedItem as Vanzare;
			if (selectedVanzare == null)
			{
				return;
			}

			dtpDataVanzare.SelectedDate = selectedVanzare.Data;
			cmbClientVanzare.SelectedItem = selectedVanzare.Client;
			cmbProdusVanzare.SelectedItem = selectedVanzare.Produs;
			txtCantVanzare.Text = selectedVanzare.Cantitate.ToString();
			UpdateCantitateDisponibilaVanzare();
		}

		private void dgProcesari_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			selectedProcesare = dgProcesari.SelectedItem as Procesare;
			if (selectedProcesare == null)
			{
				return;
			}

			SetTipLemnSelection(selectedProcesare.LemnInitial?.TipLemn);
			txtCantProcesare.Text = selectedProcesare.CantitateProcessata.ToString();

			ProdusLemn produs = selectedProcesare.ProduseRezultate.FirstOrDefault();
			if (produs != null)
			{
				cmbTipProdusProcesare.SelectedItem = produs.TipProdus;
				SetCaracteristiciFromProdus(produs.Caracteristici);
			}
			UpdateCantitateDisponibilaProcesare();
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

		private void SetCaracteristiciFromProdus(CaracteristiciProdus caracteristici)
		{
			chkUscat.IsChecked = caracteristici.HasFlag(CaracteristiciProdus.Uscat);
			chkTratat.IsChecked = caracteristici.HasFlag(CaracteristiciProdus.Tratat);
			chkLustruit.IsChecked = caracteristici.HasFlag(CaracteristiciProdus.Lustruit);
			chkCertificat.IsChecked = caracteristici.HasFlag(CaracteristiciProdus.Certificat);
		}

		private void SetTipLemnSelection(TipLemnEnum? tip)
		{
			ClearTipLemnSelection();
			if (tip == null)
			{
				return;
			}

			switch (tip.Value)
			{
				case TipLemnEnum.Molid:
					rdoMolid.IsChecked = true;
					break;
				case TipLemnEnum.Brad:
					rdoBrad.IsChecked = true;
					break;
				case TipLemnEnum.Fag:
					rdoFag.IsChecked = true;
					break;
				case TipLemnEnum.Stejar:
					rdoStejar.IsChecked = true;
					break;
				case TipLemnEnum.Pin:
					rdoPin.IsChecked = true;
					break;
			}
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

		private void RefreshProduseGrid()
		{
			dgProduse.ItemsSource = null;
			dgProduse.ItemsSource = produse;
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
				.ToList();
			dgLemnBrut.ItemsSource = null;
			dgLemnBrut.ItemsSource = lemn;
		}
	}
}

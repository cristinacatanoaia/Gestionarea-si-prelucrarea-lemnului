using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using LibrarieModele;

namespace NivelUIWPF.Pages;

public partial class ProdusLemnPage : Page
{
    private ObservableCollection<ProdusLemn> produsLemnList = new();

    public ProdusLemnPage()
    {
        InitializeComponent();
        LoadSampleData();
        ProdusLemnDataGrid.ItemsSource = produsLemnList;
        ProdusLemnDataGrid.SelectionChanged += ProdusLemnDataGrid_SelectionChanged;
    }

    private void LoadSampleData()
    {
        produsLemnList.Add(new ProdusLemn { Id = 1, TipProdus = "Scânduri Molid", Cantitate = 25.5, Caracteristici = CaracteristiciProdus.Uscat });
        produsLemnList.Add(new ProdusLemn { Id = 2, TipProdus = "Grinzi Brad", Cantitate = 15.0, Caracteristici = CaracteristiciProdus.Uscat | CaracteristiciProdus.Tratat });
        produsLemnList.Add(new ProdusLemn { Id = 3, TipProdus = "Parchet Fag", Cantitate = 8.5, Caracteristici = CaracteristiciProdus.Lustruit | CaracteristiciProdus.Certificat });
        produsLemnList.Add(new ProdusLemn { Id = 4, TipProdus = "Grinzi Stejar", Cantitate = 18.0, Caracteristici = CaracteristiciProdus.Ignifugat });
    }

    private void AddProdusLemn(object sender, RoutedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(IdTextBox.Text) || string.IsNullOrWhiteSpace(TipProdusTextBox.Text) || string.IsNullOrWhiteSpace(CantitateTextBox.Text))
            {
                MessageBox.Show("Vă rugăm completați câmpurile: ID, Tip Produs, Cantitate", "Validare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int id = int.Parse(IdTextBox.Text);
            string tipProdus = TipProdusTextBox.Text;
            double cantitate = double.Parse(CantitateTextBox.Text);

            CaracteristiciProdus caracteristici = CaracteristiciProdus.Niciuna;
            if (UscatCheck.IsChecked == true) caracteristici |= CaracteristiciProdus.Uscat;
            if (TratatCheck.IsChecked == true) caracteristici |= CaracteristiciProdus.Tratat;
            if (LustruitCheck.IsChecked == true) caracteristici |= CaracteristiciProdus.Lustruit;
            if (IgnifugatCheck.IsChecked == true) caracteristici |= CaracteristiciProdus.Ignifugat;
            if (CertificatCheck.IsChecked == true) caracteristici |= CaracteristiciProdus.Certificat;

            ProdusLemn nou = new ProdusLemn { Id = id, TipProdus = tipProdus, Cantitate = cantitate, Caracteristici = caracteristici };
            produsLemnList.Add(nou);

            IdTextBox.Clear();
            TipProdusTextBox.Clear();
            CantitateTextBox.Clear();
            UscatCheck.IsChecked = false;
            TratatCheck.IsChecked = false;
            LustruitCheck.IsChecked = false;
            IgnifugatCheck.IsChecked = false;
            CertificatCheck.IsChecked = false;

            MessageBox.Show("Produs lemn adăugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Eroare: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void DeleteProdusLemn(object sender, RoutedEventArgs e)
    {
        if (ProdusLemnDataGrid.SelectedItem is ProdusLemn selected)
        {
            produsLemnList.Remove(selected);
            MessageBox.Show("Articol șters cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show("Selectați un articol pentru a-l șterge", "Validare", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void RefreshProdusLemnList(object sender, RoutedEventArgs e)
    {
        ProdusLemnDataGrid.Items.Refresh();
    }

    private void ProdusLemnDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ProdusLemnDataGrid.SelectedItem is ProdusLemn selected)
        {
            SelectedInfoBlock.Text = $"ID: {selected.Id}" + Environment.NewLine +
                                     $"Tip Produs: {selected.TipProdus}" + Environment.NewLine +
                                     $"Cantitate: {selected.Cantitate} tone" + Environment.NewLine +
                                     $"Caracteristici: {selected.Caracteristici}";
        }
        else
        {
            SelectedInfoBlock.Text = "Selectați un articol din listă pentru a vedea detaliile";
        }
    }
}

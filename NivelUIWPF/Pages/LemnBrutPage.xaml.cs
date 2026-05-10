using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using LibrarieModele;

namespace NivelUIWPF.Pages;

public partial class LemnBrutPage : Page
{
    private ObservableCollection<LemnBrut> lemnBrutList = new();

    public LemnBrutPage()
    {
        InitializeComponent();
        LoadSampleData();
        LemnBrutDataGrid.ItemsSource = lemnBrutList;
        LemnBrutDataGrid.SelectionChanged += LemnBrutDataGrid_SelectionChanged;
    }

    private void LoadSampleData()
    {
        lemnBrutList.Add(new LemnBrut { Id = 1, TipLemn = TipLemnEnum.Molid, CantitateMc = 45.5 });
        lemnBrutList.Add(new LemnBrut { Id = 2, TipLemn = TipLemnEnum.Brad, CantitateMc = 32.0 });
        lemnBrutList.Add(new LemnBrut { Id = 3, TipLemn = TipLemnEnum.Fag, CantitateMc = 28.5 });
        lemnBrutList.Add(new LemnBrut { Id = 4, TipLemn = TipLemnEnum.Stejar, CantitateMc = 55.0 });
        lemnBrutList.Add(new LemnBrut { Id = 5, TipLemn = TipLemnEnum.Pin, CantitateMc = 40.0 });
    }

    private void AddLemnBrut(object sender, RoutedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(IdTextBox.Text) || TipLemnComboBox.SelectedIndex < 0 || string.IsNullOrWhiteSpace(CantitateTextBox.Text))
            {
                MessageBox.Show("Vă rugăm completați toate câmpurile", "Validare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int id = int.Parse(IdTextBox.Text);
            TipLemnEnum tipLemn = (TipLemnEnum)Enum.Parse(typeof(TipLemnEnum), TipLemnComboBox.SelectedItem.ToString());
            double cantitate = double.Parse(CantitateTextBox.Text);

            LemnBrut nou = new LemnBrut { Id = id, TipLemn = tipLemn, CantitateMc = cantitate };
            lemnBrutList.Add(nou);

            IdTextBox.Clear();
            TipLemnComboBox.SelectedIndex = -1;
            CantitateTextBox.Clear();

            MessageBox.Show("Lemn brut adăugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Eroare: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void DeleteLemnBrut(object sender, RoutedEventArgs e)
    {
        if (LemnBrutDataGrid.SelectedItem is LemnBrut selected)
        {
            lemnBrutList.Remove(selected);
            MessageBox.Show("Articol șters cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show("Selectați un articol pentru a-l șterge", "Validare", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void RefreshLemnBrutList(object sender, RoutedEventArgs e)
    {
        LemnBrutDataGrid.Items.Refresh();
    }

    private void LemnBrutDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LemnBrutDataGrid.SelectedItem is LemnBrut selected)
        {
            SelectedInfoBlock.Text = $"ID: {selected.Id}" + Environment.NewLine +
                                     $"Tip Lemn: {selected.TipLemn}" + Environment.NewLine +
                                     $"Cantitate: {selected.CantitateMc} m³";
        }
        else
        {
            SelectedInfoBlock.Text = "Selectați un articol din listă pentru a vedea detaliile";
        }
    }
}

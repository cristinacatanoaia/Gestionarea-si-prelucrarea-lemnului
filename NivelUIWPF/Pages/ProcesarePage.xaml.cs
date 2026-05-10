using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using GestionareLemn;
using LibrarieModele;

namespace NivelUIWPF.Pages;

public partial class ProcesarePage : Page
{
    private ObservableCollection<Procesare> procesareList = new();

    public ProcesarePage()
    {
        InitializeComponent();
        LoadSampleData();
        ProcesareDataGrid.ItemsSource = procesareList;
        ProcesareDataGrid.SelectionChanged += ProcesareDataGrid_SelectionChanged;
        DataPicker.SelectedDate = DateTime.Now;
    }

    private void LoadSampleData()
    {
        procesareList.Add(new Procesare 
        { 
            Id = 1, 
            LemnInitial = new LemnBrut { Id = 1, TipLemn = TipLemnEnum.Molid, CantitateMc = 45.5 },
            CantitateProcessata = 42.0,
            Data = DateTime.Now.AddDays(-5),
            ProduseRezultate = new List<ProdusLemn> { new ProdusLemn { Id = 1, TipProdus = "Scânduri", Cantitate = 42 } }
        });

        procesareList.Add(new Procesare 
        { 
            Id = 2, 
            LemnInitial = new LemnBrut { Id = 2, TipLemn = TipLemnEnum.Brad, CantitateMc = 32.0 },
            CantitateProcessata = 30.5,
            Data = DateTime.Now.AddDays(-3),
            ProduseRezultate = new List<ProdusLemn> { new ProdusLemn { Id = 2, TipProdus = "Grinzi", Cantitate = 30.5 } }
        });

        procesareList.Add(new Procesare 
        { 
            Id = 3, 
            LemnInitial = new LemnBrut { Id = 3, TipLemn = TipLemnEnum.Stejar, CantitateMc = 55.0 },
            CantitateProcessata = 52.0,
            Data = DateTime.Now.AddDays(-1),
            ProduseRezultate = new List<ProdusLemn> { new ProdusLemn { Id = 3, TipProdus = "Parchet", Cantitate = 52 } }
        });
    }

    private void AddProcesare(object sender, RoutedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(IdTextBox.Text) || string.IsNullOrWhiteSpace(CantitateTextBox.Text))
            {
                MessageBox.Show("Vă rugăm completați ID și Cantitate", "Validare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int id = int.Parse(IdTextBox.Text);
            double cantitate = double.Parse(CantitateTextBox.Text);
            DateTime data = DataPicker.SelectedDate ?? DateTime.Now;

            Procesare nou = new Procesare 
            { 
                Id = id, 
                CantitateProcessata = cantitate,
                Data = data,
                LemnInitial = new LemnBrut { Id = 1, TipLemn = TipLemnEnum.Molid, CantitateMc = cantitate },
                ProduseRezultate = new List<ProdusLemn>()
            };
            procesareList.Add(nou);

            IdTextBox.Clear();
            CantitateTextBox.Clear();
            DataPicker.SelectedDate = DateTime.Now;

            MessageBox.Show("Procesare adăugată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Eroare: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void DeleteProcesare(object sender, RoutedEventArgs e)
    {
        if (ProcesareDataGrid.SelectedItem is Procesare selected)
        {
            procesareList.Remove(selected);
            MessageBox.Show("Procesare ștearsă cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show("Selectați o procesare pentru a o șterge", "Validare", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void RefreshProcesareList(object sender, RoutedEventArgs e)
    {
        ProcesareDataGrid.Items.Refresh();
    }

    private void ProcesareDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ProcesareDataGrid.SelectedItem is Procesare selected)
        {
            string produseInfo = selected.ProduseRezultate.Count > 0 
                ? string.Join(", ", selected.ProduseRezultate.Select(p => p.TipProdus))
                : "Fără produse";

            SelectedInfoBlock.Text = $"ID: {selected.Id}" + Environment.NewLine +
                                     $"Lemn Inițial: {selected.LemnInitial?.TipLemn}" + Environment.NewLine +
                                     $"Cantitate Procesată: {selected.CantitateProcessata} m³" + Environment.NewLine +
                                     $"Data: {selected.Data:dd/MM/yyyy}" + Environment.NewLine +
                                     $"Produse Rezultate: {produseInfo}";
        }
        else
        {
            SelectedInfoBlock.Text = "Selectați o procesare din listă pentru a vedea detaliile";
        }
    }
}

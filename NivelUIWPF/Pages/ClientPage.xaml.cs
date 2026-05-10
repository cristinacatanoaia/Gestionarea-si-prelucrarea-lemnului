using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using LibrarieModele;

namespace NivelUIWPF.Pages;

public partial class ClientPage : Page
{
    private ObservableCollection<Client> clientList = new();

    public ClientPage()
    {
        InitializeComponent();
        LoadSampleData();
        ClientDataGrid.ItemsSource = clientList;
        ClientDataGrid.SelectionChanged += ClientDataGrid_SelectionChanged;
    }

    private void LoadSampleData()
    {
        clientList.Add(new Client { Id = 1, Nume = "Ion Popescu", Telefon = "0722123456", Email = "ion.popescu@email.com" });
        clientList.Add(new Client { Id = 2, Nume = "Maria Georgescu", Telefon = "0733234567", Email = "maria.georgescu@email.com" });
        clientList.Add(new Client { Id = 3, Nume = "Alexandru Ionescu", Telefon = "0744345678", Email = "alex.ionescu@email.com" });
        clientList.Add(new Client { Id = 4, Nume = "Cristina Bălan", Telefon = "0755456789", Email = "cristina.balan@email.com" });
        clientList.Add(new Client { Id = 5, Nume = "Vlad Mîndru", Telefon = "0766567890", Email = "vlad.mindru@email.com" });
    }

    private void AddClient(object sender, RoutedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(IdTextBox.Text) || string.IsNullOrWhiteSpace(NumeTextBox.Text))
            {
                MessageBox.Show("Vă rugăm completați cel puțin ID și Nume", "Validare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int id = int.Parse(IdTextBox.Text);
            string nume = NumeTextBox.Text;
            string telefon = TelefonTextBox.Text ?? "NECUNOSCUT";
            string email = EmailTextBox.Text ?? "NECUNOSCUT";

            Client nou = new Client { Id = id, Nume = nume, Telefon = telefon, Email = email };
            clientList.Add(nou);

            IdTextBox.Clear();
            NumeTextBox.Clear();
            TelefonTextBox.Clear();
            EmailTextBox.Clear();

            MessageBox.Show("Client adăugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Eroare: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void DeleteClient(object sender, RoutedEventArgs e)
    {
        if (ClientDataGrid.SelectedItem is Client selected)
        {
            clientList.Remove(selected);
            MessageBox.Show("Client șters cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show("Selectați un client pentru a-l șterge", "Validare", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void RefreshClientList(object sender, RoutedEventArgs e)
    {
        ClientDataGrid.Items.Refresh();
    }

    private void ClientDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ClientDataGrid.SelectedItem is Client selected)
        {
            SelectedInfoBlock.Text = $"ID: {selected.Id}" + Environment.NewLine +
                                     $"Nume: {selected.Nume}" + Environment.NewLine +
                                     $"Telefon: {selected.Telefon}" + Environment.NewLine +
                                     $"Email: {selected.Email}";
        }
        else
        {
            SelectedInfoBlock.Text = "Selectați un client din listă pentru a vedea detaliile";
        }
    }
}

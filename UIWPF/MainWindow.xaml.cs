using System.Windows;
using System.Windows.Controls;
using LibrarieModele;
using NivelStocareDate;

namespace UIWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private AdministrareClientiMemorie administrare = new AdministrareClientiMemorie();
		List<Client> clients = new List<Client>();				

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			clients = administrare.GetClienti();
			ListView.ItemsSource = clients;	
		}

		private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}
	}
}
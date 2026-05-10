using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibrarieModele;
using NivelStocareDate;
using GestionareLemn;

namespace WPF
{
	
	public partial class MainWindow : Window
	{
		private IStocareClienti stocareClienti;

		public MainWindow()
		{
			InitializeComponent();
			stocareClienti = StocareFactory.GetStocareClienti();
			AdaugaDateInitiale();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var clienti = stocareClienti.GetClienti();
			ClientiListView.ItemsSource = clienti;
		}

		private void AdaugaDateInitiale()
		{
			if (stocareClienti.GetClienti().Count == 0)
			{
				stocareClienti.AddClient(new Client { Id = 1, Nume = "Construct SRL", Telefon = "0740123456", Email = "contact@construct.ro" });
				stocareClienti.AddClient(new Client { Id = 2, Nume = "Casa Lemn SRL", Telefon = "0751987654", Email = "office@casalemn.ro" });
			}
		}
	}
}
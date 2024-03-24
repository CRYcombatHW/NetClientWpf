using NetworkService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NetClientWpf
{
	/// <summary>
	/// Логика взаимодействия для ConnectionWindow.xaml
	/// </summary>
	public partial class ConnectionWindow : Window
	{
		public Client? Client { get; private set; }

		public ConnectionWindow() {
			Client = null;

			InitializeComponent();
		}

		private async void Button_Connect_Click(object sender, RoutedEventArgs e) {
			if (!IPAddress.TryParse(TextBox_Address.Text, out IPAddress address)) {
				MessageBox.Show("Incorrect ip address");
				return;
			}
			if (!int.TryParse(TextBox_Port.Text, out int port)) {
				MessageBox.Show("Incorrect ip port");
				return;
			}

			if (await TryCreateClient(new IPEndPoint(
				address,
				port
			))) {
				Close();
			}
			else {
				MessageBox.Show("Cant connect to that server");
			}
		}
		private async void Button_ConnectToRiddleServer_Click(object sender, RoutedEventArgs e) {
			if (await TryCreateClient(ConfigurationManager.RiddleServerEndpoint)) {
				Close();
			}
			else {
				MessageBox.Show("Cant connect to that server");
			}
		}
		private async Task<bool> TryCreateClient(IPEndPoint endPoint) {
			Client = new Client();
			if (!await Client.TryConnectAsync(endPoint)) {
				return false;
			}
			return true;
		}
	}
}

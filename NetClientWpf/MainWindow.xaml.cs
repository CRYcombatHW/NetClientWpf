using NetworkService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetClientWpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Client _client;

		public MainWindow(Client client) {
			_client = client;
			_client.OnMessage += Client_OnMessage;
			_client.OnDisconnect += Client_OnDisconnect;

			InitializeComponent();

			Dispatcher.Invoke(_client.ListenMessages);
		}

		private void Client_OnDisconnect() {
			Close();
		}

		private void Client_OnMessage(NetworkMessage message) {
            AddMessage(message);
		}

		public void AddMessage(NetworkMessage message, bool ownMessage = false) {
			MessageElement messageElement = new MessageElement(message);
			if (ownMessage) {
				messageElement.Margin = new Thickness(200, 4, 4, 0);
				messageElement.HorizontalAlignment = HorizontalAlignment.Right;
			}
			else {
				messageElement.Margin = new Thickness(4, 4, 200, 0);
				messageElement.HorizontalAlignment = HorizontalAlignment.Left;
			}

			StackPanel_Dialog.Children.Add(messageElement);
			ScrollViewer_Dialog.ScrollToBottom();
		}

		private async void Button_Send_Click(object sender, RoutedEventArgs e) {
			await SendMessage();
		}

		private async void TextBox_Message_KeyDown(object sender, KeyEventArgs e) {
			if (e.Key == Key.Enter) {
				await SendMessage();
			}
		}

		private async Task SendMessage() {
			if (TextBox_Message.Text.Length == 0) {
				return;
			}

			NetworkMessage message = new NetworkMessage("Client", TextBox_Message.Text);
			TextBox_Message.Text = "";

			await _client.SendMessage(message);
			AddMessage(message, true);
		}
	}
}

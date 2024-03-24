using ClientLib;
using NetworkService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
		private NetworkClient _client;

		public MainWindow(NetworkClient client) {
			_client = client;
			_client.OnMessage += Client_OnMessage;
			_client.OnDisconnection += Client_OnDisconnection;

			InitializeComponent();

			Dispatcher.Invoke(_client.Listen);
		}

		private void Client_OnDisconnection(Socket sender) {
			Close();
		}

		private void Client_OnMessage(Socket sender, NetworkMessage message) {
            AddMessage(message);
		}

		public void AddMessage(NetworkMessage message, bool ownMessage = false) {
			//SystenNetMessage

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

			NetworkMessage message = new NetworkMessage("You", TextBox_Message.Text);
			TextBox_Message.Text = "";

			await _client.SendMessage(message);
			AddMessage(message, true);
		}
	}
}

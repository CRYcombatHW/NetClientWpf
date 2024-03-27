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
		
		private string _name;

		

		public MainWindow(NetworkClient client) {
			_client = client;
			_client.OnMessage += Client_OnMessage;
			_client.OnDisconnection += Client_OnDisconnection;

			InitializeComponent();

			_name = $"Guest{new Random().Next(1000, 10000)}";
			Dispatcher.Invoke(_client.Listen);
		}

		private void Client_OnDisconnection(Socket sender) {
			Close();
		}

		private void Client_OnMessage(Socket sender, NetworkMessage message) {
			if (message.Header == "Service") {
				AddMessage(message, MessageType.Service);
			}
            else {
				AddMessage(message, MessageType.Foreign);
			}
		}

		public enum MessageType {
			Ours,
			Foreign,
			Service,
		}
		public void AddMessage(NetworkMessage message, MessageType type = MessageType.Ours) {
			UserControl newControl;

			switch (type) {
				case MessageType.Ours: {
					newControl = new MessageElement(message);
					newControl.Margin = new Thickness(200, 4, 4, 0);
					newControl.HorizontalAlignment = HorizontalAlignment.Right;
				} break;
				case MessageType.Foreign: {
					newControl = new MessageElement(message);
					newControl.Margin = new Thickness(4, 4, 200, 0);
					newControl.HorizontalAlignment = HorizontalAlignment.Left;
				} break;
				case MessageType.Service: {
					newControl = new ServiceMessageElement(message);
					newControl.Margin = new Thickness(160, 4, 160, 0);
					newControl.HorizontalAlignment = HorizontalAlignment.Center;
				} break;
				default: return;
			}

			StackPanel_Dialog.Children.Add(newControl);
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

			if(TextBox_Message.Text.Length > 12 && TextBox_Message.Text.StartsWith("!changename "))
			{ 
				_name = TextBox_Message.Text.Substring(12);
                AddMessage(new NetworkMessage("", $"Your nickname was changed to {_name}"), MessageType.Service);

                TextBox_Message.Text = "";
                return;
			}
			NetworkMessage message = new NetworkMessage(_name, TextBox_Message.Text);
			TextBox_Message.Text = "";

			await _client.SendMessage(message);
			AddMessage(message, MessageType.Ours);
		}
	}
}

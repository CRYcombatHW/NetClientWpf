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
	/// Логика взаимодействия для MessageElement.xaml
	/// </summary>
	public partial class MessageElement : UserControl
	{
		public string Sender {
			get {
				return TextBlock_MessageSender.Text;
			}
			set {
				TextBlock_MessageSender.Text = value;
			}
		}
		public string MessageContent {
			get {
				return TextBlock_MessageContent.Text;
			}
			set {
				TextBlock_MessageContent.Text = value;
			}
		}

		public MessageElement() {
			InitializeComponent();
		}
		public MessageElement(NetworkMessage message) {
			InitializeComponent();

			TextBlock_MessageSender.Text = message.Header;
			TextBlock_MessageContent.Text = message.Content;
		}
	}
}

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
	/// Логика взаимодействия для ServiceMessageElement.xaml
	/// </summary>
	public partial class ServiceMessageElement : UserControl
	{
		public string MessageContent {
			get {
				return TextBlock_MessageContent.Text;
			}
			set {
				TextBlock_MessageContent.Text = value;
			}
		}

		public ServiceMessageElement() {
			InitializeComponent();
		}
		public ServiceMessageElement(NetworkMessage message) {
			InitializeComponent();

			TextBlock_MessageContent.Text = message.Content;
		}
	}
}

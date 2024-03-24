using ClientLib;
using NetworkService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace NetClientWpf
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void Application_Startup(object sender, StartupEventArgs e) {
			while (true) {
				ConnectionWindow connectionWindow = new ConnectionWindow();
				connectionWindow.ShowDialog();

				NetworkClient? client = connectionWindow.Client;
				if (client is null) {
					Current.Shutdown();
					return;
				}

				MainWindow = new MainWindow(client);
				MainWindow.ShowDialog();
			}
		}

		private void Application_Exit(object sender, ExitEventArgs e) {

        }
    }
}

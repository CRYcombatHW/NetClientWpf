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
            NetworkClient? client;
            ConnectionWindow connectionWindow;

            while (true) {
				connectionWindow = new ConnectionWindow();
                client = null;
				connectionWindow.ShowDialog();

				client = connectionWindow.Client;
                if (client is null) {
					Current.Shutdown();
					return;
				}

				MainWindow = new MainWindow(client);
				MainWindow.ShowDialog();
				
				client.Disconnect();
			}
		}

		private void Application_Exit(object sender, ExitEventArgs e) {

        }
    }
}

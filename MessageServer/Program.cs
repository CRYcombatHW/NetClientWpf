using NetworkService;
using ServerLib;
using System.Net.Sockets;

namespace MessageServer
{
    internal class Program
    {
        private static List<NetworkMessage> _chatHistory = new List<NetworkMessage>();

        private static NetworkServer _server = new NetworkServer(ConfigurationManager.MessageServerEndpoint);

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Staring message server");

            List<Socket> users = new List<Socket>();

            _server.OnConnection += async (socket) =>
            {
                await OnConnection(socket, users);
            };
            _server.OnMessage += async (socket, message) =>
            {
                await OnMessage(socket, message, users);
            };
            _server.OnDisconnection += async (socket) =>
            {
                await OnDisconnection(users, socket);
            };

            await _server.AcceptClientsAsync();


        }
        private static void SaveChatHistory(NetworkMessage message)
        {
            _chatHistory.Add(message);
            Console.WriteLine($"[Saved to history] {message.Header}: {message.Content}");
        }

        private static async Task OnConnection(Socket socket, List<Socket> users)
        {

            users.Add(socket);
            foreach (NetworkMessage message in _chatHistory)
            {
                await _server.SendMessage(socket, message);
            }


            SaveChatHistory(new NetworkMessage("Service", $"User connected"));
            foreach (Socket user in users)
            {
                await _server.SendMessage(user, new NetworkMessage("Service", $"New user connected"));
            }

        }

        private static async Task OnMessage(Socket socket, NetworkMessage message, List<Socket> users)
        {
            SaveChatHistory(message);
            foreach (Socket user in users)
            {
                if (user.RemoteEndPoint != socket.RemoteEndPoint)
                {
                    await _server.SendMessage(user, message);
                }
            }
        }

        private static async Task OnDisconnection(List<Socket> users, Socket socket)
        {
            Socket? disconnectedUser = null;

            disconnectedUser = users.FirstOrDefault(u => u.RemoteEndPoint == socket.RemoteEndPoint);

            if (disconnectedUser is null)
            {
                return;
            }
            users.Remove(disconnectedUser);
            SaveChatHistory(new NetworkMessage("Service", $"User disconnected"));
            foreach (Socket user in users)
            {
                await _server.SendMessage(user, new NetworkMessage("Service", $"User disconnected"));
            }
        }
    }
}

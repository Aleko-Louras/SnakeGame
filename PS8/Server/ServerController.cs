using System;
using System.Net;
using System.Net.Sockets;
using NetworkUtil;

namespace Server
{
	public class ServerController
	{
		private Dictionary<int, Socket> clients;
		public ServerController()
		{
			clients = new Dictionary<int, Socket>();
		}


		public void StartServer()
		{
			Networking.StartServer(OnClientConnect, 11000);
		}

        private void OnClientConnect(SocketState state)
		{
			Console.WriteLine("Contact from client");
	
			lock (clients)
			{
				int playerID = (int)state.ID;
				Console.WriteLine("New player connected with ID: " + playerID);
                clients.Add(playerID, state.TheSocket);
			}

			state.OnNetworkAction = OnReceive;

			Networking.GetData(state);
		}

		private void OnReceive(SocketState state)
		{
			Console.WriteLine("Received data");
			Networking.GetData(state);
		}

    }
}


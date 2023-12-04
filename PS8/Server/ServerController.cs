using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using NetworkUtil;

namespace Server
{
	public class ServerController
	{
		string? playerName;
		/// <summary>
		/// Holds the clients
		/// Key: integer for playerID
		/// Value: the Socket for that client connection
		/// </summary>
		private Dictionary<int, Socket> clients = new Dictionary<int, Socket>();

		public void StartServer()
		{
			Networking.StartServer(OnClientConnect, 11000);
		}

        private void OnClientConnect(SocketState state)
		{
			if (state.ErrorOccurred) {
				return;
			}
			
			Console.WriteLine("Contact from client");

            lock (clients)
			{
				int playerID = (int)state.ID;
				Console.WriteLine("New player connected with ID: " + playerID);
                clients.Add(playerID, state.TheSocket);
			}
            state.OnNetworkAction = OnReceive;

			
        }

		/// <summary>
		/// Handles receiving data from the client.
		/// The only data the client should send is its name and movement commands
		/// </summary>
		/// <param name="state"></param>
		private void OnReceive(SocketState state)
		{
			Console.WriteLine("Received data");
			Networking.GetData(state);
			Console.WriteLine(state.buffer.ToString());
		}

    }
}


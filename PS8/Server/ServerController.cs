using System;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.RegularExpressions;
using NetworkUtil;
using WorldModel;

namespace Server
{
	public class ServerController
	{
		public World world = new World(0);
		int playerID = -1;
		string? playerName;

        

        /// <summary>
        /// Holds the clients
        /// Key: integer for playerID
        /// Value: the Socket for that client connection
        /// </summary>
        private List<Socket> clients = new List<Socket>();
        //private Dictionary<int, Socket> clients = new Dictionary<int, Socket>();

        public void StartServer()
		{
			Networking.StartServer(OnClientConnect, 11000);
		}

        private void OnClientConnect(SocketState state)
		{
			if (state.ErrorOccurred)
			{
				return;
			}

            Console.WriteLine("Contact from client");

            lock (clients)
			{
				playerID = world.Snakes.Count;
				Console.WriteLine("New player connected with ID: " + playerID);
                clients.Add( state.TheSocket);
			}

            state.OnNetworkAction = AddNewSnake;

			string toSend = playerID + "\n" + world.Size + "\n";

            
            foreach (Wall w in world.Walls.Values)
			{
				string ret = JsonSerializer.Serialize(w);
				toSend = toSend + ret + "\n";

            }

			Console.WriteLine(toSend);

			Networking.Send(state.TheSocket, toSend);
            Networking.GetData(state);


        }

		private void AddNewSnake(SocketState state)
		{
			playerName = state.GetData();
			Console.WriteLine(playerName);

			lock (world)
			{
				world.Snakes.Add(playerID, new Snake(playerID, playerName));
			}

			state.OnNetworkAction = RecieveMovements;
			Networking.GetData(state);
		}
		/// <summary>
		/// Handles receiving data from the client.
		/// The only data the client should send is its name and movement commands
		/// </summary>
		/// <param name="state"></param>
		private void RecieveMovements(SocketState state)
		{
			Console.WriteLine("Received data");
			// state.getdata to get movement commands


			Networking.GetData(state);
		}

		public void Update()
		{
			lock (clients)
			{
				foreach(Socket s in clients)
				{
					if (!s.Connected)
					{
						clients.Remove(s);
						Console.WriteLine("Client disconnnected");
					}
				}
			}
			string toSend = "";



			foreach (Snake s in world.Snakes.Values)
			{
				string ret = JsonSerializer.Serialize(s);
				toSend = toSend + ret + "\n";

			}
			foreach (Powerup p in world.Powerups.Values)
			{
				string ret = JsonSerializer.Serialize(p);
				toSend = toSend + ret + "\n";
			}
			foreach (Socket s in clients)
			{
				//Networking.Send(s, toSend);
			}
			Console.WriteLine("Hello, I am one frame!");
		}


		public void setWorld(World w)
		{
			world = w;
		}

    }
}


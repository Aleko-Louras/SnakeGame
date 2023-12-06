using System;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.RegularExpressions;
using NetworkUtil;
using SnakeGame;
using WorldModel;

namespace Server
{
	public class ServerController
	{
        private Random? rng;
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
            rng = new Random();
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
                Snake curSnake = world.Snakes[playerID];
                CreateSnake(curSnake);
            }

			state.OnNetworkAction = RecieveMovements;
			Networking.GetData(state);
		}
		private void CreateSnake(Snake snake)
		{
            int x = rng!.Next(-world!.Size / 2, world.Size / 2);
            int y = rng.Next(-world.Size / 2, world.Size / 2);

            // Initialize the snake with the random values created above.
            snake.Create(snake.speed, 120, x, y);
        }

		/// <summary>
		/// Handles receiving data from the client.
		/// The only data the client should send is its name and movement commands
		/// </summary>
		/// <param name="state"></param>
		private void RecieveMovements(SocketState state)
		{
			//Console.WriteLine("Received data");
            string totalData = state.GetData();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

            // For every part in the client's message...
            foreach (string p in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;

                // Ignore the last string if it doesn't end with a '\n'
                if (p[p.Length - 1] != '\n')
                    break;

                // If the message is a movement command...
                if (p[0] == '{')
                {
                    lock (world)
                    {
                        Snake thisSnake = world.Snakes[playerID];
                        if (p.Contains("none"))
                        {
                            // Don't turn the snake
                        }
                        else if (p.Contains("up")) // Turn the snake up.
                        {
                            thisSnake.Turn(new Vector2D(0, -1));
                            Console.WriteLine("client moves up");
                        }
                        else if (p.Contains("left")) // Turn the snake left.
                        {
                            thisSnake.Turn(new Vector2D(-1, 0));
                        }
                        else if (p.Contains("down")) // Turn the snake down.
                        {
                            thisSnake.Turn(new Vector2D(0, 1));
                        }
                        else if (p.Contains("right")) // Turn the snake right.
                        {
                            thisSnake.Turn(new Vector2D(1, 0));
                        }
                    }
                }
                else
                {
                    // Handle the client's message as a name.
                    world.Snakes[playerID].name = p.Substring(0, p.Length - 1);
                }

                // Remove the data as it has been handled by this point.
                state.RemoveData(0, p.Length);
            }


            Networking.GetData(state);
		}

		public void Update()
		{
			lock (clients)
			{
				foreach (Socket s in clients)
				{
					if (!s.Connected)
					{
						clients.Remove(s);
						Console.WriteLine("Client disconnnected");
					}
				}

				string toSend = "";



				foreach (Snake s in world.Snakes.Values)
				{
                    s.Move();
					string ret = JsonSerializer.Serialize(s);
					toSend = toSend + ret + "\n";
					//Console.WriteLine(toSend);

				}
				foreach (Powerup p in world.Powerups.Values)
				{
					string ret = JsonSerializer.Serialize(p);
					toSend = toSend + ret + "\n";
				}
				foreach (Socket s in clients)
				{
                    //Console.WriteLine(s);
					Networking.Send(s, toSend);
				}
			}
			//Console.WriteLine("Hello, I am one frame!");
		}


		public void setWorld(World w)
		{
			world = w;
		}

    }
}


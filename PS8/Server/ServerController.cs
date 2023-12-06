using System;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.RegularExpressions;
using NetworkUtil;
using SnakeGame;
using WorldModel;

namespace Server {
    public class ServerController {
        public World world = new World(0);
        //int playerID = -1;
        string playerName = "";

        public static List<Powerup> powerupsToRemove = new List<Powerup>();
        public static List<Snake> snakesToRemove = new List<Snake>();


        private Random rng = new Random();

        /// <summary>
        /// Holds the clients
        /// Key: integer for playerID
        /// Value: the Socket for that client connection
        /// </summary>
        private List<Socket> clients = new List<Socket>();


        public void StartServer() {
            Networking.StartServer(OnClientConnect, 11000);
        }

        private void OnClientConnect(SocketState state) {
            if (state.ErrorOccurred) {
                return;
            }

            Console.WriteLine("Contact from client");

            //lock (clients) {
            //    //playerID = world.Snakes.Count;
            //    Console.WriteLine("New player connected with ID: " + playerID);
                
            //}

            state.OnNetworkAction = ConnectSnake;

            string toSend = state.ID + "\n" + world.Size + "\n";



            foreach (Wall w in world.Walls.Values) {
                string ret = JsonSerializer.Serialize(w);
                toSend = toSend + ret + "\n";

            }


            Networking.Send(state.TheSocket, toSend);
            lock (clients) {
                clients.Add(state.TheSocket);
            }

            Networking.GetData(state);


        }

        private void ConnectSnake(SocketState state) {

            playerName = state.GetData()[..(state.GetData().Length - 1)];
            Console.WriteLine(playerName);

            lock (world) {
                world.Snakes.Add((int)state.ID, new Snake((int)state.ID, playerName));
                Snake snake = world.Snakes[(int)state.ID];

                int startingX = rng.Next(-world.Size / 2, world.Size / 2);
                int startingY = rng.Next(-world.Size / 2, world.Size / 2);
                snake.MakeSnake(startingX, startingY);
            }

            state.RemoveData(0, playerName.Length + 1);
            state.OnNetworkAction = RecieveMovements;
            Networking.GetData(state);

        }




        /// <summary>
        /// Handles receiving data from the client.
        /// The only data the client should send is its name and movement commands
        /// </summary>
        /// <param name="state"></param>
        private void RecieveMovements(SocketState state) {

            if (state.ErrorOccurred) {
                lock (clients) {
                    clients.Remove(state.TheSocket);
                }
                return;
            }

            //Console.WriteLine("Received data");
            string totalData = state.GetData();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

            foreach (string p in parts) {
                if (p.Length == 0)
                    continue;


                if (p[p.Length - 1] != '\n')
                    break;

                if (p[0] == '{') {

                        Snake sendingSnake = world.Snakes[(int)state.ID];

                        if (p.Contains("up")) {
                            sendingSnake.Turn(new Vector2D(0, -1));
                        } else if (p.Contains("down")) {
                            sendingSnake.Turn(new Vector2D(0, 1));
                        } else if (p.Contains("left")) {
                            sendingSnake.Turn(new Vector2D(-1, 0));
                        } else if (p.Contains("right")) {
                            sendingSnake.Turn(new Vector2D(1, 0));
                        } else { }
                } else {
                    lock (world) {
                        world.Snakes[(int)state.ID].name = p.Substring(0, p.Length - 1);
                    }
                }

                state.RemoveData(0, p.Length);
            }
            Networking.GetData(state);
        }

        public void Update() {
            lock (clients) {
                string toSend = "";
                lock (world) {
                    foreach (Powerup p in powerupsToRemove) {
                        world.Powerups.Remove(p.power);
                    }
                    foreach (Snake deadSnake in snakesToRemove) {
                        world.Snakes.Remove(deadSnake.snake);
                    }
                }


                foreach (Snake s in world.Snakes.Values) {
                    s.Move();
                    s.hitPowerup(world, powerupsToRemove);
                    s.hitWall(world, snakesToRemove);
                    string ret = JsonSerializer.Serialize(s);
                    toSend = toSend + ret + "\n";
                    //Console.WriteLine(toSend);

                }

                foreach (Powerup p in world.Powerups.Values) {
                    string ret = JsonSerializer.Serialize(p);
                    toSend = toSend + ret + "\n";
                }

                foreach (Socket s in clients) {
                    //Console.WriteLine(s);
                    Networking.Send(s, toSend);
                }

            }
        }


        public void SetWorld(World w) {
            world = w;
        }

    }
}


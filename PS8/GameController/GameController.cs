namespace GController;
using System.Net;
using NetworkUtil;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Text.Json;

public class GameController
{
    private WorldModel.World theWorld;
    private string connectedPlayer;
    bool recievedSetup;
    int playerId;

    public GameController()
    {
        theWorld = new WorldModel.World(2000);//TODO
    }
    public void Connect(string serverAddress, string playerName)
    {
        Networking.ConnectToServer(OnConnect, serverAddress, 11000);
        connectedPlayer = playerName;
    }

    private void OnConnect(SocketState state)
    {

        SocketState theServer = state;
        Networking.Send(state.TheSocket, connectedPlayer + "\n");

        // Event loop to receive messages from server
        state.OnNetworkAction = RecieveMessage;
        Networking.GetData(state);

    }

    private void RecieveMessage(SocketState state)
    {
        if (state.ErrorOccurred)
        {
            return;
        }
        if (!recievedSetup)
        {
            InitialSetup(state);
        }
        else
        {
            UpdateFromServer(state);
        }

        //Continue the event loop
        Networking.GetData(state);

    }

    private void InitialSetup(SocketState state)
    {

        string totalData = state.GetData();
        string[] parts = Regex.Split(totalData, @"(?<=[\n])");

        // Player ID comes first, assign it
        playerId = int.Parse(parts[0]);
        state.RemoveData(0, parts[0].Length);
        theWorld = new WorldModel.World(int.Parse(parts[1]));
        state.RemoveData(0, parts[1].Length);

        // Walls come second, write them
        for (int i = 2; i < parts.Length - 1; i++)
        {
            WorldModel.Wall? nextWall = JsonSerializer.Deserialize<WorldModel.Wall>(parts[i])!;

            theWorld.Walls.Add(nextWall.wall, nextWall);
            state.RemoveData(0, parts[i].Length);
        }

        recievedSetup = true;
        Networking.GetData(state);
    }

    private void UpdateFromServer(SocketState state)
    {
        // Parse the incoming message
        string totalData = state.GetData();
        string[] parts = Regex.Split(totalData, @"(?<=[\n])");

        // Loop through the snakes

        for (int i = 0; i < parts.Length - 1; i++)
        {
            WorldModel.Snake? nextSnake = JsonSerializer.Deserialize<WorldModel.Snake>(parts[i])!;

            // Check if the incoming object is a snake or a powerup

            JsonDocument doc = JsonDocument.Parse(parts[i]);
            if (doc.RootElement.TryGetProperty("snake", out _))
            {
                if (nextSnake.died)
                    theWorld.Snakes.Remove(nextSnake.snake);
                else
                    theWorld.Snakes.Add(nextSnake.snake, nextSnake);
            }
            else if (doc.RootElement.TryGetProperty("power", out _))
            {
                Console.WriteLine("hello");
            }
        }

        recievedSetup = true;
        Networking.GetData(state);
    }
}






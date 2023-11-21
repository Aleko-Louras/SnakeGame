namespace GController;
using NetworkUtil;
using System.Text.RegularExpressions;
using WorldModel;
using System.Text.Json;

public class GameController {
    private World theWorld;
    private string connectedPlayer;
    private bool recievedSetup;
    private int playerID;

    public delegate void GameUpdateHandler();
    public event GameUpdateHandler UpdateArrived;

    public GameController() {
        theWorld = new World(2000);
        connectedPlayer = "";

    }
    public void Connect(string serverAddress, string playerName) {
        Networking.ConnectToServer(OnConnect, serverAddress, 11000);
        connectedPlayer = playerName;
    }

    private void OnConnect(SocketState state) {

        SocketState theServer = state;
        Networking.Send(state.TheSocket, connectedPlayer + "\n");

        // Event loop to receive messages from server
        state.OnNetworkAction = RecieveMessage;
        Networking.GetData(state);

    }

    private void RecieveMessage(SocketState state) {
        if (state.ErrorOccurred) {
            return;
        }
        if (!recievedSetup) {
            InitialSetup(state);
        } else {
            UpdateFromServer(state);
        }

        //Continue the event loop
        Networking.GetData(state);

    }

    private void InitialSetup(SocketState state) {

        string totalData = state.GetData();
        string[] parts = Regex.Split(totalData, @"(?<=[\n])");
        lock (theWorld) {

            // Player ID comes first, assign it
            playerID = int.Parse(parts[0]);
            theWorld.playerID = playerID;
            state.RemoveData(0, parts[0].Length);
            theWorld = new World(int.Parse(parts[1]));
            state.RemoveData(0, parts[1].Length);

            // Walls come second, write them
            for (int i = 2; i < parts.Length - 1; i++) {
                Wall? nextWall = JsonSerializer.Deserialize<Wall>(parts[i])!;

                theWorld.Walls.Add(nextWall.wall, nextWall);
                state.RemoveData(0, parts[i].Length);
            }
        }

        recievedSetup = true;
        Networking.GetData(state);
    }

    private void UpdateFromServer(SocketState state) {
        // Parse the incoming message
        string totalData = state.GetData();
        string[] parts = Regex.Split(totalData, @"(?<=[\n])");

        // Loop through the snakes
        lock (theWorld) {
            for (int i = 0; i < parts.Length - 1; i++) {
                // Check if the incoming object is a snake or a powerup

                JsonDocument doc = JsonDocument.Parse(parts[i]);
                if (doc.RootElement.TryGetProperty("snake", out _)) {
                    Snake? nextSnake = JsonSerializer.Deserialize<Snake>(parts[i])!;
                    if (nextSnake.died) {
                        state.RemoveData(0, parts[i].Length);
                    } else {
                        theWorld.Snakes[nextSnake.snake] = nextSnake;
                        state.RemoveData(0, parts[i].Length);
                    }
                } else if (doc.RootElement.TryGetProperty("power", out _)) {
                    Powerup? nextPowerup = JsonSerializer.Deserialize<Powerup>(parts[i])!;

                    if (nextPowerup.died) {
                        theWorld.Powerups.Remove(nextPowerup.power); // maybe a problem?
                        state.RemoveData(0, parts[i].Length);
                    } else {
                        theWorld.Powerups[nextPowerup.power] = nextPowerup;
                        state.RemoveData(0, parts[i].Length);
                    }
                }
            }
        }

        // TODO: Notify the view that a new game world has arrived from the server
        UpdateArrived?.Invoke();

        // Continue the Networ loop
        Networking.GetData(state);
    }
}






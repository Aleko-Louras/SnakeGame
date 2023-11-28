///
/// GameController handles moving information between the Server and the
/// Game world.
///
/// Aleko Louras and Quinn Pritchett
/// November 2023
///


namespace GController;
using NetworkUtil;
using System.Text.RegularExpressions;
using WorldModel;
using System.Text.Json;

public class GameController {
    private World? theWorld;
    private string? connectedPlayer = "";
    private bool recievedSetup;
    private int? playerID;

    public delegate void GameUpdateHandler();
    public event GameUpdateHandler UpdateArrived;

    public delegate void ErrorHandler();
    public event ErrorHandler NetworkError;

    private SocketState theServer;


    public World? GetWorld() {
        return theWorld;
    }

    /// <summary>
    /// Sends movment instructions to the server
    /// </summary>
    /// <param name="directionText"></param>
    public void SendMovement(string directionText) {

        switch (directionText) {
            case "w":
                string upCommand = "{\"moving\":\"up\"}" + "\n";
                Networking.Send(theServer.TheSocket, upCommand);
                break;
            case "a":
                string leftCommand = "{\"moving\":\"left\"}" + "\n";
                Networking.Send(theServer.TheSocket, leftCommand);
                break;
            case "s":
                string downCommand = "{\"moving\":\"down\"}" + "\n";
                Networking.Send(theServer.TheSocket, downCommand);
                break;
            case "d":
                string rightCommand = "{\"moving\":\"right\"}" + "\n";
                Networking.Send(theServer.TheSocket,rightCommand);
                break;
            default:
                break;
           
        }
    }
    /// <summary>
    /// Begin Connecting to Server
    /// </summary>
    /// <param name="serverAddress"></param>
    /// <param name="playerName"></param>
    public void Connect(string serverAddress, string playerName) {
        Networking.ConnectToServer(OnConnect, serverAddress, 11000);
        connectedPlayer = playerName;
    }

    /// <summary>
    /// On Connection method 
    /// </summary>
    /// <param name="state"></param>
    private void OnConnect(SocketState state) {

        theServer = state;
        Networking.Send(state.TheSocket, connectedPlayer + "\n");

        // Event loop to receive messages from server
        state.OnNetworkAction = RecieveMessage;
        Networking.GetData(state);
        
    }

    /// <summary>
    /// Processes recived messages and begins the event loop
    /// </summary>
    /// <param name="state"></param>
    private void RecieveMessage(SocketState state) {
        if (state.ErrorOccurred) {
            NetworkError.Invoke();
            return;
        }
        if (!recievedSetup) {
            InitialSetup(state);
        }
        UpdateFromServer(state);
        

        //Continue the event loop
        // Add check that the main player exits

        if (theWorld?.playerID == playerID) {
            UpdateArrived?.Invoke();
        }
        Networking.GetData(state);

    }

    /// <summary>
    /// Initial setup of the playerID, and theWorld
    /// </summary>
    /// <param name="state"></param>
    private void InitialSetup(SocketState state) {

        string totalData = state.GetData();
        string[] parts = Regex.Split(totalData, @"(?<=[\n])");

       
        for(int i = 0; i < 2; i++) { 
            if (parts[i].Length == 0)
                continue;
            if (parts[parts.Length - 1] != "")
                break;

            if (playerID is null) {
                int.TryParse(parts[0], out int result);
                playerID = result;
                state.RemoveData(0, parts[0].Length);

            }
            if (theWorld is null) {
                int.TryParse(parts[1], out int result);

                theWorld = new World(result);
                theWorld.playerID = (int)playerID;
                state.RemoveData(0, parts[1].Length);
            }
        }
        if (!(theWorld is null) && !(playerID is null))
            recievedSetup = true;
    }

    /// <summary>
    /// Updates theWorld each time new data is recieved from the server
    /// </summary>
    /// <param name="state"></param>
    private void UpdateFromServer(SocketState state) {
        // Parse the incoming message
        string totalData = state.GetData();
        string[] parts = Regex.Split(totalData, @"(?<=[\n])");

        lock (theWorld!) {
            for (int i = 0; i < parts.Length - 1; i++) {

                // Check if the incoming object is a wall, snake or a powerup
                // the deserialze as appropriate 

                if (parts[i][0] == '{' && parts[i][parts[i].Length - 1] == '\n') {
                    try {
                        JsonDocument doc = JsonDocument.Parse(parts[i]);

                        if (doc.RootElement.TryGetProperty("wall", out _)) {
                            Wall? nextWall = JsonSerializer.Deserialize<Wall>(doc)!;

                            if (!theWorld.Walls.ContainsKey(nextWall.wall)) {
                                theWorld.Walls.Add(nextWall.wall, nextWall);
                                state.RemoveData(0, parts[i].Length);
                            } else {
                                theWorld.Walls[nextWall.wall] = nextWall;
                                state.RemoveData(0, parts[i].Length);
                            }

                        } else if (doc.RootElement.TryGetProperty("snake", out _)) {
                            Snake? nextSnake = JsonSerializer.Deserialize<Snake>(parts[i])!;

                            if (nextSnake.dc == false) {
                                if (theWorld.Snakes.ContainsKey(nextSnake.snake)) {
                                    theWorld.Snakes[nextSnake.snake] = nextSnake;
                                    //state.RemoveData(0, parts[i].Length);
                                } else {
                                    theWorld.Snakes.Add(nextSnake.snake, nextSnake);
                                    //state.RemoveData(0, parts[i].Length);
                                }
                            } else {
                                if (theWorld.Snakes.ContainsKey(nextSnake.snake)) {
                                    theWorld.Snakes.Remove(nextSnake.snake);
                                    //state.RemoveData(0, parts[i].Length);
                                }
                            }


                        } else if (doc.RootElement.TryGetProperty("power", out _)) {
                            Powerup? nextPowerup = JsonSerializer.Deserialize<Powerup>(parts[i])!;

                            if (nextPowerup.died) {
                                theWorld.Powerups.Remove(nextPowerup.power);
                                state.RemoveData(0, parts[i].Length);
                            } else {
                                if (theWorld.Powerups.ContainsKey(nextPowerup.power)) {
                                    theWorld.Powerups[nextPowerup.power] = nextPowerup;
                                    state.RemoveData(0, parts[i].Length);
                                } else {
                                    theWorld.Powerups.Add(nextPowerup.power, nextPowerup);
                                    state.RemoveData(0, parts[i].Length);
                                }
                            }
                        }

                        
                    } catch (Exception) {
                       // do nothing
                    }
                    
                }
            }
                
        } 
    }
}






namespace GController;
using System.Net;
using NetworkUtil;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Diagnostics;

public class GameController { 

    public void Connect(string serverAddress) {
        Networking.ConnectToServer(OnConnect, serverAddress, 11000);
    }

    private void OnConnect(SocketState state) {

        SocketState theServer = state;
        Networking.Send(state.TheSocket, "player\n");

        // Event loop to receive messages from server
        state.OnNetworkAction = RecieveMessage;
        Networking.GetData(state);

    }

    private void RecieveMessage(SocketState state) {
        if(state.ErrorOccurred) {
            return;
        }
        ProcessMessages(state);

        //Continue the event loop
        Networking.GetData(state);
        
    }

    private void ProcessMessages(SocketState state) {
        string totalData = state.GetData();
        Debug.WriteLine(totalData);
    }
}




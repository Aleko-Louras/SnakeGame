namespace SnakeGame;
using GController;

public partial class MainPage : ContentPage
   
{
    GameController gController;
    public MainPage()
    {
        InitializeComponent();
        gController = new GameController();
        gController.UpdateArrived += OnFrame;
        gController.NetworkError += OnError;

    }

    void OnTapped(object sender, EventArgs args)
    {
        keyboardHack.Focus();
    }

    void OnTextChanged(object sender, TextChangedEventArgs args)
    {
        Entry entry = (Entry)sender;
        String text = entry.Text.ToLower();
        if (text == "w")
        {
            // Move up
            gController.SendMovement(text);
        }
        else if (text == "a")
        {
            gController.SendMovement(text);
        }
        else if (text == "s")
        {
            gController.SendMovement(text);
        }
        else if (text == "d")
        {
            gController.SendMovement(text);
        }
        entry.Text = "";
    }

    private void NetworkErrorHandler()
    {
        DisplayAlert("Error", "Disconnected from server", "OK");
    }


    /// <summary>
    /// Event handler for the connect button
    /// We will put the connection attempt interface here in the view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ConnectClick(object sender, EventArgs args)
    {
        if (serverText.Text == "")
        {
            DisplayAlert("Error", "Please enter a server address", "OK");
            return;
        }
        if (nameText.Text == "")
        {
            DisplayAlert("Error", "Please enter a name", "OK");
            return;
        }
        if (nameText.Text.Length > 16)
        {
            DisplayAlert("Error", "Name must be less than 16 characters", "OK");
            return;
        }
        //DisplayAlert("Delete this", "Code to start the controller's connecting process goes here", "OK");
        gController.Connect(serverText.Text, nameText.Text);
        connectButton.IsEnabled = false;
        keyboardHack.Focus();
    }

    /// <summary>
    /// Use this method as an event handler for when the controller has updated the world
    /// </summary>
    public void OnFrame()
    {
        worldPanel.SetWorld(gController.GetWorld());

        Dispatcher.Dispatch(() => graphicsView.Invalidate());
        
    }

    public void OnError() {
        Dispatcher.Dispatch(() => DisplayAlert("Network Error", "A network error occured, retry connecting", "OK"));
        Dispatcher.Dispatch(() => connectButton.IsEnabled = true);
    }

    private void ControlsButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Controls",
                     "W:\t\t Move up\n" +
                     "A:\t\t Move left\n" +
                     "S:\t\t Move down\n" +
                     "D:\t\t Move right\n",
                     "OK");
    }

    private void AboutButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("About",
      "SnakeGame solution\nArtwork by Jolie Uk and Alex Smith\nGame design by Daniel Kopta and Travis Martin\n" +
      "Implementation by ...\n" +
        "CS 3500 Fall 2022, University of Utah", "OK");
    }

    private void ContentPage_Focused(object sender, FocusEventArgs e)
    {
        if (!connectButton.IsEnabled)
            keyboardHack.Focus();
    }
}
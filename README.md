# Decision Making
After setting up the skeleton code, we decided to begin with the Networking handshake process. We believed that after receiving and processing the data into our own data structures we could begin the process of drawing more easily. 
Our model consisted of a World object that could contain dictionaries of the relevant 
walls, powerups, and snakes. Each element of the game, Walls, Snakes, Powerups etc. had their own class that contained the information relevant to it. This made our handshake part easier, because once we figured out the handshake, adding
to our dictionaries properly was easier. 

We went up until the client sending back commands to the server, and switched
to the view, making sure our snake draws properly in order to check if the commands we send are being properly implemented.
Once we got the view working properly and connected to our handshake process in the controller, we drew everything from the
gathered data from the server. We implemented the commands to the server (moving). 

# Challenges/difficulties
During the handshake process, it first was smooth to implement with TA help, making sure the proper events are being invoked, etc. During
the end of our program, we encountered a bug with the server that during the first client connection, the JSON data was getting cut
from totalData in the buffer by remove data, which a TA said was due to our Networking DLL, so we decided to switch to the provided DLL.

During our view/drawing phase, we were having a hard time getting the right information from the world in GameController to get drawn in our world,
never updating the world object when un update occurs. The screen dimensions and transform process was a challenging process that took
longer than expected, but through trial and error it succeeded. We sat with 2 TA's for 30+ minutes with some challenging JSON bugs, as well as
removeData exceptions that we could not seem to handle. The exceptions seemed to be extreme when we would add AI clients, specifically the argumentoutofbounds exception.
The TA Raynard Christian helped us, but in the end we could not figure out what was happening.

# Features
We decided instead of a typical explosion, we would implement a game over screen that changes text depending on what your score was. You get different text as your score gets above 5, 10, and 15 points.

Up to 10 snakes connected will get a unique color color snake


# Known Issues:

The client will begin to slow down and input will lag with multiple clients connected.
With many clients connected it may become unplayable. 

The client snakes can draw over the game over screen. 

Network errors may not report properly. 

Snakes may not wrap around the world properly. 

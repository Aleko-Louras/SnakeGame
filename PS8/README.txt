DecisionMaking:
After setting up the skeleton code, we decided to start on the handshake process, since it processing the JSON data was more
important, ensuring we were grabbing the data to draw any of it. We also made part of our model structure, having dictionaries
for our different walls, powerups, and snakes. All of these classes have the useful information such as id, locations
, and for the snake, a vector list for the body. This made our handshake part easier, because once we figured out the handshake, adding
to our dictionaries properly was easier. We went up until the client sending back commands to the server, and switched
to the view, making sure our snake draws properly in order to check if the commands we send are being properly implemented.
Once we got the view working properly and connected to our handshake process in the controller, we drew everything from the
gathered data from the server. We implemented the commands to the server (moving). 
Challenges/difficulties:
During the handshake process, it first was smooth to implement with TA help, making sure the proper events are being invoked, etc. During
the end of our program, we encountered a bug with the server that during the first client connection, the JSON data was getting cut
from totalData in the buffer by remove data, which a TA said was due to our Networking DLL, so we decided to switch to the provided DLL.
During our view/drawing phase, we were having a hard time getting the right information from the world in gamecontroller to get drawn in our world,
never updating the world object when un update occurs. The screen dimensions and transform process was a challenging process that took
longer than expected, but through trial and error it succeeded. We sat with 2 TA's for 30+ minutes with some challenging JSON bugs, as well as
removeData exceptions that we could not seem to handle. The exceptions seemed to be extreme when we would add AI clients, specifically the argumentoutofbounds exception.
The TA Raynard Christian helped us, but in the end we could not figure out what was happening.

Features:
We decided instead of a typical explosion, we would implement a game over screen that changes text 
depending on what your score was. We used a switch statement for the snake color changing.

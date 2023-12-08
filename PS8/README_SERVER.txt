# Design Decisions

We maintained the principles of MVC as we worked on this assignment. To do this we keep the model(world, powerups, snakes, walls, etc.) seperate from our server controller.The methods for updating the properties of different world elements remained in the classes for those elements. 

# Notes on the process 

We decided to start with the handshake process, establishing a client collection for all of the clients,
and used unique ids to get and recieve data from every client. Once we had that working and drew the walls, we knew
we could start on the snakes and powerup drawings/movement mechanics. After some trial and error and computation with vectors, we
were able to see the snakes across the server, and test our movement. After the movement worked, we implemented turning using
the client commands from the controller through a method. Any type of collision with each object is in snake, everything involving 
the spawning of objects is in each objects class respectively. Anything modifying the shared world is locked.

# Gameplay and things to know

Settings: A settings.xml file is located in the folder containing the solution for this project. This file allows you to set preferences for the behavior of the server, such as the number and positions of walls, frame rate, the respawn rate for snakes, and the size of the world.

This is a vanilla Implementation of the snake game. There are no additional features other
than basic networked gameplay. 

# Implementation details

The server class instantiates the server, calling update every frame and allowing the connection of clients, as well
as deserializing the XML to the world. The controller takes that world, and calles the handshake (transfer of data) between
client and server. The update method lies in the controller, making sure to get each object data and resend it to the client
every frame (thanks to the server). The model contains each method involving the mechanic behind each object, but the controller
manages when each mechanic happens. The game gets laggy with more clients, but we believe this is a mac/tcp issue.
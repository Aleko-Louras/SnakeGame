namespace WorldModel;
/// <summary>
/// A class representing a world object, that is the world in our snake game.
/// </summary>
public class World
{
    /// <summary>
    /// A dictionary of snakes to keep track of in the world.
    /// </summary>
    public Dictionary<int, Snake> Snakes;
    /// <summary>
    /// A dictionary of powerups to keep track of in the world.
    /// </summary>
    public Dictionary<int, Powerup> Powerups;
    /// <summary>
    /// A dictionary of walls to keep track of in the world.
    /// </summary>
    public Dictionary<int, Wall> Walls;
    /// <summary>
    /// A playerID to keep track of the player client.
    /// </summary>
    public int playerID;
    /// <summary>
    /// A size for the size dimension of our world.
    /// </summary>
    public int Size
    {
        //get; private set;
        get; set;
    }
    /// <summary>
    /// A constructor for the world object initializing the dictionaries and setting the size
    /// to the passed in values.
    /// </summary>
    /// <param name="size"></param>
    public World(int size)
    {
        Snakes = new Dictionary<int, Snake>();
        Powerups = new Dictionary<int, Powerup>();
        Walls = new Dictionary<int, Wall>();
        Size = size;
    }
}


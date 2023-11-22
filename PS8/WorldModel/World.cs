namespace WorldModel;

public class World
{
    public Dictionary<int, Snake> Snakes;
    public Dictionary<int, Powerup> Powerups;
    public Dictionary<int, Wall> Walls;
    public int playerID;
    public int Size
    {
        //get; private set;
        get; set;
    }
    public World(int size)
    {
        Snakes = new Dictionary<int, Snake>();
        Powerups = new Dictionary<int, Powerup>();
        Walls = new Dictionary<int, Wall>();
        Size = size;
    }
}


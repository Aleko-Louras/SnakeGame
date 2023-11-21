namespace WorldModel;

public class World
{
    public Dictionary<int, Snake> Snakes;
    public Dictionary<int, Powerup> Powerups;
    public Dictionary<int, Wall> Walls;
    public int Size
    {
        get; private set;
    }
    public World(int size)
    {
        Snakes = new Dictionary<int, Snake>();
        Powerups = new Dictionary<int, Powerup>();
        Walls = new Dictionary<int, Wall>();
        Size = size;
    }
}


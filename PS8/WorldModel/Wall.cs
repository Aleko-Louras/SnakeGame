using System;
namespace WorldModel
{
	public class Wall
	{
        public int wall { get; private set; }

		public SnakeGame.Vector2D p1 { get; private set; }
        public SnakeGame.Vector2D p2 { get; private set; }

		public Wall(int wall, SnakeGame.Vector2D p1, SnakeGame.Vector2D p2)
		{
			this.wall = wall;
			this.p1 = p1;
			this.p2 = p2;
		}
	}
}


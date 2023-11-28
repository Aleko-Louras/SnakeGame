using System;
using SnakeGame;
namespace WorldModel
{
	/// <summary>
	/// A class representing a wall object that is drawn in the snake game.
	/// </summary>
	public class Wall
	{
		/// <summary>
		/// a field for the walls unique ID
		/// </summary>
        public int wall { get; private set; }
		/// <summary>
		/// A vector location for the first point of the wall.
		/// </summary>
		public Vector2D p1 { get; private set; }
		/// <summary>
		/// A vector location for the second point of the wall.
		/// </summary>
        public Vector2D p2 { get; private set; }
		/// <summary>
		/// A constructor for the wall object, setting values to each value passed in.
		/// </summary>
		/// <param name="wall"></param>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		public Wall(int wall, Vector2D p1, Vector2D p2)
		{
			this.wall = wall;
			this.p1 = p1;
			this.p2 = p2;
		}
	}
}


using System;
using SnakeGame;
namespace WorldModel
{
	public class Powerup
	{
		public int power
		{
			get; private set;
		} // powerup's unique ID
		public Vector2D loc
		{
			get; private set;
		} // location of powerup
		public bool died
		{
			get; private set;
		} // indicates if the powerup was collected on this frame

		public Powerup(int power, Vector2D loc, bool died)
		{
			this.power = power; this.loc = loc; this.died = died;
		}
	}
}


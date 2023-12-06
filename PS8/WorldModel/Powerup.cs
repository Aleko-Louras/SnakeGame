using System;
using SnakeGame;
namespace WorldModel
{
	/// <summary>
	/// The powerup class represents a Powerup object in our snake game.
	/// </summary>
	public class Powerup
	{
		//Power ups unique power level id.
		public int power
		{
			get; private set;
		}
		//The vector location of the powerup
		public Vector2D loc
		{
			get; private set;
		}

		//If the powerup has died.
		public bool died
		{
			get;  set;
		}

		public int width = 5;


		/// <summary>
		/// Constructor for the powerup taking in power, location, and if its died.
		/// </summary>
		/// <param name="power"></param>
		/// <param name="loc"></param>
		/// <param name="died"></param>
		public Powerup(int power, Vector2D loc, bool died)
		{
			this.power = power; this.loc = loc; this.died = died;
		}
	}
}


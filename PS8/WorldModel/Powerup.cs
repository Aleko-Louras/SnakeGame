using System;
using System.Drawing;
using SnakeGame;
namespace WorldModel
{
	
	/// <summary>
	/// The powerup class represents a Powerup object in our snake game.
	/// </summary>
	public class Powerup
	{
        public static int powerupCounter = 75;
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

		//public int powerupCount = 20;
		

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

		public static void movePowerup(World world, Powerup oldPowerup)
		{
            Random rng = new Random();
            powerupCounter--;
			if (powerupCounter == 0)
			{
				powerupCounter = rng.Next(75);
			}
			else
			{
				if (oldPowerup.died)
				{
					oldPowerup.died = false;
					
					int x = rng.Next(-world.Size / 2, world.Size / 2);
					int y = rng.Next(-world.Size / 2, world.Size / 2);

					Vector2D v = new Vector2D(x, y);
					world.Powerups[oldPowerup.power].died = false;
					world.Powerups[oldPowerup.power].loc = v;
				}
			}
                
            
		}
	}
}


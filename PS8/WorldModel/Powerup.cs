using System;
namespace WorldModel
{
	public class Powerup
	{
		int power; // powerup's unique ID
		SnakeGame.Vector2D loc; // location of powerup
		bool died; // indicates if the powerup was collected on this frame

		public Powerup()
		{
		}
	}
}


using System;
namespace WorldModel
{
	public class Snake
	{
		int snake;
		string name;
		List<SnakeGame.Vector2D> body;
		SnakeGame.Vector2D dir;
		int score;
		bool died;
		bool alive;
		bool dc;
		bool join;

		public Snake()
		{
		}
	}
}


using System;
using SnakeGame;
namespace WorldModel
{
	/// <summary>
	/// A class representing a snake object in our game.
	/// </summary>
	public class Snake
	{
		/// <summary>
		/// A field for the snakes unique ID.
		/// </summary>
		public int snake { get; private set; }
		/// <summary>
		/// The field for the snakes name
		/// </summary>
        public string name { get; private set; }
		/// <summary>
		/// A list of vectors representing the line segments of the snake body.
		/// </summary>
		public List<Vector2D> body { get; private set; }
		/// <summary>
		/// A directional vector for the head of the snake
		/// </summary>
        public Vector2D dir { get; private set; }
		/// <summary>
		/// The score of a snake.
		/// </summary>
        public int score { get; private set; }
		/// <summary>
		/// A bool if a snake has died.
		/// </summary>
        public bool died { get; private set; }
		/// <summary>
		/// A bool if a snake is alive.
		/// </summary>
        public bool alive { get; private set; }
		/// <summary>
		/// A bool to see if a snake disconnected.
		/// </summary>
        public bool dc { get; private set; }
		/// <summary>
		/// A bool for a snake is joined or not.
		/// </summary>
        public bool join { get; private set; }
		/// <summary>
		/// A constructor for a snake object setting every field to a value passed in.
		/// </summary>
		/// <param name="snake"></param>
		/// <param name="name"></param>
		/// <param name="body"></param>
		/// <param name="dir"></param>
		/// <param name="score"></param>
		/// <param name="died"></param>
		/// <param name="alive"></param>
		/// <param name="dc"></param>
		/// <param name="join"></param>
        public Snake(int snake,
					string name,
					List<Vector2D> body,
					Vector2D dir,
					int score,
                    bool died,
					bool alive,
					bool dc,
					bool join)
		{
			this.snake = snake;
			this.name = name;
			this.body = body;
			this.dir = dir;
			this.score = score;
			this.died = died;
			this.alive = alive;
			this.dc = dc;
			this.join = join;

		}
	}
}


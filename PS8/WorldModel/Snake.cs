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
        public string name { get; set; }
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

		public int speed = 6;
		public int startingLength;
		public bool turning;
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

		public Snake(int snake, string name)
		{

            this.snake = snake;
            this.name = name;
			this.body = new List<Vector2D>();
			this.dir = new Vector2D(0, 1);
			this.score = 0;
			this.died = false;
			this.alive = true;
			this.dc = false;
			this.join = true;
            this.turning = false;
        }
        public void Create(int speed, int startingLength, int x, int y)
        {
            this.startingLength = startingLength;
            // Create a head and tail Vector2D and add it to the snake's body.
            Vector2D head = new Vector2D(x, y);
            Vector2D tail = new Vector2D(x + (dir.X * startingLength), y + (dir.Y * startingLength));
            this.body.Add(tail);
            this.body.Add(head);
        }

        public void Turn(Vector2D dirChange)
		{
            Vector2D head = body[body.Count - 1];
            Vector2D neck = body[body.Count - 2];

			Vector2D velocity = dirChange * speed;

			

			//// Don't allow the snake to turn in on itself.
			//if ((Math.Abs(head.X - neck.X) < 10 && Math.Abs(head.Y - neck.Y) < 10) ||
			//	(dir.X == dirChange.X && dir.Y == dirChange.Y))
			//{
			//	return;
			//}

			// Duplicate the snake's head and add it to the body.
			Vector2D dupHead = new Vector2D(body[body.Count - 1].X, body[body.Count - 1].Y);
            dupHead += velocity;
            body.Add(dupHead);

			// Set the snake's direction to the new direction.
			//this.dir = dirChange;
		}

		public void Move()
		{
            Vector2D head = body[body.Count - 1];
			Vector2D neck = body[body.Count - 2];

			Vector2D abdomen = body[1];
            Vector2D tail = body[0];


			Vector2D head_difference = head - neck;
			head_difference.Normalize();

			Vector2D tail_difference = abdomen - tail;
			tail_difference.Normalize();

			body[body.Count -1 ] = head + head_difference;
			body[0] = tail + tail_difference;

        }
	}
}


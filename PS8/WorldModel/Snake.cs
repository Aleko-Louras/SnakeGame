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

		public int width = 10;
		public int speed = 3;
		public int initialLength = 120;
		public bool isGrowing;

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
        }
        public void MakeSnake( int x, int y)
        {
            Vector2D head = new Vector2D(x, y);
            Vector2D tail = new Vector2D(x + dir.X * initialLength, y + dir.Y * initialLength);
            this.body.Add(tail);
            this.body.Add(head);
        }


        public void Move() {
            Vector2D head = body[body.Count - 1];
            Vector2D neck = body[body.Count - 2];

            Vector2D head_difference = head - neck;
            head_difference.Normalize();

            body[body.Count - 1] = head + (head_difference * speed);

            // Move the tail by its velocity if the snake is not growing.
            if (!isGrowing) {
                Vector2D tail = body[0];
                Vector2D abdomen = body[1];

                if (tail.Equals(abdomen)) {
                    body.RemoveAt(0);
                    tail = body[0];
                    abdomen = body[1];
                }
                Vector2D tail_difference = abdomen - tail;
                tail_difference.Normalize();

                body[0] = tail + (tail_difference * speed);
            }
        }

        public void Turn(Vector2D dirChange) {

            Vector2D velocity = dirChange * speed;

            // Add a new vertex to the snakes body to act as the new head 
            Vector2D newHead = new Vector2D(body[body.Count - 1].X, body[body.Count - 1].Y);
            newHead += velocity;
            body.Add(newHead);

        }

		public void hitPowerup(World theWorld, List<Powerup> powerups) {
			lock (theWorld) {
				foreach (Powerup p in theWorld.Powerups.Values) {
					double distance = (p.loc - body[body.Count - 1]).Length();
					if (distance <= p.width + width) {
						p.died = true;
						powerups.Add(p);
					}
				}
			}
		}

        public void hitWall(World theWorld) {
            lock (theWorld) {
                foreach (Wall w in theWorld.Walls.Values) {
                    
                }
            }
        }


    }
}


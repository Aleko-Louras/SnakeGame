using System;
namespace WorldModel
{
	public class Snake
	{
		public int snake { get; private set; }
        public string name { get; private set; }
		public List<SnakeGame.Vector2D> body { get; private set; }
        public SnakeGame.Vector2D dir { get; private set; }
        public int score { get; private set; }
        public bool died { get; private set; }
        public bool alive { get; private set; }
        public bool dc { get; private set; }
        public bool join { get; private set; }

        public Snake(int snake,
					string name,
					List<SnakeGame.Vector2D> body,
					SnakeGame.Vector2D dir,
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


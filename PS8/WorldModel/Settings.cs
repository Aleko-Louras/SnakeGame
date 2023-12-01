using System;
namespace WorldModel
{
	public class Settings
	{
		public int UniverseSize { get; private set; }
		public int RespawnRate{ get; private set; }
        public int MSPerFrame { get; private set; }
        public Wall[]? Walls { get; private set; }

        public Settings()
		{
			this.UniverseSize = UniverseSize;
			this.RespawnRate = RespawnRate;
			this.MSPerFrame = MSPerFrame;
			this.Walls = Walls;

		}
	}
}


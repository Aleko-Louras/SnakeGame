using System;
using System.Runtime.Serialization;
namespace WorldModel
{
    [DataContract(Namespace = "")]
    public class GameSettings
	{
		[DataMember(Name ="UniverseSize")]
		public int UniverseSize { get;  set; }
        [DataMember(Name = "RespawnRate")]
        public int RespawnRate{ get;  set; }
        [DataMember(Name = "MSPerFrame")]
        public int MSPerFrame { get;  set; }

        //TODO: Fix wall deserialization
        [DataMember(Name = "Walls")]
        public List<Wall> Walls { get; private set; }

        public GameSettings()
		{ 
		}
	}
}


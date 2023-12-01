using System;
using System.Xml.Serialization;
namespace WorldModel
{
	[Serializable]
	public class Settings
	{
		[XmlElement]
		public int UniverseSize { get;  set; }
        [XmlElement]
        public int RespawnRate{ get;  set; }
        [XmlElement]
        public int MSPerFrame { get;  set; }
        [XmlIgnore]
        public Wall[]? Walls { get; private set; }

        public Settings()
		{ 
		}
	}
}


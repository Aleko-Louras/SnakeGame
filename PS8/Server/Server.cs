using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Xml;
using WorldModel;

namespace Server;

public class Server
{

    public static void Main(string[] args)
    {
        // Decode the XML Settings into the World Model
        DataContractSerializer ser = new(typeof(GameSettings));
        string relativePath = Path.Combine("..", "..", "..", "..", "settings.xml");
        FileStream fs = new FileStream(relativePath, FileMode.Open);
        XmlDictionaryReader reader =
                XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
        
        GameSettings settings = (GameSettings)ser.ReadObject(reader, true)!;
        reader.Close();
        fs.Close();
        Console.WriteLine(settings.UniverseSize);
        Console.WriteLine(settings.MSPerFrame);
        Console.WriteLine(settings.RespawnRate);
        Console.WriteLine(settings.Walls);
        
       
        World theWorld = new World(settings.UniverseSize);
        
        Console.WriteLine("The world size is: " + theWorld.Size);
        foreach (Wall w in settings.Walls)
        {
            theWorld.Walls.Add(w.wall,w);
            

        }
        ServerController server = new ServerController();
        server.SetWorld(theWorld);
        server.StartServer();

        Console.WriteLine("Server running...");
        //Console.Read();

        Stopwatch watch = new Stopwatch();
        watch.Start();
        while (true)
        {
            while (watch.ElapsedMilliseconds < settings.MSPerFrame) { }

            watch.Restart();
            server.Update();
        }
    }
    
} 


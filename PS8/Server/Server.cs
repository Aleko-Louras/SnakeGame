using System.IO;
using System.Net.NetworkInformation;
using System.Xml.Serialization;
using WorldModel;

namespace Server;

public class Server
{
    public static void Main(string[] args)
    {
        // Decode the XML Settings into the World Model
        XmlSerializer xMLSerializer = new XmlSerializer(typeof(Settings));
        using FileStream myFileStream = new FileStream("myFileName.xml", FileMode.Open);
        // Call the Deserialize method and cast to the object type.
        Settings settings = (Settings)xMLSerializer.Deserialize(myFileStream);

        World theWorld = new World(settings.UniverseSize);
        Console.WriteLine("The world size is: " + theWorld.Size);

        ServerController server = new ServerController();
        server.StartServer();

        Console.WriteLine("Server running...");
        Console.Read();


    }

} 


#define STAGE01
#define STAGE02

using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Lab07;

public class Program
{

	static void Main(string[] args)
	{
#if STAGE01
		Console.WriteLine("STAGE01:\n");

		var firstServer = new Server("192.168.1.10", "Server00", Status.Running, 25);
		Console.WriteLine(firstServer);

        //-----------------------------------------------------------

        firstServer.PropertyChanged += (sender, args) =>
        {
            Console.WriteLine($"{firstServer.Address}: {args.PropertyName} => {firstServer.GetPropertyValue(args.PropertyName)}");
        };

        //-----------------------------------------------------------

        firstServer.Status = Status.Stopped;
		firstServer.Load = 0.0;
		Console.WriteLine();

#endif // STAGE01
#if STAGE02
		Console.WriteLine("STAGE02:\n");

		var random = new Random(2137);
		var system = new ServerSystem();
		var servers = new List<Server> {
			new("192.168.1.10", "Server00"),
			new("192.168.1.20", "Server01"),
			new("192.168.1.30", "Server02"),
			new("192.168.1.40", "Server03")
		};

		//-----------------------------------------------------------
		// Add your code for here...



		//-----------------------------------------------------------

		Console.WriteLine();

		var server03 = servers[3];
		system.Remove(server03);
		servers.Remove(server03);

		server03.Load = 90;
		server03.Status = Status.Failed;

		if (!system.Remove(server03))
		{
			Console.WriteLine($"Couldn't remove server {server03} (it has already been removed).");
		}

		Console.WriteLine();
	}
}




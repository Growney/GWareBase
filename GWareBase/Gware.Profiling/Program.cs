using Gware.Gaming.Common.Networking;
using Gware.Gaming.Common.Networking.GamePacket;
using Gware.Gaming.Common.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Profiling
{
    class Program
    {
        private static int c_port = 1337;
        public static void Main(string[] args)
        {
            Gware.Gaming.Common.Networking.GameClient client = null;
            Gware.Gaming.Common.Networking.GameServer server = null;
            string input;
            do
            {
                Console.WriteLine("Please enter command:");
                input = Console.ReadLine();
                switch (input.ToLower())
                {
                    case "startserver":
                        Console.WriteLine("Starting Server");
                        server = new GameServer(c_port);
                        server.Start();
                        Console.WriteLine("Server Started");
                        break;
                    case "startclient":
                        Console.WriteLine("Starting Client");
                        System.Net.IPAddress addr;
                        System.Net.IPAddress.TryParse("127.0.0.1", out addr);
                        client = new GameClient(new System.Net.IPEndPoint(addr, c_port));
                        client.Start();
                        Console.WriteLine("Started Client");
                        break;
                    case "ping":
                        if(client != null)
                        {
                            Console.WriteLine("Ping started:");
                            client.Send(new InfoRequestPacket(InfoRequestPacketType.Ping));
                        }
                        else
                        {
                            Console.WriteLine("No Client Initialised");
                        }
                        break;
                    case "stress":
                        if (client != null)
                        {
                            Console.WriteLine("Stress Started");
                            for (int i = 0; i < 5000; i++)
                            {
                                Console.CursorLeft = 0;
                                Console.WriteLine(String.Format("Ping started: {0}",i+1));
                                client.Send(new InfoRequestPacket(InfoRequestPacketType.Ping));
                            }
                            Console.WriteLine("Stress Complete");
                        }
                        else
                        {
                            Console.WriteLine("No Client Initialised");
                        }
                        break;
                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }

            } while (input != "Stop");
            
            if(client != null)
            {
                client.Stop();
            }

            if(server != null)
            {
                server.Stop();
            }
        }
    }
}

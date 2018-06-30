using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        public static void Run()
        {
            TcpListener server = new TcpListener(IPAddress.Loopback, 5670);

            while (true)
            {
                Console.WriteLine("waiting for client...");
                server.Start();
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("client connected");
                new ClientHandler(client);
            }
        }
    }
}

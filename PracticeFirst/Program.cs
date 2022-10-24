using Common;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Server
{
    class Program
    {
        static Storage storage = new Storage();

        static void Main(string[] args)
        {
            int port = int.Parse(args[0]);
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var endpoint = new IPEndPoint(IPAddress.Any, port);

            listener.Bind(endpoint);
            listener.Listen(100);

            while (true)
            {
                var socketToClient = listener.Accept();
                var thread = new Thread(ProcessClient);
                thread.Start(socketToClient);
            }
        }

        private static void ProcessClient(object socket)
        {
            var socketToClient = (Socket)socket;
            try
            {
                while (true)
                {
                    var buffer = new byte[1000];

                    var count = socketToClient.Receive(buffer);
                    var json = Encoding.UTF8.GetString(buffer, 0, count);
                    var request = (Request)JsonSerializer.Deserialize(json, typeof(Request));

                    string response = string.Empty;
                    string[] responseForReceive;
                    Console.WriteLine("On serv");
                    byte[] responseBytes;
                    switch (request.Mode)
                    {
                        case RequestType.Transmit:
                            Console.WriteLine("Have tr");
                            response = storage.Add(request.Files);
                            responseBytes = Encoding.UTF8.GetBytes(response);
                            socketToClient.Send(responseBytes);
                            break;
                        case RequestType.Receive:
                            Console.WriteLine("Have re");
                            responseForReceive = storage.Get();
                            responseBytes = Encoding.UTF8.GetBytes(responseForReceive[0]);
                            socketToClient.Send(responseBytes);
                            break;
                    }

                    //var responseBytes = Encoding.UTF8.GetBytes(response);
                    //socketToClient.Send(responseBytes);
                }
            }
            catch
            {
                Console.WriteLine($"Клиент {(socketToClient.RemoteEndPoint as IPEndPoint).Address} отключился");
                return;
            }
        }
    }
}

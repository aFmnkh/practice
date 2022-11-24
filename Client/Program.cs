using Common;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var ip = IPAddress.Parse(args[0]);
            var port = int.Parse(args[1]);
            Console.WriteLine(args[2]);
            Console.WriteLine(args[3]);


            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var endpoint = new IPEndPoint(ip, port);
            socket.Connect(endpoint);

            //var files = new string[args.Length - 3];
            byte[] bytesToSend = null;
            var request = new Request();

            if (args[2].ToString() == "-transmit")
            {

                for (var i = 0; i != args.Length - 3; i++)
                {
                    var streamReader = new StreamReader(args[i + 3]);
                    try
                    {
                        //files[i] = streamReader.ReadToEnd();
                        bytesToSend = Encoding.UTF8.GetBytes(streamReader.ReadToEnd());
                        streamReader.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: " + e);
                    }
                }
                Console.WriteLine("Req create transmit");
                request = new Request()
                {
                    Mode = ParseRequestType(args[2]),
                    Bytes = bytesToSend,
                };
            }
            if (args[2].ToString() == "-receive")
            {
                Console.WriteLine("Req create receive");
                request = new Request()
                {
                    Mode = ParseRequestType(args[2]),
                };
            }

            /*var request = new Request()
        {
            Mode = ParseRequestType(args[2]),
            Files = files,
        };*/
            var json = JsonSerializer.Serialize(request);
            var bytes = Encoding.UTF8.GetBytes(json);

            socket.Send(bytes);

            var inputBuffer = new byte[1000];
            var count = socket.Receive(inputBuffer);

            if (args[2].ToString() == "-transmit")
            {
                Console.WriteLine("Transmit take res");
                var response = Encoding.Default.GetString(inputBuffer, 0, count);
                Console.WriteLine(response);
            }
            if (args[2].ToString() == "-receive")
            {
                Console.WriteLine(args[3]);
                Console.WriteLine(args[3].ToString());
                var response = Encoding.Default.GetString(inputBuffer, 0, count);
                Console.WriteLine(response);
                //File.Create(args[3]);
                //FileInfo fileInfo = new FileInfo(args[3]);
                //fileInfo.Delete();
                try
                {
                    var streamWriter = new StreamWriter(args[3].ToString());
                    streamWriter.WriteLine(response.ToString());
                    streamWriter.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e);
                }

            }


        }

        private static RequestType ParseRequestType(string requestTupe)
        {
            switch (requestTupe)
            {
                case "-transmit":
                    return RequestType.Transmit;
                case "-receive":
                    return RequestType.Receive;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

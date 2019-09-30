using System;
using System.Text;
using System.Net;  
using System.Net.Sockets;

class MainProg
{


    class Sockets 
    {
        public const string localhost = "localhost";
        public const int port = 8888;
        private const int maxBuffer = 255;
        private const int maxClients = 1;
        public void Start(string host, int port)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(host);  
            IPAddress ipAddress = ipHostInfo.AddressList[maxClients];
            IPEndPoint serverIP = new IPEndPoint(ipAddress, port);
            Socket server = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp );
            server.Bind(serverIP);
            Console.WriteLine("Connected to {0}", serverIP);
            server.Listen(1);
            while (true)
            {
                try 
                {
                    Socket client = server.Accept();
                    Console.WriteLine("Client connected from {0}", client.RemoteEndPoint);
                    while (true)
                    {
                        byte[] recvData = new byte[maxBuffer];
                        int num = client.Receive(recvData);
                        string recvString = Encoding.UTF8.GetString(recvData, 0, num);
                        if (recvString == "Done\n\n") 
                        {
                            Console.WriteLine("Client done.");
                            break;
                        }
                        Console.WriteLine("Received message: {0}", recvString);
                        Console.WriteLine("Sending reply");
                        byte[] sendData = Encoding.UTF8.GetBytes(recvString);
                        client.Send(sendData);
                    }
                }

                catch (System.Net.Sockets.SocketException e)
                {
                    if (e.ErrorCode == 10054)
                    {
                        Console.WriteLine("Client disconnected");
                    }  
                }
            }
        }
    }

    static void Main()
    {
        Sockets socket = new Sockets();
        socket.Start(Sockets.localhost, Sockets.port);
    }
}
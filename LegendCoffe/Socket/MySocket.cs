using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LegendCoffe
{
    class MySocket
    {
        // State object for reading client data asynchronously  
        public class StateObject
        {
            // Size of receive buffer.  
            public const int BufferSize = 1024;

            // Receive buffer.  
            public byte[] buffer = new byte[BufferSize];

            // Received data string.
            public StringBuilder sb = new StringBuilder();

            // Client socket.
            public Socket workSocket = null;

            public Func<string, string> handleMSG;
        }


        Func<string, string> handleMSG;

        // Thread signal.  
        public ManualResetEvent allDone = new ManualResetEvent(false);

        public MySocket(Func<string, string> handle)
        {
            this.handleMSG = handle;
        }

        public void StartListening()
        {
            // Establish the local endpoint for the socket.  
            // The DNS name of the computer  
            // running the listener is "host.contoso.com".  
            //IPHostEntry ipHostInfo = Dns.GetHostEntry("0.0.0.0");
            IPAddress ipAddress = IPAddress.Parse("0.0.0.0");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 8888);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.  
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    Console.WriteLine("Waiting for a connection...");
                    Debug.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }


        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            StateObject state = new StateObject();
            state.handleMSG = handleMSG;
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket.
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.sb.Append(Encoding.UTF8.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read
                // more data.  
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the
                    // client. Display it on the console.  
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);
                    Debug.WriteLine(string.Format("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content));
                    // Echo the data back to the client.  
                    Send(handler, state.handleMSG(content.Replace("<EOF>", "")));
                }
                else
                {
                    // Not all data received. Get more.  
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using UTF8ASCII encoding.  
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);
                Debug.WriteLine(string.Format("Sent {0} bytes to client.", bytesSent));

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void StartServer()
        {
            ThreadPool.QueueUserWorkItem((unused) =>
            {
                StartListening();
            }, null);

        }

        public void StartServer__()
        {
            System.Threading.ThreadPool.QueueUserWorkItem((unused) =>
            {
                TcpListener server = new TcpListener(IPAddress.Parse("0.0.0.0"), 8888);

                server.Start();
                Console.WriteLine("Server has started on 127.0.0.1:80.{0}Waiting for a connection...", Environment.NewLine);
                Debug.WriteLine("Server has started on 127.0.0.1:80.{0}Waiting for a connection...", Environment.NewLine);

                TcpClient client = server.AcceptTcpClient();

                Console.WriteLine("A client connected.");
                Debug.WriteLine("A client connected.");

                NetworkStream stream = client.GetStream();
                stream.Write(Encoding.UTF8.GetBytes("hello from server!"+Environment.NewLine));
                /*
                //enter to an infinite cycle to be able to handle every change in stream
                while (true)
                {
                    while (!stream.DataAvailable) ;

                    Byte[] bytes = new Byte[client.Available];

                    stream.Read(bytes, 0, bytes.Length);

                    Debug.WriteLine(Encoding.UTF8.GetString(bytes));
                }
                */

            }, null);
        }
    }
}

using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace HttpServer
{
    class Server
    {
        public static HttpListener listener;
        public static string url = "http://localhost:8000/";

        public static async Task HandleIncomingConnection() {
            bool runserver = true;

            while(runserver) {
                HttpListenerContext context = await listener.GetContextAsync();

                HttpListenerRequest req = context.Request;
                HttpListenerResponse res = context.Response;

                if((req.HttpMethod == "GET") && (req.Url.AbsolutePath == "/")) {
                    byte[] data = Encoding.UTF8.GetBytes("Hello from Richard's HttpServer");

                    res.ContentEncoding = Encoding.UTF8;
                    res.StatusCode = 200;
                    await res.OutputStream.WriteAsync(data);
                    res.Close();
                }

                else if(req.HttpMethod == "GET" && req.Url.AbsolutePath == "/home") {
                    byte[] data = Encoding.UTF8.GetBytes("This is Home Page");

                    res.ContentEncoding = Encoding.UTF8;
                    res.StatusCode = 200;
                    await res.OutputStream.WriteAsync(data);
                    res.Close();
                }
            }
        }

        public static void Main(string[] args)
        {
            listener = new HttpListener();

            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("Listening for connections on {0}", url);

            Task listenTask = HandleIncomingConnection();
            listenTask.GetAwaiter().GetResult();

            listener.Close(); 

        }
    }
}

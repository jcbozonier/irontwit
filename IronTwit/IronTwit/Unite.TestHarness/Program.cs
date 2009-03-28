using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using jabber.client;

namespace Unite.TestHarness
{
    class Program
    {
        private static ManualResetEvent done = new ManualResetEvent(false);
        static void Main(string[] args)
        {
            var client = new JabberClient();

            client.AutoPresence = false;
            client.AutoRoster = false;
            client.AutoReconnect = -1;

            client.User = "herbstudent";
            client.Password = "samhain8";
            client.Server = "gmail.com";
            client.Port = 5222;
            client.Resource = "Pidgin";

            client.OnMessage += (s, e) =>
                                    {
                                        Console.WriteLine("Message sent...");
                                    };

            client.OnAuthError += (s, e) =>
                                      {
                                          Console.WriteLine("Auth Error: " + e.InnerText);
                                          client.Close();
                                      };

            client.OnAuthenticate += s =>
                                         {
                                             Console.WriteLine("Authenticated...");
                                             client.Message("darkxanthos@gmail.com", "Testing");
                                         };

            client.OnError += (s, e) =>
                                  {
                                      Console.WriteLine("Error! " + e.Message);
                                      client.Close();
                                  };

            

            client.Connect();

            done.WaitOne();
            client.Close();
        }

        static void client_OnError(object sender, Exception ex)
        {
            
        }

        static void client_OnMessage(object sender, jabber.protocol.client.Message msg)
        {
            throw new NotImplementedException();
        }
    }
}

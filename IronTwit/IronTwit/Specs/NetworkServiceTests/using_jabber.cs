using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoogleTalkPlugIn;
using NUnit.Framework;
using SpecUnit;

namespace Unite.Specs.NetworkServiceTests
{
    [TestFixture]
    public class using_jabber
    {
        [Test]
        public void Connecting_Test()
        {
            var erroredOut = false;
            var isConnected = false;

            using (var client = new jabber.client.JabberClient())
            {
                client.User = "herbstudent";
                client.Password = "samhain8";
                client.Server = "gmail.com";
                client.Port = 5222;
                client.Resource = "Library";
                client.AutoLogin = true;

                client.OnError += (s, e) => erroredOut = true;
                client.OnAuthError += (s, e) =>
                                          {
                                              erroredOut = true;
                                          };
                client.OnLoginRequired += s =>
                                              {
                                                  isConnected = true;
                                              };

                client.OnAuthenticate += s=>
                                             {
                                                 client.Message("darkxanthos@gmail.com", "Testing");
                                             };

                client.Connect();
                client.Login();
                //client.Message("darkxanthos@gmail.com", "Testing");
                
                erroredOut.ShouldBeFalse();
                
                client.IsAuthenticated.ShouldBeTrue();
            }
        }

        void client_OnAuthenticate(object sender)
        {
            throw new NotImplementedException();
        }

        void client_OnLoginRequired(object sender)
        {
            
        }
    }
}

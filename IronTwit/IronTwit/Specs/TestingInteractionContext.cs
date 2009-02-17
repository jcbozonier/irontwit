using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronTwit.Models;

namespace Specs
{
    public class TestingInteractionContext : IInteractionContext
    {
        public Credentials GetCredentials()
        {
            return new Credentials()
                       {
                           UserName = "username",
                           Password = "password"
                       };
        }
    }
}

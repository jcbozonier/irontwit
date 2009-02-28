using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniteMessaging;

namespace IronTwit.Utilities
{
    public interface IInteractionContext
    {
        Credentials GetCredentials();
        bool AuthenticationFailedRetryQuery();
    }
}

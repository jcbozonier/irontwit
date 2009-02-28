using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unite.Messaging;

namespace Unite.UI.Utilities
{
    public interface IInteractionContext
    {
        Credentials GetCredentials();
        bool AuthenticationFailedRetryQuery();
    }
}

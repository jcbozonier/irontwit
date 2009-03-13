﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unite.Messaging;

namespace Unite.Messaging
{
    public interface IInteractionContext
    {
        Credentials GetCredentials(IServiceInformation serviceInformation);
        bool AuthenticationFailedRetryQuery();
    }
}

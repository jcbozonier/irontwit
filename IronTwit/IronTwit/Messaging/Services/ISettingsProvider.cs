using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging.Services
{
    public interface ISettingsProvider
    {
        IEnumerable<Credentials> GetStoredCredentials();
    }
}

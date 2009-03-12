using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging
{
    //Service specific (i.e. not an "Identity" which is a semantic grouping of ISenders under one friendly name)
    public interface IRecipient
    {
        /// <summary>
        /// For Twitter, e.g. '@darkxanthos'
        /// For Email, e.g. 'darkxanthos@gmail.com'
        /// </summary>
        string UserName { get; }
    }
}

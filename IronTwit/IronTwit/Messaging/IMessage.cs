using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging
{
    public interface IMessage
    {
        string Text { get; set; }
        IRecipient Recipient { get; set; }
    }
}

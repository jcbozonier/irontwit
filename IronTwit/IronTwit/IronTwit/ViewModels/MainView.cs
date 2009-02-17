using System.Collections.ObjectModel;
using System.Windows;
using IronTwit.Models;
using IronTwit.Models.Twitter;

namespace IronTwit.ViewModels
{
    public class MainView : DependencyObject
    {
        public IInteractionContext Interactions;

        public string UserName { get; set; }
        public string Password { get; set; }

        public ObservableCollection<Tweet> Tweets { get; set; }
        public ObservableCollection<Tweet> MyReplies { get; set; }

        public string MessageToSend { get; set; }
        public string Recipient { get; set; }
        public SendMessageCommand SendMessage { get; set; }

        public void ApplicationStarting()
        {
            var credentials = Interactions.GetCredentials();
            UserName = credentials.UserName;
            Password = credentials.Password;
        }
    }
}

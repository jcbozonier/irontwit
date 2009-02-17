using System.Collections.ObjectModel;
using System.Windows;
using IronTwit.Models.Twitter;

namespace IronTwit.ViewModels
{
    public class MainView : DependencyObject
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public static DependencyProperty TweetsProperty =
            DependencyProperty.Register("Tweets", typeof (ObservableCollection<Tweet>), typeof (MainView));
        public ObservableCollection<Tweet> Tweets
        {
            get
            {
                return (ObservableCollection<Tweet>)GetValue(TweetsProperty);
            }
            set
            {
                SetValue(TweetsProperty, value);
            }
        }
        public ObservableCollection<Tweet> MyReplies { get; set; }
        public string MessageToSend { get; set; }
        public string Recipient { get; set; }
        public SendMessageCommand SendMessage { get; set; }
    }
}

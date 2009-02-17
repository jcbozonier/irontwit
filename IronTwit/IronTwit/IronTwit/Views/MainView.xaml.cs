using System.Collections.Generic;
using System.IO;
using System.Windows;
using IronTwit.Models.Twitter;
using IronTwit.ViewModels;
using Yedda;
using Newtonsoft.Json;

namespace IronTwit.Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            Loaded += Window1_Loaded;
        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            
            var twit = new Twitter();

            var resultString = twit.GetFriendsTimeline("darkxanthos@gmail.com", "K230184J", Twitter.OutputFormatType.JSON);
            var str = new StringReader(resultString);
            var converter = new JsonSerializer();
            converter.MissingMemberHandling = MissingMemberHandling.Ignore;
            var obj = (List<Tweet>)converter.Deserialize(str, typeof(List<Tweet>));
            Inbox.ItemsSource = obj; 
        }
    }
}

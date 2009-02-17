using System.Collections.Generic;
using System.IO;
using System.Windows;
using IronTwit.Models;
using IronTwit.ViewModels;
using Yedda;
using Newtonsoft.Json;

namespace IronTwit
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Window1_Loaded);
        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            
            var twit = new Twitter();

            var resultString = twit.GetFriendsTimeline("darkxanthos@gmail.com", "K230184J", Twitter.OutputFormatType.JSON);
            var str = new StringReader(resultString);
            var converter = new JsonSerializer();
            converter.MissingMemberHandling = MissingMemberHandling.Ignore;
            var obj = (List<Tweet>)converter.Deserialize(str, typeof(List<Tweet>));
            Inbox.DataContext = obj; 
        }
    }
}

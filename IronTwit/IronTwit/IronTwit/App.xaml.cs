using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using IronTwit.Models.Twitter;
using IronTwit.ViewModels;

namespace IronTwit
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            var model = new MainView()
                            {
                                Tweets = new ObservableCollection<Tweet>
                                             {
                                                 new Tweet()
                                                     {
                                                         text = "Testing 1",
                                                         user = new TwitterUser() {screen_name = "darkxanthos"}
                                                     },
                                                 new Tweet()
                                                     {
                                                         text = "Testing 2",
                                                         user = new TwitterUser() {screen_name = "darkxantho"}
                                                     },
                                                 new Tweet()
                                                     {
                                                         text = "Testing 3",
                                                         user = new TwitterUser() {screen_name = "darkxanths"}
                                                     },
                                                 new Tweet()
                                                     {
                                                         text = "Testing 4",
                                                         user = new TwitterUser() {screen_name = "darkxantos"}
                                                     },
                                             },
                                MyReplies = new ObservableCollection<Tweet>()
                                                 {
                                                     new Tweet()
                                                     {
                                                         text = "Testing 1",
                                                         user = new TwitterUser() {screen_name = "darkxanthos"}
                                                     }
                                                 }
                            };
            var window = new Views.MainView();
            window.DataContext = model;
            window.Show();
        }
    }
}

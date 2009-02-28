using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using IronTwit.ViewModels;
using Specs;
using StructureMap;

namespace IronTwit
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Startup += App_Startup;
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            ContainerBootstrapper.BootstrapStructureMap();

            var window = new Views.MainView();
            window.DataContext = ObjectFactory.GetInstance<ViewModels.MainView>();
            window.Show();
        }
    }
}

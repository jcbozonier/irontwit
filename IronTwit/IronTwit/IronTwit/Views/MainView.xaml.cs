using System.Collections.Generic;
using System.IO;
using System.Windows;
using IronTwit.Utilities;
using IronTwit.ViewModels;
using StructureMap;

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
            var model = ObjectFactory.GetInstance<ViewModels.MainView>();
            DataContext = model;

            model.ApplicationStarting();
        }

        
    }
}

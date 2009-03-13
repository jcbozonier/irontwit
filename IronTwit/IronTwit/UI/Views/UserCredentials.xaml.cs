using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Unite.UI.ViewModels;

namespace Unite.UI.Views
{
    /// <summary>
    /// Interaction logic for UserCredentials.xaml
    /// </summary>
    public partial class UserCredentialsWindow : Window
    {
        public UserCredentialsWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UserName.Focus();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((UserCredentialsViewModel) DataContext).Password = Password.Password;
        }
    }
}

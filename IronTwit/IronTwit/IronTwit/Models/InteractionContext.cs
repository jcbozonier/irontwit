using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using IronTwit.ViewModels;
using IronTwit.Views;

namespace IronTwit.Models
{
    public class GuiInteractionContext : IInteractionContext
    {
        public Credentials GetCredentials()
        {
            var model = new UserCredentialsViewModel();

            var dialog = new UserCredentialsWindow
                             {
                                 DataContext = model
                             };
            dialog.ShowDialog();

            return new Credentials()
                       {
                           UserName = model.UserName,
                           Password = model.Password
                       };
        }

        public bool AuthenticationFailedRetryQuery()
        {
            var result = MessageBox.Show("Username and/or password are not correct. Retry?",
                            "IronTwit by Justin Bozonier",
                            MessageBoxButton.YesNo);
            return result == MessageBoxResult.Yes;
        }
    }

    public interface IInteractionContext
    {
        Credentials GetCredentials();
        bool AuthenticationFailedRetryQuery();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }

    public interface IInteractionContext
    {
        Credentials GetCredentials();
    }
}

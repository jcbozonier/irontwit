using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Unite.Messaging.Entities;
using Unite.UI.ViewModels;
using Unite.UI.Views;
using Unite.Messaging;

namespace Unite.UI.Utilities
{
    public class GuiInteractionContext : IInteractionContext
    {
        private Thread _mainThread;

        public GuiInteractionContext(Thread mainThread)
        {
            _mainThread = mainThread;
        }

        public Credentials GetCredentials(IServiceInformation serviceInformation)
        {
            var username = "";
            var password = "";

            var dispatcher = Dispatcher.FromThread(_mainThread);
            dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                                             {
                                                                 var model = new UserCredentialsViewModel()
                                                                 {
                                                                     Caption = serviceInformation.ServiceName + " Login"
                                                                 };
                                                                 var dialog = new UserCredentialsWindow
                                                                     {
                                                                         DataContext = model
                                                                     };
                                                                 var mainWindow = Application.Current.MainWindow;
                                                                 dialog.Owner = mainWindow;
                                                                 dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                                                                 dialog.ShowDialog();
                                                                 username = model.UserName;
                                                                 password = model.Password;
                                                             }));

            return new Credentials()
                       {
                           UserName = username,
                           Password = password,
                           ServiceInformation = serviceInformation
                       };
        }

        public bool AuthenticationFailedRetryQuery()
        {
            var result = MessageBox.Show("Username and/or password are not correct. Retry?",
                            "Unit3 by Justin Bozonier",
                            MessageBoxButton.YesNo);
            return result == MessageBoxResult.Yes;
        }
    }
}

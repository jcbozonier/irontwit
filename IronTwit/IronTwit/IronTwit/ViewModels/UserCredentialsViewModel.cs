using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace IronTwit.ViewModels
{
    public class UserCredentialsViewModel : DependencyObject
    {
        public static DependencyProperty UserNameProperty = DependencyProperty
            .Register("UserName", typeof (string), typeof (UserCredentialsViewModel));
        public string UserName
        {
            get
            {
                return (string)GetValue(UserNameProperty);
            }
            set
            {
                SetValue(UserNameProperty, value);
            }
        }

        public static DependencyProperty PasswordProperty = DependencyProperty
            .Register("Password", typeof(string), typeof(UserCredentialsViewModel));
        public string Password
        {
            get
            {
                return (string)GetValue(PasswordProperty);
            }
            set
            {
                SetValue(PasswordProperty, value);
            }
        }
    }
}

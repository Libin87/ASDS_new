using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ASDS_dev.Pages.LoginPage;
using ASDS_dev.Pages.Reports.Controls;
using ASDS_dev.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ASDS_dev.Controls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HeaderControl : UserControl
    {
        public HeaderControl()
        {
            this.InitializeComponent();
            if (!string.IsNullOrEmpty(SessionManager.CurrentUsername))
            {
                // User is logged in → Show Logout, Hide Login
                LoginButton.Visibility = Visibility.Collapsed;
                LogoutButton.Visibility = Visibility.Visible;

                UsernameText.Text = $"User: {SessionManager.CurrentUsername}";
            }
            else
            {
                // User is not logged in → Show Login, Hide Logout
                LoginButton.Visibility = Visibility.Visible;
                LogoutButton.Visibility = Visibility.Collapsed;

                UsernameText.Text = "User: Guest";
            }


        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            App.RootFrame.Navigate(typeof(ASDS_dev.Pages.LoginPage.Login), null,
            new SuppressNavigationTransitionInfo());
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var audit = new AuditEvent
            {
                EventTime = DateTime.Now,
                EventType = 2, 
                UserId = SessionManager.Uid,  
                UserName = SessionManager.CurrentUsername,
                EventMessage = "Logout Successful",
                OldValue = "",
                NewValue = "",
                Remarks = "",
                RemarksAdded = 0
            };


            AuditLogger.LogEvent(audit);
            SessionManager.CurrentUsername = null;

            // Refresh UI
            LoginButton.Visibility = Visibility.Visible;
            LogoutButton.Visibility = Visibility.Collapsed;
            UsernameText.Text = "User: Guest";

            App.RootFrame.Navigate(typeof(ASDS_dev.Pages.HomePage.HomePage), null,
            new SuppressNavigationTransitionInfo()); // Change `MainWindow` to your actual home page class
           

        }
    }
        

    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dateTime)
                return dateTime.ToString("HH:mm:ss"); // Format as needed

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
       
    }
}

using System;
using System.Threading.Tasks;
using ASDS_dev.Pages.UserManagement;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ASDS_dev.ViewModels;
using ASDS_dev.Pages.Reports.Controls; 


namespace ASDS_dev.Pages.LoginPage
{
    public sealed partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void ChangePswd(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ASDS_dev.Pages.ChangePswd));
        }


        private async void Login_click(object sender, RoutedEventArgs e)
        {
            string userId = usernameBox.Text;
            string password = PasswordBox.Password;

            var result = UserDatabase.ValidateLogin(userId, password);

            if (result.IsUserNotFound)
            {
                await ShowDialog("Login Failed", "User not found. Please sign up first.");
                return;
            }

            if (result.IsPasswordIncorrect)
            {
                await ShowDialog("Login Failed", "Incorrect password. Please try again.");
                return;
            }

            if (result.IsSuspended)
            {
                await ShowDialog("Access Denied", "Your account is Blocked. Please contact admin.");
                return;
            }


            var audit = new AuditEvent
            {
                EventTime = DateTime.Now,
                EventType = 1, 
                UserId = userId,
                UserName = userId, 
                EventMessage = "Login Successful",
                RemarksAdded = 0
            };

            AuditLogger.LogEvent(audit); 

           
            LoginButton.Visibility = Visibility.Collapsed;
            SessionManager.CurrentUsername = userId;
            Frame.Navigate(typeof(HomePage.HomePage));
            await ShowDialog("Login Successful", $"Welcome back, {userId}!");
        }



        private async Task ShowDialog(string title, string message)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await dialog.ShowAsync();
        }
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ASDS_dev.Pages.HomePage.HomePage));
        }




    }
}

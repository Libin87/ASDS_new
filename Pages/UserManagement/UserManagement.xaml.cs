using ASDS_dev.Pages.UsrMgmt;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ASDS_dev.Pages.UserManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserManagement : Page
    {
        public UserManagement()
        {
            this.InitializeComponent();                  // Initializes XAML elements � must be first
            UserDatabase.InitializeDatabase();           // Ensures DB and table exist
            this.Loaded += UserManagementPage_Loaded;    // Event for loading users after XAML is ready
            XamlRoot = this.Content.XamlRoot;
            //  UsernameText.Text = $"User: {SessionManager.CurrentUsername}";
           
        }
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
        private void ClearInputFields()
        {
            UserIdBox.Text = "";
            FirstNameBox.Text = "";
            LastNameBox.Text = "";
            UserRoleBox.SelectedIndex = -1;
            StatusBox.SelectedIndex = -1;
        }

        private void AddOrUpdateUser(object sender, RoutedEventArgs e)
        {
            // Clear previous messages
            UserIdValidation.Visibility = Visibility.Collapsed;
            FirstNameValidation.Visibility = Visibility.Collapsed;
            LastNameValidation.Visibility = Visibility.Collapsed;
            RoleValidation.Visibility = Visibility.Collapsed;

            string pass1 = "Pass@123";
            string pass = PasswordHelper.HashPassword(pass1);
            string userId = UserIdBox.Text.Trim();
            string firstName = FirstNameBox.Text.Trim();
            string lastName = LastNameBox.Text.Trim();
            string role = (UserRoleBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string status = (StatusBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            bool isValid = true;

            // Validate User ID
            if (string.IsNullOrWhiteSpace(userId) || userId.Length < 5 || !userId.All(char.IsLetterOrDigit))
            {
                UserIdValidation.Text = "User ID must be at least 5 alphanumeric characters.";
                UserIdValidation.Visibility = Visibility.Visible;
                isValid = false;
            }

            // Validate First Name
            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 3 || !firstName.All(char.IsLetter))
            {
                FirstNameValidation.Text = "First Name must be at least 3 letters (no numbers).";
                FirstNameValidation.Visibility = Visibility.Visible;
                isValid = false;
            }

            // Validate Last Name
            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 3 || !lastName.All(char.IsLetter))
            {
                LastNameValidation.Text = "Last Name must be at least 3 letters (no numbers).";
                LastNameValidation.Visibility = Visibility.Visible;
                isValid = false;
            }

            // Validate Role
            if (string.IsNullOrWhiteSpace(role))
            {
                RoleValidation.Text = "Please select a user role.";
                RoleValidation.Visibility = Visibility.Visible;
                isValid = false;
            }

            // Default status to Active
            if (string.IsNullOrWhiteSpace(status))
            {
                status = "Active";
            }

            if (!isValid) return; // Stop processing if validation failed

            try
            {
                if (selectedUser == null)
                {
                    // Add new user
                    UserDatabase.AddUser(userId, firstName, lastName, role, status, pass);
                    Users.Add(new User
                    {
                        UserId = userId,
                        FirstName = firstName,
                        LastName = lastName,
                        UserRole = role,
                        Status = status,
                        Pass = pass,
                        CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    });
                }
                else
                {
                    // Update user
                    UserDatabase.UpdateUser(userId, firstName, lastName, role, status);

                    selectedUser.FirstName = firstName;
                    selectedUser.LastName = lastName;
                    selectedUser.UserRole = role;
                    selectedUser.Status = status;

                    Users.Clear();
                    foreach (var u in UserDatabase.GetAllUsers()) Users.Add(u);

                    UserIdBox.IsEnabled = true;
                    selectedUser = null;
                }

                ClearInputFields();
                AddUserButton.Content = "Add User";
            }
            catch (Exception ex)
            {
                ShowMessage($"Error: {ex.Message}");
            }
        }




        private async void ShowMessage(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Message",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot // Needed for UWP
            };
            await dialog.ShowAsync();
        }





        private async void UserManagementPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadUsersAsync();
        }
        private async Task LoadUsersAsync()
{
    Users.Clear();
    var userList = await Task.Run(() => UserDatabase.GetAllUsers());
    foreach (var user in userList)
        Users.Add(user);
}
        private async void DeleteClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var user = button?.Tag as User;

            if (user == null) return;

            var confirmDialog = new ContentDialog
            {
                Title = "Delete User",
                Content = $"Are you sure you want to delete user '{user.UserId}'?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.Content.XamlRoot
            };

            var result = await confirmDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    UserDatabase.DeleteUser(user.UserId);  
                    Users.Remove(user);                    
                }
                catch (Exception ex)
                {
                    await new ContentDialog
                    {
                        Title = "Error",
                        Content = $"Failed to delete user: {ex.Message}",
                        CloseButtonText = "OK",
                        XamlRoot = this.Content.XamlRoot
                    }.ShowAsync();
                }
            }
        }
        private User selectedUser = null;
        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var user = button?.Tag as User;
            if (user == null) return;

            // Populate input fields
            UserIdBox.Text = user.UserId;
            FirstNameBox.Text = user.FirstName;
            LastNameBox.Text = user.LastName;
            UserRoleBox.SelectedItem = UserRoleBox.Items
       .OfType<ComboBoxItem>()
       .FirstOrDefault(i => i.Content.ToString() == user.UserRole);

            StatusBox.SelectedItem = StatusBox.Items
                .OfType<ComboBoxItem>()
                .FirstOrDefault(i => i.Content.ToString() == user.Status); ;

            selectedUser = user;
            UserIdBox.IsEnabled = false;
            AddUserButton.Content = "Update User"; // Change button content
        }


        private void CancelClick(object sender, RoutedEventArgs e)
        {
            ClearInputFields();
            selectedUser = null;
            AddUserButton.Content = "Add User";
            UserIdBox.IsEnabled = true;
        }



    }
}

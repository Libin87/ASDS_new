using ASDS_dev.Pages.MaintPage;
using ASDS_dev.Pages.Settings;
using ASDS_dev.Pages.SyncDatetime;
using ASDS_dev.Pages.Reports;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ASDS_dev.Controls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FooterControl : UserControl
    {
        public FooterControl()
        {
            this.InitializeComponent();
        }
        private void ReportsButton_Click(object sender, RoutedEventArgs e)
        {
           
            App.RootFrame.Navigate(typeof(ASDS_dev.Pages.Reports.Reports), null,
    new SuppressNavigationTransitionInfo());
        }

        private void MaintenanceModeButton_Click(object sender, RoutedEventArgs e)
        {
            
            App.RootFrame.Navigate(typeof(ASDS_dev.Pages.MaintPage.MaintenanceModePage), null,
    new SuppressNavigationTransitionInfo());
        }
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {

            App.RootFrame.Navigate(typeof(ASDS_dev.Pages.Settings.Settings), null,
    new SuppressNavigationTransitionInfo());
        }
        private void SyncDateTimeModeButton_Click(object sender, RoutedEventArgs e)
        {

            App.RootFrame.Navigate(typeof(ASDS_dev.Pages.SyncDatetime.SyncDateTime), null,
    new SuppressNavigationTransitionInfo());
        }
        private void UserManagementModeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.RootFrame.Navigate(typeof(ASDS_dev.Pages.UserManagement.UserManagement), null,
                new SuppressNavigationTransitionInfo());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Navigation error: " + ex.Message);
            }
        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.RootFrame.BackStack.Clear();
                App.RootFrame.Navigate(
    typeof(ASDS_dev.Pages.HomePage.HomePage),
    null,
    new SuppressNavigationTransitionInfo()
);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Navigation failed: " + ex.Message);
            }
        }
    }
}

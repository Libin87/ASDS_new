using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using ASDS_dev.Pages.Reports.Controls;


namespace ASDS_dev.Pages.Reports
{
    public sealed partial class Reports : Page
    {
        public Reports()
        {
            this.InitializeComponent();
        }
        private void ToggleSidebar_Click(object sender, RoutedEventArgs e)
        {
            if (Sidebar.Visibility == Visibility.Visible)
            {
                Sidebar.Visibility = Visibility.Collapsed;
                SidebarColumn.Width = new GridLength(0);
            }
            else
            {
                Sidebar.Visibility = Visibility.Visible;
                SidebarColumn.Width = new GridLength(150); 
            }
        }


        private void SidebarButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tag)
            {
                ContentArea.Children.Clear(); 

                switch (tag)
                {
                    case "Audit":
                        ContentArea.Children.Add(new Audit());
                        break;

                    case "Users":
                        ContentArea.Children.Add(new Users());
                        break;

                    case "Events":
                        ContentArea.Children.Add(new Events()); 
                        break;

                    case "Alarm":
                        ContentArea.Children.Add(new Alarm()); 
                        break;

                    default:
                        break;
                }
            }
        }


    }
}

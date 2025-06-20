using Microsoft.UI;
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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ASDS_dev.Pages.MaintPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MaintenanceModePage : Page
    {
        private bool isOn = false;

        public MaintenanceModePage()
        {
            this.InitializeComponent();
        }
        private void CustomToggle_Checked(object sender, RoutedEventArgs e)
        {
            ToggleLabel.Text = "ON";
            ToggleBackground.Background = new SolidColorBrush(Colors.Purple);
            var toggleOnStoryboard = (Storyboard)this.Resources["ToggleOnStoryboard"];
            toggleOnStoryboard.Begin();
        }

        private void CustomToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            ToggleLabel.Text = "OFF";
            ToggleBackground.Background = new SolidColorBrush(Colors.Gray);
            var toggleOffStoryboard = (Storyboard)this.Resources["ToggleOffStoryboard"];
            toggleOffStoryboard.Begin();
        }


        private void VolumeClick(object sender, RoutedEventArgs e)
        {
            App.RootFrame.Navigate(typeof(ASDS_dev.Pages.MaintPage.TestVolume), null,
                new SuppressNavigationTransitionInfo());
        }

        private void CalibrationClick(object sender, RoutedEventArgs e)
        {
            App.RootFrame.Navigate(typeof(ASDS_dev.Pages.MaintPage.PumpCalibration), null,
                new SuppressNavigationTransitionInfo());
        }
      

    }
}

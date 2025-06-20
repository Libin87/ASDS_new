using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace ASDS_dev.Pages.SyncDatetime
{
    public sealed partial class SyncDateTime : Page
    {
        private DispatcherTimer timer;

        public SyncDateTime()
        {
            this.InitializeComponent();
            txtDateTime.Text = DateTime.Now.ToString("MMMM dd, yyyy - hh:mm:ss tt");
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            txtDateTime.Text = DateTime.Now.ToString("MMMM dd, yyyy - hh:mm:ss tt");
        }
    }
}

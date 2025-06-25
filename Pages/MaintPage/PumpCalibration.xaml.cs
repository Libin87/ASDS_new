using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;

namespace ASDS_dev.Pages.MaintPage
{
    public sealed partial class PumpCalibration : Page
    {
        private bool isPlaying = false;

        public PumpCalibration()
        {
            this.InitializeComponent();
            UpdateIcon();
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            isPlaying = !isPlaying;
            UpdateIcon();
        }

        private void UpdateIcon()
        {
            IconShape.Children.Clear();
            ButtonText.Text = isPlaying ? "Pause" : "Run 1 Min";
            IconShape.Children.Add(isPlaying ? CreatePauseIcon() : CreatePlayIcon());
        }

        private UIElement CreatePlayIcon()
        {
            return new Path
            {
                Data = new PathGeometry
                {
                    Figures =
                    {
                        new PathFigure
                        {
                            StartPoint = new Point(0, 0),
                            IsClosed = true,
                            Segments =
                            {
                                new LineSegment { Point = new Point(0, 20) },
                                new LineSegment { Point = new Point(17, 10) }
                            }
                        }
                    }
                },
                Fill = new SolidColorBrush(Colors.LightGray)
            };
        }

        private UIElement CreatePauseIcon()
        {
            return new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Children =
                {
                    new Rectangle { Width = 5, Height = 20, Fill = new SolidColorBrush(Colors.LightGray), Margin = new Thickness(2,0,2,0) },
                    new Rectangle { Width = 5, Height = 20, Fill = new SolidColorBrush(Colors.LightGray), Margin = new Thickness(2,0,2,0) }
                }
            };
        }








        private void MntnsPage(object sender, RoutedEventArgs e)
        {
            App.RootFrame.Navigate(typeof(ASDS_dev.Pages.MaintPage.MaintenanceModePage), null,
                new SuppressNavigationTransitionInfo());
        }
    }
}

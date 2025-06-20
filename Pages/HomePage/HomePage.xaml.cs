using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Diagnostics;
using Windows.Storage;
using Windows.Storage.Streams;
using Microsoft.UI.Text;
using ASDS_dev.Controls;
using Microsoft.UI.Xaml.Shapes;

namespace ASDS_dev.Pages.HomePage
{
    public sealed partial class HomePage : Page
    {
        private CanvasBitmap svgImage;
        Storyboard _flowStoryboard;
        public HomePage()
        {
            this.InitializeComponent();
            
            this.Loaded += MainWindow_Loaded;
            HeaderControl.DataContext = App.GlobalHeaderViewModel;

        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/diagram.svg"));
            using IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
            var device = CanvasDevice.GetSharedDevice();
            //svgImage = await CanvasBitmap.LoadAsync(device, stream);
            //SvgCanvas.Invalidate();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SVG Load failed: " + ex.Message);
            }
        }
     

        private bool _isDialogOpen = false;

        private async void BatchSet_Click(object sender, RoutedEventArgs e)
        {
            var solventListPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Spacing = 10,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Padding = new Thickness(10)
            };

            string[] solvents = { "Divosan 2%", "Extran 2%", "IPA 70%", "IPA 100%", "Water", "Bio SP Base 2%", "Bio Actvtr 2%" };
            string selectedSolvent = "";

            var selectedSolventTextBox = new TextBox
            {
                Text = selectedSolvent,
                IsReadOnly = true
            };

            foreach (var solvent in solvents)
            {
                var btn = new Button
                {
                    Content = solvent,
                    Tag = solvent,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Padding = new Thickness(5),
                    Background = new SolidColorBrush(Colors.LightGray),
                    BorderBrush = new SolidColorBrush(Colors.DarkGray),
                    BorderThickness = new Thickness(1),
                    Height = 40,
                    FontWeight = FontWeights.SemiBold,
                    CornerRadius = new CornerRadius(4)
                };

                btn.Click += (s, args) =>
                {
                    selectedSolvent = (string)((Button)s).Tag;
                    selectedSolventTextBox.Text = selectedSolvent;

                    foreach (var child in solventListPanel.Children)
                    {
                        if (child is Button b)
                            b.Background = new SolidColorBrush(Colors.LightGray);
                    }

                    ((Button)s).Background = new SolidColorBrush(Colors.LightBlue);
                };

                solventListPanel.Children.Add(btn);
            }

            // EMP ID TextBox
            var empIdTextBox = new TextBox();

            // Location TextBox
            var locationTextBox = new TextBox();

            // Unit + Purpose in same row
            var unitPurposeGrid = new Grid { ColumnSpacing = 10 };
            unitPurposeGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            unitPurposeGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var unitPanel = new StackPanel();
            unitPanel.Children.Add(new TextBlock { Text = "Unit" });
            var unitComboBox = new ComboBox
            {
                Items =
        {
            new ComboBoxItem { Content = "Unit 1" },
            new ComboBoxItem { Content = "Unit 2" }
        },
                Height = 32
            };
            unitPanel.Children.Add(unitComboBox);
            Grid.SetColumn(unitPanel, 0);

            var purposePanel = new StackPanel();
            purposePanel.Children.Add(new TextBlock { Text = "Purpose" });
            var purposeComboBox = new ComboBox
            {
                Items =
        {
            new ComboBoxItem { Content = "Cleaning" },
            new ComboBoxItem { Content = "Rinse" }
        },
                Height = 32
            };
            purposePanel.Children.Add(purposeComboBox);
            Grid.SetColumn(purposePanel, 1);

            unitPurposeGrid.Children.Add(unitPanel);
            unitPurposeGrid.Children.Add(purposePanel);

            // Quantity Required section with labels
            var qtyGrid = new Grid { ColumnSpacing = 10 };
            qtyGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            qtyGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var qtyTextBox1 = new TextBox { Width = 100 };
            var qtyTextBox2 = new TextBox { Width = 100 };

            var qtyStack1 = new StackPanel();
            qtyStack1.Children.Add(new TextBlock { Text = "Quantity" });
            qtyStack1.Children.Add(qtyTextBox1);
            Grid.SetColumn(qtyStack1, 0);

            var qtyStack2 = new StackPanel();
            qtyStack2.Children.Add(new TextBlock { Text = "Required" });
            qtyStack2.Children.Add(qtyTextBox2);
            Grid.SetColumn(qtyStack2, 1);

            qtyGrid.Children.Add(qtyStack1);
            qtyGrid.Children.Add(qtyStack2);

            // Right-side form layout
            var formPanel = new StackPanel
            {
                Spacing = 10,
                Margin = new Thickness(0),
                Width = 300,
                Children =
        {
            new TextBlock { Text = "Selected" },
           new Border
{
    BorderBrush = new SolidColorBrush(Colors.Gray),
    BorderThickness = new Thickness(1),
    CornerRadius = new CornerRadius(2),
     Width = 260,
     Margin=new Thickness(0, 0, 30, 0),
    Child = selectedSolventTextBox
},

            new TextBlock { Text = "EMP ID Number" },
            new Border
{
    BorderBrush = new SolidColorBrush(Colors.Gray),
    BorderThickness = new Thickness(1),
    CornerRadius = new CornerRadius(2),
     Width = 260,
        Margin=new Thickness(0, 0, 30, 0),
    Child = empIdTextBox
},

            unitPurposeGrid,

            new TextBlock { Text = "Location" },
            new Border
{
    BorderBrush = new SolidColorBrush(Colors.Gray),
    BorderThickness = new Thickness(1),
    CornerRadius = new CornerRadius(2),
     Width = 260,
        Margin=new Thickness(0, 0, 30, 0),
    Child = locationTextBox
},

            qtyGrid
        }
            };

            // Grid layout with two bordered panels
            var layoutGrid = new Grid { Margin = new Thickness(0, 0, 0, 0) };
            layoutGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(160) });
            layoutGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var leftBorder = new Border
            {
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Background = new SolidColorBrush(Colors.WhiteSmoke),
                Margin = new Thickness(0, 0, 0, 0),
                Padding = new Thickness(8),
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Width = 160,
                Child = solventListPanel
            };

            var rightBorder = new Border
            {
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Background = new SolidColorBrush(Colors.WhiteSmoke),
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Padding = new Thickness(8),
                Child = formPanel
            };

            Grid.SetColumn(leftBorder, 0);
            Grid.SetColumn(rightBorder, 1);
            layoutGrid.Children.Add(leftBorder);
            layoutGrid.Children.Add(rightBorder);

            var dialog = new ContentDialog
            {
                Title = "Batch Set",
                PrimaryButtonText = "Set",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot,
                Width = 800,
                Height = 600,
                Background = new SolidColorBrush(Colors.White),
                CornerRadius = new CornerRadius(8),
                Content = layoutGrid
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                // Access entered data here if needed
                string selectedSolventResult = selectedSolventTextBox.Text;
                string empId = empIdTextBox.Text;
                string location = locationTextBox.Text;
                string unit = (unitComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
                string purpose = (purposeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
                string qty1 = qtyTextBox1.Text;
                string qty2 = qtyTextBox2.Text;

                // TODO: Handle the values as needed
            }
        }

        void StartFlowAnimation(Path flowPath)
        {
            flowPath.Opacity = 1; // Make it visible

            var animation = new DoubleAnimation
            {
                From = 0,
                To = 4,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                RepeatBehavior = RepeatBehavior.Forever
            };

            Storyboard.SetTarget(animation, flowPath);
            Storyboard.SetTargetProperty(animation, "StrokeDashOffset");

            var storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }

        void StopFlowAnimation()
        {
            if (_flowStoryboard != null)
            {
                _flowStoryboard.Stop();
            }
        }





        //private void SolventButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (sender is Button button)
        //    {
        //        var textBox = (TextBox)BatchDialog.FindName("SelectedSolvent");
        //        if (textBox != null)
        //            textBox.Text = button.Content.ToString();
        //    }
        //}


        //private void SvgCanvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        //{
        //    if (svgImage != null)
        //    {
        //        args.DrawingSession.DrawImage(svgImage);
        //    }
        //}

        //private void WaterVolumeInput_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (double.TryParse(WaterVolumeInput.Text, out double volume))
        //    {
        //        double maxVolume = 100.0;
        //        double targetScale = Math.Clamp(volume / maxVolume, 0, 1);

        //        DoubleAnimation scaleAnimation = new DoubleAnimation
        //        {
        //            To = targetScale,
        //            Duration = new Duration(TimeSpan.FromMilliseconds(500)),
        //            EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
        //        };

        //        Storyboard storyboard = new Storyboard();
        //        storyboard.Children.Add(scaleAnimation);

        //        Storyboard.SetTarget(scaleAnimation, WaterLevelScale);
        //        Storyboard.SetTargetProperty(scaleAnimation, "(UIElement.RenderTransform).(ScaleTransform.ScaleY)");

        //        storyboard.Begin();
        //    }
        //}
    }
}
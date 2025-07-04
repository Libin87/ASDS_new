using System;
using ASDS_dev.Pages.Reports.Controls;
using ASDS_dev.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Graphics;

namespace ASDS_dev
{
    public partial class App : Application
    {
        private Window m_window;
        public static HeaderViewModel GlobalHeaderViewModel { get; } = new HeaderViewModel();
        public App()
        {
            this.InitializeComponent();
        }

        public static Frame RootFrame { get; private set; }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {

            AuditLogger.InitializeDatabase();
            m_window = new MainWindow();

            RootFrame = new Frame();
            m_window.Content = RootFrame;
            RootFrame.Navigate(typeof(ASDS_dev.Pages.Reports.Reports), null,
                new SuppressNavigationTransitionInfo());

            // 💡 Fix window size to match 10-inch IPC (example: 1280x800)
            IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(m_window);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

            // Set size to 1280x800
            appWindow.Resize(new SizeInt32 { Width = 1280, Height = 800 });

            // Disable resizing
            if (appWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsResizable = false;
                presenter.IsMaximizable = false;
                presenter.IsMinimizable = true;
            }

            m_window.Activate();
        }
    }
}

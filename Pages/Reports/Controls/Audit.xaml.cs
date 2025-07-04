using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace ASDS_dev.Pages.Reports.Controls;
public class AuditRecord
{
    public int RecordId { get; set; }
    public string Timestamp { get; set; }
    public string UserName { get; set; }
    public string Operation { get; set; }
    public string Status { get; set; }
    public string Information { get; set; }
}
public sealed partial class Audit : UserControl
{
    public Audit()
    {
        InitializeComponent();
        AuditGridView.ItemsSource = new List<AuditRecord>
{
    new AuditRecord { RecordId = 1001, Timestamp = "2025-07-04 10:00", UserName = "john", Operation = "LOGIN", Status = "S_OK", Information = "Logged in successfully" },
    new AuditRecord { RecordId = 1002, Timestamp = "2025-07-04 11:00", UserName = "admin", Operation = "LOGOUT", Status = "S_OK", Information = "Session closed" },
};

    }
    public ObservableCollection<AuditRecord> AuditRecords { get; set; } = new();
    private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        string query = args.QueryText;
       
    }

    private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var suggestions = new List<string>
        {
            "Audit Logs",
            "User Management",
            "Settings"
        };

            sender.ItemsSource = suggestions.Where(item => item.Contains(sender.Text, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }



    private void ReportPage(object sender, RoutedEventArgs e)
    {
        App.RootFrame.Navigate(typeof(ASDS_dev.Pages.Reports.Reports), null,
            new SuppressNavigationTransitionInfo());
    }
    
}

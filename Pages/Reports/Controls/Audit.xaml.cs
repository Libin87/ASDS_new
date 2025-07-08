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
    private List<AuditRecord> AllAuditData = new(); // Full data set
    private ObservableCollection<AuditRecord> PagedData = new(); // Data to bind to UI
    private int currentPage = 1;
    private int itemsPerPage = 6;

    public Audit()
    {
        this.InitializeComponent();

        // Sample Data (replace with DB fetch later)
        AllAuditData = new List<AuditRecord>
            {
                new AuditRecord { RecordId = 1, Timestamp = "2025-07-01", UserName = "user1", Operation = "LOGIN", Status = "S_OK", Information = "Logged in" },
                new AuditRecord { RecordId = 2, Timestamp = "2025-07-02", UserName = "user2", Operation = "LOGOUT", Status = "S_OK", Information = "Logged out" },
                new AuditRecord { RecordId = 1, Timestamp = "2025-07-01", UserName = "user1", Operation = "LOGIN", Status = "S_OK", Information = "Logged in" },
                new AuditRecord { RecordId = 2, Timestamp = "2025-07-02", UserName = "user2", Operation = "LOGOUT", Status = "S_OK", Information = "Logged out" },
                new AuditRecord { RecordId = 1, Timestamp = "2025-07-01", UserName = "user1", Operation = "LOGIN", Status = "S_OK", Information = "Logged in" },
                new AuditRecord { RecordId = 2, Timestamp = "2025-07-02", UserName = "user2", Operation = "LOGOUT", Status = "S_OK", Information = "Logged out" },
                new AuditRecord { RecordId = 1, Timestamp = "2025-07-01", UserName = "user1", Operation = "LOGIN", Status = "S_OK", Information = "Logged in" },
                new AuditRecord { RecordId = 2, Timestamp = "2025-07-02", UserName = "user2", Operation = "LOGOUT", Status = "S_OK", Information = "Logged out" },

            };

        LoadPage(currentPage);
        AuditGridView.ItemsSource = PagedData;
    }

    private void LoadPage(int page)
    {
        PagedData.Clear();

        int skip = (page - 1) * itemsPerPage;
        var pageItems = AllAuditData.Skip(skip).Take(itemsPerPage).ToList();

        foreach (var item in pageItems)
            PagedData.Add(item);

        int totalPages = (int)Math.Ceiling((double)AllAuditData.Count / itemsPerPage);

        PageInfoText.Text = $"Page {page} of {totalPages}";
        PrevPageButton.IsEnabled = page > 1;
        NextPageButton.IsEnabled = page < totalPages;
    }

    private void PrevPageButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (currentPage > 1)
        {
            currentPage--;
            LoadPage(currentPage);
        }
    }

    private void NextPageButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        int totalPages = (int)Math.Ceiling((double)AllAuditData.Count / itemsPerPage);
        if (currentPage < totalPages)
        {
            currentPage++;
            LoadPage(currentPage);
        }
    }




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

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CourseWorkLabunDOTNETCORE
{
    /// <summary>
    /// Interaction logic for HistoryViewer.xaml
    /// </summary>
    public partial class HistoryViewer : Window
    {
        public HistoryViewer()
        {
            InitializeComponent();
        }
        public HistoryViewer(MainPage instance, string senderWalletNumber)
        {
            InitializeComponent();
            InitStateDefault(instance, senderWalletNumber);
        }
        void layoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void viewHistory_Click(object sender, RoutedEventArgs e)
        {
            search.IsEnabled = true;
            InitHistory(HistoryViewerContent.historyDefaultInitializer.Item1,
                        HistoryViewerContent.historyDefaultInitializer.Item2);
            listOfHistories.ItemsSource = HistoryViewerContent.history;
            listOfHistories.Items.Refresh();
            RefreshDropdown();
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            if (searchCriteria.SelectedItem == null)
            {
                if (sumSearch.Text == "")
                {
                    sumSearch.Text = "0";
                }
                InitHistory(HistoryViewerContent.historyDefaultInitializer.Item1,
                            HistoryViewerContent.historyDefaultInitializer.Item2);
                HistoryViewerContent.history = TransactionStringsSumFilter(HistoryViewerContent.history, sumSearch.Text) as List<string>;
                HistoryViewerContent.historyAll = TransactionStringsSumFilter(HistoryViewerContent.historyAll, sumSearch.Text) as List<string>;
                if (HistoryViewerContent.history == null || HistoryViewerContent.historyAll == null)
                {
                    HistoryViewerContent.history = new List<string>();
                    HistoryViewerContent.history.Add("Nothing found");
                    HistoryViewerContent.historyAll = new List<string>();
                    HistoryViewerContent.historyAll.Add("Nothing found");
                }
            }
            else
            {
                HistoryViewerContent.filteredAll =
                    HistoryViewerContent.historyAll =
                        BLL_DOTNETCORE
                            .UserProceduresProvider
                            .GetHistoryStrings("All", searchCriteria.SelectedItem.ToString()) as List<string>;
                HistoryViewerContent.filtered =
                    HistoryViewerContent.history =
                        BLL_DOTNETCORE
                            .UserProceduresProvider
                            .GetHistoryStrings(HistoryViewerContent.SenderWalletNumber,
                                               searchCriteria.SelectedItem.ToString()) as List<string>;
                if (sumSearch.Text != "")
                {
                    HistoryViewerContent.history = TransactionStringsSumFilter(HistoryViewerContent.history, sumSearch.Text) as List<string>;
                    HistoryViewerContent.historyAll = TransactionStringsSumFilter(HistoryViewerContent.historyAll, sumSearch.Text) as List<string>;
                    if (HistoryViewerContent.history == null || HistoryViewerContent.historyAll == null)
                    {
                        HistoryViewerContent.history = new List<string>();
                        HistoryViewerContent.history.Add("Nothing found");
                        HistoryViewerContent.historyAll = new List<string>();
                        HistoryViewerContent.historyAll.Add("Nothing found");
                    }
                }
            }
            onlyYourWallet.IsChecked = true;
            listOfHistories.ItemsSource = null;
            listOfHistories.ItemsSource = HistoryViewerContent.history;
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            HistoryViewerContent.Instance.Close();
            new Authorization().Show();
            this.Close();
        }

        private void backToUserWindow_Click(object sender, RoutedEventArgs e)
        {
            HistoryViewerContent.Instance.Show();
            this.Close();
        }

        private void exitCloseButton_Click(object sender, RoutedEventArgs e)
        {
            HistoryViewerContent.Instance.Close();
            this.Close();
        }
        private List<string> TransactionStringsSumFilter(List<string> transactionsStrings, string sum)
        {
            List<string> results = new List<string>();
            foreach(var line in transactionsStrings)
            {
                if (line.Contains("Sum - " + Convert.ToDecimal(sum.Trim())))
                    results.Add(line);
            }
            return results;
        }
        private void UpdateCategories()
        {
            var regex = new Regex("Category - [A-Za-z0-9]*");
            foreach(var line in HistoryViewerContent.history)
            {
                string temp = "Error";
                int matchCounter = 0;
                foreach(var category in Categories.GetCategories())
                {
                    temp = regex.Match(line).ToString().Replace("Category - ", "").Trim();
                    if(category.Name == temp)
                    {
                        matchCounter++;
                    }
                }
                if(matchCounter == 0)
                {
                    Categories.AddCategory(temp);
                }
            }
            foreach (var line in HistoryViewerContent.historyAll)
            {
                string temp = "Error";
                int matchCounter = 0;
                foreach (var category in Categories.GetCategories())
                {
                    temp = regex.Match(line).ToString().Replace("Category - ", "").Trim();
                    if (category.Name == temp)
                    {
                        matchCounter++;
                    }
                }
                if (matchCounter == 0)
                {
                    Categories.AddCategory(temp);
                }
            }
            RefreshDropdown();
        }
        private void InitStateDefault(MainPage instance, string senderWalletNumber)
        {
            search.IsEnabled = false;
            HistoryViewerContent.historyDefaultInitializer.Item1 = instance;
            HistoryViewerContent.historyDefaultInitializer.Item2 = senderWalletNumber;
            onlyYourWallet.IsChecked = true;
            this.MouseLeftButtonDown += new MouseButtonEventHandler(layoutRoot_MouseLeftButtonDown);
            Blurer.SetIsEnabled(this, true);
            InitHistory(HistoryViewerContent.historyDefaultInitializer.Item1,
                        HistoryViewerContent.historyDefaultInitializer.Item2);
            UpdateCategories();
            RefreshDropdown();
        }
        private void InitHistory(MainPage instance, string senderWalletNumber)
        {
            HistoryViewerContent.Instance = instance;
            HistoryViewerContent.SenderWalletNumber = senderWalletNumber;
            HistoryViewerContent.history = BLL_DOTNETCORE
                                            .UserProceduresProvider
                                            .GetHistoryStrings(HistoryViewerContent.SenderWalletNumber) as List<string>;
            HistoryViewerContent.historyAll = BLL_DOTNETCORE
                                                .UserProceduresProvider
                                                .GetHistoryStrings() as List<string>;
            if (HistoryViewerContent.history == null || HistoryViewerContent.historyAll == null)
            {
                HistoryViewerContent.history.Add("Nothing found");
                HistoryViewerContent.historyAll.Add("Nothing found");
            }
        }
        private void RefreshDropdown()
        {
            searchCriteria.ItemsSource = null;
            searchCriteria.ItemsSource = HistoryViewerContent.CategoriesAsSearchOption;
            searchCriteria.Items.Refresh();
        }

        private void onlyYourWallet_Unchecked(object sender, RoutedEventArgs e)
        {
            RefreshListView();
        }

        private void onlyYourWallet_Checked(object sender, RoutedEventArgs e)
        {
            RefreshListView();
        }
        private void RefreshListView()
        {
            if(onlyYourWallet.IsChecked == true)
            {
                listOfHistories.ItemsSource = null;
                listOfHistories.ItemsSource = HistoryViewerContent.history;
                listOfHistories.Items.Refresh();
            }
            else
            {
                listOfHistories.ItemsSource = null;
                listOfHistories.ItemsSource = HistoryViewerContent.historyAll;
                listOfHistories.Items.Refresh();
            }
        }

        private void sumSearch_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Char.IsDigit(e.Text, 0) || 
                        ((e.Text == System
                                        .Globalization
                                        .CultureInfo
                                        .CurrentCulture
                                        .NumberFormat
                                        .NumberDecimalSeparator[0].ToString()) && (BLL_DOTNETCORE
                                                                                        .DSCounter
                                                                                        .DS_Count(((TextBox)sender).Text) < 1));
        }

        private void filterByDate_Click(object sender, RoutedEventArgs e)
        {
            if (dateFrom.SelectedDate != null && dateTo.SelectedDate != null)
            {
                var tempList = new List<string>();
                var tempListAll = new List<string>();
                var regex = new Regex("Transaction performed at [0-9]*.[0-9]*.[0-9]*");
                foreach (var line in HistoryViewerContent.history)
                {
                    DateTime tempDate = new DateTime();
                    if (DateTime.TryParse(regex.Match(line).Value, out tempDate))
                    {
                        if (dateFrom.SelectedDate < tempDate.Date && dateTo.SelectedDate > tempDate.Date)
                            tempList.Add(line);
                    }
                }
                foreach (var line in HistoryViewerContent.historyAll)
                {
                    DateTime tempDate = new DateTime();
                    if (DateTime.TryParse(regex.Match(line).Value, out tempDate))
                    {
                        if (dateFrom.SelectedDate < tempDate.Date && dateTo.SelectedDate > tempDate.Date)
                            tempListAll.Add(line);
                    }
                }
                HistoryViewerContent.history = tempList;
                HistoryViewerContent.historyAll = tempListAll;
            }
            if (HistoryViewerContent.history == null || HistoryViewerContent.historyAll == null)
            {
                HistoryViewerContent.history = new List<string>();
                HistoryViewerContent.history.Add("Nothing found");
                HistoryViewerContent.historyAll = new List<string>();
                HistoryViewerContent.historyAll.Add("Nothing found");
            }
            RefreshListView();
        }
    }
}

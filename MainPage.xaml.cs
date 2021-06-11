using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using static BLL_DOTNETCORE.RegexMatcher;
using BLL_DOTNETCORE;

namespace CourseWorkLabunDOTNETCORE
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        public MainPage()
        {
            InitializeComponent();
        }
        public MainPage(string fName, string lName, string email, string pNumber, string tin, string passpId)
        {
            InitializeComponent();
            InitStateDefault(fName, lName, email, pNumber, tin, passpId);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(layoutRoot_MouseLeftButtonDown);
            Blurer.SetIsEnabled(this, true);
        }
        void layoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void InitStateDefault(string fName, string lName, string email, string pNumber, string tin, string passpId)
        {
            errorMessage.IsReadOnly = true;
            Categories.InitCategories();
            MainPageContent.FirstName = fName;
            MainPageContent.LastName = lName;
            MainPageContent.PhoneNumber = pNumber;
            MainPageContent.Email = email;
            MainPageContent.PassportID = passpId;
            MainPageContent.TIN = tin;
            ownerBlock.Text = $"{MainPageContent.FirstName} {MainPageContent.LastName} - {MainPageContent.Email}";
        }

        private void removeWallet_Click(object sender, RoutedEventArgs e)
        {
            if(listOfEntities.SelectedValue == null)
            {
                errorMessage.Text = "You have to peek a wallet to delete!";
                return;
            }
            if(Convert.ToDecimal(listOfEntities.SelectedItem.ToString().GetWalletBalance()) != 0)
            {
                errorMessage.Text = "Can't delete active wallet!";
                return;
            }
            if (listOfEntities.SelectedItem == null || listOfEntities.SelectedItems == null)
                return;
            if(listOfEntities.SelectedItems.Count == 1)
            {
                var nameNumber = listOfEntities.SelectedItem.ToString().GetWalletNameAndNumber();
                BLL_DOTNETCORE
                    .UserProceduresProviderNONStatic
                    .instance
                    .RemoveWallet(nameNumber.Item1, nameNumber.Item2);
            }
            else
            {
                var items = listOfEntities.SelectedItems;
                List<(string, string)> nameNumbers = new List<(string, string)>();
                foreach (var item in items)
                {
                    nameNumbers.Add(item.ToString().GetWalletNameAndNumber());
                }
                BLL_DOTNETCORE
                    .UserProceduresProviderNONStatic
                    .instance
                    .RemoveWalletRange(nameNumbers, MainPageContent.PassportID, MainPageContent.TIN);
            }
            var strings = BLL_DOTNETCORE
                            .UserProceduresProvider
                            .GetWalletsStrings(MainPageContent.PassportID, MainPageContent.TIN);
            listOfEntities.ItemsSource = strings;
            listOfEntities.Items.Refresh();
        }

        private void deleteAccount_Click(object sender, RoutedEventArgs e)
        {
            if(BLL_DOTNETCORE
                .UserProceduresProvider
                .IsActiveWallets(MainPageContent.PassportID, MainPageContent.TIN))
            {
                errorMessage.Text = "You have active wallets!";
            }
            else
            {
                BLL_DOTNETCORE
                    .UserProceduresProviderNONStatic
                    .instance
                    .RemoveUser(MainPageContent.PassportID, MainPageContent.TIN);
                new Authorization().Show();
                this.Close();
            }
        }

        private void createWallet_Click(object sender, RoutedEventArgs e)
        {
            BLL_DOTNETCORE
                .UserProceduresProviderNONStatic
                .instance
                .CreateWallet("", MainPageContent.PassportID, MainPageContent.TIN);
            var strings = BLL_DOTNETCORE
                            .UserProceduresProvider
                            .GetWalletsStrings(MainPageContent.PassportID, MainPageContent.TIN);
            listOfEntities.ItemsSource = strings;
            listOfEntities.Items.Refresh();
        }
        private void viewUsers_Click(object sender, RoutedEventArgs e)
        {
            var strings = BLL_DOTNETCORE
                .UserProceduresProvider
                .GetUsersStrings();
            listOfEntities.ItemsSource = strings;
            listOfEntities.Items.Refresh();
        }
        private void listWallets_Click(object sender, RoutedEventArgs e)
        {
            if(listOfEntities.Items.Count > 0)
                removeWallet.IsEnabled = true;
            var strings = BLL_DOTNETCORE
                            .UserProceduresProvider
                            .GetWalletsStrings(MainPageContent.PassportID, MainPageContent.TIN);
            listOfEntities.ItemsSource = strings;
            listOfEntities.Items.Refresh();
        }
        private void logout_Click(object sender, RoutedEventArgs e)
        {
            new Authorization().Show();
            this.Close();
        }

        private void exitCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void changeUserData_Click(object sender, RoutedEventArgs e)
        {
            var window = new ChangeUserData();
            window.Show();
            this.Close();
        }

        private void changeWalletName_Click(object sender, RoutedEventArgs e)
        {
            if(listOfEntities.SelectedValue == null)
            {
                errorMessage.Text = "You have to peek a wallet to rename!";
                return;
            }
            var nameNumber = listOfEntities.SelectedItem.ToString().GetWalletNameAndNumber();
            BLL_DOTNETCORE
                .UserProceduresProvider
                .RenameWallet(nameNumber.Item1, nameNumber.Item2, newWalletName.Text);
            var strings = BLL_DOTNETCORE
                            .UserProceduresProvider
                            .GetWalletsStrings(MainPageContent.PassportID, MainPageContent.TIN);
            listOfEntities.ItemsSource = strings;
            listOfEntities.Items.Refresh();
        }

        private void transferToOther_Click(object sender, RoutedEventArgs e)
        {
            if (listOfEntities.SelectedItem == null)
            {
                errorMessage.Text = "Select your wallet first!";
                return;
            }
            new TransactionWindow(listOfEntities.SelectedItem.ToString().GetWalletNameAndNumber().Item2, this).Show();
            this.Hide();
        }

        private void transferBetween_Click(object sender, RoutedEventArgs e)
        {

        }

        private void categories_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            if(listOfEntities.SelectedValue != null)
            {
                new CategoryManager(MainPageContent.PassportID, MainPageContent.TIN, this, 
                                    listOfEntities.SelectedItem.ToString().GetWalletNameAndNumber().Item2).Show();
            }
            else
            {
                new CategoryManager(MainPageContent.PassportID, MainPageContent.TIN, this).Show();
            }
        }

        private void history_Click(object sender, RoutedEventArgs e)
        {
            if(listOfEntities.SelectedItem == null)
            {
                errorMessage.Text = "You have to choose wallet to view it's history!";
                return;
            }
            new HistoryViewer(this, listOfEntities.SelectedItem.ToString()
                                                               .GetWalletNameAndNumber().Item2).Show();
            this.Hide();
        }
    }
}

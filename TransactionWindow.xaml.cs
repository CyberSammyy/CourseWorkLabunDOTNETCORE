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

namespace CourseWorkLabunDOTNETCORE
{
    /// <summary>
    /// Interaction logic for TransactionWindow.xaml
    /// </summary>
    public partial class TransactionWindow : Window
    {
        private string SenderWalletNumber { get; set; }
        private MainPage Instance { get; set; }
        public TransactionWindow()
        {
            InitializeComponent();
        }
        public TransactionWindow(string senderWalletNumber, MainPage instance)
        {
            InitializeComponent();
            Categories.InitCategories();
            categories.ItemsSource = Categories.GetCategories();
            categories.Items.Refresh();
            SenderWalletNumber = senderWalletNumber;
            Instance = instance;
            this.MouseLeftButtonDown += new MouseButtonEventHandler(layoutRoot_MouseLeftButtonDown);
        }
        void layoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private async void performTransaction_Click(object sender, RoutedEventArgs e)
        {
            if(categories.SelectedItem == null)
            {
                categories.Visibility = Visibility.Hidden;
                categories.Visibility = Visibility.Visible;
                categories.Visibility = Visibility.Hidden;
                categories.Visibility = Visibility.Visible;
                categories.Visibility = Visibility.Hidden;
                categories.Visibility = Visibility.Visible;
                return;
            }
            if(cvvInput.Text.Length != 3)
                return;
            if(Convert.ToDecimal(amountOfMoneyToTransact.Text) > 0)
            {
                if(targetWalletNumberInput.Text.Length == 0)
                {
                    await BLL_DOTNETCORE
                            .UserProceduresProviderNONStatic
                            .instance
                            .AddRemoveMoney(Convert.ToDecimal(amountOfMoneyToTransact.Text), 
                                                              SenderWalletNumber);
                    if(categories.SelectedItem == null)
                    {
                        return;
                    }
                    await BLL_DOTNETCORE
                            .UserProceduresProviderNONStatic
                            .instance
                            .AddTransaction(SenderWalletNumber, SenderWalletNumber, 
                                            categories.SelectedItem.ToString(), Convert.ToDecimal(amountOfMoneyToTransact.Text));
                }
                else
                {
                    var check = await BLL_DOTNETCORE
                        .UserProceduresProviderNONStatic
                        .instance
                        .PerformTransaction(Convert.ToDecimal(amountOfMoneyToTransact.Text),
                                                              SenderWalletNumber, cvvInput.Text,
                                                              dateMonth.Text, dateYear.Text,
                                                              targetWalletNumberInput.Text);
                    if(check.Item1 == true)
                    {
                        await BLL_DOTNETCORE
                            .UserProceduresProviderNONStatic
                            .instance
                            .AddTransaction(SenderWalletNumber, targetWalletNumberInput.Text,
                                            categories.SelectedItem.ToString(), Convert.ToDecimal(amountOfMoneyToTransact.Text));
                    }
                    else
                    {
                        errorMsg.Text = check.Item2;
                        return;
                    }
                }
            }
            Instance.Show();
            this.Close();
        }

        private void transactionCancel_Click(object sender, RoutedEventArgs e)
        {
            Instance.Show();
            this.Close();
        }

        private void cvvInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Char.IsDigit(e.Text, 0);
        }

        private void amountOfMoneyToTransact_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
    }
}

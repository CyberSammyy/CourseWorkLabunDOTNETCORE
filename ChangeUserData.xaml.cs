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
using BLL_DOTNETCORE;

namespace CourseWorkLabunDOTNETCORE
{
    /// <summary>
    /// Interaction logic for ChangeUserData.xaml
    /// </summary>
    public partial class ChangeUserData : Window
    {
        public ChangeUserData()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += new MouseButtonEventHandler(layoutRoot_MouseLeftButtonDown);
            Blurer.SetIsEnabled(this, true);
        }
        void layoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private async void saveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            string fName = firstNameInputCh.Text;
            string lName = lastNameInputCh.Text;
            string country = countryInputCh.Text;
            string city = cityInputCh.Text;
            string phone = phoneInputCh.Text;
            string id = idInputCh.Text;
            string tin = tinInputCh.Text;
            string email = emailInputCh.Text;
            string passI = passInputCh.Password;
            string passC = passConfirmCh.Password;
            var result = await BLL_DOTNETCORE
                                    .UserProceduresProviderNONStatic
                                    .instance
                                    .ChangeUserData(lName, fName, 
                                                    city, country, 
                                                    phone, tin, 
                                                    id, email, 
                                                    MainPageContent.PassportID, 
                                                    MainPageContent.TIN, 
                                                    passI, passC);
            MainPageContent.FirstName = fName;
            MainPageContent.LastName = lName;
            MainPageContent.Email = email;
            MainPageContent.PassportID = id;
            MainPageContent.PhoneNumber = phone;
            MainPageContent.TIN = tin;
            new Authorization().Show();
            this.Close();
        }
        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            new Authorization().Show();
            this.Close();
        }

        private void phoneInputCh_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void tinInputCh_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void idInputCh_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }
    }
}

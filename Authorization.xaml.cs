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
    /// Interaction logic for Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void regButtonAuth_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void authButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationByClick(firstNameAuth.Text, lastNameAuth.Text, phoneAuth.Text, passAuth.Password, emailAuth.Text);
        }
        public void AuthorizationByClick(string firstNameInput, 
                                         string lastNameInput,
                                         string phoneNumberInput,
                                         string passwordInput,
                                         string email)
        {
            var check = UserProceduresProviderNONStatic.instance.LoginUser(ref firstNameInput, ref lastNameInput, 
                                                                          phoneNumberInput, passwordInput,
                                                                          email);
            if(check.Item1)
            {
                var check2 = UserProceduresProviderNONStatic.instance.CheckUser(firstNameInput, lastNameInput, phoneNumberInput, passwordInput.GetMD5Hash(), email);
                if(check2.Item1)
                {
                    var tinPasspId = BLL_DOTNETCORE.UserProceduresProvider.GetTINAndPassportID(email);
                    var window = new MainPage(firstNameInput, lastNameInput, email, phoneNumberInput, tinPasspId.Item1, tinPasspId.Item2);
                    window.Show();
                    this.Close();
                }
            }
        }

        private void restorePassword_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).IsEnabled = false;
            (sender as Button).Content = "I'm sorry :(";
        }

        private void phoneAuth_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLL_DOTNETCORE;

namespace CourseWorkLabunDOTNETCORE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void logButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new Authorization();
            window.Show();
            this.Close();
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            Registration();
        }
        private async void Registration()
        {
            string fName = firstNameInput.Text;
            string lName = lastNameInput.Text;
            string country = countryInput.Text;
            string city = cityInput.Text;
            string phone = phoneInput.Text;
            string id = idInput.Text;
            string tin = tinInput.Text;
            string email = emailInput.Text;
            string passI = passInput.Password;
            string passC = passConfirm.Password;
            string hash = "";
            var check = await UserProceduresProviderNONStatic
                                    .instance
                                    .RegisterUser(fName, lName, country, city, 
                                                  phone, id, tin, email, 
                                                  passI, passC, hash);
            if(check.Item1 == false)
            {
                errorText.Text = check.Item2;
                return;
            }
            new Authorization().Show();
            this.Close();
        }

        private void phoneInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void tinInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void idInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }
    }
}

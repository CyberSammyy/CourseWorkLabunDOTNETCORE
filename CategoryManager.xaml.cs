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
using static BLL_DOTNETCORE.RegexMatcher;

namespace CourseWorkLabunDOTNETCORE
{
    /// <summary>
    /// Interaction logic for TransactionManager.xaml
    /// </summary>
    public partial class CategoryManager : Window
    {
        private MainPage Instance { get; set; }
        private string UserWalletNumber { get; set; }
        public CategoryManager()
        {
            InitializeComponent();
        }
        public CategoryManager(string userPassId, string userTin, MainPage instance, string userWalletNumber = null )
        {
            InitializeComponent();
            //Categories.InitCategories();
            if(userWalletNumber == null)
            {
                removeCategory.IsEnabled = false;
                changeCategoryData.IsEnabled = false;
            }
            else
            {
                UserWalletNumber = userWalletNumber;
            }
            Instance = instance;
            listOfCategories.ItemsSource = Categories.GetCategories();
            listOfCategories.Items.Refresh();
            this.MouseLeftButtonDown += new MouseButtonEventHandler(layoutRoot_MouseLeftButtonDown);
            Blurer.SetIsEnabled(this, true);
        }
        void layoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void createCategory_Click(object sender, RoutedEventArgs e)
        {
            if (categoryName.Text.Length < 2)
                return;
            Categories.AddCategory(categoryName.Text);
            categoryName.Text = "";
            Refresh();
        }

        private void changeCategoryData_Click(object sender, RoutedEventArgs e)
        {
            if (categoryName.Text.Length < 2)
                return;
            if(UserWalletNumber != null)
                BLL_DOTNETCORE
                    .UserProceduresProvider
                    .ChangeCategories((listOfCategories.SelectedItem as Category).Name, 
                                       categoryName.Text, UserWalletNumber);
            (listOfCategories.SelectedItem as Category).SetName(categoryName.Text);
            categoryName.Text = "";
            Refresh();
        }

        private void removeCategory_Click(object sender, RoutedEventArgs e)
        {
            BLL_DOTNETCORE
                .UserProceduresProvider
                .ChangeCategories((listOfCategories.SelectedItem as Category).Name,
                                  "DefaultCategory", UserWalletNumber);
            Categories.RemoveCategory(listOfCategories.SelectedItem.ToString());
            Refresh();
        }

        private void backToUserWindow_Click(object sender, RoutedEventArgs e)
        {
            Instance.Show();
            this.Close();
        }

        private void exitCloseButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            Instance.Close();
            this.Close();
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            new Authorization().Show();
            Instance.Close();
            this.Close();
        }
        private void Refresh()
        {
            listOfCategories.ItemsSource = null;
            listOfCategories.ItemsSource = Categories.GetCategories();
            listOfCategories.Items.Refresh();
        }
        private void eraseDuplicates_Click(object sender, RoutedEventArgs e)
        {
            Categories.EraseDuplicates();
            Refresh();
        }
    }
}
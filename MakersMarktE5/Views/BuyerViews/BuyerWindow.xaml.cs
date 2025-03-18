using System.Linq;
using Microsoft.UI.Xaml.Controls;
using MakersMarktE5.Data;
using Microsoft.EntityFrameworkCore;
using MakersMarktE5.Services;
using Microsoft.UI.Xaml;
using MakersMarktE5.Views.ModeratorViews;
using MakersMarktE5.Views.CreatorViews;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MakersMarktE5.Views.BuyerViews
{
    public sealed partial class BuyerWindow : Window
    {
        public BuyerWindow()
        {
            this.InitializeComponent();

            this.Title = "Buyers Dashboard";
            Fullscreen fullscreenService = new Fullscreen();
            fullscreenService.SetFullscreen(this);

            MainFrame.Navigate(typeof(ProductPage));

            LoadCategories();
        }

        private void LoadCategories()
        {
            using (var db = new AppDbContext())
            {
                var categories = db.Categories.ToList();
                categories.Insert(0, new Category { Id = 0, Name = "All Categories" }); 
                CategoryComboBox.ItemsSource = categories;
                CategoryComboBox.SelectedIndex = 0; 
            }
        }

        private void Search_Button_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchBar.Text.Trim();

            // Assuming 'MainFrame' is displaying 'ProductPage'
            if (MainFrame.Content is ProductPage productPage)
            {
                productPage.Productfilter(searchTerm);
            }
        }
        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(ProductPage));
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var creatorWindow = new LoginWindow();
            creatorWindow.Activate();
            this.Close();
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainFrame?.Content is ProductPage productPage)
            {
                var selectedCategory = (CategoryComboBox.SelectedItem as Category)?.Name;
                productPage.Categoryfilter(selectedCategory);
            }
        }
        
        private void settingButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(SettingPage));
        }

    }
}

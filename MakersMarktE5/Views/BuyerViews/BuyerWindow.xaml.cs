using System.Linq;
using Microsoft.UI.Xaml.Controls;
using MakersMarktE5.Data;
using Microsoft.EntityFrameworkCore;
using MakersMarktE5.Services;
using Microsoft.UI.Xaml;

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
                categories.Insert(0, new Category { Id = 0, Name = "All Categories" }); // Add "All Categories" option
                CategoryComboBox.ItemsSource = categories;
                CategoryComboBox.DisplayMemberPath = "Name";
                CategoryComboBox.SelectedIndex = 0; // Select "All Categories" by default
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

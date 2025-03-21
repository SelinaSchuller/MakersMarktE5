using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MakersMarktE5.Services;
using MakersMarktE5.Views.ModeratorViews;
using MakersMarktE5.Data;
using MakersMarktE5.Views.BuyerViews;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MakersMarktE5.Views.CreatorViews
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreatorWindow : Window
    {
        public CreatorWindow()
        {
            this.InitializeComponent();

            this.Title = "Creator Dashboard";
            Fullscreen fullscreenService = new Fullscreen();
            fullscreenService.SetFullscreen(this);

			MainFrame.Navigate(typeof(ProductPage));
			LoadCategories();
		}
		private void LoadCategories()
		{
			using(var db = new AppDbContext())
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
			if(MainFrame.Content is ProductPage productPage)
			{
				productPage.Productfilter(searchTerm);
			}
			else if(MainFrame.Content is MyCollectionPage myCollectionPage)
			{
				myCollectionPage.Productfilter(searchTerm);
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
			if(MainFrame?.Content is ProductPage productPage)
			{
				var selectedCategory = (CategoryComboBox.SelectedItem as Category)?.Name;
				productPage.Categoryfilter(selectedCategory);
			}
			else if(MainFrame?.Content is MyCollectionPage myCollectionPage)
			{
				var selectedCategory = (CategoryComboBox.SelectedItem as Category)?.Name;
				myCollectionPage.Categoryfilter(selectedCategory);
			}
		}

		private void settingButton_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(typeof(SettingPage));
		}

		private void MyCollectionPageButton_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(typeof(MyCollectionPage));
		}
	}
}

using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using Microsoft.EntityFrameworkCore;
using MakersMarktE5.Data;
using Microsoft.UI.Xaml;
using System;
using MakersMarktE5.Views.Dialogs;

namespace MakersMarktE5.Views.CreatorViews
{
	public sealed partial class MyCollectionPage : Page
	{
		private int _userId { get; set; }
		public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

		public MyCollectionPage()
		{
			this.InitializeComponent();
			_userId = Data.User.LoggedInUser.Id;
			LoadProducts();
		}

		private void LoadProducts()
		{
			using(var db = new AppDbContext())
			{
				var productList = db.Products
									.Include(p => p.Categories)
									.Include(p => p.Materials)
									.Where(p => p.CreatorId == _userId)
									.ToList();

				Products.Clear();
				foreach(var product in productList)
				{
					Products.Add(product);
				}
			}

			ProductListView.ItemsSource = Products;
		}

		public void Productfilter(string searchTerm = "")
		{
			using(var db = new AppDbContext())
			{
				var products = db.Products
								 .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
								 .Where(p => p.CreatorId == _userId)
								 .ToList();
				ProductListView.ItemsSource = products;
			}
		}

		public void Categoryfilter(string selectedCategory)
		{
			using(var db = new AppDbContext())
			{
				var products = db.Products
								 .Include(p => p.ProductCategories)
								 .ThenInclude(pc => pc.Category)
								 .Where(p => selectedCategory == "All Categories" || p.ProductCategories.Any(pc => pc.Category.Name == selectedCategory))
								 .Where(p => p.CreatorId == _userId)
								 .ToList();

				ProductListView.ItemsSource = products;
			}
		}

		private async void EditProduct_Click(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			var product = button?.Tag as Product;
			if(product == null)
				return;

			using(var db = new AppDbContext())
			{
				product = db.Products
							.Include(p => p.ProductCategories).ThenInclude(pc => pc.Category)
							.Include(p => p.MaterialProducts).ThenInclude(mp => mp.Material)
							.Include(p => p.Type)
							.Include(p => p.UniqueProperty)
							.FirstOrDefault(p => p.Id == product.Id);
			}

			// Open EditProductDialog
			EditProductDialog editDialog = new EditProductDialog(product);
			editDialog.XamlRoot = this.XamlRoot;

			await editDialog.ShowAsync();

			LoadProducts();
		}

		private async void DeleteProduct_Click(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			if(button == null)
				return;
			var product = button.Tag as Product;
			if(product == null)
				return;

			ContentDialog deleteDialog = new ContentDialog
			{
				Title = "Delete Product",
				Content = $"Are you sure you want to delete {product.Name}?",
				PrimaryButtonText = "Yes",
				CloseButtonText = "Cancel",
				DefaultButton = ContentDialogButton.Primary,
				XamlRoot = button.XamlRoot
			};

			var result = await deleteDialog.ShowAsync();
			if(result == ContentDialogResult.Primary)
			{
				using(var db = new AppDbContext())
				{
					var dbProduct = db.Products.FirstOrDefault(p => p.Id == product.Id);
					if(dbProduct != null)
					{
						db.Products.Remove(dbProduct);
						db.SaveChanges();
					}
				}

				LoadProducts(); // Refresh list
			}
		}
	}
}

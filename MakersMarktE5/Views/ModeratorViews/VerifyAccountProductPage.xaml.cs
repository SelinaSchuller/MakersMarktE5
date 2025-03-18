using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using Microsoft.EntityFrameworkCore;
using MakersMarktE5.Data;
using System.Threading.Tasks;
using System;

namespace MakersMarktE5.Views.ModeratorViews
{
	public sealed partial class VerifyAccountProductPage : Page
	{
		public ObservableCollection<User> UnverifiedUsers { get; set; } = new ObservableCollection<User>();
		public ObservableCollection<Product> UnverifiedProducts { get; set; } = new ObservableCollection<Product>();

		public VerifyAccountProductPage()
		{
			this.InitializeComponent();
			LoadData();
		}

		private void LoadData()
		{
			using(var db = new AppDbContext())
			{
				var userList = db.Users.Include(u => u.Role).Where(u => !u.IsVerified).ToList();
				UnverifiedUsers.Clear();
				foreach(var user in userList)
				{
					UnverifiedUsers.Add(user);
				}

				var productList = db.Products
									.Include(p => p.Type)
									.Include(p => p.Materials)
									.Include(p => p.Categories)
									.Include(p => p.UniqueProperty)
									.Include(p => p.Creator)
									.Where(p => !p.IsVerified)
									.ToList();
				UnverifiedProducts.Clear();
				foreach(var product in productList)
				{
					UnverifiedProducts.Add(product);
				}
			}

			UnverifiedUserListView.ItemsSource = UnverifiedUsers;
			ProductListView.ItemsSource = UnverifiedProducts;
		}

		private async void VerifyUser_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
		{
			var button = sender as Button;
			if(button == null)
				return;
			var user = button.Tag as User;
			if(user == null)
				return;

			ContentDialog verifyDialog = new ContentDialog
			{
				Title = "Verify User",
				Content = $"Are you sure you want to verify {user.Name}?",
				PrimaryButtonText = "Yes",
				CloseButtonText = "Cancel",
				XamlRoot = this.XamlRoot
			};

			var result = await verifyDialog.ShowAsync();
			if(result == ContentDialogResult.Primary)
			{
				using(var db = new AppDbContext())
				{
					var dbUser = db.Users.FirstOrDefault(u => u.Id == user.Id);
					if(dbUser != null)
					{
						dbUser.IsVerified = true;
						db.SaveChanges();
					}
				}

				LoadData();
			}
		}

		private async void VerifyProduct_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
		{
			var button = sender as Button;
			if(button == null)
				return;
			var product = button.Tag as Product;
			if(product == null)
				return;

			ContentDialog verifyDialog = new ContentDialog
			{
				Title = "Verify Product",
				Content = $"Are you sure you want to verify {product.Name}?",
				PrimaryButtonText = "Yes",
				CloseButtonText = "Cancel",
				XamlRoot = this.XamlRoot
			};

			var result = await verifyDialog.ShowAsync();
			if(result == ContentDialogResult.Primary)
			{
				using(var db = new AppDbContext())
				{
					var dbProduct = db.Products.FirstOrDefault(p => p.Id == product.Id);
					if(dbProduct != null)
					{
						dbProduct.IsVerified = true;
						db.SaveChanges();
					}
				}

				LoadData();
			}
		}
	}
}

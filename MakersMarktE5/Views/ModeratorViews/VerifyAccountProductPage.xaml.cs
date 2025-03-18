using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using Microsoft.EntityFrameworkCore;
using MakersMarktE5.Data;

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
				// Fetch Unverified Users
				var userList = db.Users.Include(u => u.Role).Where(u => !u.IsVerified).ToList();
				UnverifiedUsers.Clear();
				foreach(var user in userList)
				{
					UnverifiedUsers.Add(user);
				}

				// Fetch Unverified Products with Includes
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

			// Bind Data to UI
			UnverifiedUserListView.ItemsSource = UnverifiedUsers;
			ProductListView.ItemsSource = UnverifiedProducts;
		}
	}
}

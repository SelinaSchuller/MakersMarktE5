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
using MakersMarktE5.Data;
using Microsoft.EntityFrameworkCore;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MakersMarktE5.Views.CreatorViews
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MyCollectionPage : Page
	{
		private int _userId { get; set; }
		public MyCollectionPage()
		{
			this.InitializeComponent();


			using(var db = new AppDbContext())
			{
				_userId = Data.User.LoggedInUser.Id;

				var products = db.Products
					.Include(p => p.Categories)
					.Include(p => p.Materials)
					.Where(p => p.CreatorId == _userId)
					.ToList();
				ProductListView.ItemsSource = products;
			}
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
	}
}

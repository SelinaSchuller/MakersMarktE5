using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using MakersMarktE5.Data;
using Microsoft.EntityFrameworkCore;

namespace MakersMarktE5.Views.BuyerViews
{
	public sealed partial class ProductPage : Page
	{
		public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

		public ProductPage()
		{
			this.InitializeComponent();
			LoadProducts();
		}

		private void LoadProducts()
		{
			using(var db = new AppDbContext())
			{
				var productList = db.Products
					.Include(p => p.Categories)
					.Include(p => p.Materials)
					.ToList();

				Products.Clear();
				foreach(var product in productList)
				{
					Products.Add(product);
				}
			}

			ProductListView.ItemsSource = Products;
		}
	}
}

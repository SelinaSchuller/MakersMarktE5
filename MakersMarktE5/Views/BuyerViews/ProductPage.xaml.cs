using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using MakersMarktE5.Data;

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
			using(var db = new AppDbContext())  // Connect to the database
			{
				var productList = db.Products.ToList(); // Retrieve products

				// Update the ObservableCollection
				Products.Clear();
				foreach(var product in productList)
				{
					Products.Add(product);
				}
			}

			// Bind the list to the UI
			ProductListView.ItemsSource = Products;
		}
	}
}

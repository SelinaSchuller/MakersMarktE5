using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using Microsoft.EntityFrameworkCore;
using MakersMarktE5.Data;

namespace MakersMarktE5.Views.ModeratorViews
{
	public sealed partial class ProductEditPage : Page
	{
		public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
		public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
		private Product? SelectedProduct;

		public ProductEditPage()
		{
			this.InitializeComponent();
			LoadData();
		}

		private void LoadData()
		{
			using(var db = new AppDbContext())
			{
				var productList = db.Products
					.Include(p => p.ProductCategories)
					.ThenInclude(pc => pc.Category)
					.Include(p => p.MaterialProducts)
					.ThenInclude(mp => mp.Material)
					.ToList();

				Products.Clear();
				foreach(var product in productList)
				{
					Products.Add(product);
				}

				var categoryList = db.Categories
					.ToList();
				Categories.Clear();
				foreach(var category in categoryList)
				{
					Categories.Add(category);
				}
			}

			ProductListView.ItemsSource = Products;
			CategoryListView.ItemsSource = Categories;
		}

		private void ProductListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SelectedProduct = ProductListView.SelectedItem as Product;

			if(SelectedProduct != null)
			{
				foreach(var item in CategoryListView.Items.Cast<Category>())
				{
					var listViewItem = (ListViewItem)CategoryListView.ContainerFromItem(item);
					if(listViewItem != null)
					{
						listViewItem.IsSelected = SelectedProduct.ProductCategories.Any(pc => pc.CategoryId == item.Id);
					}
				}
			}
		}

		private void AssignCategories_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
		{
			if(SelectedProduct == null)
				return;

			using(var db = new AppDbContext())
			{
				var product = db.Products
					.Include(p => p.ProductCategories)
					.FirstOrDefault(p => p.Id == SelectedProduct.Id);

				if(product != null)
				{
					product.ProductCategories.Clear();

					foreach(var selectedCategory in CategoryListView.SelectedItems.Cast<Category>())
					{
						product.ProductCategories.Add(new ProductCategory
						{
							ProductId = product.Id,
							CategoryId = selectedCategory.Id
						});
					}

					db.SaveChanges();
				}
			}

			LoadData();
		}

		public void Productfilter(string searchTerm = "")
		{
			using(var db = new AppDbContext())
			{
				var products = db.Products
								 .Where(p => p.Name.Contains(searchTerm))
								 .ToList();
				ProductListView.ItemsSource = products;
			}
		}
	}
}

using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using MakersMarktE5.Data;
using Microsoft.EntityFrameworkCore;

namespace MakersMarktE5.Views.BuyerViews
{
	public sealed partial class ProductPage : Page
	{
		public ProductPage()
		{
			this.InitializeComponent();

            using (var db = new AppDbContext())
            {
                var products = db.Products
                    .Include(p => p.Categories)
                    .Include(p => p.Materials)
					.ToList();
                ProductListView.ItemsSource = products;
            }
        }

        public void Productfilter(string searchTerm = "")
        {
            using (var db = new AppDbContext())
            {
                var products = db.Products
                                 .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                                 .Include(p => p.Categories)
                                 .Include(p => p.Materials)
                                 .ToList();
                ProductListView.ItemsSource = products;
            }
        }

        public void Categoryfilter(string selectedCategory)
        {
            using (var db = new AppDbContext())
            {
                var products = db.Products
                                 .Include(p => p.ProductCategories)
                                 .ThenInclude(pc => pc.Category)
                                 .Include(p => p.Materials)
                                 .Where(p => selectedCategory == "All Categories" || p.ProductCategories.Any(pc => pc.Category.Name == selectedCategory))
                                 .ToList();

                ProductListView.ItemsSource = products;
            }
        }

    }
}

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
		


            //Products = new ObservableCollection<Product>
            //{
            //	new Product { Name = "Handmade Necklace", Description = "A beautiful necklace made of silver and gemstones." },
            //	new Product { Name = "Colorful Vase Art", Description = "Hand-painted ceramic vase with unique patterns." },
            //	new Product { Name = "Handwoven Scarf", Description = "A warm scarf, lovingly woven from 100% wool." },
            //	new Product { Name = "Handmade Necklace", Description = "A beautiful necklace made of silver and gemstones." },
            //	new Product { Name = "Colorful Vase Art", Description = "Hand-painted ceramic vase with unique patterns." },
            //	new Product { Name = "Handwoven Scarf", Description = "A warm scarf, lovingly woven from 100% wool." },
            //	new Product { Name = "Handmade Necklace", Description = "A beautiful necklace made of silver and gemstones." },
            //	new Product { Name = "Colorful Vase Art", Description = "Hand-painted ceramic vase with unique patterns." },
            //	new Product { Name = "Handwoven Scarf", Description = "A warm scarf, lovingly woven from 100% wool." }
            //};

            using (var db = new AppDbContext())
            {
                var products = db.Products.ToList();
                ProductListView.ItemsSource = products;

            }

            //ProductListView.ItemsSource = Products;
        }

        public void Productfilter(string searchTerm = "")
        {
            using (var db = new AppDbContext())
            {
                var products = db.Products
                                 .Where(p => p.Name.Contains(searchTerm))
                                 .ToList();
                ProductListView.ItemsSource = products;
            }
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

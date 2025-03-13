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
                                 .Where(p => p.Name.Contains(searchTerm))
                                 .ToList();
                ProductListView.ItemsSource = products;
            }
        }
    }
}

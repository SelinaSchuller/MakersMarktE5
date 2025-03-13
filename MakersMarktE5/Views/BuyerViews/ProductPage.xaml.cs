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
using System.Collections.ObjectModel;
using MakersMarktE5.Data;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MakersMarktE5.Views.BuyerViews
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class ProductPage : Page
	{
		public ObservableCollection<Product> Products { get; set; }

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
                                 .Where(p => p.Name.Contains(searchTerm)) // Adjust property based on your model
                                 .ToList();
                ProductListView.ItemsSource = products;
            }
        }

    }
}


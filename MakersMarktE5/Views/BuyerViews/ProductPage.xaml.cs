using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Controls;

namespace MakersMarktE5.Views.BuyerViews
{
	public sealed partial class ProductPage : Page
	{
		public ObservableCollection<Product> Products { get; set; }

		public ProductPage()
		{
			this.InitializeComponent();

			Products = new ObservableCollection<Product>
			{
				new Product { Name = "Handmade Necklace", Description = "A beautiful necklace made of silver and gemstones." },
				new Product { Name = "Colorful Vase Art", Description = "Hand-painted ceramic vase with unique patterns." },
				new Product { Name = "Handwoven Scarf", Description = "A warm scarf, lovingly woven from 100% wool." },
				new Product { Name = "Handmade Necklace", Description = "A beautiful necklace made of silver and gemstones." },
				new Product { Name = "Colorful Vase Art", Description = "Hand-painted ceramic vase with unique patterns." },
				new Product { Name = "Handwoven Scarf", Description = "A warm scarf, lovingly woven from 100% wool." },
				new Product { Name = "Handmade Necklace", Description = "A beautiful necklace made of silver and gemstones." },
				new Product { Name = "Colorful Vase Art", Description = "Hand-painted ceramic vase with unique patterns." },
				new Product { Name = "Handwoven Scarf", Description = "A warm scarf, lovingly woven from 100% wool." }
			};


			ProductListView.ItemsSource = Products;
		}
	}

	public class Product
	{
		public string Name { get; set; }
		public string Description { get; set; }
	}
}

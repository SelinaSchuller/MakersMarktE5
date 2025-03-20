using Microsoft.UI.Xaml.Controls;
using MakersMarktE5.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MakersMarktE5.Views.Dialogs
{
	public sealed partial class EditProductDialog : ContentDialog
	{
		private readonly AppDbContext _db = new AppDbContext();
		private readonly Product _originalProduct;

		public EditProductDialog(Product product)
		{
			this.InitializeComponent();
			_originalProduct = product; // Store original product reference

			// Populate fields
			NameTextBox.Text = product.Name;
			DescriptionTextBox.Text = product.Description;
			ProductionTimeTextBox.Text = product.ProductionTime;
			ComplexityTextBox.Text = product.Complexity;
			SustainabilityTextBox.Text = product.Sustainability;

			// Populate Type & Unique Property Dropdowns
			TypeComboBox.ItemsSource = _db.Types.ToList();
			TypeComboBox.DisplayMemberPath = "Name";
			TypeComboBox.SelectedItem = _db.Types.FirstOrDefault(t => t.Id == product.TypeId);

			UniquePropertyComboBox.ItemsSource = _db.UniqueProperties.ToList();
			UniquePropertyComboBox.DisplayMemberPath = "Name";
			UniquePropertyComboBox.SelectedItem = _db.UniqueProperties.FirstOrDefault(up => up.Id == product.PropertyId);

			// Populate Categories List
			var allCategories = _db.Categories.ToList();
			CategoryListView.ItemsSource = allCategories;
			foreach(var category in product.ProductCategories.Select(pc => pc.Category))
			{
				var listItem = allCategories.FirstOrDefault(c => c.Id == category.Id);
				if(listItem != null)
				{
					CategoryListView.SelectedItems.Add(listItem);
				}
			}

			// Populate Materials List
			var allMaterials = _db.Materials.ToList();
			MaterialListView.ItemsSource = allMaterials;
			foreach(var material in product.MaterialProducts.Select(mp => mp.Material))
			{
				var listItem = allMaterials.FirstOrDefault(m => m.Id == material.Id);
				if(listItem != null)
				{
					MaterialListView.SelectedItems.Add(listItem);
				}
			}
		}

		private void SaveButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
		{
			var dbProduct = _db.Products
				.Include(p => p.ProductCategories)
				.Include(p => p.MaterialProducts)
				.FirstOrDefault(p => p.Id == _originalProduct.Id);

			if(dbProduct == null)
				return;

			// Update product properties
			dbProduct.Name = NameTextBox.Text.Trim();
			dbProduct.Description = DescriptionTextBox.Text.Trim();
			dbProduct.ProductionTime = ProductionTimeTextBox.Text.Trim();
			dbProduct.Complexity = ComplexityTextBox.Text.Trim();
			dbProduct.Sustainability = SustainabilityTextBox.Text.Trim();

			if(TypeComboBox.SelectedItem != null)
				dbProduct.TypeId = ((Type)TypeComboBox.SelectedItem).Id;

			if(UniquePropertyComboBox.SelectedItem != null)
				dbProduct.PropertyId = ((UniqueProperty)UniquePropertyComboBox.SelectedItem).Id;

			// Update Categories
			dbProduct.ProductCategories.Clear();
			foreach(var selectedCategory in CategoryListView.SelectedItems.Cast<Category>())
			{
				dbProduct.ProductCategories.Add(new ProductCategory
				{
					ProductId = dbProduct.Id,
					CategoryId = selectedCategory.Id
				});
			}

			// Update Materials
			dbProduct.MaterialProducts.Clear();
			foreach(var selectedMaterial in MaterialListView.SelectedItems.Cast<Material>())
			{
				dbProduct.MaterialProducts.Add(new MaterialProduct
				{
					ProductId = dbProduct.Id,
					MaterialId = selectedMaterial.Id
				});
			}

			dbProduct.IsVerified = false;

			_db.SaveChanges();
			this.Hide();
		}

		private void CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
		{
			this.Hide();
		}
	}
}

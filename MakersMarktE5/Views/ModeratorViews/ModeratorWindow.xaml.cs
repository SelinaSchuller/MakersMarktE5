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
using MakersMarktE5.Services;
using MakersMarktE5.Views.BuyerViews;
using MakersMarktE5.Views.CreatorViews;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MakersMarktE5.Views.ModeratorViews
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ModeratorWindow : Window
    {
        public ModeratorWindow()
        {
            this.InitializeComponent();

			this.Title = "Moderator Dashboard";
			Fullscreen fullscreenService = new Fullscreen();
			fullscreenService.SetFullscreen(this);

			MainFrame.Navigate(typeof(ProductEditPage));
		}

		private void Search_Button_Click(object sender, RoutedEventArgs e)
		{
			string searchTerm = SearchBar.Text.Trim();

			if(MainFrame.Content is ProductEditPage productEditPage)
			{
				productEditPage.Productfilter(searchTerm);
			}
		}

		private void ProductEditPageButton_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(typeof(ProductEditPage));
		}

		private void VerifyAccountProductPageButton_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(typeof(VerifyAccountProductPage));
		}
	
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
			var closeWindow = new LoginWindow();
			closeWindow.Activate();
			this.Close();
        }
    }
}

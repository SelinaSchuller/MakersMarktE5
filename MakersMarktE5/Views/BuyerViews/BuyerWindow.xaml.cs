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
using MakersMarktE5.Data;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MakersMarktE5.Views.BuyerViews
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BuyerWindow : Window
    {
        public BuyerWindow()
        {
            this.InitializeComponent();

			this.Title = "Buyers Dashboard";
			Fullscreen fullscreenService = new Fullscreen();
			fullscreenService.SetFullscreen(this);

			MainFrame.Navigate(typeof(ProductPage));

		}

        private void Search_Button_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchBar.Text.Trim();

            // Assuming 'MainFrame' is displaying 'ProductPage'
            if (MainFrame.Content is ProductPage productPage)
            {
                productPage.Productfilter(searchTerm);
            }
        }
        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(ProductPage));
        }

        private void settingButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(SettingPage));
        }

    }
}

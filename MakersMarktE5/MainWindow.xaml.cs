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
using MakersMarktE5.Data;
using MakersMarktE5.Views.BuyerViews;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MakersMarktE5
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Navigate(typeof(template2));
        }

		private void ProductPageButton_Click(object sender, RoutedEventArgs e)
		{
			var baseWindow = new BuyerWindow();

			baseWindow.Activate();
			DispatcherQueue.TryEnqueue(() =>
			{
				this.Close();
			});
		}
	}
}

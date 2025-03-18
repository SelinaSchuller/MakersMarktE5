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
using MakersMarktE5.Views.ModeratorViews;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MakersMarktE5.Views.CreatorViews
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreatorWindow : Window
    {
        public CreatorWindow()
        {
            this.InitializeComponent();

            this.Title = "Creator Dashboard";
            Fullscreen fullscreenService = new Fullscreen();
            fullscreenService.SetFullscreen(this);
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var creatorWindow = new LoginWindow();
            creatorWindow.Activate();
            this.Close();
        }
    }
}

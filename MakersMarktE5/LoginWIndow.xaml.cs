using MakersMarktE5.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MakersMarktE5
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginWIndow : Window
    {
        private int _userId { get; set; }

        public LoginWIndow()
        {
            this.InitializeComponent();

            using (var db = new AppDbContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new AppDbContext())
            {
                string username = usernameTextBox.Text;
                string password = passwordTextBox.Password;
                string hashedPassword = HashPassword(password);

                var user = db.Users.FirstOrDefault(u => u.Name == username && u.Password == hashedPassword);

                if (user != null)
                {
                    int userId = user.Id;
                    Data.User.LoggedInUser = user;

                    //verander dit naar de juiste locatie
                    var mainWindow = new MainWindow();
                    mainWindow.Activate();
                    this.Close();
                }
                else
                {
                    ErrorTextBlock.Text = "E-mail of wachtwoord is onjuist";
                }
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

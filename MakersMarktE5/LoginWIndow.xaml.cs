using MakersMarktE5.Data;
using MakersMarktE5.Services;
using MakersMarktE5.Views.BuyerViews;
using Microsoft.EntityFrameworkCore;
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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MakersMarktE5
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginWindow : Window
    {
        private int _userId { get; set; }

        public LoginWindow()
        {
            this.InitializeComponent();

			this.Title = "Login Window";
			Fullscreen fullscreenService = new Fullscreen();
			fullscreenService.SetFullscreen(this);

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
                    if(user.RoleId == 2)
                    {
                        //Role Buyer:
						var buyerWindow = new BuyerWindow();
						buyerWindow.Activate();
						this.Close();
					}
                    else
                    {
						var mainWindow = new MainWindow();
						mainWindow.Activate();
						this.Close();
					}
                }
                else
                {
                    ErrorTextBlock.Text = "E-mail or password is incorrect.";
                }
            }
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                ContentDialog registerDialog = new ContentDialog
                {
                    Title = "Register",
                    PrimaryButtonText = "Register",
                    CloseButtonText = "Cancel",
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = button.XamlRoot
                };

                StackPanel panel = new StackPanel();
                TextBox usernameTextBox = new TextBox { Header = "Username:", Margin = new Thickness(0, 5, 0, 10) };
                PasswordBox passwordBox = new PasswordBox { Header = "Password:", Margin = new Thickness(0, 10, 0, 10) };
                PasswordBox passwordCheckBox = new PasswordBox { Header = "Password again:", Margin = new Thickness(0, 10, 0, 10) };
                ComboBox roleComboBox = new ComboBox { Header = "Role:", Margin = new Thickness(0, 10, 0, 10) };

                using (var db = new AppDbContext())
                {
                    var roles = db.Roles.Where(role => role.Id == 1 || role.Id == 2).ToList();
                    roleComboBox.ItemsSource = roles.Select(role => role.Name).ToList();
                    roleComboBox.SelectedIndex = -1;
                }

                panel.Children.Add(usernameTextBox);
                panel.Children.Add(passwordBox);
                panel.Children.Add(passwordCheckBox);
                panel.Children.Add(roleComboBox);

                registerDialog.Content = panel;


                var result = await registerDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    string username = usernameTextBox.Text.Trim();
                    string password = passwordBox.Password.Trim();
                    string passwordCheck = passwordCheckBox.Password.Trim();

                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    {
                        await ShowMessage("Username and password cannot be empty.", button);
                        return;
                    }

                    if (roleComboBox.SelectedIndex < 0)
                    {
                        await ShowMessage("Role can not be empty.", button);
                        return;
                    }

                    if (password != passwordCheck)
                    {
                        await ShowMessage("Passwords are not the same.", button);
                        return;
                    }

                    using (var db = new AppDbContext())
                    {
                        if (db.Users.Any(u => u.Name == username))
                        {
                            await ShowMessage("This username is already taken.", button);
                            return;
                        }

                        string hashedPassword = HashPassword(password);
                        var newUser = new User { Name = username, Password = hashedPassword, RoleId = roleComboBox.SelectedIndex + 1};
                        db.Users.Add(newUser);
                        db.SaveChanges();

                        await ShowMessage("Registration successful!", button);
                    }
                }
            }
        }


        private async Task ShowMessage(string message, FrameworkElement element)
        {
            ContentDialog messageDialog = new ContentDialog
            {
                Title = "Notification",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = element.XamlRoot
            };

            await messageDialog.ShowAsync();
        }


    }
}

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
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MakersMarktE5.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            this.InitializeComponent();
            if (User.LoggedInUser != null)
            {
                UsernameTextBlock.Text = User.LoggedInUser.Name;
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

        private async void UsernameChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                ContentDialog changeUsernameDialog = new ContentDialog
                {
                    Title = "Change Username",
                    PrimaryButtonText = "Change",
                    CloseButtonText = "Cancel",
                    XamlRoot = this.XamlRoot
                };

                StackPanel panel = new StackPanel();

                TextBox usernameBox = new TextBox { PlaceholderText = "Enter new username", Margin = new Thickness(0, 0, 10, 0) };
                PasswordBox passwordBox = new PasswordBox { PlaceholderText = "Enter your password", Margin = new Thickness(0, 10, 0, 0) };

                panel.Children.Add(usernameBox);
                panel.Children.Add(passwordBox);

                changeUsernameDialog.Content = panel;

                var result = await changeUsernameDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    string newUsername = usernameBox.Text.Trim();
                    string password = HashPassword(passwordBox.Password);

                    if (string.IsNullOrEmpty(newUsername) || string.IsNullOrEmpty(password))
                    {
                        await ShowMessage("Error", "Username and password cannot be empty.");
                        return;
                    }

                    if (User.LoggedInUser.Password != password)
                    {
                        await ShowMessage("Error", "Incorrect password.");
                        return;
                    }

                    using var db = new AppDbContext();

                    bool usernameExists = db.Users.Any(u => u.Name == newUsername);
                    if (usernameExists)
                    {
                        await ShowMessage("Error", "Username is already taken.");
                        return;
                    }

                    User.LoggedInUser.Name = newUsername;
                    db.Users.Update(User.LoggedInUser);
                    await db.SaveChangesAsync();

                    UsernameTextBlock.Text = newUsername;

                    await ShowMessage("Success", "Username changed successfully!");
                }
            }
        }

        private async void PasswordChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                ContentDialog changePasswordDialog = new ContentDialog
                {
                    Title = "Change Password",
                    PrimaryButtonText = "Change",
                    CloseButtonText = "Cancel",
                    XamlRoot = this.XamlRoot
                };

                StackPanel panel = new StackPanel();

                PasswordBox oldPasswordBox = new PasswordBox { PlaceholderText = "Enter old password", Margin = new Thickness(0, 0, 10, 0) };
                PasswordBox newPasswordBox = new PasswordBox { PlaceholderText = "Enter new password", Margin = new Thickness(0, 10, 10, 0) };
                PasswordBox confirmPasswordBox = new PasswordBox { PlaceholderText = "Confirm new password", Margin = new Thickness(0, 10, 0, 0) };

                panel.Children.Add(oldPasswordBox);
                panel.Children.Add(newPasswordBox);
                panel.Children.Add(confirmPasswordBox);

                changePasswordDialog.Content = panel;

                var result = await changePasswordDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    string oldPassword = HashPassword(oldPasswordBox.Password);
                    string newPassword = HashPassword(newPasswordBox.Password);
                    string confirmPassword = HashPassword(confirmPasswordBox.Password);

                    if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
                    {
                        await ShowMessage("Error", "All fields must be filled.");
                        return;
                    }

                    if (User.LoggedInUser.Password != oldPassword)
                    {
                        await ShowMessage("Error", "Incorrect old password.");
                        return;
                    }

                    if (oldPassword == newPassword)
                    {
                        await ShowMessage("Error", "New password cannot be the same as the old password.");
                        return;
                    }

                    if (newPassword != confirmPassword)
                    {
                        await ShowMessage("Error", "New passwords do not match.");
                        return;
                    }

                    // Update password
                    using var db = new AppDbContext();
                    User.LoggedInUser.Password = newPassword;
                    db.Users.Update(User.LoggedInUser);
                    await db.SaveChangesAsync();

                    await ShowMessage("Success", "Password changed successfully!");
                }
            }

        }

        private async Task ShowMessage(string title, string message)
        {
            ContentDialog messageDialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await messageDialog.ShowAsync();
        }
    }
}

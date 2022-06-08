using PasswordManagerApp.Controllers;
using PasswordManagerApp.Models.Api;
using PasswordManagerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace PasswordManagerApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthPage : ContentPage
    {
        private UsersController usersCtrl = new UsersController();
        public AuthPage()
        {
            InitializeComponent();
            Manager.currentUserEmail = null;
            Manager.currentUserPassword = null;
            
            if (Preferences.Get("remember_me", false) && !string.IsNullOrWhiteSpace(Preferences.Get("email", null)) && !string.IsNullOrWhiteSpace(Preferences.Get("password", null)))
            {
                Auth(Preferences.Get("email", null), Preferences.Get("password", null));
            }
        }

        private void AuthButton_Clicked(object sender, EventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(LoginEntry.Text) && !string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                Auth(LoginEntry.Text, PasswordEntry.Text);
            }
            else
            {
                MessageLabel.IsVisible = true;
                MessageLabel.Text = "Поля Еmail и пароль обязательны к заполнению";
            }
        }
        private async void Auth(string loginString, string passwordString)
        {
            string hashed_password = string.Join(" ", SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(passwordString)));
            ApiFrame<AuthResponse> response = usersCtrl.AuthUser(loginString, hashed_password);

            string toastMessage = response.Data.Message;
            if (response.Status == "success")
            {
                Manager.currentUserEmail = loginString;
                Manager.currentUserPassword = passwordString;
                try
                {
                    Manager.currentUserSettings = usersCtrl.GetUserSettings(loginString, hashed_password);
                }
                catch
                {
                    Manager.currentUserSettings = new List<Setting>();
                }

                await Navigation.PushAsync(new AccountsPage());
            }
            Console.WriteLine(response.Status + " " + response.Data.Message);
            MessageLabel.IsVisible = true;
            MessageLabel.Text = toastMessage;
        }

        private async void GoToSignUpButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage(), false);
        }
    }
}
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
        }

        private async void AuthButton_Clicked(object sender, EventArgs e)
        {
            string toastMessage = null;
            ApiFrame<AuthResponse> response = usersCtrl.AuthUser(LoginEntry.Text, string.Join(" ", SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(PasswordEntry.Text))));
            if (response.Status == "success")
            {
                Manager.currentUserEmail = LoginEntry.Text;
                Manager.currentUserPassword = PasswordEntry.Text;
                await Navigation.PushAsync(new AccountsPage());
                toastMessage = response.Data.Message;
            }
            else
            {
                toastMessage = response.Data.Message;
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
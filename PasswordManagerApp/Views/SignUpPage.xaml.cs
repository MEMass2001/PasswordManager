using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PasswordManagerApp.Controllers;
using PasswordManagerApp.Models.Api;
using PasswordManagerApp.Models;
using System.Security.Cryptography;
using Android.Widget;

namespace PasswordManagerApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        private UsersController usersCtrl = new UsersController();
        public SignUpPage()
        {
            InitializeComponent();
            Manager.currentUserEmail = null;
            Manager.currentUserPassword = null;
        }

        private async void SignUpButton_Clicked(object sender, EventArgs e)
        {
            string toastMessage = null;
            if (PasswordEntry.Text == PasswordRepeatEntry.Text)
            {
                ApiFrame<AuthResponse> response = usersCtrl.SignUpUser(LoginEntry.Text, string.Join(" ", SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(PasswordEntry.Text))));
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
                Console.WriteLine(response.Status+" "+response.Data.Message);
            }
            else
            {
                toastMessage = "Введённые пароли не совпадают";
            }
            MessageLabel.IsVisible = true;
            MessageLabel.Text = toastMessage;
        }

        private async void GoToAuthButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AuthPage(), false);
        }
    }
}
using Android.App;
using PasswordManagerApp.Controllers;
using PasswordManagerApp.Models;
using PasswordManagerApp.Models.Api;
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
    public partial class ConfirmationPage : ContentPage
    {
        private int _accountId;
        public ConfirmationPage(Account currentAccount)
        {
            InitializeComponent();
            _accountId = currentAccount.Id;
            this.BindingContext = currentAccount;
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AccountInfoPage(_accountId));
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            AccountsController acntCtrl = new AccountsController();
            UsersController usersCtrl = new UsersController();
            if (!string.IsNullOrWhiteSpace(LoginEntry.Text) && !string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                string hashed_password = string.Join(" ", SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(PasswordEntry.Text)));
                ApiFrame<AuthResponse> response = usersCtrl.AuthUser(LoginEntry.Text, hashed_password);
                if (response.Status == "success" && LoginEntry.Text == Manager.currentUserEmail && PasswordEntry.Text == Manager.currentUserPassword)
                {
                    if (acntCtrl.DeleteAccount(_accountId).Status == "success")
                    {
                        await Navigation.PushAsync(new AccountsPage());
                    }
                    else
                    {
                        ErrorLabel.IsVisible = true;
                        ErrorLabel.Text = "Произошла ошибка при удалении, повторите попытку позже";
                    }
                }
                else
                {
                    ErrorLabel.IsVisible = true;
                    ErrorLabel.Text = "Проверьте правильность написания логина и пароля";
                }
            }
            else
            {
                ErrorLabel.IsVisible = true;
                ErrorLabel.Text = "Поля Еmail и пароль обязательны к заполнению";
            }
        }

        private void ExitButton_Clicked(object sender, EventArgs e)
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}
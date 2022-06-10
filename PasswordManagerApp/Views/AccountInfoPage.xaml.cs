using PasswordManagerApp.Controllers;
using PasswordManagerApp.Models;
using PasswordManagerApp.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PasswordManagerApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountInfoPage : ContentPage
    {
        private int _accountId;
        private AccountsController _acntCtrl = new AccountsController();
        private Account _account;
        public AccountInfoPage(int accountId)
        {
            InitializeComponent();
            _accountId = accountId;
            this.BindingContext = _acntCtrl.GetAccount(_accountId).Data; 

            if (Preferences.Get("require_password", false))
            {
                LoginDataFrame.IsVisible = true;
                AccountDataFrame.IsVisible = false;
            }
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AccountsPage());
        }

        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditAccountPage(this.BindingContext as Account));
        }

        private void OpenButton_Clicked(object sender, EventArgs e)
        {
            UsersController usersCtrl = new UsersController();
            if (!string.IsNullOrWhiteSpace(LoginEntry.Text) && !string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                string hashed_password = string.Join(" ", SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(PasswordEntry.Text)));
                ApiFrame<AuthResponse> response = usersCtrl.AuthUser(LoginEntry.Text, hashed_password);

                string toastMessage = response.Data.Message;
                if (response.Status == "success" && LoginEntry.Text == Manager.currentUserEmail && PasswordEntry.Text == Manager.currentUserPassword)
                {
                    Manager.currentUserEmail = LoginEntry.Text;
                    Manager.currentUserPassword = PasswordEntry.Text;
                    try
                    {
                        Manager.currentUserSettings = usersCtrl.GetUserSettings(LoginEntry.Text, hashed_password);
                    }
                    catch
                    {
                        Manager.currentUserSettings = new List<Setting>();
                    }

                    LoginDataFrame.IsVisible = false;
                    AccountDataFrame.IsVisible = true;
                }
                Console.WriteLine(response.Status + " " + response.Data.Message);
                ErrorLabel.IsVisible = true;
                ErrorLabel.Text = "Проверьте правильность написания логина и пароля";
            }
            else
            {
                ErrorLabel.IsVisible = true;
                ErrorLabel.Text = "Поля Еmail и пароль обязательны к заполнению";
            }
        }
    }
}
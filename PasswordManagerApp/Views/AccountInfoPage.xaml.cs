using PasswordManagerApp.Controllers;
using PasswordManagerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AccountsPage());
        }

        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditAccountPage(this.BindingContext as Account));
        }
    }
}
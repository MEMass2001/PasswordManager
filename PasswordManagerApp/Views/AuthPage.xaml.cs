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
    public partial class AuthPage : ContentPage
    {
        public AuthPage()
        {
            InitializeComponent();
            
        }

        private void AuthButton_Clicked(object sender, EventArgs e)
        {
            // Сделать нормальный вход
            Navigation.PushAsync(new AccountsPage(), false);
        }

        private async void GoToSignUpButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage(), false);
        }
    }
}
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
    public partial class CreateNewAccountPage : ContentPage
    {
        public CreateNewAccountPage()
        {
            InitializeComponent();
            ServiceImage.Source = "@drawable/OtherService.png";
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AccountsPage());
        }

        private void AddAccountButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}
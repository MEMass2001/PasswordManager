using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PasswordManagerApp.Models.Api;
using PasswordManagerApp.Models;
using PasswordManagerApp.Controllers;

namespace PasswordManagerApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateNewAccountPage : ContentPage
    {
        private AccountsController acntCtrl = new AccountsController();
        private List<Service> _services;
        public CreateNewAccountPage()
        {
            InitializeComponent();
            _services = acntCtrl.GetServices().Data;
            List<string> serviceNames = new List<string>();
            foreach (Service service in _services)
            {
                serviceNames.Add(service.Name);
            }
            ServicePicker.Title = "Выбор сервиса";
            ServicePicker.ItemsSource = serviceNames;
            ServicePicker.SelectedIndex = 0;
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AccountsPage(), false);
        }

        private void AddAccountButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(AccountTitleEntry.Text) && !string.IsNullOrWhiteSpace(LoginEntry.Text) && !string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {

            }
            else
            {
                ErrorLabel.IsVisible = true;
            }
        }

        private void ServicePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceImage.Source = $"@drawable/{_services.Where(x => x.Name == ServicePicker.SelectedItem.ToString()).FirstOrDefault().Image}";
        }
    }
}
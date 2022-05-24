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
using PasswordManagerApp.Classes;
using Android.Widget;

namespace PasswordManagerApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateNewAccountPage : ContentPage
    {
        private List<Service> _services;
        private AccountsController _acntCtrl = new AccountsController();
        public CreateNewAccountPage()
        {
            InitializeComponent();
            _services = _acntCtrl.GetServices().Data;
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

        private async void AddAccountButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(AccountTitleEntry.Text) && !string.IsNullOrWhiteSpace(LoginEntry.Text) && !string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                string cipheredLogin = IdeaCipher.cryptString(LoginEntry.Text, Manager.currentUserPassword, true);
                string cipheredPassword = IdeaCipher.cryptString(PasswordEntry.Text, Manager.currentUserPassword, true);
                int selectedService = _services.Where(x => x.Name == ServicePicker.SelectedItem.ToString()).FirstOrDefault().Id;
                ApiFrame<string> response = _acntCtrl.AddAccount(AccountTitleEntry.Text, cipheredLogin, cipheredPassword, selectedService);
                if (response.Status == "success")
                {
                    Toast.MakeText(Android.App.Application.Context, "Создание учётной записи прошло успешно", ToastLength.Short);
                    await Navigation.PushAsync(new AccountsPage());
                }
                else
                {
                    ErrorLabel.IsVisible = true;
                    ErrorLabel.Text = "Произошла ошибка при занесении учётной записи";
                }
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
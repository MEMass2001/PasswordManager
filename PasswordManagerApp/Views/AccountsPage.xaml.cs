using Android.App;
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
    public partial class AccountsPage : ContentPage
    {
        private AccountsController _acntCtrl = new AccountsController();
        public AccountsPage()
        {
            InitializeComponent();

            Console.WriteLine($"{Manager.currentUserSettings} <================ HERE");

            List<Models.Service> services = _acntCtrl.GetServices().Data;
            List<string> serviceNames = new List<string>();
            serviceNames.Add("Все");
            foreach (Models.Service service in services)
            {
                serviceNames.Add(service.Name);
            }
            ServicePicker.ItemsSource = serviceNames;
            ServicePicker.SelectedIndex = 0;
            UpdateUI();
        }

        private async void SettingsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage(), false);
        }

        private async void AddNewAccountButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateNewAccountPage(), false);
        }

        private void UpdateUI()
        {
            List<Account> accounts = _acntCtrl.GetAccounts().Data;
            if (!string.IsNullOrWhiteSpace(SearchEntry.Text))
            {
                accounts = accounts.Where(x => x.Title.Contains(SearchEntry.Text.Trim())).ToList();
            }
            if (ServicePicker.SelectedItem.ToString() != "Все")
            {
                int selectedServiceId = _acntCtrl.GetServices().Data.Where(x => x.Name == ServicePicker.SelectedItem.ToString()).FirstOrDefault().Id;
                accounts = accounts.Where(x => x.ServiceId == selectedServiceId).ToList();
            }
            AccountsListView.ItemsSource = accounts;
        }

        private void ServicePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUI();
        }

        private async void ViewCell_Tapped(object sender, EventArgs e)
        {
            ViewCell tappedObj = sender as ViewCell;
            Account currentAccount = tappedObj.BindingContext as Account;
            int accountId = currentAccount.Id;
            await Navigation.PushAsync(new AccountInfoPage(accountId));
        }

        private void ExitButton_Clicked(object sender, EventArgs e)
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}
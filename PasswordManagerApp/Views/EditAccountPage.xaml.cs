using Android.App;
using Android.Widget;
using PasswordManagerApp.Classes;
using PasswordManagerApp.Controllers;
using PasswordManagerApp.Models;
using PasswordManagerApp.Models.Api;
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
    public partial class EditAccountPage : ContentPage
    {
        private List<Models.Service> _services;
        private AccountsController _acntCtrl = new AccountsController();
        public EditAccountPage(Account edtblAcnt)
        {
            InitializeComponent();
            this.BindingContext = edtblAcnt;
            AccountTitleEntry.Text = edtblAcnt.Title;
            LoginEntry.Text = edtblAcnt.DecipheredLogin;
            PasswordEntry.Text = edtblAcnt.DecipheredPassword;

            _services = _acntCtrl.GetServices().Data;
            List<string> serviceNames = new List<string>();
            foreach (Models.Service service in _services)
            {
                serviceNames.Add(service.Name);
            }
            ServicePicker.Title = "Выбор сервиса";
            ServicePicker.ItemsSource = serviceNames;
            ServicePicker.SelectedItem = _services.Where(x => x.Id == edtblAcnt.ServiceId).FirstOrDefault().Name;
        }

        private void ServicePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceImage.Source = $"@drawable/{_services.Where(x => x.Name == ServicePicker.SelectedItem.ToString()).FirstOrDefault().Image}";
        }

        private async void SaveAccountButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(AccountTitleEntry.Text) && !string.IsNullOrWhiteSpace(LoginEntry.Text) && !string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                string cipheredLogin = IdeaCipher.cryptString(LoginEntry.Text, Manager.currentUserPassword, true);
                string cipheredPassword = IdeaCipher.cryptString(PasswordEntry.Text, Manager.currentUserPassword, true);
                int selectedService = _services.Where(x => x.Name == ServicePicker.SelectedItem.ToString()).FirstOrDefault().Id;
                ApiFrame<string> response = _acntCtrl.EditAccount(AccountTitleEntry.Text, cipheredLogin, cipheredPassword, selectedService, ((Account)this.BindingContext).Id);
                if (response.Status == "success")
                {
                    Toast.MakeText(Android.App.Application.Context, "Редактирование учётной записи прошло успешно", ToastLength.Short);
                    await Navigation.PushAsync(new AccountInfoPage(((Account)this.BindingContext).Id));
                }
                else
                {
                    ErrorLabel.IsVisible = true;
                    ErrorLabel.Text = "Произошла ошибка при редактировании учётной записи";
                }
            }
            else
            {
                ErrorLabel.IsVisible = true;
            }
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AccountsPage(), false);
        }

        private void ExitButton_Clicked(object sender, EventArgs e)
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordManagerApp.Controllers;
using PasswordManagerApp.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PasswordManagerApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private UsersController _usersCtrl;
        public SettingsPage()
        {
            InitializeComponent();
            _usersCtrl = new UsersController();
            SettingsListView.ItemsSource = Manager.currentUserSettings;
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AccountsPage(), false);
        }

        private void QuitButton_Clicked(object sender, EventArgs e)
        {
            Preferences.Clear();
            Manager.currentUserEmail = null;
            Manager.currentUserPassword = null;
            Manager.currentUserSettings = null;

            Navigation.PushAsync(new AuthPage());
        }
        private void SettingSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            _usersCtrl = new UsersController();
            Switch currentSwith = sender as Switch;
            Setting currentSetting = currentSwith.BindingContext as Setting;

            _usersCtrl.SetSetting(new List<Setting> { currentSetting });
            Preferences.Set(currentSetting.SettingCode, currentSetting.Value);
            if (currentSetting.SettingCode == "remember_me" && currentSetting.Value)
            {
                Preferences.Set("email", Manager.currentUserEmail);
                Preferences.Set("password", Manager.currentUserPassword);
            }
        }
    }
}
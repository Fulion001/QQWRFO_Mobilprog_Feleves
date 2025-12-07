using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QQWRFO_Mobilprog_Feleves
{
    public class MainPageViewModel
    {
        public ICommand LoginCommand { get; }
        public MainPageViewModel()
        {
            LoginCommand = new AsyncRelayCommand(PerformLoginAsync);
        }
        private async Task PerformLoginAsync()
        {
            await Shell.Current.GoToAsync("GamePage");
            bool loginSuccessful = await AuthenticateUserAsync();

            if (loginSuccessful)
            {
                await Shell.Current.GoToAsync("GamePage");
            }
            else
            {
                // Hiba kezelése (pl. hibaüzenet megjelenítése)
                await Shell.Current.DisplayAlert("Hiba", "Helytelen felhasználónév vagy jelszó.", "OK");
            }
        }
        private Task<bool> AuthenticateUserAsync()
        {
            
            return Task.FromResult(true);
        }
    }
}

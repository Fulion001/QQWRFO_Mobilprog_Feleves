using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QQWRFO_Mobilprog_Feleves
{
    public partial class MainPageViewModel
    {
        public MainPageViewModel()
        {
            
        }
        [RelayCommand]
        public async Task LoginAsync()
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
        [RelayCommand]
        public async Task RegisterAsync()
        {             
            await Shell.Current.GoToAsync("RegisterPage");
        }

        private Task<bool> AuthenticateUserAsync()
        {
            
            return Task.FromResult(true);
        }
        
    }
}

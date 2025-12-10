using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QQWRFO_Mobilprog_Feleves
{
    public partial class RegisterViewModel
    {
        public RegisterViewModel()
        {

        }
        [RelayCommand]
        public async Task RegisterAsync()
        {
            await Shell.Current.GoToAsync("BackToMainPage");
        }
        [RelayCommand]
        public async Task UnregisterAsync()
        {
            await Shell.Current.GoToAsync("BackToMainPage");
        }
        }

}

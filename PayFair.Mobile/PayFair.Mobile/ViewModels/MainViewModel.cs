using PayFair.Mobile.BLL.ApiClient;
using PayFair.Mobile.BLL.DataAccess;
//using PayFair.Mobile.BLL.DataAccess;
using PayFair.DTO.Users;
using PayFair.Mobile.Models;
using PayFair.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PayFair.Mobile.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string barName;
        private string barBalance;
        
        public string BarName
        {
            get => barName;
            set => SetProperty(ref barName, value);
        }
        public string BarBalance {
            get => barBalance;
            set => SetProperty(ref barBalance, value);
        }

        public Command ProfileCommand { get; }

        public MainViewModel()
        {
            ProfileCommand = new Command(async ()=> await OnProfileClick(), () => !IsBusy);

            DataManager.Init();

            DataManager.GetData(DataManager.DataType.MyUser, OnMyUserDataUpdate);
        }

        public void OnMyUserDataUpdate(object data)
        {
            try
            {
                UserDTO userDTO = (UserDTO)data;
                BarName = userDTO.Name;
            }
            catch (Exception e)
            {

            }
        }

        public async Task OnProfileClick()
        {
            IsBusy = true;

            string[] buttons = new string[] { "Log out" };

            var choice = await Acr.UserDialogs.UserDialogs.Instance.ActionSheetAsync(
                null, "Cancel", null, System.Threading.CancellationToken.None, buttons);

            if (!string.IsNullOrEmpty(choice))
            {
                if (choice == "Log out")
                {
                    await App.LogOut();
                }
            }

            IsBusy = false;
        }
    }
}

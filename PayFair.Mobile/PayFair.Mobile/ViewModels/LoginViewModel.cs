using PayFair.Mobile.BLL;
using PayFair.Mobile.BLL.ApiClient;
using PayFair.Mobile.BLL.Session;
using PayFair.DTO.Users;
using PayFair.Mobile.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PayFair.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private bool isRegisterForm;
        private string submitText;
        private string errorText;

        public LoginModel AuthData { get; set; }

        public bool IsRegisterForm
        {
            get => isRegisterForm;
            set => SetProperty(ref isRegisterForm, value);
        }

        public string SubmitText
        {
            get => submitText;
            set => SetProperty(ref submitText, value);
        }

        public string ErrorText
        {
            get => errorText;
            set => SetProperty(ref errorText, value);
        }

        public Command SubmitCommand { get; }
        public Command LogRegToggleCommand { get; }
        public Command LoginByFacebookCommand { get; }


        public LoginViewModel()
        {
            AuthData = new LoginModel();
            OnLogRegToggleClick();

            SubmitCommand = new Command(async () => await OnSubmitClick(), () => !IsBusy);
            LogRegToggleCommand = new Command(OnLogRegToggleClick, () => !IsBusy);
            LoginByFacebookCommand = new Command(OnLoginByFacebookClick, () => !IsBusy);
        }


        private void OnLogRegToggleClick()
        {
            IsRegisterForm = !IsRegisterForm;

            if (IsRegisterForm)
            {
                SubmitText = "Register";
            }
            else
            {
                SubmitText = "Login";
            }
        }

        
        private async Task OnSubmitClick()
        {
            IsBusy = true;

            if (!AuthData.Validate(IsRegisterForm))
            {
                ErrorText = AuthData.ErrorMessage;

                IsBusy = false;
                return;
            }

            UserAuthDTO authDTO = new UserAuthDTO
            {
                Email = AuthData.Email,//AuthData.Email,
                Password = AuthData.Password//AuthData.Password
            };

            if (IsRegisterForm)
            {
                var regRes = await ApiClient.Register(authDTO);
                if (regRes.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ErrorText = "User arleady exist";

                    IsBusy = false;
                    return;
                }
            }

            bool logged = await SessionManager.Authorize(authDTO.Email, authDTO.Password) == SessionManager.SessionState.Authorized;

            if (logged)
            {
                Device.InvokeOnMainThreadAsync(() => { Application.Current.MainPage = new NavigationPage(new Views.MainView()); });
                OnLogged(authDTO);
            }
            else
            {
                ErrorText = "Login problem";
            }

            IsBusy = false;
        }


        private void OnLoginByFacebookClick()
        {
        }

        private async Task OnLogged(UserAuthDTO authDTO)
        {
            await SecureStorage.SetAsync("usn", authDTO.Email);
            await SecureStorage.SetAsync("psw", authDTO.Password);
        }

        public override void OnIsBusyChanged()
        {
            SubmitCommand.ChangeCanExecute();
            LogRegToggleCommand.ChangeCanExecute();
            LoginByFacebookCommand.ChangeCanExecute();

        }
    }
}

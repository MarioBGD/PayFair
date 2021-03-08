using PayFair.Mobile.BLL.ApiClient;
using PayFair.Mobile.BLL.DataAccess;
using PayFair.Mobile.BLL.Session;
using PayFair.Mobile.Views;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PayFair.Mobile
{
    public partial class App : Application
    {
        private static App _instance;
        public static bool IsLoginPage { get; set; }

        public App()
        {
            _instance = this;
            ApiClient.OnUnauthorized = OnUnauthorized;

            InitializeComponent();
        }

        protected override void OnStart()
        {
            SessionManager.Username = SecureStorage.GetAsync("usn").Result;
            SessionManager.Password = SecureStorage.GetAsync("psw").Result;

            if (string.IsNullOrEmpty(SessionManager.Username) || string.IsNullOrEmpty(SessionManager.Password))
            {
                IsLoginPage = true;
                MainPage = new LoginView();
            }
            else
            {
                IsLoginPage = false;
                MainPage = new NavigationPage(new MainView());
            }
        }


        private void OnUnauthorized()
        {
            if (!IsLoginPage)
            {
                DataManager.MainThreadHandler.Abort();
                Device.InvokeOnMainThreadAsync(() => { MainPage = new LoginView(); });
                SecureStorage.Remove("usn");
                SecureStorage.Remove("psw");
            }
        }

        public static async Task LogOut()
        {
            DataManager.MainThreadHandler.Abort();
            SessionManager.Unauthorize();
            await Device.InvokeOnMainThreadAsync(() => { _instance.MainPage = new LoginView(); });
            SecureStorage.Remove("usn");
            SecureStorage.Remove("psw");
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            
        }
    }
}

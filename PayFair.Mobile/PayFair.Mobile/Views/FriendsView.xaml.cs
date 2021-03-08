using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PayFair.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FriendsView : ContentPage
    {
        public FriendsView()
        {
            InitializeComponent();

            BindingContext = new ViewModels.FriendsViewModel();
        }
    }
}

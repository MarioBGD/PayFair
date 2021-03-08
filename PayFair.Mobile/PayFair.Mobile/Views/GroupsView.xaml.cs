using PayFair.Mobile.ViewModels;
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
    public partial class GroupsView : ContentPage
    {

        public GroupsView()
        {
            InitializeComponent();

            BindingContext = new GroupsViewModel();
        }
    }
}

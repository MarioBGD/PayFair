using PayFair.DTO.Groups;
using PayFair.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PayFair.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupView : ContentPage
    {
        public GroupView(GroupDTO group)
        {
            InitializeComponent();
            BindingContext = new GroupViewModel(group);
        }
    }
}
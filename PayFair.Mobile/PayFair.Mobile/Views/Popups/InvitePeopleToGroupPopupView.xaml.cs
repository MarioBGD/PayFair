using PayFair.DTO.Groups;
using PayFair.Mobile.ViewModels.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PayFair.Mobile.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InvitePeopleToGroupPopupView : Rg.Plugins.Popup.Pages.PopupPage
    {
        public InvitePeopleToGroupPopupView(GroupDTO group)
        {
            InitializeComponent();
            BindingContext = new InvitePeopleToGroupPopupViewModel(group);
        }
    }
}
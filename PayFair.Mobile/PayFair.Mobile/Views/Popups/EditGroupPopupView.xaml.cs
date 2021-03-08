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
    public partial class EditGroupPopupView : Rg.Plugins.Popup.Pages.PopupPage
    {
        public EditGroupPopupViewMdel ViewModel;

        public EditGroupPopupView(GroupDTO groupDTO)
        {
            CloseWhenBackgroundIsClicked = false;
            
            InitializeComponent();
            ViewModel = new EditGroupPopupViewMdel(groupDTO);
            BindingContext = ViewModel;
        }
    }
}
using PayFair.Mobile.BLL.ApiClient;
using PayFair.DTO.Groups;
using PayFair.Mobile.Views.Popups;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PayFair.Mobile.ViewModels
{
    public class GroupViewModel : BaseViewModel
    {
        private GroupDTO groupDTO;

        private string groupName;
        
        public string GroupName
        {
            get => groupName;
            set => SetProperty(ref groupName, value);
        }

        public Command BackCommand { get; }
        public Command SettingsCommand { get; }


        public GroupViewModel(GroupDTO groupDTO)
        {
            this.groupDTO = groupDTO;

            BackCommand = new Command(async () => await OnBackCilcked(), () => !IsBusy);
            SettingsCommand = new Command(async () => await OnSettingsClicked(), () => !IsBusy);

            GroupName = groupDTO.Name;
        }


        private async Task OnSettingsClicked()
        {
            IsBusy = true;
            Acr.UserDialogs.IUserDialogs userDialogs = Acr.UserDialogs.UserDialogs.Instance;

            string[] buttons = new string[] { "Remove group", "Invite friends", "Group settings" };

            var choice = await userDialogs.ActionSheetAsync(
                null, "Cancel", null, System.Threading.CancellationToken.None, buttons);

            if (!string.IsNullOrEmpty(choice))
            {
                if (choice == "Remove group")
                {
                    var res = await ApiClient.DeleteGroup(groupDTO.Id);

                    if (res.StatusCode == System.Net.HttpStatusCode.OK)
                        userDialogs.Toast("Group deleted");
                    else
                        userDialogs.Toast("Cannot delete group");

                    await Application.Current.MainPage.Navigation.PopAsync();
                }

                if (choice == "Invite friends")
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new InvitePeopleToGroupPopupView(groupDTO));
                }
            }

            IsBusy = false;
        }

        private async Task OnBackCilcked()
        {
            IsBusy = true;
            await Application.Current.MainPage.Navigation.PopAsync();
            IsBusy = false;
        }
    }
}

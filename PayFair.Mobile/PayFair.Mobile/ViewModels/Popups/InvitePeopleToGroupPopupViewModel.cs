using PayFair.Mobile.BLL.ApiClient;
using PayFair.Mobile.BLL.DataAccess;
using PayFair.Mobile.BLL.Session;
using PayFair.DTO.Groups;
using PayFair.DTO.Users;
using PayFair.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace PayFair.Mobile.ViewModels.Popups
{
    public class InvitePeopleToGroupPopupViewModel : BaseViewModel
    {
        private GroupDTO Group;
        public ObservableCollection<PersonModel> FriendsList { get; set; }

        public InvitePeopleToGroupPopupViewModel(GroupDTO group)
        {
            Group = group;
            FriendsList = new ObservableCollection<PersonModel>();

            DataManager.GetData(DataManager.DataType.Friendships, OnFriendshipsUpdate);
        }

        private void OnFriendshipsUpdate(object data)
        {
            IEnumerable<FriendshipDTO> friendships = (IEnumerable<FriendshipDTO>)data;

            foreach (var friendship in friendships)
            {
                int notMyId = (friendship.UserOneId == SessionManager.UserId) ? friendship.UserTwoId : friendship.UserOneId;

                PersonModel person = new PersonModel(InviteToGroup, null)
                {
                    UserId = notMyId,
                    Name = "User " + notMyId
                };

                if (friendship.Status == 1)
                { 
                    FriendsList.Add(person);
                }
            }

        }

        private async Task InviteToGroup(PersonModel person)
        {
            person.IsBusy = true;

            var res = await ApiClient.AddPersonToGroup(person.UserId, Group.Id);

            if (res.IsOk)
                Acr.UserDialogs.UserDialogs.Instance.Toast("Invited");
            else
                Acr.UserDialogs.UserDialogs.Instance.Toast("Problem");

            person.IsBusy = false;
        }
    }
}

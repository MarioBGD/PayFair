using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using PayFair.Mobile.BLL.ApiClient;
using PayFair.Mobile.BLL.BO;
using PayFair.Mobile.BLL.DataAccess;
using PayFair.DTO.Users;
using PayFair.Mobile.Models;
using Xamarin.Forms;

namespace PayFair.Mobile.ViewModels
{
    public class FriendsViewModel : BaseViewModel
    {
        public Command AddFriendCommand { get; }

        public ObservableCollection<PersonModel> InvitationsList { get; set; }
        public ObservableCollection<PersonModel> FriendsList { get; set; }
        

        public FriendsViewModel()
        {
            AddFriendCommand = new Command(async () => await OnAddFriendClick(), () => !IsBusy);

            InvitationsList = new ObservableCollection<PersonModel>();
            //InvitationsList.Add(new PersonModel(AddFriend, RemoveFriend)
            //{
            //    Name = "Marek",
            //});

            FriendsList = new ObservableCollection<PersonModel>();
            //FriendsList.Add(new PersonModel(AddFriend, RemoveFriend)
            //{
            //    Name = "Ashley",
            //});

            //FriendsList.Add(new PersonModel
            //{
            //    Name = "Jackie",
            //});

            DataManager.GetData(DataManager.DataType.Friendships, OnFriendshipsUpdate);
        }

        private void OnFriendshipsUpdate(object data)
        {
            IEnumerable<FriendshipDTO> friendships = (IEnumerable<FriendshipDTO>)data;

            foreach (var friendship in friendships)
            {
                PersonModel person = new PersonModel(AddFriend, RemoveFriend)
                {
                    UserId = friendship.UserOneId,
                    Name = "User " + friendship.UserOneId
                };

                if (friendship.Status == 0)
                {
                    if (friendship.UserOneId != PayFair.Mobile.BLL.Session.SessionManager.UserId)
                        InvitationsList.Add(person);
                }
                else
                {
                    FriendsList.Add(person);
                }
            }
            
        }

        private async Task OnAddFriendClick()
        {
            var userDialogs = Acr.UserDialogs.UserDialogs.Instance;
            var input = await userDialogs.PromptAsync("Add new friend", "Enter your friend name", "Add", "Cancel");

            if (input.Ok)
            {
                string toastMessage = "something goes wrong";
                var result = await ApiClient.AddFriendByUsername(input.Text);

                if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                    toastMessage = $"Not found {input.Text}";
                else if (result.ResultContent != null)
                {
                    if (result.ResultContent.Status == 0)
                        toastMessage = "Invited";
                    else
                        toastMessage = "Accepted";
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    toastMessage = "Unauthorized";

                userDialogs.Toast(toastMessage);
            }
        }

        private async Task AddFriend(PersonModel person)
        {
            person.IsBusy = true;
            var result = await ApiClient.AddFriendById(person.UserId.ToString());
            InvitationsList.Clear();
            FriendsList.Clear();
            DataManager.GetData(DataManager.DataType.Friendships, OnFriendshipsUpdate);
            person.IsBusy = false;
        }

        private async Task RemoveFriend(PersonModel person)
        {
            person.IsBusy = true;
            var result = await ApiClient.RemoveFriend(person.UserId.ToString());
            InvitationsList.Clear();
            FriendsList.Clear();
            DataManager.GetData(DataManager.DataType.Friendships, OnFriendshipsUpdate);
            person.IsBusy = false;
        }
    }
}

using PayFair.Mobile.BLL.DataAccess;
using PayFair.DTO.Groups;
using PayFair.Mobile.Models;
using PayFair.Mobile.ViewModels.Popups;
using PayFair.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PayFair.Mobile.ViewModels
{
    public class GroupsViewModel : BaseViewModel
    {
        private IEnumerable<GroupDTO> Groups;
        private IEnumerable<MembershipDTO> Memberships;

        public Command CreateGroupCommand { get; }

        public ObservableCollection<GroupModel> GroupsList { get; set; }
        public ObservableCollection<GroupModel> InvitedGroupList { get; set; }

        public GroupsViewModel()
        {
            CreateGroupCommand = new Command(async () => await OnCreateGroupClicked(), () => !IsBusy);

            GroupsList = new ObservableCollection<GroupModel>();
            InvitedGroupList = new ObservableCollection<GroupModel>();

            //DataManager.GetData(DataManager.DataType.Memberships, OnMembershipsDataUpdate);
        }

        public void OnMembershipsDataUpdate(object data)
        {
            Memberships = (IEnumerable<MembershipDTO>)data;
            List<int> missingGroups = new List<int>();

            GroupsList.Clear();
            InvitedGroupList.Clear();

            foreach (var membership in Memberships)
            {
                GroupModel groupModel;
                if (membership.Status == 0)
                    groupModel = new GroupModel(OnGroupAccept, OnGroupDecline);
                else
                    groupModel = new GroupModel(OnGroupEnter, null);

                GroupDTO groupDTO = Groups.Where(p => p.Id == membership.GroupId).FirstOrDefault();
                if (groupDTO != null)
                    UpdateGroupModel(groupModel, groupDTO);
                else
                    missingGroups.Add(membership.GroupId);

                if (membership.Status == 0)
                    InvitedGroupList.Add(groupModel);
                else
                    GroupsList.Add(groupModel);
            }

            if (missingGroups.Count > 0)
            {
                DataManager.GetData(DataManager.DataType.Groups, OnGroupsDataUpdate);
            }
        }

        private void UpdateGroupsData(IEnumerable<GroupDTO> groups)
        {
            foreach (var groupDTO in groups)
            {
                var groupModel = GroupsList.Union(InvitedGroupList).Where(p => p.Id == groupDTO.Id).FirstOrDefault();
                if (groupModel != null)
                    UpdateGroupModel(groupModel, groupDTO);
            }

            Groups = groups.Union(groups.Where(p => !Groups.Select(x => x.Id).Contains(p.Id)));
        }

        private void UpdateGroupModel(GroupModel groupModel, GroupDTO groupDTO)
        {
            groupModel.Name = groupDTO.Name;
            groupModel.DefaultCurrency = groupDTO.DefaultCurrency;
        }

        private async Task OnGroupAccept(GroupModel group)
        {
        }

        private async Task OnGroupDecline(GroupModel group)
        {
        }

        public void OnGroupsDataUpdate(object data)
        {
            //Groups = (IEnumerable<GroupDTO>)data;

            //GroupsList.Clear();

            //foreach (var group in Groups)
            //{
            //    GroupsList.Add(new GroupModel(OnGroupClicked)
            //    {
            //        Id = group.Id,
            //        Name = group.Name,
            //        DefaultCurrency = group.DefaultCurrency
            //    });
            //}
        }

        private async Task OnCreateGroupClicked()
        {
            IsBusy = true;
            var view = new Views.Popups.EditGroupPopupView(null);
            ((EditGroupPopupViewMdel)view.BindingContext).OnPoped = GroupEditPoped;
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(view);
            IsBusy = false;
        }

        private void GroupEditPoped(object result)
        {
            if ((bool)result)
                DataManager.GetData(DataManager.DataType.Memberships, OnMembershipsDataUpdate);
        }

        private async Task OnGroupEnter(GroupModel group)
        {
            IsBusy = true;

            await Application.Current.MainPage.Navigation.PushAsync(new GroupView(Groups.Where(p => p.Id == group.Id).FirstOrDefault()));

            IsBusy = false;
            //remove group
            //IsBusy = true;
            //var res = await PayFair.Mobile.BLL.ApiClient.ApiClient.DeleteGroup(group.Id);
            //if (res.StatusCode == System.Net.HttpStatusCode.OK)
            //{
            //    Acr.UserDialogs.UserDialogs.Instance.Toast("Deleted");
            //    DataManager.GetData(DataManager.DataType.Groups, OnGroupsDataUpdate);
            //}
            //IsBusy = false;
        }
    }
}

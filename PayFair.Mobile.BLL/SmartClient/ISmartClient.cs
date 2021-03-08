using PayFair.DTO.Groups;
using PayFair.DTO.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayFair.Mobile.BLL.SmartClient
{
    public interface ISmartClient
    {
        public Task<UserDTO> GetMyUser();
        public Task<IEnumerable<FriendshipDTO>> GetFriendships(string status = "-1");
        public Task<IEnumerable<GroupDTO>> GetGroups(List<int> ids = null);
        public Task<IEnumerable<MembershipDTO>> GetMemberships();
    }
}

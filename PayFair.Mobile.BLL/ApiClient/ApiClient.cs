using PayFair.Mobile.BLL.SmartClient;
using PayFair.DTO.Groups;
using PayFair.DTO.Users;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PayFair.Mobile.BLL.ApiClient
{
    public class ApiClient : ISmartClient
    {
        public delegate void OnUnauthorizedDel();
        public static OnUnauthorizedDel OnUnauthorized;

        //public StandardSocketsHttpHandler SocketsHandler { get; private set; }
        //public HttpClient Client;

        //public ApiClient()
        //{
        //    SocketsHandler = new StandardSocketsHttpHandler
        //    {
        //        PooledConnectionIdleTimeout = TimeSpan.FromSeconds(100),
        //        PooledConnectionLifetime = TimeSpan.FromSeconds(60),
        //        MaxConnectionsPerServer = 5,
                
        //    };
        //    Client = new HttpClient();
        //}

        public async static Task<ApiResult<AuthTokenDTO>> Login(UserAuthDTO authDTO) =>
            await new ApiRequest<AuthTokenDTO>().Invoke("usersauth", Method.Put, authDTO);

        public async static Task<ApiResult<FriendshipDTO>> AddFriendByUsername(string name) =>
           await new ApiRequest<FriendshipDTO>().Invoke($"friendships?username={name}", Method.Post);

        public async static Task<ApiResult<FriendshipDTO>> AddFriendById(string id) =>
            await new ApiRequest<FriendshipDTO>().Invoke($"friendships?userId={id}", Method.Post);

        public async static Task<ApiResult<string>> RemoveFriend(string id) =>
            await new ApiRequest<string>().Invoke($"friendships?userId={id}", Method.Delete);

        public async static Task<ApiResult<string>> Register(UserAuthDTO authDTO) =>
            await new ApiRequest<string>().Invoke("usersauth", Method.Post, authDTO);

        public async static Task<ApiResult<string>> CreateGroup(GroupDTO groupDTO) =>
            await new ApiRequest<string>().Invoke("groups", Method.Post, groupDTO);

        public async static Task<ApiResult<string>> DeleteGroup(int id) =>
            await new ApiRequest<string>().Invoke($"groups?Id={id}", Method.Delete);

        public async static Task<ApiResult<string>> AddPersonToGroup(int userId, int groupId) =>
            await new ApiRequest<string>().Invoke($"memberships?userId={userId}&groupId={groupId}", Method.Post);

        public async static Task<ApiResult<string>> DeletePersonFromGroup(int userId, int groupId) =>
            await new ApiRequest<string>().Invoke($"memberships?userId={userId}&groupId={groupId}", Method.Delete);




        public async Task<UserDTO> GetMyUser()
        {
            var res = await new ApiRequest<UserDTO[]>().Invoke($"users?usersids={Session.SessionManager.UserId}&withImage={true}", Method.Get);
            if (res.ResultContent != null && res.ResultContent.Length > 0)
                return res.ResultContent[0];
            return null;
        }

        public async Task<IEnumerable<FriendshipDTO>> GetFriendships(string status = "-1")
        {
            var res = await new ApiRequest<IEnumerable<FriendshipDTO>>().Invoke($"friendships?status={status}", Method.Get);
            return res.ResultContent;
        }

        public async Task<IEnumerable<GroupDTO>> GetGroups(List<int> ids = null)
        {
            var res = await new ApiRequest<IEnumerable<GroupDTO>>().Invoke("groups?groupsIds=" + string.Join('|', ids), Method.Get);
            return res.ResultContent;
        }

        public async Task<IEnumerable<MembershipDTO>> GetMemberships()
        {
            var res = await new ApiRequest<IEnumerable<MembershipDTO>>().Invoke("memberships", Method.Get);
            return res.ResultContent;
        }
    }
}

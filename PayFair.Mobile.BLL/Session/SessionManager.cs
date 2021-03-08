
using PayFair.Mobile.BLL.ApiClient;
using PayFair.DTO.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PayFair.Mobile.BLL.Session
{
    public static class SessionManager
    {
        public enum SessionState { Offline, Authorized, Unauthorized, Authorizing }

        public static string AuthToken { get; private set; }
        public static int UserId { get; private set; }
        public static string Username { get; set; }
        public static string Password { get; set; }

        public static SessionState State { get; private set; } = SessionState.Unauthorized;
        public static event EventHandler OnSessionStateChanged;

      
        public static async Task<SessionState> Authorize(string login, string password)
        {
            State = SessionState.Authorizing;

            UserAuthDTO authDTO = new UserAuthDTO
            {
                Email = login,
                Password = password
            };

            var resp = await ApiClient.ApiClient.Login(authDTO);

            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                AuthToken = resp.ResultContent.Token;
                UserId = resp.ResultContent.UserId;
                State = SessionState.Authorized;
                return SessionState.Authorized;
            }

            return SessionState.Unauthorized;
        }

        public static void Unauthorize()
        {
            State = SessionState.Unauthorized;
            AuthToken = null;
            UserId = 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PayFair.WebApi
{
    public static class SessionManager
    {
        public static Dictionary<string, SessionInfo> Handler = new Dictionary<string, SessionInfo>();

        private static TimeSpan SessionLifeTime = new TimeSpan(0, 30, 0);
        private static MD5 Hash = MD5.Create();

        public static string StartNewSession(int userId)
        {
            string token = "Bearer " + Convert.ToBase64String(Hash.ComputeHash(BitConverter.GetBytes(DateTime.Now.Ticks - 10000)));

            SessionInfo session = new SessionInfo(userId, DateTime.Now + SessionLifeTime);

            Handler.Add(token, session);

            return token;
        }

        public static SessionState GetSessionState(string token)
        {
            SessionInfo session = null;
            if (Handler.TryGetValue(token, out session))
            {
                var now = DateTime.Now;

                
                return SessionState.Authorized;
            }

            return SessionState.Unauthorized;
        }

        public static SessionInfo GetSessionInfo(string key)
        {
            SessionInfo info = null;
            Handler.TryGetValue(key, out info);
            return info;
        }

        public enum SessionState
        { Authorized, Unauthorized, Expired}

        public class SessionInfo
        {
            public int UserId;
            public DateTime ExpiryTime;

            public SessionInfo(int userId, DateTime expiryTime)
            {
                UserId = userId;
                ExpiryTime = expiryTime;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using PayFair.Mobile.BLL.ApiClient;
using PayFair.Mobile.BLL.SmartClient;
using PayFair.Mobile.BLL.Session;

namespace PayFair.Mobile.BLL.DataAccess
{
    public static class DataManager
    {
        public delegate void DataCallback(object data);
        public enum DataType
        {
            MyUser, Friendships, Groups, Memberships
        }

        private static LinkedList<KeyValuePair<DataType, DataCallback>> DataRequests = new LinkedList<KeyValuePair<DataType, DataCallback>>();
        private static List<Task> CurrentTasks = new List<Task>();
        public static Thread MainThreadHandler;

        private static ISmartClient OnlineClient;
        private static ISmartClient OfflineClient;

        public static void Init()
        {
            MainThreadHandler = new Thread(new ThreadStart(MainThreadProc));
            OnlineClient = new ApiClient.ApiClient();

            MainThreadHandler.Start();
        }

        private static async void MainThreadProc()
        {
            if (Session.SessionManager.State == Session.SessionManager.SessionState.Unauthorized)
            {
                var authRes = await SessionManager.Authorize(SessionManager.Username, SessionManager.Password);

                if (authRes == SessionManager.SessionState.Unauthorized)
                {
                    
                    return;
                }
            }

            while (true)
            {
                if (DataRequests.Count <= 0)
                {
                    Thread.Sleep(10);
                    continue;
                }

                KeyValuePair<DataType, DataCallback> req = DataRequests.First.Value;
                DataRequests.RemoveFirst();


                ISmartClient Client = OnlineClient;

                object result = req.Key switch
                {
                    DataType.MyUser => await Client.GetMyUser(),
                    DataType.Friendships => await Client.GetFriendships(),
                    DataType.Groups => await Client.GetGroups(),
                    DataType.Memberships => await Client.GetMemberships(),
                    _ => null
                };

                if (result != null)
                    req.Value(result);
            }
        }


        public static void GetData(DataType dataType, DataCallback callback, bool highPrio = false)
        {
            KeyValuePair<DataType, DataCallback> dataRequest = new KeyValuePair<DataType, DataCallback>(dataType, callback);

            if (highPrio)
                DataRequests.AddFirst(dataRequest);
            else
                DataRequests.AddLast(dataRequest);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace PayFair.Mobile.BLL.BO
{
    public class FriendModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }




        public string FullName
        {
            get
            {
                return FirstName + ' ' + LastName;
            }
        }
    }
}

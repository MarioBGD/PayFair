using System;
using System.Collections.Generic;
using System.Text;

namespace PayFair.DTO.Users
{
    public class FriendshipDTO
    {
        public int Id { get; set; }
        public int UserOneId { get; set; }
        public int UserTwoId { get; set; }

        /// <summary>
        /// 0 - User one invited user two
        /// 1 - Users are in friendship
        /// </summary>
        public int Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace PayFair.DTO.Groups
{
    public class MembershipDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }

        /// <summary>
        /// 0 - invited, 1 - normal, 2 - admin, 3 - owner
        /// </summary>
        public int Status { get; set; }
    }
}

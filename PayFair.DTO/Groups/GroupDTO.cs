using System;
using System.Collections.Generic;
using System.Text;

namespace PayFair.DTO.Groups
{
    public class GroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DefaultCurrency { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

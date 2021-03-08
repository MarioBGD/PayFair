using PayFair.DTO.Groups;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayFair.WebApi.DAL.Repositories.Interfaces
{
    public interface IMembershipsRepository
    {
        Task<IEnumerable<MembershipDTO>> GetAllByGroupId(int groupId);
        Task<IEnumerable<MembershipDTO>> GetAllByUserId(int groupId);
        Task<MembershipDTO> GetByUserAndGroupId(int userId, int groupId);
    }
}

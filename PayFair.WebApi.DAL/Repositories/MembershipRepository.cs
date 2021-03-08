using Dapper;
using PayFair.WebApi.DAL.Repositories.Interfaces;
using PayFair.DTO.Groups;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayFair.WebApi.DAL.Repositories
{
    public class MembershipRepository : Repository<MembershipDTO>, IMembershipsRepository
    {
        public MembershipRepository(IUnitOfWork unitOfWork) : base (unitOfWork.Connection, unitOfWork.Transaction, "Memberships")
        {

        }

        public async Task<IEnumerable<MembershipDTO>> GetAllByGroupId(int groupId)
        {
            string query = $"SELECT * FROM {TableName}" +
                $" WHERE GroupId ='{groupId}'";
            return await Connection.QueryAsync<MembershipDTO>(query, null, Transaction);
        }

        public async Task<IEnumerable<MembershipDTO>> GetAllByUserId(int userId)
        {
            string query = $"SELECT * FROM {TableName}" +
                $" WHERE UserId ='{userId}'";
            return await Connection.QueryAsync<MembershipDTO>(query, null, Transaction);
        }

        public async Task<MembershipDTO> GetByUserAndGroupId(int userId, int groupId)
        {
            string query = $"SELECT * FROM {TableName}" +
                $" WHERE UserId ='{userId}'" +
                $" AND GroupId = '{groupId}'";
            return await Connection.QueryFirstOrDefaultAsync<MembershipDTO>(query, null, Transaction);
        }
    }
}

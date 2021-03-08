using Dapper;
using PayFair.WebApi.DAL.Repositories.Interfaces;
using PayFair.DTO.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayFair.WebApi.DAL.Repositories
{
    public class FriendshipsRepository : Repository<FriendshipDTO>, IFriendshipsRepository
    {
        public FriendshipsRepository(IUnitOfWork unitOfwork) : base (unitOfwork.Connection, unitOfwork.Transaction, "Friendships")
        {

        }

        public async Task<FriendshipDTO> GetFriendship(int userOneId, int userTwoId)
        {
            string query = $"SELECT * FROM {TableName}" +
                $" WHERE (UserOneId ='{userOneId}' AND UserTwoId = '{userTwoId}')" +
                $" OR    (UserOneId ='{userTwoId}' AND UserTwoId = '{userOneId}')";
            return await Connection.QuerySingleOrDefaultAsync<FriendshipDTO>(query, null, Transaction);
        }

        public async Task<IEnumerable<FriendshipDTO>> GetFriendships(int userId, int status = -1)
        {
            string query = $"SELECT * FROM {TableName}" +
                $" WHERE (UserOneId ='{userId}' OR UserTwoId = '{userId}')" +
                (status >= 0 ? " AND Status = {status}" : "");
            return await Connection.QueryAsync<FriendshipDTO>(query, null, Transaction);
        }
    }
}
